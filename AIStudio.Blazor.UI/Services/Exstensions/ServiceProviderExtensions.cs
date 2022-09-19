using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Blazor.UI.Services.Exstensions
{
    public static class ServiceProviderExtensions
    {
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static object GetNamedService(this IServiceProvider provider, object name)
            => provider.GetService(new NamedType(name));
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static object GetRequiredNamedService(this IServiceProvider provider, object name)
            => provider.GetRequiredService(new NamedType(name));
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="name">服务名称</param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static object GetRequiredNamedService(this IServiceProvider provider, object name, Type serviceType)
            => provider.GetRequiredService(new NamedType(name, serviceType));
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static T GetRequiredNamedService<T>(this IServiceProvider provider, object name)
            => (T)provider.GetRequiredService(new NamedType(name, typeof(T)));
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static T GetNamedService<T>(this IServiceProvider provider, object name)
            => (T)provider.GetService(new NamedType(name, typeof(T)));
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="provider"></param>
        /// <param name="name">服务名称</param>
        /// <returns></returns>
        public static IEnumerable<T> GetNamedServices<T>(this IServiceProvider provider, string name)
            => provider.GetServices(new NamedType(name, typeof(T))).OfType<T>().ToArray();
        /// <summary>
        /// 获取命名服务
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="name">服务名称</param>
        /// <param name="serviceType"></param>
        /// <returns></returns>
        public static IEnumerable<object> GetNamedServices(this IServiceProvider provider, object name, Type serviceType)
            => provider.GetServices(new NamedType(name, serviceType)).Where(serviceType.IsInstanceOfType).ToArray();
    }
}
