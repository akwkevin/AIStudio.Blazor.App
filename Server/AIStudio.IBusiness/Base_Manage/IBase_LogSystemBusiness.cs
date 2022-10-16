using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_LogSystemBusiness : ISplitTableBaseBusiness<Base_LogSystem>, IEventHandler<SystemEvent>
    {
        Task<PageResult<Base_LogSystem>> GetDataListAsync(PageInput<Base_LogSystemInputDTO> input);

        Task Handle(SystemEvent @event);
    }


}