using AIStudio.Common.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;
using SqlSugar;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_UserBusiness : IBaseBusiness<Base_User>
    {
        Task<PageResult<Base_UserDTO>> GetDataListAsync(PageInput<Base_UsersInputDTO> input);
        Task<object> GetDataListByDepartmentAsync(string departmentid);
        Task<Base_UserDTO> GetTheDataDTOAsync(string id);
        Task AddDataAsync(Base_UserEditInputDTO input);
        Task UpdateDataAsync(Base_UserEditInputDTO input);
        Task SetUserRoleAsync(string userId, List<string> roleIds);
        Task<string> GetAvatar(string userId);   }


 
}