using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
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
    public class Base_UserLogController : ApiControllerBase
    {
        #region DI

        public Base_UserLogController(IBase_UserLogBusiness logBus)
        {
            _logBus = logBus;
        }

        IBase_UserLogBusiness _logBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<PageResult<Base_UserLog>> GetLogList(PageInput<Base_UserLogsInputDTO> input)
        {
            input.SortField = "CreateTime";
            input.SortType = "desc";

            return await _logBus.GetLogListAsync(input);
        }

        [HttpPost]
        public List<SelectOption> GetLogTypeList()
        {
            return EnumHelper.ToOptionList(typeof(UserLogType));
        }

        #endregion

        #region 提交

        #endregion
    }
}