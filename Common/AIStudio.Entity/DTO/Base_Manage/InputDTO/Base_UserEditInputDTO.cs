using AIStudio.Util.Mapper;
using AIStudio.Entity.Base_Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{

    [Map(typeof(Base_User))]
    public class Base_UserEditInputDTO : Base_User
    {
        public string newPwd { get; set; }
        public List<string> RoleIdList { get; set; }
    }
}
