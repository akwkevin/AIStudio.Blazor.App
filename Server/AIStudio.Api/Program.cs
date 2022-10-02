using AIStudio.Api.Controllers.Test;
using AIStudio.Common.AppSettings;
using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.Authorization;
using AIStudio.Common.Autofac;
using AIStudio.Common.Cache;
using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Core;
using AIStudio.Common.EventBus.EventHandlers;
using AIStudio.Common.EventBus.Models;
using AIStudio.Common.Filter;
using AIStudio.Common.Json.SystemTextJson;
using AIStudio.Common.Mapper;
using AIStudio.Common.Quartz;
using AIStudio.Common.Swagger;
using AIStudio.Common.Types;
using Autofac;
using Autofac.Core;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using Microsoft.OpenApi.Models;
using NLog;
using NLog.Web;
using System.Reflection;

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
        //subscribers.Add<ExceptionEvent, ExceptionEventHandler>();
        //subscribers.Add<RequestEvent, RequestEventHandler>();
    });

    ////数据过滤与Json配置
    builder.Services.AddControllers()
        .AddDataValidation() //数据验证
        .AddFilter(options =>
        {
            options.ResultFactory = resultException =>
            {
                // AppResultException 都返回 200 状态码
                var objectResult = new ObjectResult(resultException.AjaxResult);
                objectResult.StatusCode = StatusCodes.Status200OK;
                return objectResult;
            };
        })   //过滤器
        .AddTextJsonOptions();//Json配置

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    //swagger
    builder.Services.AddSwaggerGen_();

    //注册 IHttpContextAccessor
    builder.Services.AddHttpContextAccessor();

    //在 var app = builder.Build(); 前加入使用 Autofac 相关代码
    builder.AddAutoface(GlobalType.AllTypes, builder =>
    {
        builder.RegisterType<ValuesService>().As<IValuesService>().EnableInterfaceInterceptors();
    });

    //jwt Authentication 
    builder.Services.AddJwtAuthentication(builder.Configuration);
    // 授权
    builder.Services.AddAuthorization_();
    // 替换默认 PermissionChecker,测试使用
    builder.Services.Replace(new ServiceDescriptor(typeof(IPermissionChecker), typeof(TestPermissionChecker), ServiceLifetime.Transient));

    //使用AutoMapper自动映射拥有MapAttribute的类
    builder.Services.AddMapper(GlobalType.AllTypes, GlobalType.AllAssemblies);

    // 缓存
    builder.Services.AddCache(builder.Configuration);

    // 跨域
    builder.Services.AddCors_(builder.Configuration);

    // 定时任务
    //builder.Services.AddJobScheduling(options =>
    //{
    //    options.StartHandle = async sp =>
    //    {
    //        var jobService = sp.GetService<IJobService>();
    //        if (jobService == null) return;
    //        await jobService.StartAll();
    //    };
    //});

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