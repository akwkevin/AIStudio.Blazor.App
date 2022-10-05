using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_DepartmentBusiness : IBaseBusiness<Base_Department>
    {
        Task<List<Base_DepartmentTree>> GetTreeDataListAsync(Base_DepartmentTreeInputDTO input);
        Task<List<string>> GetChildrenIdsAsync(string departmentId);
    }

  
}