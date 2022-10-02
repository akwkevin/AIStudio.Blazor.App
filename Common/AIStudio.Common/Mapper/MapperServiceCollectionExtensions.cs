using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Mapper
{
    public static class MapperServiceCollectionExtensions
    {    /// <summary>
         /// 使用AutoMapper自动映射拥有MapAttribute的类
         /// </summary>
         /// <param name="services">服务集合</param>
         /// <param name="configure">自定义配置</param>
        public static IServiceCollection AddMapper(this IServiceCollection services, IEnumerable<Type> types, IEnumerable<Assembly> assemblies, Action<IMapperConfigurationExpression> configure = null)
        {
            List<(Type from, Type[] targets)> maps = new List<(Type from, Type[] targets)>();

            maps.AddRange(types.Where(x => x.GetCustomAttribute<MapAttribute>() != null)
                .Select(x => (x, x.GetCustomAttribute<MapAttribute>().TargetTypes)));

            var configuration = new MapperConfiguration(cfg =>
            {
                maps.ForEach(aMap =>
                {
                    aMap.targets.ToList().ForEach(aTarget =>
                    {
                        cfg.CreateMap(aMap.from, aTarget).IgnoreAllNonExisting(aMap.from, aTarget).ReverseMap();
                    });
                });

                cfg.AddMaps(assemblies);

                //自定义映射
                configure?.Invoke(cfg);
            });

#if DEBUG
            //只在Debug时检查配置
            configuration.AssertConfigurationIsValid();
#endif
            services.AddSingleton(configuration.CreateMapper());

            return services;
        }

        /// <summary>
        /// 忽略所有不匹配的属性。
        /// </summary>
        /// <param name="expression">配置表达式</param>
        /// <param name="from">源类型</param>
        /// <param name="to">目标类型</param>
        /// <returns></returns>
        public static IMappingExpression IgnoreAllNonExisting(this IMappingExpression expression, Type from, Type to)
        {
            var flags = BindingFlags.Public | BindingFlags.Instance;
            to.GetProperties(flags).Where(x => from.GetProperty(x.Name, flags) == null).ForEach(aProperty =>
            {
                expression.ForMember(aProperty.Name, opt => opt.Ignore());
            });

            return expression;
        }

        /// <summary>
        /// 忽略所有不匹配的属性。
        /// </summary>
        /// <typeparam name="TSource">源类型</typeparam>
        /// <typeparam name="TDestination">目标类型</typeparam>
        /// <param name="expression">配置表达式</param>
        /// <returns></returns>
        public static IMappingExpression<TSource, TDestination> IgnoreAllNonExisting<TSource, TDestination>(this IMappingExpression<TSource, TDestination> expression)
        {
            Type from = typeof(TSource);
            Type to = typeof(TDestination);
            var flags = BindingFlags.Public | BindingFlags.Instance;
            to.GetProperties(flags).Where(x => from.GetProperty(x.Name, flags) == null).ForEach(aProperty =>
            {
                expression.ForMember(aProperty.Name, opt => opt.Ignore());
            });

            return expression;
        }

        /// <summary>
        /// 给IEnumerable拓展ForEach方法
        /// </summary>
        /// <typeparam name="T">模型类</typeparam>
        /// <param name="iEnumberable">数据源</param>
        /// <param name="func">方法</param>
        public static void ForEach<T>(this IEnumerable<T> iEnumberable, Action<T> func)
        {
            foreach (var item in iEnumberable)
            {
                func(item);
            }
        }
    }
}
