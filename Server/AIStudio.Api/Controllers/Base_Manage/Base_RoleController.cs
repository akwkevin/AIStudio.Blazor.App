using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 系统角色
    /// </summary>
    
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    [Authorize]
    public class Base_RoleController : ApiControllerBase
    {
        #region DI

        public Base_RoleController(IBase_RoleBusiness roleBus)
        {
            _roleBus = roleBus;
        }

        IBase_RoleBusiness _roleBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_RoleInfoDTO>> GetDataList(PageInput<RolesInputDTO> input)
        {
            return await _roleBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_RoleInfoDTO> GetTheData(IdInputDTO input)
        {
            return await _roleBus.GetTheDataDTOAsync(input.id) ?? new Base_RoleInfoDTO();
        }

        [HttpPost]
        public async Task<List<SelectOption>> GetOptionList(PageInput input)
        {
            return await _roleBus.GetOptionListAsync(input);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_RoleInfoDTO input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                //InitEntity(input);

                await _roleBus.AddDataAsync(input);
            }
            else
            {
                //UpdateEntity(input);

                await _roleBus.UpdateDataAsync(input);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _roleBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}