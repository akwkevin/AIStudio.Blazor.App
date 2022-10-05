using AIStudio.Common.Mapper;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_RoleBusiness : IBaseBusiness<Base_Role>
    {
        Task<PageResult<Base_RoleEditInputDTO>> GetDataListAsync(PageInput<Base_RoleInputDTO> input);
        Task<Base_RoleEditInputDTO> GetTheDataDTOAsync(string id);
        Task AddDataAsync(Base_RoleEditInputDTO input);
        Task UpdateDataAsync(Base_RoleEditInputDTO input);
    }

 
}