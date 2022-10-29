using AIStudio.BlazorUI.Converter;
using AIStudio.BlazorUI.Models.Settings;
using AIStudio.BlazorUI.Services.Auth;
using AIStudio.Client.Business;
using AntDesign;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.BlazorUI.Services
{
    public static class ServiceCollectionExtensions
    {
        public static void AddServices(this IServiceCollection services, Microsoft.Extensions.Configuration.IConfiguration configuration)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            //services.Configure<ProSettings>(configuration.GetSection("ProSettings"));
            services.Configure<LayoutSettings>(configuration.GetSection("LayoutSettings"));

            services.AddAntDesign();   // 这句关键代码

            services.AddScoped<ModalService>();
            services.AddScoped<MessageService>();

            services.AddScoped<IDataProvider, ApiDataProvider>();
            services.AddHttpClient();

            services.AddAuthorizationCore();
            services.AddBlazoredLocalStorage();
            
            services.AddScoped<JwtAuthenticationStateProvider>();
            services.AddScoped<AuthenticationStateProvider>(s => s.GetRequiredService<JwtAuthenticationStateProvider>());
            services.AddScoped<IOperator, Operator>();
            services.AddScoped<IUserData, UserData>();
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IChartService, ChartService>();
            //services.AddScoped<BrowserResizeService>();

            services.AddScoped<IValueConverter, ObjectToStringConverter>();
           // services.AddNamedSingleton<IValueConverter, ObjectToStringConverter>(nameof(ObjectToStringConverter));
        }
    }
}
