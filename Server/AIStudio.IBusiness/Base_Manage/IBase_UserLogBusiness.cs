using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_UserLogBusiness
    {
        Task<PageResult<Base_UserLog>> GetLogListAsync(PageInput<UserLogsInputDTO> input);
        Task<PageResult<Base_UserLog>> GetLogList(PageInput input);
    }

    public class UserLogsInputDTO
    {
        public string logContent { get; set; }
        public string logType { get; set; }
        public string opUserName { get; set; }
        public DateTime? startTime { get; set; }
        public DateTime? endTime { get; set; }
    }
}