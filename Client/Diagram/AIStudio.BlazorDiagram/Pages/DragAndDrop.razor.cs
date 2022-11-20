using AIStudio.BlazorDiagram.Components;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Microsoft.AspNetCore.Components.Web;

namespace AIStudio.BlazorDiagram.Pages
{
    public partial class DragAndDrop
    {
        private readonly Diagram _diagram = new Diagram();
        private int? _draggedType;

        protected override void OnInitialized()
        {
            base.OnInitialized();


            _diagram.RegisterModelComponent<BotAnswerNode, BotAnswerWidget>();
        }

        private void OnDragStart(int key)
        {
            // Can also use transferData, but this is probably "faster"
            _draggedType = key;
        }

        private void OnDrop(DragEventArgs e)
        {
            if (_draggedType == null) // Unkown item
                return;

            var position = _diagram.GetRelativeMousePoint(e.ClientX, e.ClientY);
            var node = _draggedType == 0 ? new NodeModel(position) : new BotAnswerNode(position);
            node.AddPort(PortAlignment.Top);
            node.AddPort(PortAlignment.Bottom);
            _diagram.Nodes.Add(node);
            _draggedType = null;
        }
    }
}
