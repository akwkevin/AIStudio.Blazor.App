using AIStudio.Common.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;
using System.Collections.Generic;

namespace AIStudio.Entity.DTO.Base_Manage
{
    [Map(typeof(Base_Action))]
    public class Base_ActionDTO : Base_Action, IIdObject
    {
        public List<Base_ActionDTO> permissionList { get; set; }

        public bool editable { get; set; }
    }
}
