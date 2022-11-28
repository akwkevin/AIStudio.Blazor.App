using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.Mapper
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public class MapAttribute : Attribute
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MapAttribute"/> class.
        /// </summary>
        /// <param name="targetTypes">The target types.</param>
        public MapAttribute(params Type[] targetTypes)
        {
            TargetTypes = targetTypes;
        }
        /// <summary>
        /// Gets the target types.
        /// </summary>
        /// <value>
        /// The target types.
        /// </value>
        public Type[] TargetTypes { get; }
    }
}
