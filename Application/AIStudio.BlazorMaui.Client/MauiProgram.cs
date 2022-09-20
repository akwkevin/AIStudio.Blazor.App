using AIStudio.Blazor.UI.Models;
using AIStudio.Blazor.UI.Models.Charts;
using AIStudio.Blazor.UI.Services;
using AIStudio.Util;
using AIStudio.Util.Helper;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Configuration;
using System.Reflection;

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

            //方法1：只适合window平台，并且wwwroot/appsettings.json需要为内容
            //var config = new ConfigurationBuilder().AddJsonFile("wwwroot/appsettings.json").Build();

            //方法2：读取后写到默认地址，wwwroot/appsettings.json为嵌入的资源           
            //var assembly = Assembly.GetExecutingAssembly();
            //var stream = assembly.GetManifestResourceStream("AIStudio.BlazorMaui.Client.wwwroot.appsettings.json");
            //string text = "";
            //using (var reader = new System.IO.StreamReader(stream))
            //{
            //    text = reader.ReadToEnd();
            //}

            //string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "wwwroot/appsettings.json");
            //if (DirFileHelper.IsExistFile(path))
            //{
            //    var currentFileContent = DirFileHelper.ReadFile(path);
            //    var isSameContent = currentFileContent.ToMd5() == text.ToMd5();
            //    if (!isSameContent)
            //    {
            //        DirFileHelper.CreateFile(path, text);
            //    }
            //}
            //else
            //{
            //    DirFileHelper.CreateFile(path, text);

            //}
            //var config = new ConfigurationBuilder().AddJsonFile(path).Build();

            //方法3：直接读流，wwwroot/appsettings.json为嵌入的资源           
            var assembly = Assembly.GetExecutingAssembly();
            var stream = assembly.GetManifestResourceStream("AIStudio.BlazorMaui.Client.wwwroot.appsettings.json");
            var config =  new ConfigurationBuilder().AddJsonStream(stream).Build();
            builder.Configuration.AddConfiguration(config);
            builder.Services.AddServices(builder.Configuration);    // 第2外：添加扩展方法引入服务

            return builder.Build();
        }
    }
}