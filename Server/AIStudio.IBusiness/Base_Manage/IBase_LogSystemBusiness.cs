using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_LogSystemBusiness : IBaseBusiness<Base_LogSystem>, IEventHandler<SystemEvent>
    {
        Task<PageResult<Base_LogSystem>> GetLogListAsync(PageInput<Base_UserLogsInputDTO> input);
        Task<PageResult<Base_LogSystem>> GetLogList(PageInput input);

        Task Handle(SystemEvent @event);
    }


}