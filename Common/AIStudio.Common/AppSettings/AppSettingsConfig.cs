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

        /// <summary>
        /// 
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <exception cref="ArgumentNullException"></exception>
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
            /// <summary>
            /// 请求消息记录
            /// </summary>
            public static bool IsEnabled => Configuration.GetValue<bool>("RecordRequest:IsEnabled");
            /// <summary>
            /// 跳过Get方法
            /// </summary>
            public static bool IsSkipGetMethod => Configuration.GetValue<bool>("RecordRequest:IsSkipGetMethod");
        }

        /// <summary>
        /// 系统设置
        /// </summary>
        public static class AppSettingsOptions
        {   
            /// <summary>
             /// 初始化数据
             /// </summary>
            public static bool SeedData => Configuration.GetValue<bool>("AppSettings:SeedData");

            /// <summary>
            /// 自动初始化数据库表
            /// </summary>
            public static bool CodeFirst => Configuration.GetValue<bool>("AppSettings:CodeFirst");


            /// <summary>
            /// 使用Https
            /// </summary>
            public static bool UseKestrel => Configuration.GetValue<bool>("AppSettings:UseKestrel");
            

            /// <summary>
            /// 开启InjectMiniProfiler
            /// </summary>
            public static bool InjectMiniProfiler => Configuration.GetValue<bool>("AppSettings:InjectMiniProfiler");
            
            /// <summary>
            /// 开启超级管理员查看全部数据
            /// </summary>
            public static bool SuperAdminViewAllData => Configuration.GetValue<bool>("AppSettings:SuperAdminViewAllData");

            /// <summary>
            /// 是否开启多租户
            /// </summary>
            public static bool MultiTenant => Configuration.GetValue<bool>("AppSettings:MultiTenant");

            /// <summary>
            /// 是否使用工作流
            /// </summary>
            public static bool UseWorkflow => Configuration.GetValue<bool>("AppSettings:UseWorkflow");
            
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
            /// <summary>
            /// SecretKey
            /// </summary>
            public static string SecretKey => Configuration["Jwt:SecretKey"];

            /// <summary>
            /// 刷新Key
            /// </summary>
            public static string RefreshSecretKey => Configuration["Jwt:RefreshSecretKey"];
            /// <summary>
            /// Issuer
            /// </summary>
            public static string Issuer => Configuration["Jwt:Issuer"];

            /// <summary>
            /// Audience
            /// </summary>
            public static string Audience => Configuration["Jwt:Audience"];
            /// <summary>
            /// AccessExpireHours
            /// </summary>
            public static double AccessExpireHours => Configuration.GetValue<double>("Jwt:AccessExpireHours");
            /// <summary>
            /// RefreshExpireHours
            /// </summary>
            public static double RefreshExpireHours => Configuration.GetValue<double>("Jwt:RefreshExpireHours");

            /// <summary>
            /// 
            /// </summary>
            public static long ClockSkew => Configuration.GetValue<long>("Jwt:ClockSkew");
            /// <summary>
            /// 
            /// </summary>
            public static long RefreshClockSkew => Configuration.GetValue<long>("Jwt:RefreshClockSkew");
        }

        /// <summary>
        /// Redis 配置
        /// </summary>
        public static class RedisOptions
        {
            /// <summary>
            /// Enabled
            /// </summary>
            public static bool Enabled => Configuration.GetValue<bool>("Redis:Enabled");
            /// <summary>
            /// ConnectionString
            /// </summary>
            public static string ConnectionString => Configuration["Redis:ConnectionString"];
            /// <summary>
            /// Instance
            /// </summary>
            public static string Instance => Configuration["Redis:Instance"] ?? "Default";
        }

        /// <summary>
        /// 数据库配置
        /// </summary>
        public static class ConnectionStringsOptions
        {
            /// <summary>
            /// 数据库集合
            /// </summary>
            public static DbConfig[] DbConfigs => Configuration.GetSection("DbConfigs").Get<DbConfig[]>();
        }

        /// <summary>
        /// 雪花ID
        /// </summary>
        public static class SnowIdOptions
        {
            /// <summary>
            /// IdHelper的WorkerId
            /// </summary>
            public static int WorkerId => Configuration.GetValue<int>("SnowId:WorkerId");
        }
    }
}
