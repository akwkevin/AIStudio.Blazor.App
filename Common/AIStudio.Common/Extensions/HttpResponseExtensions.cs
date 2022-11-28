using AIStudio.Common.Extensions;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace AIStudio.Common.Extensions;

/// <summary>
/// 
/// </summary>
public static class HttpResponseExtensions
{
    /// <summary>
    /// Writes the asynchronous.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="result">The result.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <returns></returns>
    public static Task WriteAsync(this HttpResponse response, AjaxResult result, CancellationToken cancellationToken = default)
        => WriteAsync(response, result, Encoding.UTF8, cancellationToken);

    /// <summary>
    /// Writes the asynchronous.
    /// </summary>
    /// <param name="response">The response.</param>
    /// <param name="result">The result.</param>
    /// <param name="encoding">The encoding.</param>
    /// <param name="cancellationToken">The cancellation token.</param>
    /// <exception cref="System.ArgumentNullException">
    /// response
    /// or
    /// result
    /// or
    /// encoding
    /// </exception>
    /// <exception cref="System.Exception">HTTP响应已经开始，不可更改响应</exception>
    public static async Task WriteAsync(this HttpResponse response, AjaxResult result, Encoding encoding, CancellationToken cancellationToken = default)
    {
        if (response == null) throw new ArgumentNullException(nameof(response));
        if (response.HasStarted) throw new Exception("HTTP响应已经开始，不可更改响应");
        if (result == null) throw new ArgumentNullException(nameof(result));
        if (encoding == null) throw new ArgumentNullException(nameof(encoding));

        response.StatusCode = result.Code;
        response.ContentType = encoding == Encoding.UTF8 ? "application/json; charset=utf-8" : "application/json";
        await response.WriteAsync(result.ToJson(), encoding);
    }
}
