using Microsoft.AspNetCore.Mvc.Filters;

namespace Simple.Common.Filters;

/// <summary>
/// 异常过滤器
/// </summary>
public class ExceptionFilter : ExceptionFilterAttribute
{
    public override void OnException(ExceptionContext context)
    {

    }

    public override Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }
}
