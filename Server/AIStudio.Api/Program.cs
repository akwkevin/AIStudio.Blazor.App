using AIStudio.Api.Controllers.Test;
using AIStudio.Common.AppSettings;
using AIStudio.Common.Authentication.Jwt;
using AIStudio.Common.Authorization;
using AIStudio.Common.Autofac;
using AIStudio.Common.Cache;
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
using System.Reflection;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

//��ȡ�����ļ�appsettings
AppSettingsConfig.Configure(builder.Configuration);

//Ĭ���¼�����
builder.Services.AddEventBusDefault();
//����¼�����(Local)
builder.Services.AddEventBusLocal().AddSubscriber(subscribers =>
{
    //subscribers.Add<TestEventModel, TestEventHandler>();
    //subscribers.Add<ExceptionEvent, ExceptionEventHandler>();
    //subscribers.Add<RequestEvent, RequestEventHandler>();
});

////���ݹ�����Json����
builder.Services.AddControllers()
    .AddDataValidation() //������֤
    .AddFilter()    //������
    .AddTextJsonOptions();//Json����

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();

//swagger
builder.Services.AddSwaggerGen_();

//�� var app = builder.Build(); ǰ����ʹ�� Autofac ��ش���
builder.AddAutoface(GlobalType.AllTypes, builder =>
{
    builder.RegisterType<ValuesService>().As<IValuesService>().EnableInterfaceInterceptors();
});

//jwt Authentication 
builder.Services.AddJwtAuthentication(builder.Configuration);
// ��Ȩ
builder.Services.AddAuthorization_();
// �滻Ĭ�� PermissionChecker,����ʹ��
builder.Services.Replace(new ServiceDescriptor(typeof(IPermissionChecker), typeof(TestPermissionChecker), ServiceLifetime.Transient));

//ʹ��AutoMapper�Զ�ӳ��ӵ��MapAttribute����
builder.Services.AddMapper(GlobalType.AllTypes, GlobalType.AllAssemblies);

// ����
builder.Services.AddCache(builder.Configuration);

// ����
builder.Services.AddCors_(builder.Configuration);

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
