using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util.Common;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_AppSecretBusiness :BaseBusiness<Base_AppSecret>, IBase_AppSecretBusiness, ITransientDependency
    {
        public Base_AppSecretBusiness(ISqlSugarClient db) : base(db)
        {

        }

        #region 外部接口


        #endregion

        #region 私有成员

        #endregion
    }
}