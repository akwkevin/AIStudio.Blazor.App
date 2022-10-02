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
        if (context.Result != null || actionContext.Result != null) return;

        if (actionContext.Exception is AjaxResultException resultException)
        {
            // 如果是结果异常，处理成返回结果，并标记异常已处理
            actionContext.Result = _options.ResultFactory(resultException);
            actionContext.ExceptionHandled = true;
        }
    }
}
