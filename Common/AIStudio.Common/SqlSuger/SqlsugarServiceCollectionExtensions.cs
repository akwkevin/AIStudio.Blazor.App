using AIStudio.Common.AppSettings;
using AIStudio.Common.Cache;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.IdGenerator;
using AIStudio.Common.Jwt;
using AIStudio.Common.Mapper;
using AIStudio.Common.Quartz;
using AIStudio.Common.Service;
using AIStudio.Common.Types;
using AIStudio.Util;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using NetTaste;
using Org.BouncyCastle.Asn1.X509.Qualified;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq.Dynamic.Core;
using System.Linq.Expressions;

namespace AIStudio.Common.SqlSuger
{
    public static class SqlsugarServiceCollectionExtensions
    {

        /// <summary>
        /// SqlsugarScope的配置
        /// Scope必须用单例注入
        /// 不可以用Action委托注入
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddSqlSugar_(this IServiceCollection services)
        {
            //数据库序号从0开始,默认数据库为0

            //默认数据库
            List<DbConfig> dbList = new List<DbConfig>();
            DbConfig defaultdb = new DbConfig()
            {
                DbNumber = AppSettingsConfig.ConnectionStringsOptions.DefaultDbNumber,
                DbString = AppSettingsConfig.ConnectionStringsOptions.DefaultDbString,
                DbType = AppSettingsConfig.ConnectionStringsOptions.DefaultDbType,
            };
            dbList.Add(defaultdb);
            //业务数据库集合
            if (AppSettingsConfig.ConnectionStringsOptions.DbConfigs != null)
            {
                foreach (var item in AppSettingsConfig.ConnectionStringsOptions.DbConfigs)
                {
                    dbList.Add(item);
                }
            }

            List<ConnectionConfig> connectConfigList = new List<ConnectionConfig>();

            foreach (var item in dbList)
            {
                //防止数据库重复，导致的事务异常
                if (connectConfigList.Any(a => a.ConfigId == (dynamic)item.DbNumber || a.ConnectionString == item.DbString))
                {
                    continue;
                }
                connectConfigList.Add(new ConnectionConfig()
                {
                    ConnectionString = item.DbString,
                    DbType = (DbType)Convert.ToInt32(Enum.Parse(typeof(DbType), item.DbType)),
                    IsAutoCloseConnection = true,
                    ConfigId = item.DbNumber,
                    InitKeyType = InitKeyType.Attribute,
                    MoreSettings = new ConnMoreSettings()
                    {
                        IsAutoRemoveDataCache = true//自动清理缓存

                    },
                    ConfigureExternalServices = new ConfigureExternalServices()
                    {
                        DataInfoCacheService = new SqlSugarCache(services.BuildServiceProvider().GetService<IDistributedCache>()),
                        EntityNameService = (type, entity) =>
                        {
                            var attributes = type.GetCustomAttributes(true);
                            if (attributes.Any(it => it is TableAttribute))
                            {
                                entity.DbTableName = (attributes.First(it => it is TableAttribute) as TableAttribute).Name;
                            }
                        },
                        EntityService = (type, column) =>
                        {
                            var attributes = type.GetCustomAttributes(true);
                            if (attributes.Any(it => it is KeyAttribute))// by attribute set primarykey
                            {
                                column.IsPrimarykey = true; //有哪些特性可以看 1.2 特性明细
                            }
                            else
                            {
                                column.IsNullable = true;  //除了主键都默认可空
                            }

                            if (attributes.Any(it => it is ColumnAttribute))
                            {
                                var name = (attributes.First(it => it is ColumnAttribute) as ColumnAttribute).Name;
                                if (!string.IsNullOrEmpty(name))
                                {
                                    column.DbColumnName = name;
                                }
                            }

                            if (attributes.Any(it => it is MaxLengthAttribute))
                            {
                                var length = (attributes.First(it => it is MaxLengthAttribute) as MaxLengthAttribute).Length;
                                column.Length = length;
                            }

                            if (type.PropertyType == typeof(string))
                            {
                                if (column.Length == 0)
                                {
                                    column.DataType = "Nvarchar(Max)";
                                }
                                else
                                {
                                    column.DataType = "Nvarchar";
                                }
                            }
                        }
                    }
                });
            }

            //获取所有表类型
            List<Type> types = GlobalType.AllTypes.Where(a => !a.IsAbstract && a.IsClass &&
                (a.GetCustomAttributes(typeof(TableAttribute), true)?.FirstOrDefault() != null
                || a.GetCustomAttributes(typeof(SugarTable), true)?.FirstOrDefault() != null)).ToList();

            SqlSugarScope sqlSugarScope = new SqlSugarScope(connectConfigList,
                //全局上下文生效
                db =>
                {
                    /*
                     * 默认只会配置到第一个数据库，这里按照官方文档进行多数据库/多租户文档的说明进行循环配置
                     */
                    foreach (var c in connectConfigList)
                    {
                        var dbProvider = db.GetConnectionScope((string)c.ConfigId);
                        //执行超时时间
                        dbProvider.Ado.CommandTimeOut = 30;

                        dbProvider.Aop.OnLogExecuting = (sql, pars) =>
                        {
                            //发送给MiniProfiler，在http://localhost:5000/profiler/results查看，就不在控制台显示了
                            MiniProfilerServiceCollectionExtensions.PrintToMiniProfiler("SqlSugar", "Info", UtilMethods.GetSqlString(c.DbType, sql, pars));
                        };

                        dbProvider.Aop.DataExecuting = (oldValue, entityInfo) =>
                        {
                            // 新增操作
                            if (entityInfo.OperationType == DataFilterType.InsertByObject)
                            {
                                // 主键(long)-赋值雪花Id
                                if (entityInfo.EntityColumnInfo.IsPrimarykey)
                                {
                                    if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(string))
                                    {
                                        var id = ((dynamic)entityInfo.EntityValue).Id;
                                        if (id == null || id == "")
                                            entityInfo.SetValue(IdHelper.GetId());
                                    }
                                    else if (entityInfo.EntityColumnInfo.PropertyInfo.PropertyType == typeof(long))
                                    {
                                        var id = ((dynamic)entityInfo.EntityValue).Id;
                                        if (id == null || id == 0)
                                            entityInfo.SetValue(IdHelper.GetlongId());
                                    }
                                }

                                if (entityInfo.PropertyName == GlobalConst.CreateTime)
                                    entityInfo.SetValue(DateTime.Now);

                                if (entityInfo.PropertyName == GlobalConst.TenantId && GetTenantId(out var tenantId))
                                    entityInfo.SetValue(tenantId);

                                if (entityInfo.PropertyName == GlobalConst.CreatorId && GetUserId(out var userId))
                                    entityInfo.SetValue(userId);

                                if (entityInfo.PropertyName == GlobalConst.CreatorName && GetUserName(out var userName))
                                    entityInfo.SetValue(userName);

                            }
                            // 更新操作
                            if (entityInfo.OperationType == DataFilterType.UpdateByObject)
                            {
                                if (entityInfo.PropertyName == GlobalConst.ModifyTime)
                                    entityInfo.SetValue(DateTime.Now);

                                if (entityInfo.PropertyName == GlobalConst.ModifyId && GetUserId(out var userId))
                                    entityInfo.SetValue(userId);

                                if (entityInfo.PropertyName == GlobalConst.ModifyName && GetUserName(out var userName))
                                    entityInfo.SetValue(userName);

                            }
                        };

                        //全局过滤器
                        foreach (var entityType in types)
                        {
                            // 配置多租户全局过滤器
                            if (!entityType.GetProperty(GlobalConst.TenantId).IsNullOrEmpty())
                            { //判断实体类中包含TenantId属性
                              //构建动态Lambda
                                var lambda = DynamicExpressionParser.ParseLambda
                                (new[] { Expression.Parameter(entityType, "it") },
                                 typeof(bool), $"{nameof(GlobalConst.TenantId)} ==  @0 or (@1 and @2)",
                                  GetTenantId(), IsSuperAdmin(), AppSettingsConfig.AppSettingsOptions.SuperAdminViewAllData);
                                dbProvider.QueryFilter.Add(new TableFilterItem<object>(entityType, lambda)); //将Lambda传入过滤器
                            }
                            // 配置加删除全局过滤器
                            if (!entityType.GetProperty(GlobalConst.Deleted).IsNullOrEmpty())
                            { //判断实体类中包含IsDeleted属性
                              //构建动态Lambda
                                var lambda = DynamicExpressionParser.ParseLambda
                                (new[] { Expression.Parameter(entityType, "it") },
                                 typeof(bool), $"{nameof(GlobalConst.Deleted)} ==  @0",
                                  false);
                                dbProvider.QueryFilter.Add(new TableFilterItem<object>(entityType, lambda)
                                {
                                    IsJoinQuery = true
                                }); //将Lambda传入过滤器
                            }
                        }

                    }
                });
            services.AddSingleton<ISqlSugarClient>(sqlSugarScope);
            // 注册 SqlSugar 仓储
            //services.AddScoped(typeof(SqlSugarRepository<>));

