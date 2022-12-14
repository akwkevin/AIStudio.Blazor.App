using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.OA_Manage;
using AIStudio.Util.Common;
using AIStudio.Util.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.OA_Manage
{
    [Map(typeof(OA_DefForm))]
    public class OA_DefFormDTO : OA_DefForm
    {
        public IEnumerable<string>? ValueRoles
        {
            get { return Value?.Split(new string[] { "^" }, System.StringSplitOptions.RemoveEmptyEntries); }
            set
            {
                if (value != null)
                {
                    Value = "^" + string.Join("^", value) + "^";
                }
                else
                {
                    Value = null;
                }
            }
        }

   
    }
}
