using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public class DatabaseDesignerTableNode : DiagramNode
    {
        public List<Column> Columns { get; } = new List<Column>();
    }
}
