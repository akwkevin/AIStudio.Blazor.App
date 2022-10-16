using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_LogOperatingBusiness : ISplitTableBaseBusiness<Base_LogOperating>, IEventHandler<RequestEvent>
    {
        Task Handle(RequestEvent @event);
    }


}