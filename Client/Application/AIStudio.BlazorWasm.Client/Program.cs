using AIStudio.BlazorUI.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
//builder.RootComponents.Add<AIStudio.BlazorWasm.Client.App>("#app");
builder.RootComponents.Add<AIStudio.BlazorUI.Main>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddServices(builder.Configuration);    // ��2�⣺�����չ�����������
await builder.Build().RunAsync();
