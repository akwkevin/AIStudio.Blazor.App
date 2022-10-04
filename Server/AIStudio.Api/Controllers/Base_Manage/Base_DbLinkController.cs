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
    [Authorize]
    public class Base_DbLinkController : ApiControllerBase
    {
        #region DI

        public Base_DbLinkController(IBase_DbLinkBusiness dbLinkBus)
        {
            _dbLinkBus = dbLinkBus;
        }

        IBase_DbLinkBusiness _dbLinkBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<AjaxResult<List<Base_DbLink>>> GetDataList(PageInput input)
        {
            return await _dbLinkBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_DbLink> GetTheData(IdInputDTO input)
        {
            return await _dbLinkBus.GetTheDataAsync(input.id) ?? new Base_DbLink();
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存
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
        /// 删除数据
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