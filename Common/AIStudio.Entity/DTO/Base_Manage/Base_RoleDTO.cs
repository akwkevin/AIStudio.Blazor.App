using AIStudio.Util.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Base_Manage
{
    [Map(typeof(Base_Role))]
    public class Base_RoleDTO : Base_Role, IIdObject
    {
        public string[]? Actions { get; set; }     
    }
}
