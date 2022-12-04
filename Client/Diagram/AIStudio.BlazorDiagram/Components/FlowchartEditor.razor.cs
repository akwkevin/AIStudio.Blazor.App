using AIStudio.BlazorDiagram.Components;
using AIStudio.BlazorDiagram.Models;
using AIStudio.Util;
using AIStudio.Util.Common;
using AIStudio.Util.DiagramEntity;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace AIStudio.BlazorDiagram.Components
{
    public partial class FlowchartEditor
    {
        private readonly Diagram Diagram = new Diagram();
        private NodeKinds? _draggedType;

        [Parameter]
        public string Data { get; set; }

        [Parameter]
        public EventCallback<string> DataChanged { get; set; }

        [Parameter]
        public List<SelectOption> Users { get; set; } = new List<SelectOption>();

        [Parameter]
        public List<SelectOption> Roles { get; set; } = new List<SelectOption>();

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Diagram.RegisterModelComponent<FlowchartNodelModel, FlowchartWidget>();
        }

        private void OnDragStart(NodeKinds key)
        {
            // Can also use transferData, but this is probably "faster"
            _draggedType = key;
        }

        private void OnDrop(DragEventArgs e)
        {
            if (_draggedType == null) // Unkown item
                return;

            var position = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var node = NewNode(position, _draggedType.Value);

            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            Diagram.Nodes.Add(node);
            _draggedType = null;
        }

        private NodeModel NewNode(Blazor.Diagrams.Core.Geometry.Point position, NodeKinds nodeKinds)
        {
            var node = new FlowchartNodelModel(position);
            node.Kind = nodeKinds;
            node.Title = nodeKinds.GetDescription();
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Left);
            node.AddPort(PortAlignment.Right);
            return node;
        }

        protected override void OnParametersSet()
        {
            Diagram.ToObject(Data??"");
        }

        public string GetData()
        {
            var data = Diagram.ToJson();
            return data;
        }

    }
}
