﻿using AIStudio.Common.DI;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    class PermissionBusiness : IPermissionBusiness, ITransientDependency
    {
        ISqlSugarClient Db;
        public PermissionBusiness(IBase_ActionBusiness actionBus, IBase_UserBusiness userBus, ISqlSugarClient su)
        {
            _actionBus = actionBus;
            _userBus = userBus;
            Db = su;
        }
        IBase_ActionBusiness _actionBus { get; }
        IBase_UserBusiness _userBus { get; }

        async Task<string[]> GetUserActionIds(string userId)
        {
            var theUser = await _userBus.GetTheDataDTOAsync(userId);
            var P = await Db.Queryable<Base_UserRole, Base_RoleAction>((x, r) => new object[] {
                JoinType.Inner,x.RoleId.Equals(r.RoleId)
                }).WhereIF(userId != AdminTypes.Admin.ToString() || !theUser.RoleType.HasFlag(RoleTypes.超级管理员), (x, r) => x.UserId.Equals(userId))
            .Select((x, r) => r.ActionId).ToListAsync();
            return await Db.Queryable<Base_Action>().Where(x => P.Contains(x.Id)).Select(x => x.Id).ToArrayAsync();
        }
        public async Task<List<Base_ActionTree>> GetUserMenuListAsync(string userId)
        {
            var actionIds = await GetUserActionIds(userId);
            return await _actionBus.GetTreeDataListAsync(new Base_ActionsInputDTO
            {
                types = new ActionType[] { ActionType.菜单, ActionType.页面 },
                ActionIds = actionIds,
                checkEmptyChildren = true
            });
        }

        public async Task<List<string>> GetUserPermissionValuesAsync(string userId)
        {
            var actionIds = await GetUserActionIds(userId);
            return (await _actionBus
                .GetDataListAsync(new Base_ActionsInputDTO
                {
                    types = new ActionType[] { ActionType.权限 },
                    ActionIds = actionIds
                }))
                .Select(x => x.Value)
                .ToList();
        }
    }
}