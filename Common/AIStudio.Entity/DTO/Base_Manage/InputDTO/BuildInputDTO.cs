using AIStudio.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{
    public class BuildInputDTO : KeyBaseEntity
    {
        public string? linkId { get; set; }
        public string? areaName { get; set; }
        public List<string>? tables { get; set; }
        public string[]? buildTypes { get; set; }

    }
}
