/*
 * MIT License
 *
 * Copyright (c) Microsoft Corporation.
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
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

using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace Microsoft.Playwright;

public class APIRequestContextOptions
{
    public APIRequestContextOptions() { }

    public APIRequestContextOptions(APIRequestContextOptions clone)
    {
        if (clone == null)
        {
            return;
        }

        Data = clone.Data;
        DataByte = clone.DataByte;
        DataObject = clone.DataObject;
        DataString = clone.DataString;
        FailOnStatusCode = clone.FailOnStatusCode;
        Form = clone.Form;
        Headers = clone.Headers;
        IgnoreHTTPSErrors = clone.IgnoreHTTPSErrors;
        MaxRedirects = clone.MaxRedirects;
        MaxRetries = clone.MaxRetries;
        Method = clone.Method;
        Multipart = clone.Multipart;
        Params = clone.Params;
        ParamsString = clone.ParamsString;
        Timeout = clone.Timeout;
    }

    /// <summary>
    /// <para>
    /// Allows to set post data of the request. If the data parameter is an object, it will
    /// be serialized to json string and <c>content-type</c> header will be set to <c>application/json</c>
    /// if not explicitly set. Otherwise the <c>content-type</c> header will be set to <c>application/octet-stream</c>
    /// if not explicitly set.
    /// </para>
    /// </summary>
    [JsonPropertyName("data")]
    public string? Data { get; set; }

    /// <summary>
    /// <para>
    /// Allows to set post data of the request. If the data parameter is an object, it will
    /// be serialized to json string and <c>content-type</c> header will be set to <c>application/json</c>
    /// if not explicitly set. Otherwise the <c>content-type</c> header will be set to <c>application/octet-stream</c>
    /// if not explicitly set.
    /// </para>
    /// </summary>
    [JsonPropertyName("dataByte")]
    public byte[]? DataByte { get; set; }

    /// <summary>
    /// <para>
    /// Allows to set post data of the request. If the data parameter is an object, it will
    /// be serialized to json string and <c>content-type</c> header will be set to <c>application/json</c>
    /// if not explicitly set. Otherwise the <c>content-type</c> header will be set to <c>application/octet-stream</c>
    /// if not explicitly set.
    /// </para>
    /// </summary>
    [JsonPropertyName("dataObject")]
    public object? DataObject { get; set; }

    /// <summary>
    /// <para>
    /// Allows to set post data of the request. If the data parameter is an object, it will
    /// be serialized to json string and <c>content-type</c> header will be set to <c>application/json</c>
    /// if not explicitly set. Otherwise the <c>content-type</c> header will be set to <c>application/octet-stream</c>
    /// if not explicitly set.
    /// </para>
    /// </summary>
    [JsonPropertyName("dataString")]
    public string? DataString { get; set; }

    /// <summary>
    /// <para>
    /// Whether to throw on response codes other than 2xx and 3xx. By default response object
    /// is returned for all status codes.
    /// </para>
    /// </summary>
    [JsonPropertyName("failOnStatusCode")]
    public bool? FailOnStatusCode { get; set; }

    /// <summary>
    /// <para>
    /// Provides an object that will be serialized as html form using <c>application/x-www-form-urlencoded</c>
    /// encoding and sent as this request body. If this parameter is specified <c>content-type</c>
    /// header will be set to <c>application/x-www-form-urlencoded</c> unless explicitly
    /// provided.
    /// </para>
    /// <para>An instance of <see cref="IFormData"/> can be created via <see cref="IAPIRequestContext.CreateFormData"/>.</para>
    /// </summary>
    [JsonPropertyName("form")]
    public IFormData? Form { get; set; }

    /// <summary>
    /// <para>
    /// Allows to set HTTP headers. These headers will apply to the fetched request as well
    /// as any redirects initiated by it.
    /// </para>
    /// </summary>
    [JsonPropertyName("headers")]
    public IEnumerable<KeyValuePair<string, string>>? Headers { get; set; }

    /// <summary><para>Whether to ignore HTTPS errors when sending network requests. Defaults to <c>false</c>.</para></summary>
    [JsonPropertyName("ignoreHTTPSErrors")]
    public bool? IgnoreHTTPSErrors { get; set; }

    /// <summary>
    /// <para>
    /// Maximum number of request redirects that will be followed automatically. An error
    /// will be thrown if the number is exceeded. Defaults to <c>20</c>. Pass <c>0</c> to
    /// not follow redirects.
    /// </para>
    /// </summary>
    [JsonPropertyName("maxRedirects")]
    public int? MaxRedirects { get; set; }

    /// <summary>
    /// <para>
    /// Maximum number of times network errors should be retried. Currently only <c>ECONNRESET</c>
    /// error is retried. Does not retry based on HTTP response codes. An error will be
    /// thrown if the limit is exceeded. Defaults to <c>0</c> - no retries.
    /// </para>
    /// </summary>
    [JsonPropertyName("maxRetries")]
    public int? MaxRetries { get; set; }

    /// <summary>
    /// <para>
    /// If set changes the fetch method (e.g. <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/PUT">PUT</a>
    /// or <a href="https://developer.mozilla.org/en-US/docs/Web/HTTP/Methods/POST">POST</a>).
    /// If not specified, GET method is used.
    /// </para>
    /// </summary>
    [JsonPropertyName("method")]
    public string? Method { get; set; }

    /// <summary>
    /// <para>
    /// Provides an object that will be serialized as html form using <c>multipart/form-data</c>
    /// encoding and sent as this request body. If this parameter is specified <c>content-type</c>
    /// header will be set to <c>multipart/form-data</c> unless explicitly provided. File
    /// values can be passed as file-like object containing file name, mime-type and its
    /// content.
    /// </para>
    /// <para>An instance of <see cref="IFormData"/> can be created via <see cref="IAPIRequestContext.CreateFormData"/>.</para>
    /// </summary>
    [JsonPropertyName("multipart")]
    public IFormData? Multipart { get; set; }

    /// <summary><para>Query parameters to be sent with the URL.</para></summary>
    [JsonPropertyName("params")]
    public IEnumerable<KeyValuePair<string, object>>? Params { get; set; }

    /// <summary><para>Query parameters to be sent with the URL.</para></summary>
    [JsonPropertyName("paramsString")]
    public string? ParamsString { get; set; }

    /// <summary>
    /// <para>
    /// Request timeout in milliseconds. Defaults to <c>30000</c> (30 seconds). Pass <c>0</c>
    /// to disable timeout.
    /// </para>
    /// </summary>
    [JsonPropertyName("timeout")]
    public float? Timeout { get; set; }
}
