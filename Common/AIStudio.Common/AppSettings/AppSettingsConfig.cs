using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.Common.AppSettings
{
    /// <summary>
    /// 全局静态配置
    /// </summary>
    public static class AppSettingsConfig
    {
        private static IConfiguration? _configuration;

        /// <summary>
        /// 获取 Configuration 的单例
        /// </summary>
        public static IConfiguration Configuration
        {
            get
            {
                if (_configuration == null) throw new NullReferenceException(nameof(Configuration));
                return _configuration;
            }
        }

        [Obsolete("这只是一个示例，请使用 Configure(IConfiguration) 方法")]
        public static void Configure(IServiceProvider serviceProvider)
        {
            if (serviceProvider == null) throw new ArgumentNullException(nameof(serviceProvider));

            var configuration = serviceProvider.GetService<IConfiguration>();

            Configure_(configuration);
        }

        /// <summary>
        /// 设置 Configuration 的实例
        /// </summary>
        /// <param name="configuration"></param>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentNullException"></exception>
        public static void Configure_(IConfiguration? configuration)
        {
            if (_configuration != null)
            {
                throw new Exception($"{nameof(Configuration)}不可修改！");
            }
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// 记录请求
        /// </summary>
        public static class RecordRequestOptions
        {
            public static bool IsEnabled => Configuration.GetValue<bool>("RecordRequest:IsEnabled");
            public static bool IsSkipGetMethod => Configuration.GetValue<bool>("RecordRequest:IsSkipGetMethod");
        }

        public static class AppSettingsOptions
        {
            public static bool InjectMiniProfiler => Configuration.GetValue<bool>("AppSettings:InjectMiniProfiler");

            public static bool SuperAdminViewAllData => Configuration.GetValue<bool>("AppSettings:SuperAdminViewAllData");
        }

        /// <summary>
        /// 允许跨域请求列表
        /// </summary>
        public static string[] AllowCors => Configuration.GetSection("AllowCors").Get<string[]>();

        /// <summary>
        /// Jwt 配置
        /// </summary>
        public static class JwtOptions
        {
            public static string SecretKey => Configuration["Jwt:SecretKey"];
            public static string Issuer => Configuration["Jwt:Issuer"];
            public static string Audience => Configuration["Jwt:Audience"];
            public static double AccessExpireHours => Configuration.GetValue<double>("Jwt:AccessExpireHours");
            public static double RefreshExpireHours => Configuration.GetValue<double>("Jwt:RefreshExpireHours");
        }

        /// <summary>
        /// Redis 配置
        /// </summary>
        public static class RedisOptions
        {
            public static bool Enabled => Configuration.GetValue<bool>("Redis:Enabled");
            public static string ConnectionString => Configuration["Redis:ConnectionString"];
            public static string Instance => Configuration["Redis:Instance"] ?? "Default";
        }

        public static class ConnectionStringsOptions
        {
            /// <summary>
            /// 默认数据库编号
            /// </summary>
            public static string DefaultDbNumber => Configuration["ConnectionStrings:DefaultDbNumber"];
            /// <summary>
            /// 默认数据库类型
            /// </summary>
            public static string DefaultDbType => Configuration["ConnectionStrings:DefaultDbType"];
            /// <summary>
            /// 默认数据库连接字符串
            /// </summary>

            public static string DefaultDbString => Configuration["ConnectionStrings:DefaultDbString"];

            public static string DefaultDbName => Configuration["ConnectionStrings:DefaultDbName"];
            /// <summary>
            /// 业务库集合
            /// </summary>
            public static List<DbConfig> DbConfigs => Configuration.GetValue<List<DbConfig>>("ConnectionStrings:DbConfigs");
        }

        public static class SnowIdOptions
        {
            public static int WorkerId => Configuration.GetValue<int>("SnowId:WorkerId");
        }
    }
}
