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
    /// 系统日志
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_LogExceptionController : ApiControllerBase
    {
        #region DI
        /// <summary>
        /// Base_UserLogController
        /// </summary>
        /// <param name="logBus"></param>
        public Base_LogExceptionController(IBase_LogExceptionBusiness logBus)
        {
            _logBus = logBus;
        }

        IBase_LogExceptionBusiness _logBus { get; }

        #endregion

        #region 获取
        /// <summary>
        /// 
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_LogException>> GetDataList(PageInput<HistorySearch> input)
        {
            input.SortField = GlobalConst.CreateTime;
            input.SortType = "desc";

            return await _logBus.GetDataListAsync(input);
        }
        #endregion

        #region 提交

        #endregion
    }
}