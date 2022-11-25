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

        public DiagramNode()
        {

        }

        /// <summary>
        /// 将diagram信息转换成自己的类
        /// </summary>
        /// <param name="nodeModel"></param>
        public DiagramNode(NodeModel nodeModel)
        {
            Id = nodeModel.Id;
            //Name
            //Color
            Label = nodeModel.Title;
            Width = nodeModel.Size.Width;
            Height = nodeModel.Size.Height;
            X = nodeModel.Position.X;
            Y = nodeModel.Position.Y;
            Type = this.GetType().Name;
            PortAlignmentList = nodeModel.Ports.Select(p => p.Alignment).ToList();
        }

        public virtual NodeModel ToNodelModel()
        {
            return ToNodelModel(new NodeModel(Id));
        }

        public virtual void AddPorts(NodeModel nodeModel)
        {
            PortAlignmentList?.ForEach(p => nodeModel.AddPort(p));
        }

        public NodeModel ToNodelModel(NodeModel nodeModel)
        {
            AddPorts(nodeModel);

            nodeModel.Title = Label;
            nodeModel.Size = new Blazor.Diagrams.Core.Geometry.Size(Width, Height);
            nodeModel.Position = new Blazor.Diagrams.Core.Geometry.Point(X, Y);

            return nodeModel;
        }
    }
}
