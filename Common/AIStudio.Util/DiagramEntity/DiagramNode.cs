namespace AIStudio.Util.DiagramEntity
{
    /// <summary>
    /// FlowNode
    /// </summary>
    public class DiagramNode
    {
        public string? Id { get; set; }
        public string? Name { get; set; }
        public string? Label { get; set; }
        public double Width { get; set; }
        public double Height { get; set; }
        public double X { get; set; }
        public double Y { get; set; }
        public string? Type { get; set; }
        public int ZIndex { get; set; }
        public List<string> PortAlignmentList { get; set; }
    }
}
