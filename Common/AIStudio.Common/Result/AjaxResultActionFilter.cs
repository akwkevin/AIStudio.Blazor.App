using AIStudio.Common.CustomAttribute;
using AIStudio.Common.Json.SystemTextJson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Simple.Common;

namespace AIStudio.Common.Result;

[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AjaxResultActionFilter : Attribute, IAsyncActionFilter, IOrderedFilter
{
    private readonly AjaxResultOptions _options;

    public int Order { get; set; } = -6000;

    public AjaxResultActionFilter(IOptions<AjaxResultOptions> options)
    {
        _options = options.Value;
    }

    public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
    {
        var actionContext = await next();

        // 如果已经设置了结果，则直接返回
        if (context.Result != null || actionContext.Result != null)
        {
            //若Action返回对象为自定义对象,则将其转为JSON
            if (context.ContainsFilter<NoFormatResponseAttribute>())
                return;
            if (actionContext.Result is EmptyResult)
                actionContext.Result = Success();
            else if (actionContext.Result is ObjectResult res)
            {
                if (res.Value is AjaxResult)
                    actionContext.Result = JsonContent(res.Value.ToJson());
                else
                    actionContext.Result = Success(res.Value);
            }

            return;
        }
        if (actionContext.Exception is AjaxResultException resultException)
        {
            // 如果是结果异常，处理成返回结果，并标记异常已处理
            actionContext.Result = _options.ResultFactory(resultException);
            actionContext.ExceptionHandled = true;
        }
    }

    /// <summary>
    /// 返回JSON
    /// </summary>
    /// <param name="json">json字符串</param>
    /// <returns></returns>
    public ContentResult JsonContent(string json)
    {
        return new ContentResult { Content = json, StatusCode = 200, ContentType = "application/json; charset=utf-8" };
    }

    /// <summary>
    /// 返回成功
    /// </summary>
    /// <returns></returns>
    public ContentResult Success()
    {
        AjaxResult res = new AjaxResult
        {
            Success = true,
            Msg = "请求成功！"
        };

        return JsonContent(res.ToJson());
    }

    /// <summary>
    /// 返回成功
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public ContentResult Success(string msg)
    {
        AjaxResult res = new AjaxResult
        {
            Success = true,
            Msg = msg
        };

        return JsonContent(res.ToJson());
    }

    /// <summary>
    /// 返回成功
    /// </summary>
    /// <param name="data">返回的数据</param>
    /// <returns></returns>
    public ContentResult Success<T>(T data)
    {
        AjaxResult<T> res = new AjaxResult<T>
        {
            Success = true,
            Msg = "请求成功！",
            Data = data
        };

        return JsonContent(res.ToJson());
    }

    /// <summary>
    /// 返回错误
    /// </summary>
    /// <returns></returns>
    public ContentResult Error()
    {
        AjaxResult res = new AjaxResult
        {
            Success = false,
            Msg = "请求失败！"
        };

        return JsonContent(res.ToJson());
    }

    /// <summary>
    /// 返回错误
    /// </summary>
    /// <param name="msg">错误提示</param>
    /// <returns></returns>
    public ContentResult Error(string msg)
    {
        AjaxResult res = new AjaxResult
        {
            Success = false,
            Msg = msg,
        };

        return JsonContent(res.ToJson());
    }

    /// <summary>
    /// 返回错误
    /// </summary>
    /// <param name="msg">错误提示</param>
    /// <param name="errorCode">错误代码</param>
    /// <returns></returns>
    public ContentResult Error(string msg, int errorCode)
    {
        AjaxResult res = new AjaxResult
        {
            Success = false,
            Msg = msg,
            Code = errorCode
        };

        return JsonContent(res.ToJson());
    }
}
