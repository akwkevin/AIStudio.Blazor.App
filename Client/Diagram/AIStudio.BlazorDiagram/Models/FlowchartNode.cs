using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public class FlowchartNode : DiagramNode
    {
        public FlowchartNode(NodeModel nodeModel) : base(nodeModel)
        {
        }

        public List<string> UserIds { get; set; }
        public List<string> RoleIds { get; set; }
        public string ActType { get; set; }
    }
}
