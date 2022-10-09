using AIStudio.Util.Mapper;
using AIStudio.Entity.Base_Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{
    [Map(typeof(Base_Action))]
    public class Base_ActionEditInputDTO : Base_Action
    {
        public List<Base_Action> permissionList { get; set; } = new List<Base_Action>();
    }

}
