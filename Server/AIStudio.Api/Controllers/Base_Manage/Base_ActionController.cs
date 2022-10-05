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

        public Base_ActionController(IBase_ActionBusiness actionBus)
        {
            _actionBus = actionBus;
        }

        IBase_ActionBusiness _actionBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<Base_Action> GetTheData(IdInputDTO input)
        {
            return await _actionBus.GetTheDataAsync(input.id) ?? new Base_Action();
        }

        [HttpPost]
        public async Task<List<Base_Action>> GetPermissionList(Base_ActionsInputDTO input)
        {
            input.types = new ActionType[] { Entity.ActionType.权限 };

            return await _actionBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<Base_Action>> GetAllActionList()
        {
            return await _actionBus.GetDataListAsync(new Base_ActionsInputDTO
            {
                types = new ActionType[] { ActionType.菜单, ActionType.页面, ActionType.权限 }
            });
        }

        [HttpPost]
        public async Task<List<Base_ActionTree>> GetMenuTreeList(Base_ActionsInputDTO input)
        {
            input.selectable = true;
            input.types = new ActionType[] { ActionType.菜单, ActionType.页面 };

            return await _actionBus.GetTreeDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<Base_ActionTree>> GetActionTreeList(Base_ActionsInputDTO input)
        {
            input.selectable = false;

            return await _actionBus.GetTreeDataListAsync(input);
        }

        #endregion

        #region 提交

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

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _actionBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}