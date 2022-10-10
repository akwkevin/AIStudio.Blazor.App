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
        /// Base_ActionController
        /// </summary>
        /// <param name="actionBus"></param>
        public Base_ActionController(IBase_ActionBusiness actionBus)
        {
            _actionBus = actionBus;
        }
        #endregion

        #region 获取     
        /// <summary>
        /// 获取数据列表Base_Action
        /// </summary>
        [HttpPost]
        public async Task<List<Base_Action>> GetAllActionList()
        {
            return await _actionBus.GetDataListAsync(new Base_ActionsInputDTO
            {
                types = new ActionType[] { ActionType.菜单, ActionType.页面, ActionType.权限 }
            });
        }

        /// <summary>
        /// 获取数据树Base_ActionTree
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionTree>> GetMenuTreeList(Base_ActionsInputDTO input)
        {

            input.types = new ActionType[] { ActionType.菜单, ActionType.页面 };

            return await _actionBus.GetTreeDataListAsync(input);
        }

        /// <summary>
        /// 获取数据树Base_ActionTree
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_ActionTree>> GetActionTreeList(Base_ActionsInputDTO input)
        {
            return await _actionBus.GetTreeDataListAsync(input);
        }

        /// <summary>
        /// 获取数据Base_Action
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_Action> GetTheData(IdInputDTO input)
        {
            return await _actionBus.GetTheDataAsync(input.id) ?? new Base_Action();
        }

        /// <summary>
        /// 获取权限
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_Action>> GetPermissionList(Base_ActionsInputDTO input)
        {
            input.types = new ActionType[] { Entity.ActionType.权限 };

            return await _actionBus.GetDataListAsync(input);
        }
        #endregion

        #region 提交
        /// <summary>
        /// 保存数据Base_Action
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]  
        public async Task SaveData(Base_ActionEditInputDTO input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                //InitEntity(input);

                await _actionBus.AddDataAsync(input);
            }
            else
            {
                //UpdateEntity(input);

                await _actionBus.UpdateDataAsync(input);
            }
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _actionBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}