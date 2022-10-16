using AIStudio.Common.Types;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.SqlSuger
{
    /// <summary>
    /// 按TenantId进行分表
    /// </summary>
    public class SqlSugarTenantSplitService : ISplitTableService
    {
        /// <summary>
        /// 返回数据库中所有分表
        /// </summary>
        /// <param name="db"></param>
        /// <param name="EntityInfo"></param>
        /// <param name="tableInfos"></param>
        /// <returns></returns>
        public List<SplitTableInfo> GetAllTables(ISqlSugarClient db, EntityInfo EntityInfo, List<DbTableInfo> tableInfos)
        {
            List<SplitTableInfo> result = new List<SplitTableInfo>();
            foreach (var item in tableInfos)
            {
                if (item.Name.Contains($"_{GlobalConst.TenantId}")) //区分标识如果不用正则符复杂一些，防止找错表
                {
                    SplitTableInfo data = new SplitTableInfo()
                    {
                        TableName = item.Name //要用item.name不要写错了
                    };
                    result.Add(data);
                }
            }
            return result.OrderBy(it => it.TableName).ToList();//打断点看一下有没有查出所有分表
        }

        /// <summary>
        /// 获取分表字段的值
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityInfo"></param>
        /// <param name="splitType"></param>
        /// <param name="entityValue"></param>
        /// <returns></returns>
        public object GetFieldValue(ISqlSugarClient db, EntityInfo entityInfo, SplitType splitType, object entityValue)
        {
            var splitColumn = entityInfo.Columns.FirstOrDefault(it => it.PropertyInfo.GetCustomAttribute<SplitFieldAttribute>() != null);
            var value = splitColumn?.PropertyInfo.GetValue(entityValue, null);
            return value;
        }

        /// <summary>
        /// 默认表名
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityInfo"></param>
        /// <returns></returns>
        public string GetTableName(ISqlSugarClient db, EntityInfo entityInfo)
        {
            return entityInfo.DbTableName + $"_{GlobalConst.TenantId}";
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityInfo"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public string GetTableName(ISqlSugarClient db, EntityInfo entityInfo, SplitType type)
        {
            //如果按租户分的表还需要按年月，是不是按照分库，再按年月日更简洁一些。
            return entityInfo.DbTableName + $"_{GlobalConst.TenantId}";//目前模式少不需要分类(自带的有 日、周、月、季、年等进行区分)
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="db"></param>
        /// <param name="entityInfo"></param>
        /// <param name="splitType"></param>
        /// <param name="fieldValue"></param>
        /// <returns></returns>
        public string GetTableName(ISqlSugarClient db, EntityInfo entityInfo, SplitType splitType, object fieldValue)
        {
            return entityInfo.DbTableName + $"_{GlobalConst.TenantId}" + fieldValue; //根据值
        }
    }
}
