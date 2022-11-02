using AIStudio.Api.Controllers;
using AIStudio.Business.OA_Manage;
using AIStudio.Business.OA_Manage.Step;
using AIStudio.Common.CurrentUser;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Mvc;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;

namespace AIStudio.Api.Controllers.OA_Manage
{
    [Route("/OA_Manage/[controller]/[action]")]
    public class OA_DefFormController : ApiControllerBase
    {
        #region DI

        public OA_DefFormController(IOA_DefFormBusiness oA_DefFormBus, IOA_UserFormBusiness oA_UserFormBus, IDefinitionLoader definitionLoader, IWorkflowRegistry workflowRegistry, IOperator ioperator )
        {
            _oA_DefFormBus = oA_DefFormBus;
            _oA_UserFormBus = oA_UserFormBus;
            _definitionLoader = definitionLoader;
            _workflowRegistry = workflowRegistry;
            _operator = ioperator;
        }

        IOA_DefFormBusiness _oA_DefFormBus { get; }
        IOA_UserFormBusiness _oA_UserFormBus { get; }
        IDefinitionLoader _definitionLoader { get; }
        IWorkflowRegistry _workflowRegistry { get; }
        IOperator _operator { get; }
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

        #region
        [HttpPost]
        public async Task<List<OA_DefFormTree>> GetTreeDataList(string type)
        {
            //var roleidlist = _operator.Property.RoleIdList;
            //return await _oA_DefFormBus.GetTreeDataListAsync(type, roleidlist);

            throw new NotImplementedException();
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
        public async Task SaveData(OA_DefFormDTO data)
        {
            if (data.Id.IsNullOrEmpty())
            {
                data.WorkflowJSON = OAExtension.InitOAData(data.WorkflowJSON, data.Id);

                //去掉事务，sqlite不支持
                //var res = await _oA_DefFormBus.RunTransactionAsync(async () =>
                //{
                    var def = _definitionLoader.LoadDefinition(data.WorkflowJSON, Deserializers.Json);
                    data.JSONId = def.Id;
                    data.JSONVersion = def.Version;
                    data.Status = 0;
                    await _oA_DefFormBus.AddDataAsync(data);

                //});
                //if (!res.Success)
                //    throw res.ex;
            }
            else
            {
                //修改只能改基础属性
                await _oA_DefFormBus.UpdateDataAsync(data);
            }
        }

        /// <summary>
        /// 启动数据
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task StartData(IdInputDTO input)
        {
            var data = await _oA_DefFormBus.GetTheDataAsync(input.id);
            data.Status = 1;           

            await SaveData(data);           
        }

        /// <summary>
        /// 停用数据
        /// </summary>
        /// <param name="input"></param>
        [HttpPost]
        public async Task StopData(IdInputDTO input)
        {
            var data = await _oA_DefFormBus.GetTheDataAsync(input.id);
            data.Status = 0;

            await SaveData(data);
        }

        /// <summary>
        /// 删除数据
        /// </summary>
        /// <param name="ids">id数组,JSON数组</param>
        [HttpPost]
        public async Task DeleteData(List<string> ids)
        {
            int count =  _oA_UserFormBus.GetDataListCount(ids, OAStatus.Being);
            if (count > 0)
            {
                throw new Exception("还有正在使用该流程的审批,不能删除该流程");
            }
            await _oA_DefFormBus.DeleteDataAsync(ids);

        }

        #endregion
    }
}