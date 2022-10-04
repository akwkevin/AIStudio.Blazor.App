using AIStudio.Common.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Util.Common;
using SqlSugar;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_UserBusiness : IBaseBusiness<Base_User>
    {
        Task<PageResult<Base_UserDTO>> GetDataListAsync(PageInput<Base_UsersInputDTO> input);
        Task<object> GetDataListByDepartmentAsync(string departmentid);
        Task<Base_UserDTO> GetTheDataDTOAsync(string id);
        Task AddDataAsync(UserEditInputDTO input);
        Task UpdateDataAsync(UserEditInputDTO input);
        Task SetUserRoleAsync(string userId, List<string> roleIds);
        Task<string> GetAvatar(string userId);
    }

    [Map(typeof(Base_User))]
    public class UserEditInputDTO : Base_User
    {
        public string newPwd { get; set; }
        public List<string> RoleIdList { get; set; }
    }

    public class Base_UsersInputDTO
    {
        public bool all { get; set; }
        public string userId { get; set; }
        public string keyword { get; set; }
    }
}