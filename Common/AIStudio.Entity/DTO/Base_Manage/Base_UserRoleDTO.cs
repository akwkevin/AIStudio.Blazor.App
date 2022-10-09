using AIStudio.Util.Mapper;

namespace AIStudio.Entity.Base_Manage
{
    [Map(typeof(Base_UserRole))]
    public class Base_UserRoleDTO : Base_UserRole
    {
  
        public Base_User User { get; set; }
  
        public Base_Role Role { get; set; }

    }
}