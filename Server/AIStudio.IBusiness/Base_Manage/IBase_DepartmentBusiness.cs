using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_DepartmentBusiness : IBaseBusiness<Base_Department>
    {
        Task<List<Base_DepartmentTreeDTO>> GetTreeDataListAsync(DepartmentsTreeInputDTO input);
        Task<List<string>> GetChildrenIdsAsync(string departmentId);
    }

    public class DepartmentsTreeInputDTO
    {
        public string parentId { get; set; }
    }

    public class Base_DepartmentTreeDTO : TreeModel
    {
        public object children { get => Children; }
        public string title { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }
        public string ParentIds { get; set; }
    }
}