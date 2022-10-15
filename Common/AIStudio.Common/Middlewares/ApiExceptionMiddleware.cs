using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Jwt;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.Extensions.Logging;
using System.Runtime.ExceptionServices;

namespace Simple.Common.Middlewares;

public class ApiExceptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger _logger;
    private readonly IEventPublisher _publisher;

    public ApiExceptionMiddleware(RequestDelegate next,
                                  ILogger<ApiExceptionMiddleware> logger,
                                  IEventPublisher publisher)
    {
        _next = next;
        _logger = logger;
        _publisher = publisher;
    }

    public async Task InvokeAsync(HttpContext context)
    {
        ExceptionDispatchInfo edi;

        try
        {
            await _next(context);
            return;
        }
        catch (Exception ex)
        {
            // 捕获异常，但不在 catch 块中继续处理，因为这样不利于堆栈的使用
            edi = ExceptionDispatchInfo.Capture(ex);
        }

        await HandleExceptionAsync(context, edi);

    }

    private async Task HandleExceptionAsync(HttpContext context, ExceptionDispatchInfo edi)
    {
        // 发布异常事件
        string eventId = await PublishEventAsync(edi.SourceException, context.User.Identity?.Name, context.User.FindFirst(SimpleClaimTypes.UserId)?.Value, context.User.FindFirst(SimpleClaimTypes.TenantId)?.Value);

        // 如果已经开始响应客户端，则该异常将无法处理
        if (context.Response.HasStarted)
        {
            _logger.LogError(edi.SourceException, $"EventId: {eventId}. Message: HTTP响应已经开始，无法处理该异常");
            return;
        }

        // 日志记录
        _logger.LogError(edi.SourceException, $"EventId: {eventId}. Message: 全局异常拦截");

        // 清空 Response，重设终结点（Endpoint）（!Problem：为什么要重设终结点？）
        context.Response.Clear();
        context.SetEndpoint(endpoint: null);
        var routeValuesFeature = context.Features.Get<IRouteValuesFeature>();
        if (routeValuesFeature != null)
        {
            routeValuesFeature.RouteValues = null!;
        }

        // 响应头处理
        context.Response.Headers.CacheControl = "no-cache,no-store";
        context.Response.Headers.Pragma = "no-cache";
        context.Response.Headers.Expires = "-1";
        context.Response.Headers.ETag = default;

        // 响应
        var result = AjaxResult.Status500InternalServerError($"系统异常，异常Id: {eventId}，异常信息：{edi.SourceException.Message}，请联系管理员。");
        await context.Response.WriteAsync(result.ToJson());
    }

    /// <summary>
    /// 发布异常事件
    /// </summary>
    /// <param name="exception">异常</param>
    /// <param name="userId">操作人账号</param>
    /// <returns>事件Id</returns>
    private async Task<string> PublishEventAsync(Exception exception, string? userName, string? userId,  string? tenantId)
    {
        // 定义异常事件模型
        ExceptionEvent exceptionEvent = new ExceptionEvent()
        {
            CreatorId = userId,
            CreatorName = userName,
            TenantId = tenantId,
            Name = exception.Message,
            Message = exception.Message,
            ClassName = exception.TargetSite?.DeclaringType?.FullName,
            MethodName = exception.TargetSite?.Name,
            ExceptionSource = exception.Source,
            StackTrace = exception.StackTrace,
            Parameters = exception.TargetSite?.GetParameters().ToString(),
            ExceptionTime = DateTimeOffset.Now
        };

        // 发布异常事件
        await _publisher.PublishAsync(exceptionEvent);

        return exceptionEvent.Id;
    }
}
