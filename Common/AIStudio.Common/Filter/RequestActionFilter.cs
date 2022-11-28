using AIStudio.Common.AppSettings;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.EventHandlers;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Extensions;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text;
using UAParser;

namespace Simple.Common.Filters;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IAsyncActionFilter" />
/// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter" />
public class RequestActionFilter : IAsyncActionFilter, IOrderedFilter
{
    /// <summary>
    /// The publisher
    /// </summary>
    private readonly IEventPublisher _publisher;
    /// <summary>
    /// The operator
    /// </summary>
    private readonly IOperator _operator;

    /// <summary>
    /// Gets the order value for determining the order of execution of filters. Filters execute in
    /// ascending numeric value of the <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" /> property.
    /// </summary>
    /// <remarks>
    /// <para>
    /// Filters are executed in an ordering determined by an ascending sort of the <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" /> property.
    /// </para>
    /// <para>
    /// Asynchronous filters, such as <see cref="T:Microsoft.AspNetCore.Mvc.Filters.IAsyncActionFilter" />, surround the execution of subsequent
    /// filters of the same filter kind. An asynchronous filter with a lower numeric <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" />
    /// value will have its filter method, such as <see cref="M:Microsoft.AspNetCore.Mvc.Filters.IAsyncActionFilter.OnActionExecutionAsync(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext,Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate)" />,
    /// executed before that of a filter with a higher value of <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" />.
    /// </para>
    /// <para>
    /// Synchronous filters, such as <see cref="T:Microsoft.AspNetCore.Mvc.Filters.IActionFilter" />, have a before-method, such as
    /// <see cref="M:Microsoft.AspNetCore.Mvc.Filters.IActionFilter.OnActionExecuting(Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext)" />, and an after-method, such as
    /// <see cref="M:Microsoft.AspNetCore.Mvc.Filters.IActionFilter.OnActionExecuted(Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext)" />. A synchronous filter with a lower numeric <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" />
    /// value will have its before-method executed before that of a filter with a higher value of
    /// <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" />. During the after-stage of the filter, a synchronous filter with a lower
    /// numeric <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" /> value will have its after-method executed after that of a filter with a higher
    /// value of <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" />.
    /// </para>
    /// <para>
    /// If two filters have the same numeric value of <see cref="P:Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter.Order" />, then their relative execution order
    /// is determined by the filter scope.
    /// </para>
    /// </remarks>
    public int Order { get; set; } = -8000;

    /// <summary>
    /// Initializes a new instance of the <see cref="RequestActionFilter"/> class.
    /// </summary>
    /// <param name="publisher">The publisher.</param>
    /// <param name="operator">The operator.</param>
    public RequestActionFilter(IEventPublisher publisher, IOperator @operator)
    {
        _publisher = publisher;
        _operator = @operator;
    }

    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
    /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        bool isSkipRecord = false;
        var httpContext = context.HttpContext;
        var request = context.HttpContext.Request;
        var headers = request.Headers;
        var actionDescriptor = context.ActionDescriptor as ControllerActionDescriptor;

        // 判断是否需要跳过
        if (!AppSettingsConfig.RecordRequestOptions.IsEnabled) isSkipRecord = true;
        if (actionDescriptor == null) isSkipRecord = true;
        if (AppSettingsConfig.RecordRequestOptions.IsSkipGetMethod && request.Method.ToUpper() == "GET") isSkipRecord = true;

        //如果没有请求记录属性则跳过
        var requestRecord = context.ContainsFilter<RequestRecordAttribute>();
        if (!requestRecord)
        {
#if !DEBUG
            isSkipRecord = true;
#endif
        }

        // 进入管道的下一个过滤器，并跳过剩下动作
        if (isSkipRecord)
        {
            await next();
            return;
        }

        request.Body.Seek(0, SeekOrigin.Begin);
        var reader = new StreamReader(request.Body, Encoding.UTF8);
        string body = await reader.ReadToEndAsync();
        request.Body.Seek(0, SeekOrigin.Begin);

        var sw = new Stopwatch();
        sw.Start();
        var actionContext = await next();
        sw.Stop();

        bool isSuccess = actionContext.Exception == null; // 没有异常即认为请求成功
        var clientInfo = headers.ContainsKey("User-Agent") ? Parser.GetDefault().Parse(headers["User-Agent"]) : null;
        string name = actionDescriptor == null ? "" : actionDescriptor.MethodInfo.GetSummary();

        string result = "";
        string message = "";

        // 目前只处理 ObjectResult
        if (actionContext.Result is ObjectResult objectResult)
        {
            // 正常接口都是 ObjectResult
            result = objectResult.Value.ToJson();
            if (objectResult.Value is AjaxResult appResult)
            {
                message = appResult.Msg;
            }
        }
        else if (actionContext.Result is ContentResult contentResult)
        {
            message = contentResult.StatusCode.ToString();
            result = contentResult.Content;
        }

        EventModel @event = null;
        //登录接口
        if (request.Path == "/Base_Manage/Home/SubmitLogin" || request.Path == "/Base_Manage/Home/SubmitLogout")
        {
            @event = new VisitEvent()
            {
                Name = name,
                Message = message,
                CreatorId = _operator.UserId ?? _operator.LoginUserId,
                CreatorName = _operator.UserName ?? _operator.LoginUserName,
                TenantId = _operator.TenantId ?? _operator.LoginTenantId,
                IsSuccess = isSuccess,
                Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                OperatingSystem = clientInfo?.OS.Family + clientInfo?.OS.Major,
                Ip = httpContext.GetRequestIPv4(),
                Url = request.GetRequestUrlAddress(),
                Path = request.Path,
                ClassName = context.Controller.ToString(),
                MethodName = actionDescriptor?.ActionName,
                RequestMethod = request.Method,
                Body = body,
                Result = result,
                ElapsedTime = sw.ElapsedMilliseconds,
                OperatingTime = DateTime.Now,
            };
        }
        else if (requestRecord)
        {
            @event = new RequestEvent()
            {
                Name = name,
                Message = message,
                CreatorId = _operator.UserId,
                CreatorName = _operator.UserName,
                TenantId = _operator.TenantId,
                IsSuccess = isSuccess,
                Browser = clientInfo?.UA.Family + clientInfo?.UA.Major,
                OperatingSystem = clientInfo?.OS.Family + clientInfo?.OS.Major,
                Ip = httpContext.GetRequestIPv4(),
                Url = request.GetRequestUrlAddress(),
                Path = request.Path,
                ClassName = context.Controller.ToString(),
                MethodName = actionDescriptor?.ActionName,
                RequestMethod = request.Method,
                Body = body,
                Result = result,
                ElapsedTime = sw.ElapsedMilliseconds,
                OperatingTime = DateTime.Now,
            };
        }

        if (@event != null)
        {
            await _publisher.PublishAsync(@event);
        }
        if (result?.Length > 1000)
        {
            result = new string(result.Copy(0, 1000).ToArray());
            result += "......内容太长已忽略";
        }

        var testEventModel = new TestEvent() { Message = @$"方向:请求本系统
Url:{request.GetRequestUrlAddress()}
Time:{sw.ElapsedMilliseconds}ms
Method:{request.Method}
ContentType:{request.ContentType}
Body:{body}
Message:{message}
Result:{result}
"};
        await _publisher.PublishAsync(testEventModel);
    }
}
