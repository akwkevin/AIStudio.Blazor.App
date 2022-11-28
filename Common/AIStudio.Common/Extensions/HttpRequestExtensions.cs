using Microsoft.AspNetCore.Http;
using System.Text;

namespace AIStudio.Common.Extensions;

/// <summary>
/// 
/// </summary>
public static class HttpRequestExtensions
{
    /// <summary>
    /// 获取完整请求地址
    /// </summary>
    /// <param name="request">The request.</param>
    /// <returns></returns>
    public static string GetRequestUrlAddress(this HttpRequest request)
    {
        return new StringBuilder()
            .Append(request.Scheme).Append("://").Append(request.Host)
            .Append(request.PathBase.ToString())
            .Append(request.Path.ToString())
            .Append(request.QueryString)
            .ToString();
    }
}
