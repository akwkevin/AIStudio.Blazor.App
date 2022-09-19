using System.Collections.Generic;

namespace AIStudio.Util.Common
{
    public class DictionaryTreeModel : TreeModel
    {
        public int Type { get; set; }
        //public ControlType ControlType { get; set; }
        public string Code { get; set; }

        /// <summary>
        /// 孩子节点
        /// </summary>
        public new List<DictionaryTreeModel> Children { get; set; }
    }
}
