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

        #endregion

        #region 私有成员

        #endregion
    }
}