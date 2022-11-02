using AIStudio.Entity.DTO.Base_Manage;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IPermissionBusiness
    {
        Task<List<string>> GetUserPermissionListAsync(string userId);
        Task<List<Base_ActionTree>> GetUserMenuListAsync(string userId);
    }
}
