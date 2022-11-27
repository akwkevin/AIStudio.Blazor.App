using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
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
        public List<PortAlignment> PortAlignmentList { get; set; }      
    }
}
