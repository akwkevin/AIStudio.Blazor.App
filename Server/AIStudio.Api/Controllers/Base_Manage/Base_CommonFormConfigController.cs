using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.Common.Filters;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 通用查询配置
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_CommonFormConfigController : ApiControllerBase
    {
        #region DI
        IBase_CommonFormConfigBusiness _base_CommonFormConfigBus { get; }
        /// <summary>
        /// Base_CommonFormConfigController
        /// </summary>
        /// <param name="base_CommonFormConfigBus"></param>
        public Base_CommonFormConfigController(IBase_CommonFormConfigBusiness base_CommonFormConfigBus)
        {
            _base_CommonFormConfigBus = base_CommonFormConfigBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取数据列表Base_CommonFormConfig
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_CommonFormConfig>> GetDataList(PageInput input)
        {
            return await _base_CommonFormConfigBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取数据Base_CommonFormConfig
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_CommonFormConfig> GetTheData(IdInputDTO input)
        {
            return await _base_CommonFormConfigBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 保存数据Base_CommonFormConfig
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task SaveData(Base_CommonFormConfig data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                await _base_CommonFormConfigBus.AddDataAsync(data);
            }
            else
            {
                await _base_CommonFormConfigBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 删除数据Base_CommonFormConfig
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_CommonFormConfigBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}