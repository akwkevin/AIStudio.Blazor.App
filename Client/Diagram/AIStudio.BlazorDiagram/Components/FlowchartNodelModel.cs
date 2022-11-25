using AIStudio.BlazorDiagram.Models;
using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace AIStudio.BlazorDiagram.Components
{
    public class FlowchartNodelModel : NodeModel
    {
        public FlowchartNodelModel(Point position = null) : base(position) { }

        public FlowchartNodelModel(string id, Point position = null) : base(id, position)
        {

        }
        public string Color { get; set; } = "#1cbbb4";
        public NodeKinds Kind { get; set; }
        public List<string> UserIds { get; set; }
        public List<string> RoleIds { get; set; }
        public string ActType { get; set; }
    }
}
