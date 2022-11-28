using AIStudio.Util.DiagramEntity;

namespace AIStudio.BlazorDiagram.Models
{
    public class DatabaseDesignerTableLink : DiagramLink
    {
        public Column SourceColumn { get; set; }
        public Column TargetColumn { get; set; }
    }
}
