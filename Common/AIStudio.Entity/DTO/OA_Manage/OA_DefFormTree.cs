using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.OA_Manage
{
    public class OA_DefFormTree : TreeModel<OA_DefFormTree>
    {
        public string title { get => Text; }
        public string text { get => Text; }
        public string value { get => Id; }
        public string key { get => Id; }

        public object scopedSlots { get; set; }

        public int jsonVersion { get; set; }
        public string jsonId { get; set; }
        public string json { get; set; }

        public string type { get; set; }
    }
}
