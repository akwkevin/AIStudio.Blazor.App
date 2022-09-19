using AIStudio.Blazor.UI;
using AIStudio.Blazor.UI.Services;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace AIStudio.BlazorWasm.Client
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<Main>("#app");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });
           
            builder.Services.AddServices(builder.Configuration);    // 第2外：添加扩展方法引入服务

            await builder.Build().RunAsync();
        }
    }
}