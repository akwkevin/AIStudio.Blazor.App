using AIStudio.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage.InputDTO
{
    public class BuildInputDTO : IIdObject
    {
        public string Id { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
        public string? linkId { get; set; }
        public string? areaName { get; set; }
        public List<string>? tables { get; set; }
        public string[]? buildTypesBinding { get; set; }
        public List<int>? buildTypes { get; set; }

    }
}
