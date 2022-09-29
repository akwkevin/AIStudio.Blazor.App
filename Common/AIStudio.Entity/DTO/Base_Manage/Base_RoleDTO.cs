using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;
using System.Collections.Generic;

namespace AIStudio.Entity.DTO.Base_Manage
{
    public class Base_RoleDTO : Base_Role, IIdObject
    {
        public string[] Actions { get; set; }     
    }
}
