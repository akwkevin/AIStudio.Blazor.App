using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Service
{
    public static class ServiceLocator
    {
        public static IServiceProvider Instance { get; set; }

        /// <summary>
        /// 获取请求上下文
        /// </summary>
        public static HttpContext HttpContext => Instance?.GetService<IHttpContextAccessor>()?.HttpContext;

        /// <summary>
        /// 获取请求上下文用户,只有授权访问的页面或接口才存在值，否则为 null
        /// </summary>
        public static ClaimsPrincipal User => HttpContext?.User;
    }
}
