using AIStudio.Common.DI;
using AIStudio.Common.EventBus.Models;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Microsoft.Extensions.Logging;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_LogSystemBusiness :BaseBusiness<Base_LogSystem>, IBase_LogSystemBusiness, ITransientDependency
    {

        public Base_LogSystemBusiness(ISqlSugarClient db) : base(db)
        {
            
        }

        public async Task<PageResult<Base_LogSystem>> GetLogListAsync(PageInput<Base_UserLogsInputDTO> input)
        {
            var search = input.Search;
            RefAsync<int> total = 0;
            var data = await GetIQueryable()
                .WhereIF(!search.logContent.IsNullOrEmpty(), x => x.Message.Contains(search.logContent))
                .WhereIF(!search.logType.IsNullOrEmpty(), x => x.LogType == search.logType)
                .WhereIF(!search.opUserName.IsNullOrEmpty(), x => x.CreatorName.Contains(search.opUserName))
                .WhereIF(!search.startTime.IsNullOrEmpty(), x => x.CreateTime >= search.startTime)
                .WhereIF(!search.endTime.IsNullOrEmpty(), x => x.CreateTime <= search.endTime)
                 .ToPageListAsync(input.PageIndex, input.PageRows, total);
            return new PageResult<Base_LogSystem> { Data = data, Total = total }; ;
        }
        public async Task<PageResult<Base_LogSystem>> GetLogList(PageInput input)
        {
            RefAsync<int> total = 0;
            var data = await GetIQueryable()
                 .ToPageListAsync(input.PageIndex, input.PageRows, total);
            return new PageResult<Base_LogSystem> { Data = data, Total = total }; ;
        }

        public async Task Handle(SystemEvent @event)
        {
            Base_LogSystem log = new Base_LogSystem()
            {
                CreatorId = @event.CreatorId,
                CreatorName = @event.CreatorName,
                TenantId = @event.TenantId,
                LogType = @event.LogType,
                Message = @event.LogContent
            };
            await InsertAsync(log);
        }
    }
}