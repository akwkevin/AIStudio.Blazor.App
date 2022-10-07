using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 系统用户
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_UserController : ApiControllerBase
    {
        #region DI
        IBase_UserBusiness _userBus { get; }

        /// <summary>
        /// Base_UserController
        /// </summary>
        /// <param name="userBus"></param>
        public Base_UserController(IBase_UserBusiness userBus)
        {
            _userBus = userBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取数据列表Base_User
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_UserDTO>> GetDataList(PageInput input)
        {
            return await _userBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取数据Base_User
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_UserDTO> GetTheData(IdInputDTO input)
        {
            return await _userBus.GetTheDataAsync(input.id) ?? new Base_UserDTO();
        }

        /// <summary>
        /// 获取下拉参数Base_User
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<SelectOption>> GetOptionList(PageInput input)
        {
            return await _userBus.GetOptionListAsync(input);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 保存数据Base_User
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task SaveData(Base_UserEditInputDTO input)
        {
            if (!input.newPwd.IsNullOrEmpty())
                input.Password = input.newPwd.ToMD5String();
            else if (!input.Password.IsNullOrEmpty() && !input.Password.IsMd5())
            {
                input.Password = input.Password.ToMD5String();
            }

            if (input.Id.IsNullOrEmpty())
            {
                await _userBus.AddDataAsync(input);
            }
            else
            {
                await _userBus.UpdateDataAsync(input);
            }
        }

        /// <summary>
        /// 删除数据Base_User
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _userBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}