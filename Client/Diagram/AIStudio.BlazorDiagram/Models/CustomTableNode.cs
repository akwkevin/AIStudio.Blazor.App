using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public class CustomTableNode : DiagramNode
    {
        public List<Column> Columns { get; } = new List<Column>();

        /// <summary>
        /// 将diagram信息转换成自己的类
        /// </summary>
        /// <param name="nodeModel"></param>
        public CustomTableNode(NodeModel nodeModel) : base(nodeModel) 
        {
            if (nodeModel is Table table)
            {
                Columns = table.Columns;
                Name= table.Name;
            }
        }

        public override NodeModel ToNodelModel()
        {
            Table table = new Table();
            table.Name = Name;
            table.Columns.Clear();
            table.Columns.AddRange(Columns);
            ToNodelModel(table);

            return table;

        }
    }
}
