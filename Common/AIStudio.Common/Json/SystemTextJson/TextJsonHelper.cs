using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Text.Unicode;
using System.Threading.Tasks;

namespace AIStudio.Common.Json.SystemTextJson
{
    public class TextJsonHelper
    {
        static TextJsonHelper()
        {
            // 驼峰命名
            SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;

            // Unicode 编码
            SerializerOptions.Encoder = JavaScriptEncoder.Create(UnicodeRanges.All);

            // 忽略循环引用
            // https://docs.microsoft.com/zh-cn/dotnet/standard/serialization/system-text-json-preserve-references
            SerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;

            // 自定义 Converter
            SerializerOptions.Converters.Add(new DateTimeJsonConverter());
            SerializerOptions.Converters.Add(new EnumJsonConverter());
        }

        /// <summary>
        /// 获取序列化/反序列化Json的配置
        /// </summary>
        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions();



        public static string Serialize<TValue>(TValue value)
        {
            return JsonSerializer.Serialize(value, SerializerOptions);
        }

        public static byte[] SerializeToUtf8Bytes<TValue>(TValue value)
        {
            return JsonSerializer.SerializeToUtf8Bytes(value, SerializerOptions);
        }

        public static TValue? Deserialize<TValue>(string json, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize<TValue>(json, options ?? SerializerOptions);
        }

        public static object? Deserialize(string json, Type returnType, JsonSerializerOptions? options = null)
        {
            return JsonSerializer.Deserialize(json, returnType, options ?? SerializerOptions);
        }

        public static TValue? Deserialize<TValue>(ReadOnlySpan<byte> utf8Json)
        {
            if (utf8Json == null)
            {
                return default(TValue);
            }
            return JsonSerializer.Deserialize<TValue>(utf8Json, SerializerOptions);
        }
    }
}
