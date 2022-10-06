using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Base_Manage
{
    public class Base_DepartmentTree : TreeModel<Base_DepartmentTree>
    {
        public string Name { get; set; }
        public string ParentIds { get; set; }
    }
}
