using AIStudio.Business.OA_Manage;
using AIStudio.Business.OA_Manage.Steps;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.Swagger;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;
using static AIStudio.Common.Authentication.Jwt.JwtHelper;

namespace AIStudio.Api.Controllers.OA_Manage
{
    /// <summary>
    /// 流程定义
    /// </summary>
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/OA_Manage/[controller]/[action]")]
    public class OA_DefFormController : ApiControllerBase
    {
        #region DI
        IOA_DefFormBusiness _oA_DefFormBus { get; }
        /// <summary>
        /// 流程定义控制器
        /// </summary>
        /// <param name="oA_DefFormBus"></param>
        public OA_DefFormController(IOA_DefFormBusiness oA_DefFormBus)
        {
            _oA_DefFormBus = oA_DefFormBus;
        }

        #endregion

        #region 获取

        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="input">分页参数</param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<OA_DefFormDTO>> GetDataList(PageInput input)
        {
            var dataList = await _oA_DefFormBus.GetDataListAsync(input);
            return dataList;
        }

        /// <summary>
        /// 获取树列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        #region
        [HttpPost]
        public async Task<List<OA_DefFormTree>> GetTreeDataList(SearchInput input)
        {
            var dataList = await _oA_DefFormBus.GetTreeDataListAsync(input);
            return dataList;
        }
        #endregion

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<OA_DefFormDTO> GetTheData(IdInputDTO input)
        {
            return await _oA_DefFormBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存
        /// </summary>
        /// <param name="data">保存的数据</param>
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task SaveData(OA_DefFormDTO data)
        {
            await _oA_DefFormBus.SaveDataAsync(data);           
        }

        /// <summary>
        /// 启动数据
        /// </summary>
        /// <param name="ids"></param>
        [HttpPost]
        [Authorize("Edit")]
        public async Task StartData(List<string> ids)
        {
            await _oA_DefFormBus.StartDataAsync(ids);
        }

        /// <summary>
        /// 停用数据
        /// </summary>
        /// <param name="ids"></param>
        [HttpPost]
        [Authorize("Edit")]
        public async Task StopData(List<string> ids)
        {
            await _oA_DefFormBus.StopDataAsync(ids);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task DeleteData(List<string> ids)
        {
            await _oA_DefFormBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}