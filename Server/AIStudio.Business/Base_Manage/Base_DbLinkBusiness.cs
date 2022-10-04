using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util.Common;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_DbLinkBusiness : BaseBusiness<Base_DbLink>, IBase_DbLinkBusiness, ITransientDependency
    {
        public Base_DbLinkBusiness(ISqlSugarClient db) : base(db)
        {

        }

        #region 外部接口

        public async Task<PageResult<Base_DbLink>> GetDataListAsync(PageInput input)
        {
            RefAsync<int> total = 0;
            var P = await Db.Queryable<Base_DbLink>()
                .ToPageListAsync(input.PageIndex, input.PageRows, total);
            return new PageResult<Base_DbLink> { Data = P, Total = total };
        }

        public async Task<Base_DbLink> GetTheDataAsync(string id)
        {
            return await Db.Queryable<Base_DbLink>().Where(x => x.Id == id).FirstAsync();
        }

        public async Task AddDataAsync(Base_DbLink newData)
        {
            await Db.Insertable(newData).ExecuteCommandAsync();
        }

        public async Task UpdateDataAsync(Base_DbLink theData)
        {
            await Db.Updateable<Base_DbLink>().SetColumns(x => new Base_DbLink { LinkName = theData.LinkName, ConnectionStr = theData.ConnectionStr, DbType = theData.DbType }).Where(x => x.Id.Equals(theData.Id)).ExecuteCommandAsync();
        }

        public async Task DeleteDataAsync(List<string> ids)
        {
            await Db.Deleteable<Base_AppSecret>().Where(x => ids.Contains(x.Id)).ExecuteCommandAsync();
        }

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}