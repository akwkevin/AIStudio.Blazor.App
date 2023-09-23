using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Filter.FilterAttribute
{
    /// <summary>
    /// 忽略请求记录
    /// </summary>
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class IgnoreRequestRecordAttribute : Attribute, IActionFilter
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
}