            //If no exist create datebase 
            sqlSugarScope.DbMaintenance.CreateDatabase();

            //Create tables 
            //SetStringDefaultLength(int.MaxValue)
            sqlSugarScope.CodeFirst.InitTables(types.ToArray());

            return services;
        }

        /// <summary>
        /// 获取当前租户id
        /// </summary>
        /// <returns></returns>
        private static object GetTenantId()
        {
            if (ServiceLocator.User == null) return null;
            return ServiceLocator.User.FindFirst(SimpleClaimTypes.TenantId)?.Value;
        }

        /// <summary>
        /// 获取当前租户id
        /// </summary>
        /// <returns></returns>
        private static bool GetTenantId(out object obj)
        {
            obj = null;
            if (ServiceLocator.User == null) return false;
            obj = ServiceLocator.User.FindFirst(SimpleClaimTypes.TenantId)?.Value;
            return obj != null;
        }

        private static bool GetUserId(out object obj)
        {
            obj = null;
            if (ServiceLocator.User == null) return false;
            obj = ServiceLocator.User.FindFirst(SimpleClaimTypes.UserId)?.Value;
            return obj != null;
        }

        private static bool GetUserName(out object obj)
        {
            obj = null;
            if (ServiceLocator.User == null) return false;
            obj = ServiceLocator.User.FindFirst(SimpleClaimTypes.Name)?.Value;
            return obj != null;
        }

