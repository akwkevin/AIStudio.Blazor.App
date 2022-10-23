using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Common.Types;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Entity.Enum;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util.Common;
using AIStudio.Util.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 访问日志
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_LogVisitController : ApiControllerBase
    {
        #region DI
        /// <summary>
        /// 访问日志控制器
        /// </summary>
        /// <param name="logBus"></param>
        public Base_LogVisitController(IBase_LogVisitBusiness logBus)
        {
            _logBus = logBus;
        }

        IBase_LogVisitBusiness _logBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 获取访问日志历史列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_LogVisit>> GetDataList(PageInput<HistorySearch> input)
        {
            return await _logBus.GetDataListAsync(input);
        }
        #endregion

        #region 提交

        #endregion
    }
}