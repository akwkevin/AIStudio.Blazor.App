using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.Common.Filters;
using static AIStudio.Common.Authentication.Jwt.JwtHelper;

namespace AIStudio.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 系统权限
    /// </summary>

    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_ActionController : ApiControllerBase
    {
        #region DI
        IBase_ActionBusiness _actionBus { get; }

        /// <summary>
        /// 系统权限控制器
        /// </summary>
        /// <param name="actionBus"></param>
        public Base_ActionController(IBase_ActionBusiness actionBus)
        {
            _actionBus = actionBus;
        }
        #endregion

        #region 获取     
        /// <summary>
        /// 获取所有权限列表
        /// </summary>
        [HttpPost]
        public async Task<List<Base_Action>> GetAllActionList()
        {
            return await _actionBus.GetAllActionListAsync();
        }

        /// <summary>
        /// 获取菜单树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionTree>> GetMenuTreeList(Base_ActionsInputDTO input)
        {
            return await _actionBus.GetMenuTreeListAsync(input);
        }

        /// <summary>
        /// 获取角色权限设置树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionTree>> GetActionTreeList(Base_ActionsInputDTO input)
        {
            return await _actionBus.GetActionTreeListAsync(input);
        }

        /// <summary>
        /// 根据Id获取权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_Action> GetTheData(IdInputDTO input)
        {
            return await _actionBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 获取动作权限列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_Action>> GetPermissionList(Base_ActionsInputDTO input)
        {
            return await _actionBus.GetPermissionListAsync(input);
        }
        #endregion

        #region 提交
        /// <summary>
        /// 保存权限数据
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task SaveData(Base_ActionEditInputDTO input)
        {
            await _actionBus.SaveDataAsync(input);
        }

        /// <summary>
        /// 删除权限数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task DeleteData(List<string> ids)
        {
            await _actionBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}