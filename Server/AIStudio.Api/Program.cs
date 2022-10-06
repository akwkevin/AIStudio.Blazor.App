using AIStudio.Common.AppSettings;
using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.Authorization;
using AIStudio.Common.Cache;
using AIStudio.Common.DI;
using AIStudio.Common.EventBus.EventHandlers;
using AIStudio.Common.Filter;
using AIStudio.Common.Mapper;
using AIStudio.Common.Service;
using AIStudio.Common.SqlSuger;
using AIStudio.Common.Swagger;
using AIStudio.Common.Types;
using AIStudio.Util;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection.Extensions;
using NLog;
using NLog.Web;
using Yitter.IdGenerator;

var logger = NLog.LogManager.Setup().LoadConfigurationFromAppSettings().GetCurrentClassLogger();
logger.Debug("�����С���");

try
{
    var builder = WebApplication.CreateBuilder(args);

    // Add services to the container.

    //��ȡ�����ļ�appsettings
    AppSettingsConfig.Configure(builder.Configuration);

    // ��־
    builder.Host.UseNLog();

    //Ĭ���¼�����
    builder.Services.AddEventBusDefault();
    //����¼�����(Local)
    builder.Services.AddEventBusLocal().AddSubscriber(subscribers =>
    {
        subscribers.Add<TestEventModel, TestEventHandler>();
        //subscribers.Add<ExceptionEvent, ExceptionEventHandler>();
        //subscribers.Add<RequestEvent, RequestEventHandler>();
    });

    ////���ݹ�����Json����
    builder.Services.AddControllers()
        .AddDataValidation() //������֤
        .AddFilter(options =>
        {
            options.ResultFactory = resultException =>
            {
                // AppResultException ������ 200 ״̬��
                var objectResult = new ObjectResult(resultException.AjaxResult);
                objectResult.StatusCode = StatusCodes.Status200OK;
                return objectResult;
            };
        })   //������
        .AddNewtonsoftJson(options =>
        {
            options.SerializerSettings.GetType().GetProperties().ForEach(aProperty =>
            {
                var value = aProperty.GetValue(JsonExtention.DefaultJsonSetting);
                aProperty.SetValue(options.SerializerSettings, value);
            });
        });

    var workerId = (ushort)(AppSettingsConfig.SnowIdOptions.WorkerId);
    // ����ѩ��id��workerId��ȷ��ÿ��ʵ��workerId��Ӧ��ͬ
    YitIdHelper.SetIdGenerator(new IdGeneratorOptions { WorkerId = workerId });

    // ����
    builder.Services.AddCache();

    builder.Services.AddSqlSugar();

    // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
    builder.Services.AddEndpointsApiExplorer();

    //swagger
    builder.Services.AddSwaggerGen_();

    //ע�� IHttpContextAccessor
    builder.Services.AddHttpContextAccessor();

    //�Զ�ע�����
    builder.Services.AddServices(GlobalType.AllTypes);

    //jwt Authentication 
    builder.Services.AddJwtAuthentication();
    // ��Ȩ
    builder.Services.AddAuthorization_();
    // �滻Ĭ�� PermissionChecker,����ʹ��
    builder.Services.Replace(new ServiceDescriptor(typeof(IPermissionChecker), typeof(TestPermissionChecker), ServiceLifetime.Transient));

    //ʹ��AutoMapper�Զ�ӳ��ӵ��MapAttribute����
    builder.Services.AddMapper(GlobalType.AllTypes, GlobalType.AllAssemblies);

    // ����
    builder.Services.AddCors_();

    // ��ʱ����
    //builder.Services.AddJobScheduling(options =>
    //{
    //    options.StartHandle = async sp =>
    //    {
    //        var jobService = sp.GetService<IJobService>();
    //        if (jobService == null) return;
    //        await jobService.StartAll();
    //    };
    //});

    ServiceLocator.Instance = builder.Services.BuildServiceProvider(false);

    var app = builder.Build();

    // Configure the HTTP request pipeline.
    if (app.Environment.IsDevelopment())
    {
        app.UseSwagger();
        app.UseSwaggerUI(c =>
        {
            //���ʹ���˰汾���ƣ��������޸Ĵ˴�
            foreach (var field in ApiVersionInfo.GetFieldValues())
            {
                c.SwaggerEndpoint($"/swagger/{field.Key}/swagger.json", $"{field.Key}");
            }
        });
    }

    app.UseHttpsRedirection();

    // UseCors ������ UseRouting ֮��UseResponseCaching��UseAuthorization ֮ǰ
    app.UseCors();

    // ����Զ����м����������Body�ظ���ȡ���쳣����
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
    logger.Error(ex, "���ڷ����쳣�����³�����ֹ��");
    throw;
}
finally
{
    LogManager.Shutdown();
}