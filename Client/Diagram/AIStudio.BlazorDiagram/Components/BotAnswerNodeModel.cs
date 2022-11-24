using Blazor.Diagrams.Core.Geometry;
using Blazor.Diagrams.Core.Models;

namespace AIStudio.BlazorDiagram.Components
{
    public class BotAnswerNodeModel : NodeModel
    {
        public BotAnswerNodeModel(Point position = null) : base(position) { }

        public BotAnswerNodeModel(string id, Point position = null) : base(id, position)
        {
        
        }

        public string Answer { get; set; }
    }
}
