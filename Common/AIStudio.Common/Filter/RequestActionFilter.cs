using AIStudio.Common.AppSettings;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.EventHandlers;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Extensions;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Diagnostics;
using System.Text;
using UAParser;

namespace Simple.Common.Filters;

public class RequestActionFilter : IAsyncActionFilter, IOrderedFilter
{
    private readonly IEventPublisher _publisher;
    private readonly IOperator _operator;

    public int Order { get; set; } = -8000;

    public RequestActionFilter(IEventPublisher publisher, IOperator @operator)
    {
        _publisher = publisher;
        _operator = @operator;
    }

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

        if (isSkipRecord == false)
        {
            bool hasRequestRecord = false;           
            foreach (var metadata in actionDescriptor!.EndpointMetadata)
            {
                if (metadata is RequestRecordAttribute)
                {
                    hasRequestRecord = true;
                    break;
                }
            }

            if (hasRequestRecord == false)
            {
                isSkipRecord = true;
            }
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
            var appResult = contentResult.Content.ToObject<AjaxResult>();
            message = appResult?.Msg;
            result = contentResult.Content;
        }

        var @event = new RequestEvent()
        {
            Name = name,
            Message = message,
            Account = _operator.UserName,
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
            OperatingTime = DateTimeOffset.Now,
        };
        await _publisher.PublishAsync(@event);

        //var testEventModel = new TestEventModel() { Message = @event.ToJson() };
        //await _publisher.PublishAsync(testEventModel);
    }
}
