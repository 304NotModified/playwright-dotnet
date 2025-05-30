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

public class BrowserTypeConnectOverCDPOptions
{
    public BrowserTypeConnectOverCDPOptions() { }

    public BrowserTypeConnectOverCDPOptions(BrowserTypeConnectOverCDPOptions clone)
    {
        if (clone == null)
        {
            return;
        }

        Headers = clone.Headers;
        SlowMo = clone.SlowMo;
        Timeout = clone.Timeout;
    }

    /// <summary><para>Additional HTTP headers to be sent with connect request. Optional.</para></summary>
    [JsonPropertyName("headers")]
    public IEnumerable<KeyValuePair<string, string>>? Headers { get; set; }

    /// <summary>
    /// <para>
    /// Slows down Playwright operations by the specified amount of milliseconds. Useful
    /// so that you can see what is going on. Defaults to 0.
    /// </para>
    /// </summary>
    [JsonPropertyName("slowMo")]
    public float? SlowMo { get; set; }

    /// <summary>
    /// <para>
    /// Maximum time in milliseconds to wait for the connection to be established. Defaults
    /// to <c>30000</c> (30 seconds). Pass <c>0</c> to disable timeout.
    /// </para>
    /// </summary>
    [JsonPropertyName("timeout")]
    public float? Timeout { get; set; }
}
