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

        public List<string> UserIds { get; set; }
        public List<string> RoleIds { get; set; }
        public string ActType { get; set; }

        public FlowchartNode() : base() 
        {

        }

        public FlowchartNode(NodeModel nodeModel) : base(nodeModel)
        {
            if (nodeModel is FlowchartNodelModel flowchartNodelModel)
            {
                Color = flowchartNodelModel.Color;
                Kind = flowchartNodelModel.Kind;
                UserIds = flowchartNodelModel.UserIds;
                RoleIds= flowchartNodelModel.RoleIds;
                ActType= flowchartNodelModel.ActType;
            }
        }

        public override NodeModel ToNodelModel()
        {
            FlowchartNodelModel flowchartNodelModel = new FlowchartNodelModel(Id);
            flowchartNodelModel.Color = Color;
            flowchartNodelModel.Kind = Kind;
            flowchartNodelModel.UserIds = UserIds;
            flowchartNodelModel.RoleIds = RoleIds;
            flowchartNodelModel.ActType = ActType;

            return ToNodelModel(flowchartNodelModel);

        }

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
