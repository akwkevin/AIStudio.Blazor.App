using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_UserLogBusiness
    {
        Task<PageResult<Base_UserLog>> GetLogListAsync(PageInput<Base_UserLogsInputDTO> input);
        Task<PageResult<Base_UserLog>> GetLogList(PageInput input);
    }


}