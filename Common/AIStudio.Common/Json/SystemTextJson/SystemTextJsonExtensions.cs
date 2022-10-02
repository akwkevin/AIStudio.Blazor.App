using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.Common.Json.SystemTextJson
{
    public static class SystemTextJsonExtensions
    {
        /// <summary>
        /// 添加默认 Json 序列化/反序列化 配置
        /// </summary>
        /// <param name="services"></param>
        /// <returns></returns>
        public static IMvcBuilder AddTextJsonOptions(this IMvcBuilder builder, Action<JsonSerializerOptions>? setupAction = null)
        {
            //全局配置Json序列化处理
            builder.AddJsonOptions(options =>
            {
                 // 驼峰命名
                 options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

                 // Unicode 编码
                 options.JsonSerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

                 // 忽略循环引用
                 // https://docs.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json-preserve-references
                 options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

                 // 自定义 Converter
                 options.JsonSerializerOptions.Converters.Add(new DateTimeJsonConverter());
                 options.JsonSerializerOptions.Converters.Add(new EnumJsonConverter());

                // 如果传入自定义配置
                if (setupAction != null) setupAction(options.JsonSerializerOptions);
            });

            return builder;
        }
    }
}
