using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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

        public Base_DepartmentController(IBase_DepartmentBusiness departmentBus)
        {
            _departmentBus = departmentBus;
        }

        IBase_DepartmentBusiness _departmentBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<Base_Department> GetTheData(IdInputDTO input)
        {
            return await _departmentBus.GetTheDataAsync(input.id) ?? new Base_Department();
        }

        [HttpPost]
        public async Task<List<Base_DepartmentTree>> GetTreeDataList(SearchInput input)
        {
            return await _departmentBus.GetTreeDataListAsync(input);
        }
        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_Department theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                //InitEntity(theData);

                await _departmentBus.AddDataAsync(theData);
            }
            else
            {
                //UpdateEntity(theData);

                await _departmentBus.UpdateDataAsync(theData);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _departmentBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}