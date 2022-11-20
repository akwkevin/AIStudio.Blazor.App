using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace AIStudio.BlazorDiagram.Components
{
    public class BotAnswerNode : NodeModel
    {
        public BotAnswerNode(Point position = null) : base(position) { }

        public string Answer { get; set; }
    }
}
