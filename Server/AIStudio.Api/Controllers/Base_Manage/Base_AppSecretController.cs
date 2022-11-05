using AIStudio.Api.Controllers;
using AIStudio.Common.Filter.FilterAttribute;
using AIStudio.Common.Swagger;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using static AIStudio.Common.Authentication.Jwt.JwtHelper;

namespace AIStudio.Api.Controllers.Base_Manage
{
    /// <summary>
    /// 应用密钥
    /// </summary>

    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_AppSecretController : ApiControllerBase
    {
        #region DI
        IBase_AppSecretBusiness _appSecretBus { get; }

        /// <summary>
        /// 应用密钥控制器
        /// </summary>
        /// <param name="appSecretBus"></param>
        public Base_AppSecretController(IBase_AppSecretBusiness appSecretBus)
        {
            _appSecretBus = appSecretBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取应用密钥列表
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_AppSecret>> GetDataList(PageInput input)
        {
            return await _appSecretBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 根据Id获取应用密钥
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_AppSecret> GetTheData(IdInputDTO input)
        {
            return await _appSecretBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存应用密钥
        /// </summary>
        /// <param name="theData">保存的数据</param>
        [RequestRecord]
        [HttpPost]
        [Authorize(Permissions.Auto)]
        public async Task SaveData(Base_AppSecret theData)
        {
            await _appSecretBus.SaveDataAsync(theData);
        }

        /// <summary>
        /// 删除应用密钥
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [RequestRecord]
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _appSecretBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}