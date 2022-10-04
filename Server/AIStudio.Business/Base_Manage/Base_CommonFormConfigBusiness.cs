using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.Helper;
using SqlSugar;
using System.Linq.Dynamic.Core;

namespace AIStudio.Business.Base_Manage
{
    public class Base_CommonFormConfigBusiness : BaseBusiness<Base_CommonFormConfig>, IBase_CommonFormConfigBusiness, ITransientDependency
    {
        public Base_CommonFormConfigBusiness(ISqlSugarClient db) : base(db)
        {
        }

        #region 外部接口

        public async Task<PageResult<Base_CommonFormConfig>> GetDataListAsync(PageInput input)
        {
            var q = GetIQueryable();

            //按字典筛选
            if (input.SearchKeyValues != null)
            {
                foreach(var keyValuePair in input.SearchKeyValues)
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<Base_CommonFormConfig, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    q = q.Where(newWhere);
                }
            }

            return await q.GetPageResultAsync(input);
        }

        public async Task<Base_CommonFormConfig> GetTheDataAsync(string id)
        {
            return await GetEntityAsync(id);
        }

        public async Task AddDataAsync(Base_CommonFormConfig data)
        {
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(Base_CommonFormConfig data)
        {
            await UpdateAsync(data);
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}