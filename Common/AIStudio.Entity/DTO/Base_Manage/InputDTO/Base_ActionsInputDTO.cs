using AIStudio.Util.Mapper;
using AIStudio.Entity.Base_Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{

    public class Base_ActionsInputDTO : SearchInput
    {
        public string[]? ActionIds { get; set; }
        public ActionType[]? Types { get; set; }

        public string? ParentId { get; set; }
    }
}
