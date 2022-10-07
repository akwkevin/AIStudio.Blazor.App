using AIStudio.Api;
using AIStudio.Business.Base_Manage;
using AIStudio.Business.Quartz_Manage;
using AIStudio.Common.AppSettings;
using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.Authorization;
using AIStudio.Common.Cache;
using AIStudio.Common.DI;
using AIStudio.Common.EventBus.EventHandlers;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Filter;
using AIStudio.Common.IdGenerator;
using AIStudio.Common.Mapper;
using AIStudio.Common.Quartz;
using AIStudio.Common.Service;
using AIStudio.Common.SqlSuger;
using AIStudio.Common.Swagger;
using AIStudio.Common.Types;
using AIStudio.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog;
using NLog.Web;
using Yitter.IdGenerator;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("启动中……");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //读取配置文件appsettings
    AppSettingsConfig.Configure(builder.Configuration);

    // 日志
    builder.Host.UseNLog();

    //默认事件总线
    builder.Services.AddEventBusDefault();
    //添加事件总线(Local)
    builder.Services.AddEventBusLocal().AddSubscriber(subscribers =>
    {
        subscribers.Add<TestEventModel, TestEventHandler>();
        subscribers.Add<ExceptionEvent, Base_LogExceptionBusiness>();
        subscribers.Add<RequestEvent, Base_LogOperatingBusiness>();
    });

    ////数据过滤与Json配置
    builder.Services.AddControllers()
        .AddDataValidation() //数据验证
        .AddFilter()   //过滤器
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.GetType().GetProperties().ForEach(aProperty =>
            {
                var value = aProperty.GetValue(JsonExtention.DefaultJsonSetting);
                aProperty.SetValue(options.SerializerSettings, value);
            });
        });

    // 设置雪花id的workerId，确保每个实例workerId都应不同
    var workerId = (ushort)(AppSettingsConfig.SnowIdOptions.WorkerId);
    IdHelper.SetWorkId(workerId);

    // 缓存
    builder.Services.AddCache();

    builder.Services.AddSqlSugar();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    //swagger
    builder.Services.AddSwaggerGen_();

    //注册 IHttpContextAccessor
    builder.Services.AddHttpContextAccessor();

    //自动注册服务
    builder.Services.AddServices(GlobalType.AllTypes);

    //jwt Authentication 
    builder.Services.AddJwtAuthentication();
    // 授权
    builder.Services.AddAuthorization_();
    // 替换默认 PermissionChecker,测试使用
    builder.Services.Replace(new ServiceDescriptor(typeof(IPermissionChecker), typeof(TestPermissionChecker), ServiceLifetime.Transient));

    //使用AutoMapper自动映射拥有MapAttribute的类
    builder.Services.AddMapper(GlobalType.AllTypes, GlobalType.AllAssemblies);

    // 跨域
    builder.Services.AddCors_();

    // 定时任务
    builder.Services.AddJobScheduling(options =>
    {
        options.StartHandle = async sp =>
        {
            bool withoutTestJob = true;
#if DEBUG
            //初始化测试数据
            SeedData.EnsureSeedQuartzData(sp);
            withoutTestJob = false;
#endif
            var jobService = sp.GetService<IQuartz_TaskBusiness>();
            if (jobService == null) return;

            await jobService.StartAllAsync(withoutTestJob);
        };
    });

    //服务提供器
    ServiceLocator.Instance = builder.Services.BuildServiceProvider(false);

    //初始化数据
    SeedData.EnsureSeedData(ServiceLocator.Instance);


    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            //如果使用了版本控制，别忘了修改此处
            foreach (var field in ApiVersionInfo.GetFieldValues())
            {
                c.SwaggerEndpoint($"/swagger/{field.Key}/swagger.json", $"{field.Key}");
            }
        });
    }

    app.UseHttpsRedirection();

    // UseCors 必须在 UseRouting 之后，UseResponseCaching、UseAuthorization 之前
    app.UseCors();

    // 添加自定义中间件（包含：Body重复读取、异常处理）
    app.UseMiddleware_();

    //开启静态文件功能
    app.UseStaticFiles(new StaticFileOptions
    {
        ServeUnknownFileTypes = true,
        DefaultContentType = "application/octet-stream"
    });

    app.UseAuthentication();
    app.UseAuthorization();

    app.MapControllers();

    app.Run();
}
catch (Exception ex)
{
    logger.Error(ex, "由于发生异常，导致程序中止！");
    throw;
}
finally
{
    LogManager.Shutdown();
}