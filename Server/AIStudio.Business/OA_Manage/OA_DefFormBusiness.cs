using AIStudio.Business;
using AIStudio.Business.OA_Manage;
using AIStudio.Business.OA_Manage.Steps;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Common.IdGenerator;
using AIStudio.Common.Service;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Entity.Enum;
using AIStudio.Entity.OA_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.DiagramEntity;
using AIStudio.Util.Helper;
using AutoMapper;
using Castle.Core.Logging;
using LinqKit;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Quartz.Impl.AdoJobStore.Common;
using SqlSugar;
using System.Linq.Dynamic.Core;
using WorkflowCore.Interface;
using WorkflowCore.Services.DefinitionStorage;

namespace AIStudio.Business.OA_Manage
{
    public class OA_DefFormBusiness : BaseBusiness<OA_DefForm>, IOA_DefFormBusiness, ITransientDependency
    {
        private readonly IMapper _mapper;
        private readonly ILogger<OA_DefFormBusiness> _logger;
        private readonly IDefinitionLoader _definitionLoader;

        /// <summary>
        /// 流程定义
        /// </summary>
        /// <param name="db"></param>
        /// <param name="mapper"></param>
        /// <param name="oA_UserFormBus"></param>
        /// <param name="logger"></param>
        /// <param name="serviceProvider"></param>
        public OA_DefFormBusiness(ISqlSugarClient db,
            IMapper mapper,
            ILogger<OA_DefFormBusiness> logger,
            IServiceProvider serviceProvider)
            : base(db)
        {
            _mapper = mapper;
            _logger = logger;
            _definitionLoader = serviceProvider.GetRequiredService<IDefinitionLoader>();
        }

        #region 外部接口
        public async Task LoadDefinitionAsync()
        {
            var defForms = await GetListAsync();
            LoadDefinition(defForms);
        }

        public void LoadDefinition()
        {
            var defForms = GetList();
            LoadDefinition(defForms);
        }

        private void LoadDefinition(List<OA_DefForm> defForms)
        {
            foreach (var defform in defForms)
            {
                try
                {
                    var def = _definitionLoader.LoadDefinition(defform.WorkflowJSON, Deserializers.Json);
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Debug, new EventId((int)UserLogType.工作流程, UserLogType.工作流程.ToString()), "工作流" + def.Id + "-" + def.Version + "加载成功");
                }
                catch (Exception ex)
                {
                    _logger.Log(Microsoft.Extensions.Logging.LogLevel.Error, new EventId((int)UserLogType.工作流程, UserLogType.工作流程.ToString()), "工作流" + defform.Name + "-" + ex.Message);
                }
            }
        }

        public async Task<List<OA_DefFormTree>> GetTreeDataListAsync(SearchInput input)
        {
            var list = await GetIQueryable(input.SearchKeyValues).ToListAsync();

            List<OA_DefFormTree> treeList = new List<OA_DefFormTree>();
            foreach (var data in list.GroupBy(p => p.Type))
            {
                OA_DefFormTree node = new OA_DefFormTree()
                {
                    Id = data.Key,
                    Text = data.Key,
                    Value = data.Key
                };

                node.Children = data.Select(x => new OA_DefFormTree
                {
                    Id = x.Id,
                    Text = x.Name,
                    Value = x.Id,
                    type = data.Key,
                    jsonId = x.JSONId,
                    jsonVersion = x.JSONVersion,
                    json = x.WorkflowJSON
                }).ToList();

                treeList.Add(node);
            }

            return treeList;
        }

        /// <summary>
        /// Gets the data list asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public async Task<PageResult<OA_DefFormDTO>> GetDataListAsync(PageInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);

            return await q.Select<OA_DefFormDTO>().GetPageResultAsync(input);
        }

        /// <summary>
        /// Gets the data asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public async Task<OA_DefFormDTO> GetTheDataAsync(string id)
        {
            return _mapper.Map<OA_DefFormDTO>(await GetEntityAsync(id));
        }

        /// <summary>
        /// Saves the data asynchronous.
        /// </summary>
        /// <param name="theData">The data.</param>
        public async Task SaveDataAsync(OA_DefFormDTO theData)
        {
            theData.WorkflowJSON = OAExtension.InitOAData(theData.WorkflowJSON, IdHelper.GetId());

            //去掉事务，sqlite不支持
            //var res = await _oA_DefFormBus.RunTransactionAsync(async () =>
            //{
            var def = _definitionLoader.LoadDefinition(theData.WorkflowJSON, Deserializers.Json);
            theData.JSONId = def.Id;
            theData.JSONVersion = def.Version;
            theData.Status = 0;
            if (theData.Id.IsNullOrEmpty())
            {
                await AddDataAsync(theData);
            }
            else
            {
                await UpdateDataAsync(theData);
            }         
            //});
            //if (!res.Success)
            //    throw res.ex;
        }

        /// <summary>
        /// Starts the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        public async Task StartDataAsync(IdInputDTO input)
        {
            var data = await GetTheDataAsync(input.id);
            data.Status = 1;

            await SaveDataAsync(data);
        }

        /// <summary>
        /// Stops the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        public async Task StopDataAsync(IdInputDTO input)
        {
            var data = await GetTheDataAsync(input.id);
            data.Status = 0;

            await SaveDataAsync(data);
        }

        /// <summary>
        /// Deletes the data asynchronous.
        /// </summary>
        /// <param name="ids">The ids.</param>
        /// <exception cref="System.Exception">还有正在使用该流程的审批,不能删除该流程</exception>
        public override async Task DeleteDataAsync(List<string> ids)
        {
            await base.DeleteDataAsync(ids);
        }
        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }




}