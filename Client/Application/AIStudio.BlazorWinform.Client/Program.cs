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
            //δʹ������ע���д��
            //Application.Run(new FormMain());

            //���� DI ����
            ServiceCollection = new ServiceCollection();
            ConfigureServices(ServiceCollection);  //ע����ַ�����

            //����DI�������� serviceProvider, Ȼ��ͨ�� serviceProvider ��ȡMain Form��ע��ʵ��
            var serviceProvider = ServiceCollection.BuildServiceProvider();

            var formMain = serviceProvider.GetRequiredService<FormMain>();   //�����������л�ȡFormMainʵ��, ���Ǽ��д��
                                                                             // var formMain = (FormMain)serviceProvider.GetService(typeof(FormMain));  //��������д��
            Application.Run(formMain);
        }

        /// <summary>
        /// ��DI������ע�����еķ������� 
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

            services.AddServices(configuration);    // ��2�⣺�����չ�����������

            //ע�� FormMain ��
            services.AddScoped<FormMain>();

        }
    }
}