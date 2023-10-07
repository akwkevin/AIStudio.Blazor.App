using AIStudio.Common.Service;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Filter
{
    /// <summary>
    /// 
    /// </summary>
    public class CheckAppKeyPermissionAttribute :Attribute, IActionFilter
    {
        /// <summary>
        /// 
        /// </summary>
        public CheckAppKeyPermissionAttribute()
        {
           
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuting(ActionExecutingContext context)
        {
            var request = context.HttpContext.Request;

            if (!string.IsNullOrEmpty(AppSettings.AppSettingsConfig.JwtOptions.SecretKey))
            {
                string appKey = request.Headers["appkey"].ToString();
                if (string.IsNullOrEmpty(appKey))
                {
                    ReturnError("缺少appkey参数");
                    return;
                }

                bool hasPermission = appKey == AppSettings.AppSettingsConfig.JwtOptions.SecretKey;
                if (hasPermission)
                    return;
                else
                {
                    ReturnError("权限不足，访问失败！");
                    return;
                }
            }

            void ReturnError(string msg)
            {
                AjaxResult res = new AjaxResult
                {
                    Success = false,
                    Msg = msg
                };

                context.Result = new ContentResult { Content = res.ToJson(), ContentType = "application/json;charset=utf-8" };
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public void OnActionExecuted(ActionExecutedContext context)
        {
            
        }
    }
}
