using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.DbFactory.DataAccess;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AIStudio.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 代码生成
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class BuildCodeController : ApiControllerBase
    {
        #region DI
        IBuildCodeBusiness _buildCodeBus { get; }

        /// <summary>
        /// 代码生成控制器
        /// </summary>
        /// <param name="buildCodeBus"></param>

        public BuildCodeController(IBuildCodeBusiness buildCodeBus)
        {
            _buildCodeBus = buildCodeBus;
        }
        #endregion

        /// <summary>
        /// 获取连接数据库列表
        /// </summary>
        /// <returns></returns>
        [HttpPost]
        public List<Base_DbLink> GetAllDbLink()
        {
            return _buildCodeBus.GetAllDbLink();
        }

        /// <summary>
        /// 获取连接数据库的所有表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public List<DbTableInfo> GetDbTableList(DbTablesInputDTO input)
        {
            return _buildCodeBus.GetDbTableList(input.linkId);
        }

        /// <summary>
        /// 代码生成
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public void Build(BuildInputDTO input)
        {
            _buildCodeBus.Build(input);
        }

        /// <summary>
        /// 获取多个表信息
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public Dictionary<string, List<TableInfo>> GetDbTableInfo(BuildInputDTO input)
        {
            return _buildCodeBus.GetDbTableInfo(input);
        }
    }
}