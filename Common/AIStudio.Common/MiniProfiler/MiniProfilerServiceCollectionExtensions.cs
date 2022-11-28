using AIStudio.Common.AppSettings;
using AIStudio.Util;
using AIStudio.Util.Mapper;
using AutoMapper;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using StackExchange.Profiling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Mapper
{
    /// <summary>
    /// 
    /// </summary>
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

        /// <summary>
        /// 打印信息到 MiniProfiler
        /// </summary>
        /// <param name="category">分类</param>
        /// <param name="state">状态</param>
        /// <param name="message">消息</param>
        /// <param name="isError">是否为警告消息</param>
        public static void PrintToMiniProfiler(string category, string state, string message = null, bool isError = false)
        {
            if (!CanBeMiniProfiler()) return;

            // 打印消息
            var titleCaseCategory = Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(category);
            var customTiming = MiniProfiler.Current?.CustomTiming(category, string.IsNullOrWhiteSpace(message) ? $"{titleCaseCategory} {state}" : message, state);
            if (customTiming == null) return;

            // 判断是否是警告消息
            if (isError) customTiming.Errored = true;
        }

        /// <summary>
        /// 判断是否启用 MiniProfiler
        /// </summary>
        /// <returns></returns>
        internal static bool CanBeMiniProfiler()
        {
            // 减少不必要的监听
            if (AppSettingsConfig.AppSettingsOptions.InjectMiniProfiler != true) return false;

            return true;
        }
    }
}
