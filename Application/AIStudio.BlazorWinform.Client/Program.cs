using AIStudio.Blazor.UI.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System.Configuration;

namespace AIStudio.BlazorWinform.Client
{
    internal static class Program
    {
        public static ServiceCollection ServiceCollection;
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();
            //未使用依赖注入的写法
            //Application.Run(new FormMain());

            //生成 DI 容器
            ServiceCollection = new ServiceCollection();
            ConfigureServices(ServiceCollection);  //注册各种服务类

            //先用DI容器生成 serviceProvider, 然后通过 serviceProvider 获取Main Form的注册实例
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var formMain = serviceProvider.GetRequiredService<FormMain>();   //主动从容器中获取FormMain实例, 这是简洁写法
                                                                             // var formMain = (FormMain)serviceProvider.GetService(typeof(FormMain));  //更繁琐的写法
            Application.Run(formMain);
        }

        /// <summary>
        /// 在DI容器中注册所有的服务类型 
        /// </summary>
        /// <param name="services"></param>
        private static void ConfigureServices(ServiceCollection services)
        {
            var builder = new ConfigurationBuilder()
              .SetBasePath(Directory.GetCurrentDirectory())
              .AddJsonFile("wwwroot/appsettings.json", optional: false, reloadOnChange: true);

            var configuration = builder.Build();
            services.AddSingleton<IConfiguration>(configuration);

            services.AddWindowsFormsBlazorWebView();
#if DEBUG
            services.AddBlazorWebViewDeveloperTools();
#endif          

            services.AddServices(configuration);    // 第2外：添加扩展方法引入服务

            //注册 FormMain 类
            services.AddScoped<FormMain>();

        }
    }
}