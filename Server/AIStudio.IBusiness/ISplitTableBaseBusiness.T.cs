using AIStudio.Common.Types;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;
using SqlSugar;
using System.Data;
using System.Linq.Expressions;

namespace AIStudio.IBusiness
{
    public interface ISplitTableBaseBusiness<T> : IBaseBusiness<T> where T : class, new()
    {
        #region 查询数据     
        /// <summary>
        /// 获取实体对应的表，延迟加载，主要用于支持Linq查询操作
        /// </summary>
        /// <returns></returns>
        ISugarQueryable<T> GetIQueryable(bool splitTable);

        ISugarQueryable<dynamic> GetIQueryableDynamic(bool splitTable);

        ISugarQueryable<T> GetIQueryable(Dictionary<string, object> searchKeyValues, bool splitTable);
        #endregion
    }
}
