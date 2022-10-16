using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_LogVisitBusiness : ISplitTableBaseBusiness<Base_LogVisit>, IEventHandler<VisitEvent>
    {
        Task Handle(VisitEvent @event);
    }


}