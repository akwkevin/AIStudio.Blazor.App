using AIStudio.BlazorDiagram.Models;
using AIStudio.Util.DiagramEntity;
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
        public string? Name { get; set; }
        public string? Color { get; set; } = "#1cbbb4";
        public NodeKinds Kind { get; set; }
        public IEnumerable<string>? UserIds { get; set; }
        public IEnumerable<string>? RoleIds { get; set; }
        public string? ActType { get; set; }
    }
}
