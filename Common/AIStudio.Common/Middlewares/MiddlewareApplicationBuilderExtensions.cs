using Microsoft.AspNetCore.Http;
using Simple.Common.Middlewares;

namespace Microsoft.AspNetCore.Builder;

public static class MiddlewareApplicationBuilderExtensions
{
    /// <summary>
    /// Api 异常处理中间件
    /// </summary>
    /// <param name="app"></param>
    /// <returns></returns>
    public static IApplicationBuilder UseMiddleware_(this IApplicationBuilder app)
    {
        app.Use((context, next) =>
        {
            // 确保 Body 可以多次读取。通常，在内存中缓冲请求正文;将大于 30K 字节的请求写入磁盘。
            context.Request.EnableBuffering();
            return next(context);
        });

        return app.UseMiddleware<ApiExceptionMiddleware>();
    }
}
