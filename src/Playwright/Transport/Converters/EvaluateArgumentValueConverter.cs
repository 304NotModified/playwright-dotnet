/*
 * MIT License
 *
 * Copyright (c) Microsoft Corporation.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in all
 * copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN THE
 * SOFTWARE.
 */
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Dynamic;
using System.Globalization;
using System.Linq;
using System.Numerics;
using System.Runtime.Serialization;
using System.Text.Json;
using System.Text.RegularExpressions;
using Microsoft.Playwright.Core;
using Microsoft.Playwright.Helpers;

namespace Microsoft.Playwright.Transport.Converters;

internal static class EvaluateArgumentValueConverter
{
    private static readonly JsonSerializerOptions _evaluateArgumentValueConverterSerializerOptions = new()
    {
        ReferenceHandler = System.Text.Json.Serialization.ReferenceHandler.Preserve,
    };

    internal static object Serialize(object? value, List<EvaluateArgumentGuidElement> handles, VisitorInfo visitorInfo)
    {
        int id;
        if (value == null)
        {
            return new { v = "null" };
        }

        if (visitorInfo.Visited.TryGetValue(visitorInfo.Identity(value), out var @ref))
        {
            return new Dictionary<string, object> { ["ref"] = @ref };
        }

        if (value is double nan && double.IsNaN(nan))
        {
            return new { v = "NaN" };
        }

        if (value is double infinity && double.IsPositiveInfinity(infinity))
        {
            return new { v = "Infinity" };
        }

        if (value is double negativeInfinity && double.IsNegativeInfinity(negativeInfinity))
        {
            return new { v = "-Infinity" };
        }

        if (value is double negativeZero && negativeZero.IsNegativeZero())
        {
            return new { v = "-0" };
        }

        if (value.GetType() == typeof(string))
        {
            return new { s = value };
        }

        if (value.GetType().IsEnum)
        {
            return new { n = (int)value };
        }

        if (
            value.GetType() == typeof(int) ||
            value.GetType() == typeof(decimal) ||
            value.GetType() == typeof(long) ||
            value.GetType() == typeof(short) ||
            value.GetType() == typeof(double) ||
            value.GetType() == typeof(int?) ||
            value.GetType() == typeof(decimal?) ||
            value.GetType() == typeof(long?) ||
            value.GetType() == typeof(short?) ||
            value.GetType() == typeof(double?))
        {
            return new { n = value };
        }

        if (value.GetType() == typeof(bool) || value.GetType() == typeof(bool?))
        {
            return new { b = value };
        }

        if (value is DateTime date)
        {
            return new { d = date.ToString("o", CultureInfo.InvariantCulture) };
        }

        if (value is Uri uri)
        {
            return new { u = uri.ToString() };
        }

        if (value is BigInteger bigInteger)
        {
            return new { bi = bigInteger.ToString(CultureInfo.InvariantCulture) };
        }

        if (value is Exception exception)
        {
            return new Dictionary<string, object>
            {
                ["e"] = new Dictionary<string, object>
                {
                    ["n"] = exception.GetType().Name,
                    ["m"] = exception.Message,
                    ["s"] = exception.StackTrace ?? string.Empty,
                },
            };
        }

        if (value is Regex regex)
        {
            return new { r = new { p = regex.ToString(), f = regex.Options.GetInlineFlags() } };
        }

        if (value is Guid guid)
        {
            return new { s = guid.ToString() };
        }

        if (value is ExpandoObject)
        {
            var o = new List<object>();
            id = ++visitorInfo.LastId;
            visitorInfo.Visited.Add(visitorInfo.Identity(value), id);
            foreach (KeyValuePair<string, object> property in (IDictionary<string, object>)value)
            {
                o.Add(new { k = property.Key, v = Serialize(property.Value, handles, visitorInfo) });
            }
            return new { o, id };
        }

        if (value is IDictionary dictionary && dictionary.Keys.OfType<string>().Any())
        {
            var o = new List<object>();
            id = ++visitorInfo.LastId;
            visitorInfo.Visited.Add(visitorInfo.Identity(value), id);
            foreach (object key in dictionary.Keys)
            {
                object obj = dictionary[key];
                o.Add(new { k = key.ToString(), v = Serialize(obj, handles, visitorInfo) });
            }

            return new { o, id };
        }

