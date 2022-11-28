using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Diagnostics;

namespace AIStudio.Common.Filter;

/// <summary>
/// 
/// </summary>
/// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IActionFilter" />
/// <seealso cref="Microsoft.AspNetCore.Mvc.Filters.IOrderedFilter" />
public class DataValidationFilter : IActionFilter, IOrderedFilter
{
    /// <summary>
    /// The filter order
    /// </summary>
    internal const int FilterOrder = -2000;

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
    public int Order => FilterOrder;

    /// <summary>
    /// Called before the action executes, after model binding is complete.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutingContext" />.</param>
    public void OnActionExecuting(ActionExecutingContext context)
    {
        // 如果其他过滤器已经设置了结果，则跳过验证
        if (context.Result != null) return;

        // 获取模型验证状态
        var modelState = context.ModelState;

        // 如果验证通过，跳过后面的动作
        if (modelState.IsValid) return;

        // 获取失败的验证信息列表
        var errors = modelState.Where(s => s.Value != null && s.Value.ValidationState == ModelValidationState.Invalid)
                               .SelectMany(s => s.Value!.Errors.ToList())
                               .Select(e => e.ErrorMessage)
                               .ToArray();

        // 统一返回
        var result = AjaxResult.Status400BadRequest($"数据验证不通过:{ string.Join(",", errors)}");

        // 设置结果
        context.Result = new ObjectResult(result);
    }

    /// <summary>
    /// Called after the action executes, before the action result.
    /// </summary>
    /// <param name="context">The <see cref="T:Microsoft.AspNetCore.Mvc.Filters.ActionExecutedContext" />.</param>
    public void OnActionExecuted(ActionExecutedContext context)
    {
    }
}
