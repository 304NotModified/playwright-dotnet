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

namespace Microsoft.Playwright.Core;

internal static class PageEvent
{
    public static PlaywrightEvent<IRequest> Request { get; } = new("Request");

    public static PlaywrightEvent<IRequest> RequestFinished { get; } = new("RequestFinished");

    public static PlaywrightEvent<IPage> Crash { get; } = new("Crash");

    public static PlaywrightEvent<IPage> Close { get; } = new("Close");

    public static PlaywrightEvent<IResponse> Response { get; } = new("Response");

    public static PlaywrightEvent<IDownload> Download { get; } = new("Download");

    public static PlaywrightEvent<IConsoleMessage> Console { get; } = new("Console");

    public static PlaywrightEvent<IPage> Popup { get; } = new("Popup");

    public static PlaywrightEvent<IFrame> FrameNavigated { get; } = new("FrameNavigated");

    public static PlaywrightEvent<IFrame> FrameDetached { get; } = new("FrameDetached");

    public static PlaywrightEvent<IWorker> Worker { get; } = new("Worker");

    public static PlaywrightEvent<IDialog> Dialog { get; } = new("Dialog");

    public static PlaywrightEvent<IFileChooser> FileChooser { get; } = new("FileChooser");

    public static PlaywrightEvent<string> PageError { get; } = new("PageError");

    public static PlaywrightEvent<IPage> Load { get; } = new("Load");

    public static PlaywrightEvent<IPage> DOMContentLoaded { get; } = new("DOMContentLoaded");

    public static PlaywrightEvent<IWebSocket> WebSocket { get; } = new("WebSocket");
}
