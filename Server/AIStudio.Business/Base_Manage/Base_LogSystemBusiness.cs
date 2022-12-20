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
    public class Base_LogSystemBusiness : SplitTableBaseBusiness<Base_LogSystem>, IBase_LogSystemBusiness, ITransientDependency
    {

        public Base_LogSystemBusiness(ISqlSugarClient db) : base(db)
        {
            
        }

        public async Task Handle(SystemEvent @event)
        {
            Base_LogSystem log = new Base_LogSystem()
            {
                CreatorId = @event.CreatorId,
                CreatorName = @event.CreatorName,
                TenantId = @event.TenantId,
                LogType = @event.LogType,
                Name = @event.Name,
                Message = @event.Message
            };
            await InsertAsync(log);
        }
    }
}