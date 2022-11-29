using System.Collections.Generic;

namespace AIStudio.Util.Common
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Util.Common.TreeModel" />
    public class DictionaryTreeModel : TreeModel
    {
        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        public int Type { get; set; }
        //public ControlType ControlType { get; set; }
        /// <summary>
        /// Gets or sets the code.
        /// </summary>
        /// <value>
        /// The code.
        /// </value>
        public string Code { get; set; }

        /// <summary>
        /// 孩子节点
        /// </summary>
        public new List<DictionaryTreeModel> Children { get; set; }
    }
}
