using AIStudio.Api.Controllers;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.Common.Filters;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 部门
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DepartmentController : ApiControllerBase
    {
        #region DI
        IBase_DepartmentBusiness _departmentBus { get; }

        /// <summary>
        /// 部门控制器
        /// </summary>
        /// <param name="departmentBus"></param>
        public Base_DepartmentController(IBase_DepartmentBusiness departmentBus)
        {
            _departmentBus = departmentBus;
        }
        #endregion

        #region 获取

        /// <summary>
        /// 获取部门树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_DepartmentTree>> GetTreeDataList(SearchInput input)
        {
            return await _departmentBus.GetTreeDataListAsync(input);
        }

        /// <summary>
        /// 根据Id获取部门
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_Department> GetTheData(IdInputDTO input)
        {
            return await _departmentBus.GetTheDataAsync(input.id);
        }
        #endregion

        #region 提交
        /// <summary>
        /// 保存部门
        /// </summary>
        /// <param name="theData"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task SaveData(Base_Department theData)
        {
            await _departmentBus.SaveDataAsync(theData);
        }

        /// <summary>
        /// 删除部门
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _departmentBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}