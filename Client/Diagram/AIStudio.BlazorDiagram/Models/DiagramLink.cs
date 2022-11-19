using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
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
        public string? Color { get; set; }
        public string? SelectedColor { get; set; }
        public double Width { get; set; }

        public string? SourceId { get; set; }
        public string? TargetId { get; set; }

        public string? SourcePort { get; set; }
        public string? TargetPort { get; set; }

        public DiagramLink(BaseLinkModel baseLinkModel )
        {
            if (baseLinkModel is LinkModel linkModel)
            {
                Color = linkModel.Color;
                SelectedColor = linkModel.SelectedColor;
                Width = linkModel.Width;              
            }

            SourceId = baseLinkModel.SourceNode?.Id;
            TargetId = baseLinkModel.TargetNode?.Id;
            SourcePort = baseLinkModel.SourcePort?.Alignment.ToString();
            TargetPort = baseLinkModel.TargetPort?.Alignment.ToString();
        }


    }
}
