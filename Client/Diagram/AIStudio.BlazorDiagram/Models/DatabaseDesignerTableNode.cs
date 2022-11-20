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

        public DatabaseDesignerTableNode() :base() 
        {

        }
        /// <summary>
        /// 将diagram信息转换成自己的类
        /// </summary>
        /// <param name="nodeModel"></param>
        public DatabaseDesignerTableNode(NodeModel nodeModel) : base(nodeModel) 
        {
            if (nodeModel is Table table)
            {
                Columns = table.Columns;
                Name= table.Name;
            }
        }

        public override void AddPorts(NodeModel nodeModel)
        {

        }

        public override NodeModel ToNodelModel()
        {
            Table table = new Table(Id);
            table.Name = Name;
            Columns.ForEach(p => table.AddColumn(p));

            return ToNodelModel(table);

        }
    }
}
