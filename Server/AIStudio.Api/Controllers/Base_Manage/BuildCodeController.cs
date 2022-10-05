using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 代码生成
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class BuildCodeController : ApiControllerBase
    {
        #region DI

        public BuildCodeController(IBuildCodeBusiness buildCodeBus)
        {
            _buildCodeBus = buildCodeBus;
        }

        IBuildCodeBusiness _buildCodeBus { get; }

        #endregion

        [HttpPost]
        public List<Base_DbLink> GetAllDbLink()
        {
            return _buildCodeBus.GetAllDbLink();
        }

        [HttpPost]
        public List<DbTableInfo> GetDbTableList(DbTablesInputDTO input)
        {
            return _buildCodeBus.GetDbTableList(input.linkId);
        }

        [HttpPost]
        public void Build(BuildInputDTO input)
        {
            _buildCodeBus.Build(input);
        }

        [HttpPost]
        public Dictionary<string, List<TableInfo>> GetDbTableInfo(BuildInputDTO input)
        {
            return _buildCodeBus.GetDbTableInfo(input);
        }
    }
}