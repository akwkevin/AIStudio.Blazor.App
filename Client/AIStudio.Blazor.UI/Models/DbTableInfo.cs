using AIStudio.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Blazor.UI.Models
{
    public class DbTableInfo: IIdObject
    {   /// <summary>
        /// 表名
        /// </summary>
        public string TableName { get; set; }

        /// <summary>
        /// 表描述说明
        /// </summary>
        public string Description { get; set; }
        public string Id { get  { return TableName; } }
    }
}
