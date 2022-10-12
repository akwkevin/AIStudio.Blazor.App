using AIStudio.Common.AppSettings;
using AIStudio.Util;
using AIStudio.Util.Mapper;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Mapper
{
    public static class MiniProfilerServiceCollectionExtensions
    {    /// <summary>
         /// 添加 MiniProfiler 配置
         /// </summary>
         /// <param name="services">服务集合</param>
        public static IServiceCollection AddMiniProfiler_(this IServiceCollection services)
        {
            // 注册MiniProfiler 组件
            if (AppSettingsConfig.AppSettingsOptions.InjectMiniProfiler == true)
            {
                services.AddMiniProfiler(options => options.RouteBasePath = "/profiler");
            }
            return services;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="app"></param>
        /// <returns></returns>
        public static IApplicationBuilder UseMiniProfiler_(this IApplicationBuilder app)
        { 
            // 启用MiniProfiler 组件
            if (AppSettingsConfig.AppSettingsOptions.InjectMiniProfiler == true)
            {
                app.UseMiniProfiler();
            }
            return app;
        }
    }
}
