using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Business.BaseBusiness&lt;AIStudio.Entity.Base_Manage.Base_AppSecret&gt;" />
    /// <seealso cref="AIStudio.IBusiness.Base_Manage.IBase_AppSecretBusiness" />
    /// <seealso cref="AIStudio.Common.DI.ITransientDependency" />
    public class Base_AppSecretBusiness :BaseBusiness<Base_AppSecret>, IBase_AppSecretBusiness, ITransientDependency
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="Base_AppSecretBusiness"/> class.
        /// </summary>
        /// <param name="db">注入数据库</param>
        public Base_AppSecretBusiness(ISqlSugarClient db) : base(db)
        {

        }

        #region 外部接口
        /// <summary>
        /// Adds the data asynchronous.
        /// </summary>
        /// <param name="newData">The new data.</param>
        [DataRepeatValidate(new string[] { "AppId" }, new string[] { "应用Id" })]
        public override async Task AddDataAsync(Base_AppSecret newData)
        {
            await base.AddDataAsync(newData);
        }

        /// <summary>
        /// Updates the data asynchronous.
        /// </summary>
        /// <param name="theData">The data.</param>
        [DataRepeatValidate(new string[] { "AppId" }, new string[] { "应用Id" })]
        public override async Task UpdateDataAsync(Base_AppSecret theData)
        {
            await base.UpdateAsync(theData);
        }

        /// <summary>
        /// Saves the data asynchronous.
        /// </summary>
        /// <param name="theData">The data.</param>
        [DataRepeatValidate(new string[] { "AppId" }, new string[] { "应用Id" })]
        public async Task SaveDataAsync(Base_AppSecret theData)
        {
            await base.SaveDataAsync(theData);
        }

        #endregion

        #region 私有成员

        #endregion
    }
}