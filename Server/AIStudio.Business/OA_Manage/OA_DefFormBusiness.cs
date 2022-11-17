using AIStudio.Business;
using AIStudio.Business.OA_Manage;
using AIStudio.Business.OA_Manage.Steps;
using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Common.Service;
using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Entity.Enum;
using AIStudio.Entity.OA_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
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
        private readonly IOA_UserFormBusiness _oA_UserFormBus;
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
            IOA_UserFormBusiness oA_UserFormBus,            
            ILogger<OA_DefFormBusiness> logger,
            IServiceProvider serviceProvider)
            : base(db)
        {
            _mapper = mapper;
            _oA_UserFormBus = oA_UserFormBus; 
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

        public async Task<PageResult<OA_DefFormDTO>> GetDataListAsync(PageInput input)
        {
            var q = GetIQueryable(input.SearchKeyValues);     

            return await q.Select<OA_DefFormDTO>().GetPageResultAsync(input);
        }

        public async Task<OA_DefFormDTO> GetTheDataAsync(string id)
        {
            return _mapper.Map<OA_DefFormDTO>(await GetEntityAsync(id));
        }

        public async Task SaveDataAsync(OA_DefFormDTO theData)
        {
            if (theData.Id.IsNullOrEmpty())
            {
                theData.WorkflowJSON = OAExtension.InitOAData(theData.WorkflowJSON, theData.Id);

                //去掉事务，sqlite不支持
                //var res = await _oA_DefFormBus.RunTransactionAsync(async () =>
                //{
                var def = _definitionLoader.LoadDefinition(theData.WorkflowJSON, Deserializers.Json);
                theData.JSONId = def.Id;
                theData.JSONVersion = def.Version;
                theData.Status = 0;
                await AddDataAsync(theData);

                //});
                //if (!res.Success)
                //    throw res.ex;
            }
            else
            {
                //修改只能改基础属性
                await UpdateDataAsync(theData);
            }
        }

        public async Task StartDataAsync(IdInputDTO input)
        {
            var data = await GetTheDataAsync(input.id);
            data.Status = 1;

            await SaveDataAsync(data);
        }

        public async Task StopDataAsync(IdInputDTO input)
        {
            var data = await GetTheDataAsync(input.id);
            data.Status = 0;

            await SaveDataAsync(data);
        }

        public override async Task DeleteDataAsync(List<string> ids)
        {
            int count = _oA_UserFormBus.GetDataListCount(ids, OAStatus.Being);
            if (count > 0)
            {
                throw new Exception("还有正在使用该流程的审批,不能删除该流程");
            }
            await base.DeleteDataAsync(ids);
        }
        #endregion

        #region 私有成员

        #endregion

        #region 数据模型

        #endregion
    }




}