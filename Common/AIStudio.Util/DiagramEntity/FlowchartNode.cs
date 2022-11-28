using System.ComponentModel;

namespace AIStudio.Util.DiagramEntity
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Util.DiagramEntity.DiagramNode" />
    public class FlowchartNode : DiagramNode
    {
        /// <summary>
        /// Gets or sets the kind.
        /// </summary>
        /// <value>
        /// The kind.
        /// </value>
        public NodeKinds Kind { get; set; }

        /// <summary>
        /// Gets or sets the user ids.
        /// </summary>
        /// <value>
        /// The user ids.
        /// </value>
        public IEnumerable<string> UserIds { get; set; }
        /// <summary>
        /// Gets or sets the role ids.
        /// </summary>
        /// <value>
        /// The role ids.
        /// </value>
        public IEnumerable<string> RoleIds { get; set; }
        /// <summary>
        /// Gets or sets the type of the act.
        /// </summary>
        /// <value>
        /// The type of the act.
        /// </value>
        public string ActType { get; set; }

    }

    /// <summary>
    /// 
    /// </summary>
    public enum NodeKinds
    {
        /// <summary>
        /// 节点
        /// </summary>
        [Description("节点")]
        Normal = 0,
        /// <summary>
        /// 开始
        /// </summary>
        [Description("开始")]
        Start = 1,
        /// <summary>
        /// 结束
        /// </summary>
        [Description("结束")]
        End = 2,
        /// <summary>
        /// 中间节点
        /// </summary>
        [Description("中间节点")]
        Middle = 3,
        /// <summary>
        /// 条件节点
        /// </summary>
        [Description("条件节点")]
        Decide = 4,
        /// <summary>
        /// 并行开始
        /// </summary>
        [Description("并行开始")]
        COBegin = 5,
        /// <summary>
        /// 并行结束
        /// </summary>
        [Description("并行结束")]
        COEnd = 6,
    }
}
