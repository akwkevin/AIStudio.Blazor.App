using Microsoft.AspNetCore.Components.Routing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorUI.Components
{
    public class MenuDataItem
    {
        public string[] Authority { get; set; }
        public virtual MenuDataItem[] Children { get; set; }
        public bool HideChildrenInMenu { get; set; }
        public bool HideInMenu { get; set; }
        public string Icon { get; set; }
        public string Locale { get; set; }
        public virtual string Name { get; set; }
        public string Key { get; set; }
        public string Path { get; set; }
        public string[] ParentKeys { get; set; }
        public NavLinkMatch Match { get; set; }
    }
}
