using Microsoft.AspNetCore.Mvc.Filters;

namespace Simple.Common.Filters;

/// <summary>
/// 异常过滤器
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.ExceptionFilterAttribute" />
public class ExceptionFilter : ExceptionFilterAttribute
{
    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <inheritdoc />
    public override void OnException(ExceptionContext context)
    {

    }

    /// <summary>
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    /// <inheritdoc />
    public override Task OnExceptionAsync(ExceptionContext context)
    {
        OnException(context);
        return Task.CompletedTask;
    }
}
