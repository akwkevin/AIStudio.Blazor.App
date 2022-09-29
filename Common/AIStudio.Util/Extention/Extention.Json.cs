using System.Data;


namespace AIStudio.Util
{
    public static partial class Extention
    {
        //private static JsonSerializerOptions defaultSerializerSettings = new JsonSerializerOptions();

        static Extention()
        {
            //设置时间格式
            //defaultSerializerSettings.Converters.Add(new DateTimeJsonConverter("yyyy-MM-dd HH:mm:ss"));
            //忽略大小写
            //defaultSerializerSettings.PropertyNameCaseInsensitive = true;
            //获取或设置要在转义字符串时使用的编码器
            //defaultSerializerSettings.Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping;

            Newtonsoft.Json.JsonSerializerSettings setting = new Newtonsoft.Json.JsonSerializerSettings();
            Newtonsoft.Json.JsonConvert.DefaultSettings = new Func<Newtonsoft.Json.JsonSerializerSettings>(() =>
            {
                //日期类型默认格式化处理
                setting.DateFormatHandling = Newtonsoft.Json.DateFormatHandling.MicrosoftDateFormat;
                setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";
                setting.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;
                setting.ContractResolver = new Newtonsoft.Json.Serialization.DefaultContractResolver();
                return setting;
            });
        }


        /// <summary>
        /// 将Json字符串转为List'T'
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr"></param>
        /// <returns></returns>
        public static List<T> ToList<T>(this string jsonStr)
        {
            //return string.IsNullOrEmpty(jsonStr) ? null : JsonSerializer.Deserialize<List<T>>(jsonStr, defaultSerializerSettings);
            return string.IsNullOrEmpty(jsonStr) ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<List<T>>(jsonStr);
        }

        /// <summary>
        /// 将Json字符串转为DataTable
        /// </summary>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        public static DataTable ToDataTable(this string jsonStr)
        {
            //return jsonStr == null ? null : JsonSerializer.Deserialize<DataTable>(jsonStr, defaultSerializerSettings);
            return jsonStr == null ? null : Newtonsoft.Json.JsonConvert.DeserializeObject<DataTable>(jsonStr);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        public static string ToJson(this object obj)
        {
            //return JsonSerializer.Serialize(obj, defaultSerializerSettings);
            return Newtonsoft.Json.JsonConvert.SerializeObject(obj);
        }

        /// <summary>
        /// 将对象序列化成Json字符串
        /// </summary>
        /// <param name="obj">需要序列化的对象</param>
        /// <returns></returns>
        //public static string ToJson(this object obj, JsonSerializerOptions settings)
        //{
        //    return JsonSerializer.Serialize(obj, settings);
        //}


        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        public static object ToObject(this string jsonStr, Type type)
        {
            //return JsonSerializer.Deserialize(jsonStr, type, defaultSerializerSettings);
            return Newtonsoft.Json.JsonConvert.DeserializeObject(jsonStr, type);
        }

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <param name="jsonStr">json字符串</param>
        /// <param name="type">对象类型</param>
        /// <returns></returns>
        //public static object ToObject(this string jsonStr, Type type, JsonSerializerOptions settings)
        //{
        //    return JsonSerializer.Deserialize(jsonStr, type, settings);
        //}

        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        //public static T ToObject<T>(this string jsonStr)
        //{
        //    return JsonSerializer.Deserialize<T>(jsonStr, defaultSerializerSettings);
        //}

        public static T ToObject<T>(this string jsonStr)
        {
            return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(jsonStr);
        }


        /// <summary>
        /// 将Json字符串反序列化为对象
        /// </summary>
        /// <typeparam name="T">对象类型</typeparam>
        /// <param name="jsonStr">Json字符串</param>
        /// <returns></returns>
        //public static T ToObject<T>(this string jsonStr, JsonSerializerOptions settings)
        //{
        //    return JsonSerializer.Deserialize<T>(jsonStr, settings);
        //}
    }

    /// <summary>
    /// JsonResult 格式化时间数据
    /// 默认 "yyyy-MM-dd HH:mm:ss"
    /// </summary>
    //public class DateTimeJsonConverter : System.Text.Json.Serialization.JsonConverter<DateTime>
    //{
    //    private readonly string _dateFormatString;
    //    public DateTimeJsonConverter(string format = "yyyy-MM-dd HH:mm:ss")
    //    {
    //        _dateFormatString = format;
    //    }
    //    public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
    //    {
    //        writer.WriteStringValue(value.ToUniversalTime().ToString(_dateFormatString));
    //    }

    //    public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    //    {
    //        return DateTime.Parse(reader.GetString());
    //    }
    //}
}
