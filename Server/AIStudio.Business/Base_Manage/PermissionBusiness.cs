using AIStudio.Common.DI;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    class PermissionBusiness : BaseBusiness<Base_Action>, IPermissionBusiness, ITransientDependency
    {
        IBase_ActionBusiness _actionBus { get; }
        IBase_UserBusiness _userBus { get; }
        public PermissionBusiness(IBase_ActionBusiness actionBus, IBase_UserBusiness userBus, ISqlSugarClient db) : base(db)
        {
            _actionBus = actionBus;
            _userBus = userBus;
        }

        async Task<string[]> GetUserActionIds(string userId)
        {
            var theUser = await _userBus.GetTheDataAsync(userId);           

            if (userId == AdminTypes.Admin.ToString() || theUser.RoleType.HasFlag(RoleTypes.超级管理员))
            {
                return await GetIQueryable().Select(x => x.Id).ToArrayAsync();
            }
            else
            {
                var actionIds = await Db.Queryable<Base_UserRole>().LeftJoin<Base_RoleAction>((o, i) => o.RoleId == i.RoleId).Where(o => o.UserId == userId).Select((o, i) => i.ActionId).ToArrayAsync();
             
                //不需要权限的菜单和有权限的菜单集合
                return await GetIQueryable().Where(x => x.NeedAction == false || actionIds.Contains(x.Id)).Select(x => x.Id).ToArrayAsync();
            }           
        }

        /// <summary>
        /// 此处还可以优化
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        public async Task<List<Base_ActionTree>> GetUserMenuListAsync(string userId)
        {
            var actionIds = await GetUserActionIds(userId);
            return await _actionBus.GetTreeDataListAsync(new Base_ActionsInputDTO
            {
                Types = new ActionType[] { ActionType.菜单, ActionType.页面 },
                ActionIds = actionIds
            });
        }

        public async Task<List<string>> GetUserPermissionValuesAsync(string userId)
        {
            //var actionIds = await GetUserActionIds(userId);
            //return (await _actionBus
            //    .GetDataListAsync(new Base_ActionsInputDTO
            //    {
            //        Types = new ActionType[] { ActionType.权限 },
            //        ActionIds = actionIds
            //    }))
            //    .Select(x => x.Value)
            //    .ToList();

            var theUser = await _userBus.GetTheDataAsync(userId);

            if (userId == AdminTypes.Admin.ToString() || theUser.RoleType.HasFlag(RoleTypes.超级管理员))
            {
                return await GetIQueryable().Where(x => x.Type == ActionType.权限).Select(p => p.Value).ToListAsync();
            }
            else
            {
                var actionIds = await Db.Queryable<Base_UserRole>().LeftJoin<Base_RoleAction>((o, i) => o.RoleId == i.RoleId).Where(o => o.UserId == userId).Select((o, i) => i.ActionId).ToArrayAsync();

                //不需要权限的菜单和有权限的菜单集合
                return await GetIQueryable().Where(x => x.Type == ActionType.权限 && (x.NeedAction == false || actionIds.Contains(x.Id))).Select(x => x.Value).ToListAsync();
            }
        }
    }
}
