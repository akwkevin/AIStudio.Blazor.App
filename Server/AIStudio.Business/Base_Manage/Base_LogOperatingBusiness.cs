using AIStudio.Common.DI;
using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_LogOperatingBusiness : BaseBusiness<Base_LogOperating>, IBase_LogOperatingBusiness, ITransientDependency
    {

        public Base_LogOperatingBusiness(ISqlSugarClient db) : base(db)
        {
            
        }

        public async Task Handle(RequestEvent @event)
        {
            Base_LogOperating log = new Base_LogOperating()
            {
                CreatorId = @event.CreatorId,
                CreatorName = @event.CreatorName,
                TenantId = @event.TenantId,
                Name = @event.Name,
                IsSuccess = @event.IsSuccess,
                Message = @event.Message,
                Result = @event.Result,
                Browser = @event.Browser,
                OperatingSystem = @event.OperatingSystem,
                Ip = @event.Ip,
                Url = @event.Url,
                Path = @event.Path,
                ClassName = @event.ClassName,
                MethodName = @event.MethodName,
                RequestMethod = @event.RequestMethod,
                Body = @event.Body,
                ElapsedTime = @event.ElapsedTime,
                LogTime = @event.OperatingTime
            };

            await InsertAsync(log);
        }
    }
}