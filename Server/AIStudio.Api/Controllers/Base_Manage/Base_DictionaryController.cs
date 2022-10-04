﻿using AIStudio.Api.Controllers;
using AIStudio.Common.Swagger;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Coldairarrow.Api.Controllers.Base_Manage
{
    [ApiExplorerSettings(GroupName = nameof(ApiVersionInfo.V1))]
    [Route("/Base_Manage/[controller]/[action]")]
    [Authorize]
    public class Base_DictionaryController : ApiControllerBase
    {
        #region DI

        public Base_DictionaryController(IBase_DictionaryBusiness base_DictionaryBus)
        {
            _base_DictionaryBus = base_DictionaryBus;
        }

        IBase_DictionaryBusiness _base_DictionaryBus { get; }

        #endregion

        #region 获取

        [HttpPost]
        public async Task<List<Base_Dictionary>> GetDataList(Base_DictionaryInputDTO input)
        {
            return await _base_DictionaryBus.GetDataListAsync(input);
        }

        [HttpPost]
        public async Task<List<Base_DictionaryTree>> GetTreeDataList(Base_DictionaryInputDTO input)
        {
            input.selectable = true;
            input.types = new DictionaryType[] { DictionaryType.字典项, DictionaryType.数据集 };

            return await _base_DictionaryBus.GetTreeDataListAsync(input);
        }

        [HttpPost]
        public async Task<Base_Dictionary> GetTheData(IdInputDTO input)
        {
            return await _base_DictionaryBus.GetTheDataAsync(input.id);
        }

        #endregion

        #region 提交

        [HttpPost]
        public async Task SaveData(Base_Dictionary data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                //InitEntity(data);

                await _base_DictionaryBus.AddDataAsync(data);
            }
            else
            {
                //UpdateEntity(data);

                await _base_DictionaryBus.UpdateDataAsync(data);
            }
        }

        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            await _base_DictionaryBus.DeleteDataAsync(ids);
        }

        #endregion
    }
}