        if (value is IEnumerable array)
        {
            var a = new List<object>();
            id = ++visitorInfo.LastId;
            visitorInfo.Visited.Add(visitorInfo.Identity(value), id);
            foreach (object item in array)
            {
                a.Add(Serialize(item, handles, visitorInfo));
            }

            return new { a, id };
        }

        if (value is ChannelOwner channelOwner)
        {
            handles.Add(new() { Guid = channelOwner.Guid });
            return new { h = handles.Count - 1 };
        }

        id = ++visitorInfo.LastId;
        visitorInfo.Visited.Add(visitorInfo.Identity(value), id);
        var entries = new List<object>();
        foreach (PropertyDescriptor propertyDescriptor in TypeDescriptor.GetProperties(value))
        {
            object obj = propertyDescriptor.GetValue(value);
            entries.Add(new { k = propertyDescriptor.Name, v = Serialize(obj, handles, visitorInfo) });
        }

        return new { o = entries, id };
    }

    internal static object? Deserialize(JsonElement result, Type t)
    {
        var parsed = ParseEvaluateResultToExpando(result, new Dictionary<int, object>());

        // If use wants expando or any object -> return as is.
        if (t == typeof(ExpandoObject) || t == typeof(object))
        {
            return parsed;
        }

        // User wants Json, serialize to JsonElement.
        if (t == typeof(JsonElement) || t == typeof(JsonElement?))
        {
            if (t == typeof(JsonElement?) && parsed == null)
            {
                return null;
            }
            return JsonSerializer.SerializeToElement(parsed, _evaluateArgumentValueConverterSerializerOptions);
        }

        // Convert recursively to a requested type.
        return ToExpectedType(parsed, t, new Dictionary<object, object>());
    }

    private static object? ToExpectedType(object? parsed, Type t, IDictionary<object, object> visited)
    {
        if (parsed == null)
        {
            return null;
        }

        if (visited.TryGetValue(parsed, out var value))
        {
            return value;
        }

        if (parsed is Array parsedArray)
        {
            var result = (IList)Activator.CreateInstance(t, parsedArray.Length);
            visited.Add(parsed, result);
            for (int i = 0; i < parsedArray.Length; ++i)
            {
                result[i] = ToExpectedType(parsedArray.GetValue(i), t.GetElementType(), visited);
            }
            return result;
        }

        if (parsed is ExpandoObject parsedExpando)
        {
            object objResult;
            try
            {
                objResult = Activator.CreateInstance(t);
            }
            catch (Exception ex)
            {
                throw new PlaywrightException("Return type mismatch. Expecting " + t.ToString() + ", got Object", ex);
            }
            visited.Add(parsed, objResult);

            foreach (var kv in parsedExpando)
            {
                var property = Array.Find(t.GetProperties(), prop => string.Equals(prop.Name, kv.Key, StringComparison.OrdinalIgnoreCase));
                property?.SetValue(objResult, ToExpectedType(kv.Value, property.PropertyType, visited));
            }

            return objResult;
        }

        return ChangeType(parsed, t);
    }

    private static object? ChangeType(object value, Type conversion)
    {
        var t = conversion;

        if (t.IsGenericType && t.GetGenericTypeDefinition().Equals(typeof(Nullable<>)))
        {
            if (value == null)
            {
                return null;
            }

            t = Nullable.GetUnderlyingType(t);
        }

        if (t == typeof(Guid))
        {
            if (value == null)
            {
                return Guid.Empty;
            }
            return Guid.Parse(value.ToString());
        }

        return Convert.ChangeType(value, t, CultureInfo.InvariantCulture);
    }

    private static object? ParseEvaluateResultToExpando(JsonElement result, IDictionary<int, object> refs)
    {
        // Parse JSON into a structure where objects/arrays are represented with expando/arrays.
        if (result.TryGetProperty("v", out var value))
        {
            if (value.ValueKind == JsonValueKind.Null)
            {
                return null;
            }

            return value.ToString() switch
            {
                "null" => null,
                "undefined" => null,
                "Infinity" => double.PositiveInfinity,
                "-Infinity" => double.NegativeInfinity,
                "-0" => -0d,
                "NaN" => double.NaN,
                _ => null,
            };
        }

