using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_TestBusiness :BaseBusiness<Base_Test>, IBase_TestBusiness, ITransientDependency
    {
        public Base_TestBusiness(ISqlSugarClient db) : base(db)
        {

        }

        #region 外部接口

        public override async Task AddDataAsync(Base_Test newData)
        {
            await base.AddDataAsync(newData);
        }


        public override async Task UpdateDataAsync(Base_Test theData)
        {
            await base.UpdateAsync(theData);
        }

        public override async Task SaveDataAsync(Base_Test theData)
        {
            await base.SaveDataAsync(theData);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}