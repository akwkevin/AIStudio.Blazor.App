using AIStudio.BlazorDiagram.Components;
using AIStudio.BlazorDiagram.Models;
using AIStudio.Util;
using AIStudio.Util.DiagramEntity;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Newtonsoft.Json;

namespace AIStudio.BlazorDiagram.Pages
{
    public partial class DragAndDrop
    {
        [Inject]
        private IJSRuntime JSRuntime { get; set; }

        private readonly Diagram Diagram = new Diagram();
        private string? _draggedType;
        private string JsonString { get; set; }

        protected override void OnInitialized()
        {
            base.OnInitialized();

            Diagram.RegisterModelComponent<BotAnswerNodeModel, BotAnswerWidget>();
            Diagram.RegisterModelComponent<FlowchartNodelModel, FlowchartWidget>();
        }

        private void OnDragStart(string key)
        {
            // Can also use transferData, but this is probably "faster"
            _draggedType = key;
        }

        private void OnDrop(DragEventArgs e)
        {
            if (_draggedType == null) // Unkown item
                return;

            var position = Diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var node = _draggedType switch
            {
                "0" => NewNode(position),
                "1" => NewNode(position),
                _ => NewNode(position, (NodeKinds)System.Enum.Parse(typeof(NodeKinds), _draggedType)),
            };

            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            Diagram.Nodes.Add(node);
            _draggedType = null;
        }

        private NodeModel NewNode(Blazor.Diagrams.Core.Geometry.Point position)
        {
            var node = _draggedType == "0" ? new NodeModel(position) : new BotAnswerNodeModel(position);
            node.AddPort(PortAlignment.Bottom);
            node.AddPort(PortAlignment.Top);
            return node;
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


        private async Task ShowJson()
        {
            JsonString = DiagramDataExtention.ToJson(Diagram);
            await JSRuntime.InvokeVoidAsync("console.log", JsonString);
        }

        private void LoadJson()
        {
            DiagramDataExtention.ToObject(Diagram, JsonString);
        }
    }
}
