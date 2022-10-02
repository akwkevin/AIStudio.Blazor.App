using AIStudio.Common.Autofac.Lifetime;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.Types;
using Autofac;
using Autofac.Builder;
using Autofac.Extensions.DependencyInjection;
using Autofac.Extras.DynamicProxy;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.SwaggerGen;
using System.Security.Cryptography.X509Certificates;
using System.Security.Cryptography.Xml;

namespace AIStudio.Common.Autofac
{

    public static class AutofacServiceCollectionExtensions
    {
        /// <summary>
        /// 自动注册程序集的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="dllNames"></param>
        /// <returns></returns>
        public static WebApplicationBuilder AddAutoface(this WebApplicationBuilder builder, List<Type> types, Action<ContainerBuilder> setupAction = null)
        {
            //在 var app = builder.Build(); 前加入使用 Autofac 相关代码
            builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
            //Autofac 专用函数注册
            builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
            {

                //这里注册关系
                //builder.RegisterType<ValuesService>().As<IValuesService>();
                //使用模块注册
                builder.RegisterModule<AutofacModule>();

                //自动注入服务
                Dictionary<Type, AutofaceLifetime> lifeTimeMap = new Dictionary<Type, AutofaceLifetime>
                {
                    {typeof(IInstancePerDependency),AutofaceLifetime.PerDependency },
                    {typeof(IInstancePerRequest),AutofaceLifetime.Transient },
                    {typeof(IInstancePerLifetimeScope),AutofaceLifetime.Scoped },
                    { typeof(ISingleInstance),AutofaceLifetime.Singleton }
                };

                types.ForEach(aType =>
                {
                    lifeTimeMap.ToList().ForEach(aMap =>
                    {
                        var theDependency = aMap.Key;
                        if (theDependency.IsAssignableFrom(aType) && theDependency != aType && !aType.IsAbstract && aType.IsClass)
                        {
                            var interfaces = types.Where(x => x.IsAssignableFrom(aType) && x.IsInterface && x != theDependency).ToList();
                            //有接口则注入接口
                            if (interfaces.Count > 0)
                            {
                                interfaces.ForEach(aInterface =>
                                {
                                    //注入AOP
                                    //builder.RegisterTypeLifetime(aType, aInterface, aMap.Value);
                                    builder.RegisterType<Operator>().As<IOperator>();
                                });
                            }
                            //无接口则注入自己
                            else
                            {
                                builder.RegisterTypeLifetime(aType, aType, aMap.Value);
                            }
                        }
                    });
                });

        
                setupAction?.Invoke(builder);
                
            });

            return builder;
        }

        public static void RegisterTypeLifetime(this ContainerBuilder builder, Type aType, Type aInterface, AutofaceLifetime autofaceLifetime)
        {
            switch (autofaceLifetime)
            {
                case AutofaceLifetime.Singleton:
                    builder.RegisterType(aType).As(aInterface).SingleInstance().EnableInterfaceInterceptors();
                    break;
                case AutofaceLifetime.Scoped:
                    builder.RegisterType(aType).As(aInterface).InstancePerLifetimeScope().EnableInterfaceInterceptors();
                    break;
                case AutofaceLifetime.Transient:
                    builder.RegisterType(aType).As(aInterface).InstancePerRequest().EnableInterfaceInterceptors();
                    break;
                case AutofaceLifetime.PerDependency:
                    builder.RegisterType(aType).As(aInterface).InstancePerDependency().EnableInterfaceInterceptors();
                    break;
            }
        }
    }

    public enum AutofaceLifetime
    {
        /// <summary>
        /// 单例 SingleInstance：在整个容器中获取的服务实例都是同一个；
        /// </summary>
        Singleton = 0,
        //
        // 摘要:
        //     Specifies that a new instance of the service will be created for each scope.
        //
        // 言论：
        //     In ASP.NET Core applications a scope is created around each server request.
        Scoped = 1,
        //
        // 摘要:
        //     Specifies that a new instance of the service will be created every time it is
        //     requested.
        Transient = 2,
        /// <summary>
        /// 瞬时 InstancePerDependency：每次获取的服务实例都不一样；
        /// </summary>
        PerDependency = 3,
    }
}