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

using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace Microsoft.Playwright;

public partial class HttpCredentials
{
    /// <summary><para></para></summary>
    [Required]
    [JsonPropertyName("username")]
    public string Username { get; set; } = default!;

    /// <summary><para></para></summary>
    [Required]
    [JsonPropertyName("password")]
    public string Password { get; set; } = default!;

    /// <summary><para>Restrain sending http credentials on specific origin (scheme://host:port).</para></summary>
    [JsonPropertyName("origin")]
    public string? Origin { get; set; }

    /// <summary>
    /// <para>
    /// This option only applies to the requests sent from corresponding <see cref="IAPIRequestContext"/>
    /// and does not affect requests sent from the browser. <c>'always'</c> - <c>Authorization</c>
    /// header with basic authentication credentials will be sent with the each API request.
    /// <c>'unauthorized</c> - the credentials are only sent when 401 (Unauthorized) response
    /// with <c>WWW-Authenticate</c> header is received. Defaults to <c>'unauthorized'</c>.
    /// </para>
    /// </summary>
    [JsonPropertyName("send")]
    public HttpCredentialsSend? Send { get; set; }
}
