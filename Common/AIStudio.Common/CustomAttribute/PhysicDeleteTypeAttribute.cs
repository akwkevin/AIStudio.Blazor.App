using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.CustomAttribute
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class PhysicDeleteTypeAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PhysicDeleteTypeAttribute"/> class.
        /// </summary>
        public PhysicDeleteTypeAttribute() { }
    }
}
