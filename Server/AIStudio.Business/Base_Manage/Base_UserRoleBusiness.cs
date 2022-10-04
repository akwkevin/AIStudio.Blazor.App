using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_UserRoleBusiness : BaseBusiness<Base_UserRole>, IBase_UserRoleBusiness, ITransientDependency
    {
        public Base_UserRoleBusiness(ISqlSugarClient db) : base(db)
        {
        }
    }
}