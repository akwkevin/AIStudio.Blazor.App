using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    [Authorize]
    public class Base_UserController : ApiControllerBase
    {
        #region DI

        public Base_UserController(IBase_UserBusiness userBus)
        {
            _userBus = userBus;
        }

        IBase_UserBusiness _userBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_UserDTO>> GetDataList(PageInput<Base_UsersInputDTO> input)
        {
            return await _userBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<object> GetDataListByDepartment(IdInputDTO input)
        {
            return await _userBus.GetDataListByDepartmentAsync(input.id);
        }

        [HttpPost]
        public async Task<Base_UserDTO> GetTheData(IdInputDTO input)
        {
            return await _userBus.GetTheDataDTOAsync(input.id) ?? new Base_UserDTO();
        }

        [HttpPost]
        public async Task<List<SelectOption>> GetOptionList(PageInput input)
        {
            return await _userBus.GetOptionListAsync(input);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(UserEditInputDTO input)
        {
            if (!input.newPwd.IsNullOrEmpty())
                input.Password = input.newPwd.ToMD5String();
            else if (!input.Password.IsNullOrEmpty() && !input.Password.IsMd5())
            {
                input.Password = input.Password.ToMD5String();
            }

            if (input.Id.IsNullOrEmpty())
            {
                //InitEntity(input);

                await _userBus.AddDataAsync(input);
            }
            else
            {
                //UpdateEntity(input);

                await _userBus.UpdateDataAsync(input);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _userBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}