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
    }
}
