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
    /// 数据库连接
    /// </summary>

    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_DbLinkController : ApiControllerBase
    {
        #region DI
        IBase_DbLinkBusiness _dbLinkBus { get; }

        /// <summary>
        /// 数据库连接控制器
        /// </summary>
        /// <param name="dbLinkBus"></param>
        public Base_DbLinkController(IBase_DbLinkBusiness dbLinkBus)
        {
            _dbLinkBus = dbLinkBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取数据库连接列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<List<Base_DbLink>>> GetDataList(PageInput input)
        {
            return await _dbLinkBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 根据Id获取数据库连接
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_DbLink> GetTheData(IdInputDTO input)
        {
            return await _dbLinkBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存数据库连接
        /// </summary>
        /// <param name="theData">保存的数据</param>
        [RequestRecord]
        [HttpPost]
        public async Task SaveData(Base_DbLink theData)
        {
            await _dbLinkBus.SaveDataAsync(theData);
        }

        /// <summary>
        /// 删除数据库连接
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [RequestRecord]
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _dbLinkBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}