        if (result.TryGetProperty("ref", out var refValue))
        {
            return refs[refValue.GetInt32()];
        }

        if (result.TryGetProperty("d", out var date))
        {
            return date.ToObject<DateTime>();
        }

        if (result.TryGetProperty("u", out var url))
        {
            return url.ToObject<Uri>();
        }

        if (result.TryGetProperty("bi", out var bigInt))
        {
            return BigInteger.Parse(bigInt.ToObject<string>(), CultureInfo.InvariantCulture);
        }

        if (result.TryGetProperty("e", out var error))
        {
            return new Exception(error.GetProperty("s").ToString());
        }

        if (result.TryGetProperty("r", out var regex))
        {
            return new Regex(regex.GetProperty("p").ToString(), RegexOptionsExtensions.FromInlineFlags(regex.GetProperty("f").ToString()));
        }

        if (result.TryGetProperty("ta", out var ta))
        {
            byte[] bytes = Convert.FromBase64String(ta.GetProperty("b").ToString());
            return ta.GetProperty("k").ToString() switch
            {
                "i8" => bytes.Select(b => unchecked((sbyte)b)).ToArray(),
                "ui8" => bytes,
                "ui8c" => bytes,
                "i16" => Enumerable.Range(0, bytes.Length / 2).Select(i => BitConverter.ToInt16(bytes, i * 2)).ToArray(),
                "ui16" => Enumerable.Range(0, bytes.Length / 2).Select(i => BitConverter.ToUInt16(bytes, i * 2)).ToArray(),
                "i32" => Enumerable.Range(0, bytes.Length / 4).Select(i => BitConverter.ToInt32(bytes, i * 4)).ToArray(),
                "ui32" => Enumerable.Range(0, bytes.Length / 4).Select(i => BitConverter.ToUInt32(bytes, i * 4)).ToArray(),
                "f32" => Enumerable.Range(0, bytes.Length / 4).Select(i => BitConverter.ToSingle(bytes, i * 4)).ToArray(),
                "f64" => Enumerable.Range(0, bytes.Length / 8).Select(i => BitConverter.ToDouble(bytes, i * 8)).ToArray(),
                "bi64" => Enumerable.Range(0, bytes.Length / 8).Select(i => BitConverter.ToInt64(bytes, i * 8)).ToArray(),
                "bui64" => Enumerable.Range(0, bytes.Length / 8).Select(i => BitConverter.ToUInt64(bytes, i * 8)).ToArray(),
                _ => null,
            };
        }

        if (result.TryGetProperty("b", out var boolean))
        {
            return boolean.ToObject<bool>();
        }

        if (result.TryGetProperty("s", out var stringValue))
        {
            return stringValue.ToObject<string>();
        }

        if (result.TryGetProperty("n", out var numericValue))
        {
            return numericValue.ToObject<double>();
        }

        if (result.TryGetProperty("o", out var obj))
        {
            var expando = new ExpandoObject();
            refs.Add(result.GetProperty("id").GetInt32(), expando);
            IDictionary<string, object?> dict = expando;
            foreach (var kv in obj.ToObject<KeyJsonElementValueObject[]>())
            {
                dict[kv.K] = ParseEvaluateResultToExpando(kv.V, refs);
            }

            return expando;
        }

        if (result.TryGetProperty("a", out var array))
        {
            List<object?> list = [];
            refs.Add(result.GetProperty("id").GetInt32(), list);
            foreach (var item in array.EnumerateArray())
            {
                list.Add(ParseEvaluateResultToExpando(item, refs));
            }
            return list.ToArray();
        }
        return null;
    }

    internal class VisitorInfo
    {
        internal VisitorInfo()
        {
            Visited = new Dictionary<long, int>();
            IDGenerator = new ObjectIDGenerator();
        }

        internal Dictionary<long, int> Visited { get; set; }

        internal int LastId { get; set; }

        private ObjectIDGenerator IDGenerator { get; }

        internal long Identity(object obj)
            => IDGenerator.GetId(obj, out _);
    }
}
