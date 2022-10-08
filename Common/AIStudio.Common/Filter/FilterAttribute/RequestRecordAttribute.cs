using Microsoft.AspNetCore.Mvc.Filters;

namespace AIStudio.Common.Filter.FilterAttribute;

/// <summary>
/// 请求记录属性
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class RequestRecordAttribute : Attribute, IActionFilter
{
    /// <summary>
    /// Action执行之前执行
    /// </summary>
    /// <param name="context">过滤器上下文</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {

    }

    /// <summary>
    /// Action执行完毕之后执行
    /// </summary>
    /// <param name="context"></param>
    public void OnActionExecuted(ActionExecutedContext context)
    {

    }
}
