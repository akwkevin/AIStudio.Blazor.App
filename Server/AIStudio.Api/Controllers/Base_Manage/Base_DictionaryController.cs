using AIStudio.Api.Controllers;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity;
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
    /// 系统字典
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DictionaryController : ApiControllerBase
    {
        #region DI
        IBase_DictionaryBusiness _base_DictionaryBus { get; }

        /// <summary>
        /// 系统字典控制器
        /// </summary>
        /// <param name="base_DictionaryBus"></param>
        public Base_DictionaryController(IBase_DictionaryBusiness base_DictionaryBus)
        {
            _base_DictionaryBus = base_DictionaryBus;
        }
        #endregion

        #region 获取

        /// <summary>
        /// 获取数据系统字典列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_Dictionary>> GetDataList(SearchInput input)
        {
            return await _base_DictionaryBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取系统字典树
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<List<Base_DictionaryTree>> GetTreeDataList(SearchInput input)
        {
            return await _base_DictionaryBus.GetTreeDataListAsync(input);
        }

        /// <summary>
        /// 根据Id获取系统字典
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_Dictionary> GetTheData(IdInputDTO input)
        {
            return await _base_DictionaryBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交
        /// <summary>
        /// 保存系统字典
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task SaveData(Base_Dictionary data)
        {
            await _base_DictionaryBus.SaveDataAsync(data);
        }

        /// <summary>
        /// 删除系统字典
        /// </summary>
        /// <param name="ids"></param>
        /// <returns></returns>
        [RequestRecord]
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_DictionaryBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}