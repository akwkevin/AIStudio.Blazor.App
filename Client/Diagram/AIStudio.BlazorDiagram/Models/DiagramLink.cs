using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Emit;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public class DiagramLink
    {
        public string? Id { get; set; }
        public string? Color { get; set; }
        public string? SelectedColor { get; set; }
        public double Width { get; set; }

        public string? SourceId { get; set; }
        public string? TargetId { get; set; }

        public string? SourcePortAlignment { get; set; }
        public string? TargetPortAlignment { get; set; }
        public string? Type { get; set; }

        public string? Router { get; set; }

        public string? PathGenerator { get; set; }

        public string? SourceMarkerPath { get; set; }
        public double? SourceMarkerWidth { get; set; }

        public string? TargetMarkerPath { get; set; }
        public double? TargetMarkerWidth { get; set; } 
    }
}
