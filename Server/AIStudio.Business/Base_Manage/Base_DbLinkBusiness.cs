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

        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }
}