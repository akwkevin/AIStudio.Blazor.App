using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Filter.FilterException;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Options;
using Simple.Common;

namespace AIStudio.Common.Filter;

/// <summary>
/// 
/// </summary>
/// <seealso cref="System.Attribute" />
/// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IAsyncActionFilter" />
/// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter" />
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method, AllowMultiple = true, Inherited = true)]
public class AjaxResultActionFilter : Attribute, IAsyncActionFilter, IOrderedFilter
{
    /// <summary>
    /// The options
    /// </summary>
    private readonly AjaxResultOptions _options;

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
    public int Order { get; set; } = -6000;

    /// <summary>
    /// Initializes a new instance of the <see cref="AjaxResultActionFilter"/> class.
    /// </summary>
    /// <param name="options">The options.</param>
    public AjaxResultActionFilter(IOptions<AjaxResultOptions> options)
    {
        _options = options.Value;
    }

    /// <summary>
    /// Called asynchronously before the action, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
    /// <param name="next">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutionDelegate" />. Invoked to execute the next action filter or the action itself.</param>
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
    public static ContentResult JsonContent(string json)
    {
        return new ContentResult { Content = json, StatusCode = 200, ContentType = "application/json; charset=utf-8" };
    }

    /// <summary>
    /// 返回成功
    /// </summary>
    /// <returns></returns>
    public static ObjectResult Success()
    {
        AjaxResult res = new AjaxResult
        {
            Success = true,
            Msg = "请求成功！"
        };

        return new ObjectResult(res);
    }

    /// <summary>
    /// 返回成功
    /// </summary>
    /// <param name="msg">消息</param>
    /// <returns></returns>
    public static ObjectResult Success(string msg)
    {
        AjaxResult res = new AjaxResult
        {
            Success = true,
            Msg = msg
        };

        return new ObjectResult(res);
    }

    /// <summary>
    /// 返回成功
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="data">返回的数据</param>
    /// <returns></returns>
    public static ObjectResult Success<T>(T data)
    {
        AjaxResult<T> res = new AjaxResult<T>
        {
            Success = true,
            Msg = "请求成功！",
            Data = data
        };

        return new ObjectResult(res);
    }

    /// <summary>
    /// 返回错误
    /// </summary>
    /// <returns></returns>
    public static ObjectResult Error()
    {
        AjaxResult res = new AjaxResult
        {
            Success = false,
            Msg = "请求失败！"
        };

        return new ObjectResult(res);
    }

    /// <summary>
    /// 返回错误
    /// </summary>
    /// <param name="msg">错误提示</param>
    /// <returns></returns>
    public static ObjectResult Error(string msg)
    {
        AjaxResult res = new AjaxResult
        {
            Success = false,
            Msg = msg,
        };

        return new ObjectResult(res);
    }

    /// <summary>
    /// 返回错误
    /// </summary>
    /// <param name="msg">错误提示</param>
    /// <param name="errorCode">错误代码</param>
    /// <returns></returns>
    public static ObjectResult Error(string msg, int errorCode)
    {
        AjaxResult res = new AjaxResult
        {
            Success = false,
            Msg = msg,
            Code = errorCode
        };

        return new ObjectResult(res);
    }
}