        /// <summary>
        /// 判断是不是超级管理员
        /// </summary>
        /// <returns></returns>
        private static bool IsSuperAdmin()
        {
            if (ServiceLocator.User == null) return false;
            return ServiceLocator.User.FindFirst(SimpleClaimTypes.SuperAdmin)?.Value == SimpleClaimTypes.SuperAdmin;
        }

        /// <summary>
        /// 测试用法
        /// </summary>
        /// <param name="services"></param>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services)
        {
            SqlSugarScope sqlSugar = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = (DbType)Convert.ToInt32(Enum.Parse(typeof(DbType), AppSettingsConfig.ConnectionStringsOptions.DefaultDbType)),
                ConnectionString = AppSettingsConfig.ConnectionStringsOptions.DefaultDbString,
                IsAutoCloseConnection = true,
            },
           db =>
           {
               //单例参数配置，所有上下文生效
               db.Aop.OnLogExecuting = (sql, pars) =>
               {
                   Console.WriteLine(sql);//输出sql
               };

           });
            services.AddSingleton<ISqlSugarClient>(sqlSugar);//这边是SqlSugarScope用AddSingleton

            return services;
        }

        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="config"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, ConnectionConfig config, Action<ISqlSugarClient> buildAction = default)
        {
            var list = new List<ConnectionConfig>();
            list.Add(config);
            return services.AddSqlSugar(list, buildAction);
        }

        /// <summary>
        /// 添加 SqlSugar 拓展
        /// </summary>
        /// <param name="services"></param>
        /// <param name="configs"></param>
        /// <param name="buildAction"></param>
        /// <returns></returns>
        public static IServiceCollection AddSqlSugar(this IServiceCollection services, List<ConnectionConfig> configs, Action<ISqlSugarClient> buildAction = default)
        {
            // 注册 SqlSugar 客户端
            services.AddScoped<ISqlSugarClient>(u =>
            {
                var db = new SqlSugarClient(configs);
                buildAction?.Invoke(db);
                return db;
            });

            // 注册 SqlSugar 仓储
            services.AddScoped(typeof(SqlSugarRepository<>));
            return services;
        }
    }
}
