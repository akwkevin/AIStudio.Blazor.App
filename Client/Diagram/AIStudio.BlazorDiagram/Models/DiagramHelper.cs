using AIStudio.BlazorDiagram.Components;
using AIStudio.Util;
using AIStudio.Util.DiagramEntity;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Newtonsoft.Json;
using System.Data;

namespace AIStudio.BlazorDiagram.Models
{
    public static class DiagramHelper
    {
        #region ToJson
        public static string ToJson(Diagram diagram)
        {
            var json = JsonConvert.SerializeObject(new
            {
                Nodes = diagram.Nodes.Select(p => p.ToDiagramNode()),
                Links = diagram.Links.Select(p => p.ToDiagramLink())
            }, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }

        public static DiagramNode ToDiagramNode(this NodeModel nodeModel)
        {
            DiagramNode diagramNode;
            if (nodeModel is Table)
            {
                var databaseDesignerTableNode = new DatabaseDesignerTableNode();
                diagramNode = databaseDesignerTableNode;
                if (nodeModel is Table table)
                {
                    databaseDesignerTableNode.Columns.AddRange(table.Columns);
                    databaseDesignerTableNode.Name = table.Name;
                }
            }
            else if (nodeModel is BotAnswerNodeModel)
            {
                var botAnswerNode = new BotAnswerNode();
                diagramNode = botAnswerNode;
                if (nodeModel is BotAnswerNodeModel botAnswerNodeModel)
                {
                    botAnswerNode.Answer = botAnswerNodeModel.Answer;
                }
            }
            else if (nodeModel is FlowchartNodelModel)
            {
                var flowchartNode = new FlowchartNode();
                diagramNode = flowchartNode;
                if (nodeModel is FlowchartNodelModel flowchartNodelModel)
                {
                    flowchartNode.Color = flowchartNodelModel.Color;
                    flowchartNode.Kind = flowchartNodelModel.Kind;
                    flowchartNode.UserIds = flowchartNodelModel.UserIds;
                    flowchartNode.RoleIds = flowchartNodelModel.RoleIds;
                    flowchartNode.ActType = flowchartNodelModel.ActType;
                }
            }
            else
            {
                diagramNode = new DiagramNode();
            }

            diagramNode.Id = nodeModel.Id;
            diagramNode.Label = nodeModel.Title;
            diagramNode.Width = nodeModel.Size.Width;
            diagramNode.Height = nodeModel.Size.Height;
            diagramNode.X = nodeModel.Position.X;
            diagramNode.Y = nodeModel.Position.Y;
            diagramNode.Type = diagramNode.GetType().Name;
            diagramNode.PortAlignmentList = nodeModel.Ports.Select(p => p.Alignment.ToString()).ToList();

            return diagramNode;
        }
    
        public static DiagramLink ToDiagramLink(this BaseLinkModel baseLinkModel)
        {
            DiagramLink diagramLink;
            if (baseLinkModel.SourcePort is ColumnPort || baseLinkModel.TargetPort is ColumnPort)
            {
                var databaseDesignerTableLink = new DatabaseDesignerTableLink();
                diagramLink = databaseDesignerTableLink;
                if (baseLinkModel.SourcePort is ColumnPort soureccolumnPort)
                {
                    databaseDesignerTableLink.SourceColumn = soureccolumnPort.Column;
                }
                if (baseLinkModel.TargetPort is ColumnPort targetcolumnPort)
                {
                    databaseDesignerTableLink.TargetColumn = targetcolumnPort.Column;
                }
            }
            else
            {
                diagramLink = new DiagramLink();
            }


            if (baseLinkModel is LinkModel linkModel)
            {
                diagramLink.Id = linkModel.Id;
                diagramLink.Color = linkModel.Color;
                diagramLink.SelectedColor = linkModel.SelectedColor;
                diagramLink.Width = linkModel.Width;
            }

            diagramLink.SourceId = baseLinkModel.SourceNode?.Id;
            diagramLink.TargetId = baseLinkModel.TargetNode?.Id;
            diagramLink.Router = baseLinkModel.Router?.Method.Name;
            diagramLink.PathGenerator = baseLinkModel.PathGenerator?.Method.Name;
            diagramLink.SourceMarkerPath = baseLinkModel.SourceMarker?.Path;
            diagramLink.SourceMarkerWidth = baseLinkModel.SourceMarker?.Width;
            diagramLink.TargetMarkerPath = baseLinkModel.TargetMarker?.Path;
            diagramLink.TargetMarkerWidth = baseLinkModel.TargetMarker?.Width;

            diagramLink.Type = diagramLink.GetType().Name;

            diagramLink.SourcePortAlignment = baseLinkModel.SourcePort?.Alignment.ToString();
            diagramLink.TargetPortAlignment = baseLinkModel.TargetPort?.Alignment.ToString();

            return diagramLink;
        }     
        #endregion

        #region ToObject

        public static void ToObject(Diagram diagram, string json)
        {
            var data = JsonConvert.DeserializeObject<DiagramData>(json, new JsonConverter[] { new DiagramNodeConverter(), new DiagramLinkConverter() });
            if (data != null)
            {
                ToObject(diagram, data);
            }
        }

        public static void ToObject(Diagram diagram, DiagramData data)
        {
            diagram.Nodes.Clear();
            diagram.Links.Clear();

            List<NodeModel> nodes = new List<NodeModel>();          
            if (data.Nodes != null)
            {
                foreach (var node in data.Nodes)
                {
                    var nodemodel = node.ToNodelModel();
                    nodes.Add(nodemodel);
                    diagram.Nodes.Add(nodemodel);
                }
            }
            if (data.Links != null)
            {
                foreach (var link in data.Links)
                {
                    var source = nodes.FirstOrDefault(p => p.Id == link.SourceId);
                    var target = nodes.FirstOrDefault(p => p.Id == link.TargetId);
                    var linkmodel = link.ToLinkModel(source, target);
                    diagram.Links.Add(linkmodel);
                }
            }
        }

        private static NodeModel ToNodelModel(this DiagramNode diagramNode)
        {
            NodeModel nodeModel;
            if (diagramNode is FlowchartNode flowchartNode)
            {
                var flowchartNodelModel = new FlowchartNodelModel(flowchartNode.Id);
                nodeModel = flowchartNodelModel;
                flowchartNodelModel.Color = flowchartNode.Color;
                flowchartNodelModel.Kind = flowchartNode.Kind;
                flowchartNodelModel.UserIds = flowchartNode.UserIds;
                flowchartNodelModel.RoleIds = flowchartNode.RoleIds;
                flowchartNodelModel.ActType = flowchartNode.ActType;

            }
            else if (diagramNode is DatabaseDesignerTableNode databaseDesignerTableNode)
            {
                Table table = new Table(databaseDesignerTableNode.Id);
                nodeModel = table;
                table.Name = databaseDesignerTableNode.Name;
                databaseDesignerTableNode.Columns.ForEach(p => table.AddColumn(p));
            }
            else if (diagramNode is BotAnswerNode botAnswerNode)
            {
                BotAnswerNodeModel botAnswerNodeModel = new BotAnswerNodeModel(botAnswerNode.Id);
                nodeModel = botAnswerNodeModel;
                botAnswerNodeModel.Answer = botAnswerNode.Answer;
            }
            else
            {
                nodeModel = new NodeModel(diagramNode.Id);
            }

            nodeModel.Title = diagramNode.Label;
            nodeModel.Size = new Blazor.Diagrams.Core.Geometry.Size(diagramNode.Width, diagramNode.Height);
            nodeModel.Position = new Blazor.Diagrams.Core.Geometry.Point(diagramNode.X, diagramNode.Y);
            diagramNode.PortAlignmentList?.ForEach(p => nodeModel.AddPort(p.ToEnum<PortAlignment>()));

            return nodeModel;
        }

        public static LinkModel ToLinkModel(this DiagramLink diagramLink, NodeModel sourceNode, NodeModel targetNode)
        {
            PortModel sourcePort = null;
            PortModel targetPort = null;
            if (diagramLink is DatabaseDesignerTableLink databaseDesignerTableLink)
            {
                if (sourceNode is Table sourcetable)
                {
                    var sourceColumn = sourcetable.Columns.FirstOrDefault(p => p.Name == databaseDesignerTableLink.SourceColumn.Name);
                    sourcePort = sourcetable.GetPort(sourceColumn);
                }
                if (targetNode is Table targettable)
                {
                    var targetColumn = targettable.Columns.FirstOrDefault(p => p.Name == databaseDesignerTableLink.TargetColumn.Name);
                    targetPort = targettable.GetPort(targetColumn);
                }
            }
            else
            {
                if (sourceNode != null)
                {
                    PortAlignment sourcePortAlignment;
                    Enum.TryParse(diagramLink.SourcePortAlignment, out sourcePortAlignment);
                    sourcePort = sourceNode.GetPort(sourcePortAlignment);
                }
                if (targetNode != null)
                {
                    PortAlignment targetPortAlignment;
                    Enum.TryParse(diagramLink.TargetPortAlignment, out targetPortAlignment);
                    targetPort = targetNode.GetPort(targetPortAlignment);
                }
            }

            LinkModel linkModel = new LinkModel(diagramLink.Id, sourcePort, targetPort);

            linkModel.Color = diagramLink.Color;
            linkModel.SelectedColor = diagramLink.SelectedColor;
            linkModel.Width = diagramLink.Width;
            switch (diagramLink.Router)
            {
                case "Normal": linkModel.Router = Routers.Normal; break;
                case "Orthogonal": linkModel.Router = Routers.Orthogonal; break;
            }
            switch (diagramLink.PathGenerator)
            {
                case "Smooth": linkModel.PathGenerator = PathGenerators.Smooth; break;
                case "Straight": linkModel.PathGenerator = PathGenerators.Straight; break;

            }

            if (!string.IsNullOrEmpty(diagramLink.SourceMarkerPath))
            {
                linkModel.SourceMarker = new LinkMarker(diagramLink.SourceMarkerPath, diagramLink.SourceMarkerWidth ?? 10.0);
            }
            if (!string.IsNullOrEmpty(diagramLink.TargetMarkerPath))
            {
                linkModel.TargetMarker = new LinkMarker(diagramLink.TargetMarkerPath, diagramLink.TargetMarkerWidth ?? 10.0);
            }
            return linkModel;
        }

        #endregion
    }
}
