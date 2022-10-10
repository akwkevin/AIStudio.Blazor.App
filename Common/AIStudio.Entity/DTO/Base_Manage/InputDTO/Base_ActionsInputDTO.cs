using AIStudio.Util.Mapper;
using AIStudio.Entity.Base_Manage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{

    public class Base_ActionsInputDTO
    {
        public string[]? ActionIds { get; set; }
        public string? parentId { get; set; }
        public ActionType[]? types { get; set; }
        public bool selectable { get; set; }
        public bool checkEmptyChildren { get; set; }
    }
}
