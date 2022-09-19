using AIStudio.Blazor.UI.Models;
using AIStudio.Blazor.UI.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;

namespace AIStudio.BlazorMaui.Client
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();
#if DEBUG
		    builder.Services.AddBlazorWebViewDeveloperTools();
#endif

            builder.Services.AddHttpClient();    

            var config =  new ConfigurationBuilder().AddJsonFile("wwwroot/appsettings.json").Build();
            builder.Configuration.AddConfiguration(config);
            builder.Services.AddServices(builder.Configuration);    // 第2外：添加扩展方法引入服务

            return builder.Build();
        }
    }
}