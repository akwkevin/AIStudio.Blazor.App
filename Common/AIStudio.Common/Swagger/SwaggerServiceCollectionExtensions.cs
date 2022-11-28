using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Reflection;

namespace AIStudio.Common.Swagger;

/// <summary>
/// 
/// </summary>
public static class SwaggerServiceCollectionExtensions
{
    /// <summary>
    /// SwaggerGen
    /// </summary>
    /// <param name="services">The services.</param>
    /// <param name="setupAction">The setup action.</param>
    /// <returns></returns>
    public static IServiceCollection AddSwaggerGen_(this IServiceCollection services, Action<SwaggerGenOptions>? setupAction = null)
    {
        services.AddSwaggerGen(options =>
        {
            //版本控制
            foreach (var field in ApiVersionInfo.GetFieldValues())
            {
                options.SwaggerDoc(field.Key, field.Value);
            }

            var basePath = AppContext.BaseDirectory;

            // 获取根目录下，所有 xml 完整路径（注：并不会获取二级目录下的文件）
            var directoryInfo = new DirectoryInfo(basePath);
            List<string> xmls = directoryInfo
                .GetFiles()
                .Where(f => f.Name.ToLower().EndsWith(".xml"))
                .Select(f => f.FullName)
                .ToList();

            // 添加注释文档
            foreach (var xml in xmls)
            {
                options.IncludeXmlComments(xml);
            }

            //添加授权
            var schemeName = "Bearer";
            options.AddSecurityDefinition(schemeName, new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "请输入不带有Bearer的Token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                Scheme = schemeName.ToLowerInvariant(),
                BearerFormat = "JWT"
            });
            options.AddSecurityRequirement(new OpenApiSecurityRequirement {
            {
                new OpenApiSecurityScheme
                {
                    Reference = new OpenApiReference
                    {
                        Type = ReferenceType.SecurityScheme,
                        Id = schemeName
                    }
                },
                new string[0]
            }
        });

        });

        // 如果有自定义配置
        if (setupAction != null) services.Configure(setupAction);

        return services;
    }
}
