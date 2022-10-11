using AIStudio.Common.AppSettings;
using AIStudio.Util;
using AIStudio.Util.Mapper;
using AutoMapper;
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
         /// <param name="configure">自定义配置</param>
        public static IServiceCollection AddMiniProfiler_(this IServiceCollection services)
        {
            // 注册MiniProfiler 组件
            if (AppSettingsConfig.AppSettingsOptions.InjectMiniProfiler == true)
            {
            
            }
            return services;
        }

     
    }
}
