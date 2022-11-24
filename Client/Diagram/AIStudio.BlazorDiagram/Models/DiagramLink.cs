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

        public LinkMarker? SourceMarker { get; set; }

        public LinkMarker? TargetMarker { get; set; }

        [JsonIgnore]
        public NodeModel SourceNode { get; set; }

        [JsonIgnore]
        public NodeModel TargetNode { get; set; }

        [JsonIgnore]
        public PortModel SourcePort { get; set; }

        [JsonIgnore]
        public PortModel TargetPort { get; set; }

        public DiagramLink()
        {

        }

        public DiagramLink(BaseLinkModel baseLinkModel)
        {
            if (baseLinkModel is LinkModel linkModel)
            {
                Id = linkModel.Id;
                Color = linkModel.Color;
                SelectedColor = linkModel.SelectedColor;
                Width = linkModel.Width;              
            }

            SourceId = baseLinkModel.SourceNode?.Id;
            TargetId = baseLinkModel.TargetNode?.Id;
            Router = baseLinkModel.Router?.Method.Name;
            PathGenerator = baseLinkModel.PathGenerator?.Method.Name;
            SourceMarker = baseLinkModel.SourceMarker;
            TargetMarker = baseLinkModel.TargetMarker;

            Type = this.GetType().Name;
   

            SourcePortAlignment = baseLinkModel.SourcePort?.Alignment.ToString(); 
            TargetPortAlignment = baseLinkModel.TargetPort?.Alignment.ToString();
        }

        public virtual BaseLinkModel ToLinkModel(NodeModel sourceNode, NodeModel targetNode)
        {
            SourceNode = sourceNode;
            TargetNode = targetNode;
            if (SourceNode != null)
            {
                PortAlignment sourcePortAlignment;
                Enum.TryParse(SourcePortAlignment, out sourcePortAlignment);
                SourcePort = SourceNode.GetPort(sourcePortAlignment);
            }
            if (TargetNode != null)
            {
                PortAlignment targetPortAlignment;
                Enum.TryParse(TargetPortAlignment, out targetPortAlignment);
                TargetPort = TargetNode.GetPort(targetPortAlignment);
            }

            LinkModel linkModel = new LinkModel(Id, SourcePort, TargetPort);
            
            return ToLinkModel(linkModel);
        }

        public LinkModel ToLinkModel(LinkModel linkModel)
        {
            linkModel.Color = Color;
            linkModel.SelectedColor = SelectedColor;
            linkModel.Width = Width;
            switch(Router)
            {
                case "Normal": linkModel.Router = Routers.Normal; break;
                case "Orthogonal": linkModel.Router = Routers.Orthogonal; break;
            }
            switch (PathGenerator)
            {
                case "Smooth": linkModel.PathGenerator = PathGenerators.Smooth; break;
                case "Straight": linkModel.PathGenerator = PathGenerators.Straight; break;
               
            }

            linkModel.SourceMarker = SourceMarker;
            linkModel.TargetMarker = TargetMarker;
            return linkModel;
        }

    }
}
