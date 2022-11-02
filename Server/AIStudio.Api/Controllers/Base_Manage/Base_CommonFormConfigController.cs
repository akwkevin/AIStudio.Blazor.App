using AIStudio.Api.Controllers;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Simple.Common.Filters;

namespace AIStudio.Api.Controllers.Base_Manage
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
        /// 通用查询配置控制器
        /// </summary>
        /// <param name="base_CommonFormConfigBus"></param>
        public Base_CommonFormConfigController(IBase_CommonFormConfigBusiness base_CommonFormConfigBus)
        {
            _base_CommonFormConfigBus = base_CommonFormConfigBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取通用查询配置列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_CommonFormConfig>> GetDataList(PageInput input)
        {
            return await _base_CommonFormConfigBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 根据Id获取通用查询配置
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
        /// 保存通用查询配置
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task SaveData(Base_CommonFormConfig data)
        {
            await _base_CommonFormConfigBus.SaveDataAsync(data);
        }

        /// <summary>
        /// 删除通用查询配置
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