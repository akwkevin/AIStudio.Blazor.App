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
    public static class TextJsonHelper
    {        /// <summary>
             /// 获取序列化/反序列化Json的配置
             /// </summary>
        public static JsonSerializerOptions SerializerOptions = new JsonSerializerOptions();

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

        public static string Serialize<T>(T value)
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

        public static T? Deserialize<T>(ReadOnlySpan<byte> utf8Json)
        {
            if (utf8Json == null)
            {
                return default(T);
            }
            return JsonSerializer.Deserialize<T>(utf8Json, SerializerOptions);
        }

        ///// <summary>
        ///// 将对象序列化成Json字符串
        ///// </summary>
        ///// <param name="obj">需要序列化的对象</param>
        ///// <returns></returns>
        //public static string ToJson(this object obj)
        //{
        //    return JsonSerializer.Serialize(obj, SerializerOptions);
        //}

        ///// <summary>
        ///// 将对象序列化成Json字符串
        ///// </summary>
        ///// <param name="obj">需要序列化的对象</param>
        ///// <returns></returns>
        //public static string ToJson(this object obj, JsonSerializerOptions settings)
        //{
        //    return JsonSerializer.Serialize(obj, settings);
        //}

        ///// <summary>
        ///// 将Json字符串反序列化为对象
        ///// </summary>
        ///// <param name="jsonStr">json字符串</param>
        ///// <param name="type">对象类型</param>
        ///// <returns></returns>
        //public static object ToObject(this string jsonStr, Type type, JsonSerializerOptions settings)
        //{
        //    return JsonSerializer.Deserialize(jsonStr, type, settings);
        //}

        ///// <summary>
        ///// 将Json字符串反序列化为对象
        ///// </summary>
        ///// <typeparam name="T">对象类型</typeparam>
        ///// <param name="jsonStr">Json字符串</param>
        ///// <returns></returns>
        //public static T ToObject<T>(this string jsonStr)
        //{
        //    return JsonSerializer.Deserialize<T>(jsonStr, SerializerOptions);
        //}

        ///// <summary>
        ///// 将Json字符串反序列化为对象
        ///// </summary>
        ///// <typeparam name="T">对象类型</typeparam>
        ///// <param name="jsonStr">Json字符串</param>
        ///// <returns></returns>
        //public static T ToObject<T>(this string jsonStr, JsonSerializerOptions settings)
        //{
        //    return JsonSerializer.Deserialize<T>(jsonStr, settings);
        //}
    }
}
