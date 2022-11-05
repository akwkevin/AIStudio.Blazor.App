using AIStudio.Common.Authorization;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Http;
using SqlSugar;
using System.Security.Claims;
using System.Text;
using static AIStudio.Common.Authentication.Jwt.JwtHelper;

namespace AIStudio.Business.Base_Manage
{
    public class PermissionBusiness : BaseBusiness<Base_Action>, IPermissionBusiness, IPermissionChecker, ITransientDependency
    {
        private readonly IBase_ActionBusiness _actionBus;
        private readonly IBase_UserBusiness _userBus;
        private readonly IOperator _operator;
        private readonly IHttpContextAccessor _httpContextAccessor;
        public PermissionBusiness(IBase_ActionBusiness actionBus, IBase_UserBusiness userBus, IOperator @operator, IHttpContextAccessor httpContextAccessor, ISqlSugarClient db) : base(db)
        {
            _actionBus = actionBus;
            _userBus = userBus;
            _operator = @operator;
            _httpContextAccessor = httpContextAccessor;
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

        public async Task<List<string>> GetUserPermissionListAsync(string userId)
        {
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

        public async Task<List<string>> GetAllPermissionListAsync()
        {
            return await GetIQueryable().Where(x => x.Type == ActionType.权限).Select(p => p.Value).ToListAsync();
        }

        /// <summary>
        /// 鉴权
        /// </summary>
        /// <param name="claimsPrincipal"></param>
        /// <param name="name"></param>
        /// <returns></returns>
        public virtual async Task<bool> IsGrantedAsync(ClaimsPrincipal claimsPrincipal, string name)
        {
            // 如果当前用户是超级管理员，跳过验证
            if (_operator.IsSuperAdmin) return true;

            string permission = name;

            if (permission == Permissions.Auto)
            {
                var request = _httpContextAccessor.HttpContext.Request;
                request.RouteValues.TryGetValue("action", out var action);
                request.RouteValues.TryGetValue("controller", out var controller);

                if (action?.ToString() == "SaveData")
                {
                    request.Body.Seek(0, SeekOrigin.Begin);
                    var reader = new StreamReader(request.Body, Encoding.UTF8);
                    string body = await reader.ReadToEndAsync();
                    request.Body.Seek(0, SeekOrigin.Begin);

                    var id = body.ToObject<IdInputDTO>();

                    if (id != null && !string.IsNullOrEmpty(id.id))
                    {
                        action = "Edit";
                    }
                    else
                    {
                        action = "Add";
                    }
                }
                else if (action?.ToString() == "DeleteData")
                {
                    action = "Delete";
                }

                // 路径形如：/Base_Manage/Base_AppSecret/SaveData 转化为 Base_AppSecret.AddData
                permission = controller + "." + action;
            }

            if (string.IsNullOrEmpty(permission)) return false;

            //// 获取录入系统中的所有权限
            //List<string> allPermissions = await GetAllPermissionListAsync();

            //// 如果没有配置该权限，则不限制该权限，通过验证
            //if (!allPermissions.Contains(permission)) return true;

            // 获取当前用户的所有权限
            List<string> permissions = await GetUserPermissionListAsync(_operator.UserId);

            // 如果当前用户拥有对应权限，则通过验证
            if (permissions.Contains(permission)) return true;

            return true;
        }
    }
}
