using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Result
{
    /// <summary>
    /// 返回结果不进行格式化
    /// </summary>
    public class NoFormatResponseAttribute : Attribute, IActionFilter
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

    public static class FilterExtentions
    {
        /// <summary>
        /// 是否拥有某过滤器
        /// </summary>
        /// <typeparam name="T">过滤器类型</typeparam>
        /// <param name="actionExecutingContext">上下文</param>
        /// <returns></returns>
        public static bool ContainsFilter<T>(this FilterContext actionExecutingContext)
        {
            return actionExecutingContext.Filters.Any(x => x.GetType() == typeof(T));
        }
    }
}
