using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Swagger
{
    /// <summary>
    /// api版本号
    /// </summary>
    public class ApiVersionInfo
    {
        public static OpenApiInfo V1 = new OpenApiInfo() { Title = "AIStudio Api V1（2022-10-02）", Description = "AIStudio Api V1 Demo版本", Version = "V1" };
        public static OpenApiInfo V2 = new OpenApiInfo() { Title = "AIStudio Api V2", Description = "AIStudio Api V2", Version = "V2" };
        public static OpenApiInfo V3 = new OpenApiInfo() { Title = "AIStudio Api V3", Description = "AIStudio Api V3", Version = "V3" };
        public static OpenApiInfo V4 = new OpenApiInfo() { Title = "AIStudio Api V4", Description = "AIStudio Api V4", Version = "V2" };
        public static OpenApiInfo V5 = new OpenApiInfo() { Title = "AIStudio Api V5", Description = "AIStudio Api V5", Version = "V2" };
        public static OpenApiInfo Test = new OpenApiInfo() { Title = "AIStudio Api Test", Description = "AIStudio Api 测试版本，测试jwt,测试数据校验", Version = "Test" };

        public static Dictionary<string, OpenApiInfo> GetFieldValues()
        {
            return typeof(ApiVersionInfo)
                      .GetFields(BindingFlags.Public | BindingFlags.Static)
                      .Where(f => f.FieldType == typeof(OpenApiInfo))
                      .ToDictionary(f => f.Name,
                                    f => (OpenApiInfo)f.GetValue(null));
        }
    }
}
