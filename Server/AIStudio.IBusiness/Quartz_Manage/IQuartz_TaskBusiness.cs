using AIStudio.Entity.Quartz_Manage;
using AIStudio.IBusiness;
using AIStudio.Util.Common;

namespace AIStudio.Business.Quartz_Manage
{
    public interface IQuartz_TaskBusiness : IBaseBusiness<Quartz_Task>
    {
        List<string> GetJobOptions();
        Task StartAllAsync();
        Task StartDataAsync(List<string> ids);
        Task PauseDataAsync(List<string> ids);
        Task TodoDataAsync(List<string> ids);
    }
}