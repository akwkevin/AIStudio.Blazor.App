using AIStudio.Api.Controllers;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.Common.Filters;
using static AIStudio.Common.Authentication.Jwt.JwtHelper;

namespace AIStudio.Api.Controllers.Base_Manage
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
        /// 系统用户控制器
        /// </summary>
        /// <param name="userBus"></param>
        public Base_UserController(IBase_UserBusiness userBus)
        {
            _userBus = userBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取系统用户列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_UserDTO>> GetDataList(PageInput input)
        {
            return await _userBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 根据Id获取系统用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_UserDTO> GetTheData(IdInputDTO input)
        {
            return await _userBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 获取系统用户下拉数据源
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
        /// 保存系统用户
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task SaveData(Base_UserEditInputDTO input)
        {
            await _userBus.SaveDataAsync(input);
        }

        /// <summary>
        /// 删除系统用户
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task DeleteData(List<string> ids)
        {
            await _userBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}