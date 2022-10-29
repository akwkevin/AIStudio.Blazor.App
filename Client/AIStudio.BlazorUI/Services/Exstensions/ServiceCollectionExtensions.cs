using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorUI.Services.Exstensions
{
    public static class ServiceCollectionExtensions
    {
        #region NamedScoped
        public static IServiceCollection AddNamedScoped(this IServiceCollection services, object name, Type serviceType)
            => services.AddScoped(new NamedType(name, serviceType));
        public static IServiceCollection AddNamedScoped(this IServiceCollection services, object name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
            => services.AddScoped(new NamedType(name, serviceType), implementationFactory);
        public static IServiceCollection AddNamedScoped(this IServiceCollection services, object name, Type serviceType, Type implementationType)
            => services.AddScoped(new NamedType(name, serviceType), implementationType);
        public static IServiceCollection AddNamedScoped<TService>(this IServiceCollection services, object name) where TService : class
            => services.AddNamedScoped(name, typeof(TService));
        public static IServiceCollection AddNamedScoped<TService>(this IServiceCollection services, object name, Func<IServiceProvider, TService> implementationFactory) where TService : class
            => services.AddNamedScoped(name, typeof(TService), sp => implementationFactory.Invoke(sp));
        public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, object name)
            where TService : class
            where TImplementation : class, TService
            => services.AddNamedScoped(name, typeof(TService), typeof(TImplementation));
        public static IServiceCollection AddNamedScoped<TService, TImplementation>(this IServiceCollection services, object name, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
            => services.AddNamedScoped(name, typeof(TService), sp => implementationFactory.Invoke(sp));
        #endregion
        #region NamedSingleton
        public static IServiceCollection AddNamedSingleton(this IServiceCollection services, object name, Type serviceType, Type implementationType)
            => services.AddSingleton(new NamedType(name, serviceType), implementationType);
        public static IServiceCollection AddNamedSingleton(this IServiceCollection services, object name, Type serviceType, object implementationInstance)
            => services.AddSingleton(new NamedType(name, serviceType), implementationInstance);
        public static IServiceCollection AddNamedSingleton(this IServiceCollection services, object name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
            => services.AddSingleton(new NamedType(name, serviceType), implementationFactory);
        public static IServiceCollection AddNamedSingleton(this IServiceCollection services, object name, Type serviceType)
            => services.AddSingleton(new NamedType(name, serviceType));
        public static IServiceCollection AddNamedSingleton<TService>(this IServiceCollection services, object name) where TService : class
            => services.AddNamedSingleton(name, typeof(TService));
        public static IServiceCollection AddNamedSingleton<TService>(this IServiceCollection services, object name, Func<IServiceProvider, TService> implementationFactory) where TService : class
            => services.AddNamedSingleton(name, typeof(TService), sp => implementationFactory.Invoke(sp));
        public static IServiceCollection AddNamedSingleton<TService>(this IServiceCollection services, object name, TService implementationInstance) where TService : class
            => services.AddNamedSingleton(name, typeof(TService), implementationInstance);
        public static IServiceCollection AddNamedSingleton<TService, TImplementation>(this IServiceCollection services, object name, Func<IServiceProvider, TImplementation> implementationFactory)
            where TService : class
            where TImplementation : class, TService
            => services.AddNamedSingleton(name, typeof(TService), sp => implementationFactory.Invoke(sp));
        public static IServiceCollection AddNamedSingleton<TService, TImplementation>(this IServiceCollection services, object name)
            where TService : class
            where TImplementation : class, TService
            => services.AddNamedSingleton(name, typeof(TService), typeof(TImplementation));
        #endregion
        #region NamedTransient
        public static IServiceCollection AddNamedTransient(this IServiceCollection services, object name, Type serviceType)
            => services.AddTransient(new NamedType(name, serviceType));
        public static IServiceCollection AddNamedTransient(this IServiceCollection services, object name, Type serviceType, Func<IServiceProvider, object> implementationFactory)
            => services.AddTransient(new NamedType(name, serviceType), implementationFactory);
        public static IServiceCollection AddNamedTransient(this IServiceCollection services, object name, Type serviceType, Type implementationType)
            => services.AddTransient(new NamedType(name, serviceType), implementationType);
        public static IServiceCollection AddNamedTransient<TService>(this IServiceCollection services, object name) where TService : class
            => services.AddNamedTransient(name, typeof(TService));
        public static IServiceCollection AddNamedTransient<TService>(this IServiceCollection services, object name, Func<IServiceProvider, TService> implementationFactory) where TService : class
            => services.AddNamedTransient(name, typeof(TService), sp => implementationFactory.Invoke(sp));
        public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, object name)
         where TService : class
         where TImplementation : class, TService
            => services.AddNamedTransient(name, typeof(TService), typeof(TImplementation));
        public static IServiceCollection AddNamedTransient<TService, TImplementation>(this IServiceCollection services, object name, Func<IServiceProvider, TImplementation> implementationFactory)
         where TService : class
         where TImplementation : class, TService
            => services.AddNamedTransient(name, typeof(TService), sp => implementationFactory.Invoke(sp));
        #endregion
    }
}
