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
        /// Base_DbLinkController
        /// </summary>
        /// <param name="dbLinkBus"></param>
        public Base_DbLinkController(IBase_DbLinkBusiness dbLinkBus)
        {
            _dbLinkBus = dbLinkBus;
        }     
        #endregion

        #region 获取
        /// <summary>
        /// 获取数据列表Base_DbLink
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<AjaxResult<List<Base_DbLink>>> GetDataList(PageInput input)
        {
            return await _dbLinkBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取数据Base_DbLink
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_DbLink> GetTheData(IdInputDTO input)
        {
            return await _dbLinkBus.GetTheDataAsync(input.id) ?? new Base_DbLink();
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存数据Base_DbLink
        /// </summary>
        /// <param name="theData">保存的数据</param>
        [HttpPost]
        public async Task SaveData(Base_DbLink theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                //InitEntity(theData);

                await _dbLinkBus.AddDataAsync(theData);
            }
            else
            {
                //UpdateEntity(theData);

                await _dbLinkBus.UpdateDataAsync(theData);
            }
        }

        /// <summary>
        /// 删除数据Base_DbLink
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _dbLinkBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}