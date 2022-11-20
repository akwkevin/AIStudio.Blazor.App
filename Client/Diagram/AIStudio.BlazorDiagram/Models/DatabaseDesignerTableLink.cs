using AIStudio.BlazorDiagram.Components;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public class DatabaseDesignerTableLink : DiagramLink
    {
        public Column SourceColumn { get; set; }
        public Column TargetColumn { get; set; }
        public DatabaseDesignerTableLink()
        {

        }

        public DatabaseDesignerTableLink(BaseLinkModel baseLinkModel) : base(baseLinkModel)
        {
            if (baseLinkModel.SourcePort is ColumnPort soureccolumnPort)
            {
                SourceColumn = soureccolumnPort.Column;
            }
            if (baseLinkModel.TargetPort is ColumnPort targetcolumnPort)
            {
                TargetColumn = targetcolumnPort.Column;
            }
        }

        public override BaseLinkModel ToLinkModel(NodeModel sourceNode, NodeModel targetNode)
        {
            SourceNode = sourceNode;
            TargetNode = targetNode;
            if (SourceNode is Table sourcetable)
            {
                SourceColumn = sourcetable.Columns.FirstOrDefault(p => p.Name == SourceColumn.Name);
                SourcePort = sourcetable.GetPort(SourceColumn);
            }
            if (TargetNode is Table targettable)
            {
                TargetColumn = targettable.Columns.FirstOrDefault(p => p.Name == TargetColumn.Name);
                TargetPort = targettable.GetPort(TargetColumn);
            }
            LinkModel linkModel = new LinkModel(Id, SourcePort, TargetPort);
            linkModel.Router = Routers.Orthogonal;
            linkModel.PathGenerator = PathGenerators.Straight;
                               
            return ToLinkModel(linkModel);
        }


    }
}
