using AIStudio.Api.Controllers;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
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
    /// 系统角色
    /// </summary>

    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_RoleController : ApiControllerBase
    {
        #region DI
        IBase_RoleBusiness _roleBus { get; }

        /// <summary>
        /// 系统角色控制器
        /// </summary>
        /// <param name="roleBus"></param>
        public Base_RoleController(IBase_RoleBusiness roleBus)
        {
            _roleBus = roleBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取系统角色列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_RoleEditInputDTO>> GetDataList(PageInput input)
        {
            return await _roleBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 根据Id获取系统角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_RoleEditInputDTO> GetTheData(IdInputDTO input)
        {
            return await _roleBus.GetTheDataAsync(input.id);
        }

        /// <summary>
        /// 获取系统角色下拉数据源
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<SelectOption>> GetOptionList(PageInput input)
        {
            return await _roleBus.GetOptionListAsync(input);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 保存系统角色
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task SaveData(Base_RoleEditInputDTO input)
        {
            await _roleBus.SaveDataAsync(input);
        }

        /// <summary>
        /// 删除系统角色
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _roleBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}