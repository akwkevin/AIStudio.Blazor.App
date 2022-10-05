using AIStudio.Common.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{
    [Map(typeof(Base_Role))]
    public class Base_RoleEditInputDTO : Base_Role
    {
        public RoleTypes? RoleType { get { try { return RoleName?.ToEnum<RoleTypes>(); } catch { return null; } } }
        public List<string> Actions { get; set; } = new List<string>();
    }
}
