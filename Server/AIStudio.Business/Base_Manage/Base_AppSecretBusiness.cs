using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_AppSecretBusiness :BaseBusiness<Base_AppSecret>, IBase_AppSecretBusiness, ITransientDependency
    {
        public Base_AppSecretBusiness(ISqlSugarClient db) : base(db)
        {

        }

        #region 外部接口
        [DataRepeatValidate(new string[] { "AppId" }, new string[] { "应用Id" })]

        public override async Task AddDataAsync(Base_AppSecret newData)
        {
            await base.AddDataAsync(newData);
        }

        [DataRepeatValidate(new string[] { "AppId" }, new string[] { "应用Id" })]

        public override async Task UpdateDataAsync(Base_AppSecret theData)
        {
            await base.UpdateAsync(theData);
        }

        public override async Task SaveDataAsync(Base_AppSecret theData)
        {
            await base.SaveDataAsync(theData);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}