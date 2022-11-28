using AIStudio.Util.DiagramEntity;

namespace AIStudio.BlazorDiagram.Models
{
    public class DatabaseDesignerTableNode : DiagramNode
    {
        public List<Column> Columns { get; } = new List<Column>();
    }
}
