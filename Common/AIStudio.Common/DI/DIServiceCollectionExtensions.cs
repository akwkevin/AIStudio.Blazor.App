using AIStudio.Common.DI.AOP;
using Castle.DynamicProxy;
using Microsoft.Extensions.DependencyInjection;

namespace AIStudio.Common.DI
{

    public static class DIServiceCollectionExtensions
    {
        private static readonly ProxyGenerator _generator = new ProxyGenerator();
        /// <summary>
        /// 自动注册程序集的服务
        /// </summary>
        /// <param name="services"></param>
        /// <param name="types"></param>
        /// <returns></returns>
        public static IServiceCollection AddServices_(this IServiceCollection services, List<Type> types)
        {
            Dictionary<Type, ServiceLifetime> lifeTimeMap = new Dictionary<Type, ServiceLifetime>
            {
                { typeof(ITransientDependency), ServiceLifetime.Transient},
                { typeof(IScopedDependency),ServiceLifetime.Scoped},
                { typeof(ISingletonDependency),ServiceLifetime.Singleton}
            };

            types.ForEach(aType =>
            {
                lifeTimeMap.ToList().ForEach(aMap =>
                {
                    var theDependency = aMap.Key;
                    if (theDependency.IsAssignableFrom(aType) && theDependency != aType && !aType.IsAbstract && aType.IsClass)
                    {
                        //注入实现
                        services.Add(new ServiceDescriptor(aType, aType, aMap.Value));

                        var interfaces = types.Where(x => x.IsAssignableFrom(aType) && x.IsInterface && x != theDependency).ToList();
                        //有接口则注入接口
                        if (interfaces.Count > 0)
                        {
                            interfaces.ForEach(aInterface =>
                            {
                                //注入AOP
                                services.Add(new ServiceDescriptor(aInterface, serviceProvider =>
                                {
                                    CastleInterceptor castleInterceptor = new CastleInterceptor(serviceProvider);

                                    return _generator.CreateInterfaceProxyWithTarget(aInterface, serviceProvider.GetService(aType), castleInterceptor);
                                }, aMap.Value));
                            });
                        }
                        //无接口则注入自己
                        else
                        {
                            services.Add(new ServiceDescriptor(aType, aType, aMap.Value));
                        }
                    }
                });
            });

            return services;
        }

        /// <summary>
        /// 注入AOP服务
        /// </summary>
        /// <typeparam name="TService"></typeparam>
        /// <typeparam name="TImplementation"></typeparam>
        /// <param name="services"></param>
        /// <param name="serviceLifetime"></param>
        /// <param name="aoptypes"></param>
        public static void AddProxiedService<TService, TImplementation>(this IServiceCollection services, ServiceLifetime serviceLifetime, params Type[] aoptypes)
            where TService : class  where TImplementation : class, TService
        {
            //注入实现
            services.Add(new ServiceDescriptor(typeof(TImplementation), typeof(TImplementation), serviceLifetime));
            //注入AOP
            services.Add(new ServiceDescriptor(typeof(TService), serviceProvider =>
            {
                var interceptors = serviceProvider.GetServices<IInterceptor>().Where(p => aoptypes == null || aoptypes.Contains(p.GetType())).ToArray();
                return _generator.CreateInterfaceProxyWithTarget(typeof(TService), serviceProvider.GetService(typeof(TImplementation)), interceptors);
            }, serviceLifetime));
        }    

    }

}