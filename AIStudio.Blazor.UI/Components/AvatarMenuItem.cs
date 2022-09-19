using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Blazor.UI.Components
{
    public class AvatarMenuItem
    {
        public string Key { get; set; }
        public string IconType { get; set; }
        public string IconTheme { get; set; } = "outline";
        public string Option { get; set; }
        public bool IsDivider { get; set; }
    }
}
