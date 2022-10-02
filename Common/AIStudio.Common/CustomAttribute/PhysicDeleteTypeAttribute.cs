using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.CustomAttribute
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PhysicDeleteTypeAttribute : Attribute
    {
        public PhysicDeleteTypeAttribute() { }
    }
}
