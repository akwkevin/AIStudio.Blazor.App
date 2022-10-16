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
    public class Base_LogExceptionBusiness : SplitTableBaseBusiness<Base_LogException>, IBase_LogExceptionBusiness, ITransientDependency
    {

        public Base_LogExceptionBusiness(ISqlSugarClient db) : base(db)
        {
            
        }

        public async Task Handle(ExceptionEvent @event)
        {
            Base_LogException log = new Base_LogException()
            {
                CreatorId = @event.CreatorId,
                CreatorName = @event.CreatorName,
                TenantId = @event.TenantId,
                EventId = @event.Id,
                Name = @event.Name,
                Message = @event.Message,
                ClassName = @event.ClassName,
                MethodName = @event.MethodName,
                ExceptionSource = @event.ExceptionSource,
                StackTrace = @event.StackTrace,
                Parameters = @event.Parameters,
                LogTime = @event.ExceptionTime,
            };

            await InsertAsync(log);
        }
    }
}