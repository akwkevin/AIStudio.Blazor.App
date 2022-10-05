using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_CommonFormConfigController : ApiControllerBase
    {
        #region DI

        public Base_CommonFormConfigController(IBase_CommonFormConfigBusiness base_CommonFormConfigBus)
        {
            _base_CommonFormConfigBus = base_CommonFormConfigBus;
        }

        IBase_CommonFormConfigBusiness _base_CommonFormConfigBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_CommonFormConfig>> GetDataList(PageInput<ConditionDTO> input)
        {
            return await _base_CommonFormConfigBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_CommonFormConfig> GetTheData(IdInputDTO input)
        {
            return await _base_CommonFormConfigBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_CommonFormConfig data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                //InitEntity(data);

                await _base_CommonFormConfigBus.AddDataAsync(data);
            }
            else
            {
                await _base_CommonFormConfigBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_CommonFormConfigBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}