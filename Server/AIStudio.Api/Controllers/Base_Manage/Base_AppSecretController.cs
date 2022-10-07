﻿using AIStudio.Api.Controllers;
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
    /// 应用密钥
    /// </summary>
    
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    public class Base_AppSecretController : ApiControllerBase
    {
        #region DI
        IBase_AppSecretBusiness _appSecretBus { get; }

        /// <summary>
        /// Base_AppSecretController
        /// </summary>
        /// <param name="appSecretBus"></param>
        public Base_AppSecretController(IBase_AppSecretBusiness appSecretBus)
        {
            _appSecretBus = appSecretBus;
        }
        #endregion

        #region 获取
        /// <summary>
        /// 获取数据列表Base_AppSecret
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<PageResult<Base_AppSecret>> GetDataList(PageInput input)
        {
            return await _appSecretBus.GetDataListAsync(input);
        }

        /// <summary>
        /// 获取数据Base_AppSecret
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<Base_AppSecret> GetTheData(IdInputDTO input)
        {
            return await _appSecretBus.GetTheDataAsync(input.id) ?? new Base_AppSecret();
        }

        #endregion

        #region 提交

        /// <summary>
        /// 保存数据Base_AppSecret
        /// </summary>
        /// <param name="theData">保存的数据</param>
        [HttpPost]
        public async Task SaveData(Base_AppSecret theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                //InitEntity(theData);

                await _appSecretBus.AddDataAsync(theData);
            }
            else
            {
                //UpdateEntity(theData);
                await _appSecretBus.UpdateDataAsync(theData);
            }
        }

        /// <summary>
        /// 删除数据Base_AppSecret
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _appSecretBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}