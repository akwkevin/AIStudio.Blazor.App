using AIStudio.Common.AppSettings;
using Microsoft.AspNetCore.Cors.Infrastructure;
using Microsoft.Extensions.Configuration;

namespace Microsoft.Extensions.DependencyInjection;

/// <summary>
/// 
/// </summary>
public static class CorsServiceCollectionExtensions
{
    /// <summary>
    /// 添加默认跨域配置
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="setupAction">The setup action.</param>
    /// <returns></returns>
    public static IServiceCollection AddCors_(this IServiceCollection services, Action<CorsOptions>? setupAction = null)
    {
        services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                if(AppSettingsConfig.AllowCors.Any(c => c == "*"))
                {
                    // 允许任意跨域
                    policy.AllowAnyOrigin()
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .DisallowCredentials();
                }
                else
                {
                    // 允许指定域名
                    policy.WithOrigins(AppSettingsConfig.AllowCors)
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .DisallowCredentials();
                }
            });
        });


        // 自定义配置
        if (setupAction != null)
        {
            services.Configure<CorsOptions>(setupAction);
        }

        return services;
    }
}
