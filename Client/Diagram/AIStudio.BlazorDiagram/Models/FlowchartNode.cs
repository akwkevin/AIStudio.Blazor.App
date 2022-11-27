using AIStudio.BlazorDiagram.Components;
using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public class FlowchartNode : DiagramNode
    {
        public string? Color { get; set; }
        public NodeKinds Kind { get; set; }

        public IEnumerable<string> UserIds { get; set; }
        public IEnumerable<string> RoleIds { get; set; }
        public string ActType { get; set; }

    }

    public enum NodeKinds
    {
        [Description("节点")]
        Normal = 0,
        [Description("开始")]
        Start = 1,
        [Description("结束")]
        End = 2,
        [Description("中间节点")]
        Middle = 3,
        [Description("条件节点")]
        Decide = 4,
        [Description("并行开始")]
        COBegin = 5,
        [Description("并行结束")]
        COEnd = 6,
    }
}
