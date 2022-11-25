using AIStudio.BlazorDiagram.Components;
using Blazor.Diagrams.Core;
using Blazor.Diagrams.Core.Models;
using Blazor.Diagrams.Core.Models.Base;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public static class DiagramHelper
    {
        public static DiagramNode GetDiagramNode(NodeModel nodeModel)
        {
            if (nodeModel is Table)
            {
                return new DatabaseDesignerTableNode(nodeModel);
            }
            else if (nodeModel is BotAnswerNodeModel)
            {
                return new BotAnswerNode(nodeModel);
            }
            else if (nodeModel is FlowchartNodelModel)
            {
                return new FlowchartNode(nodeModel);
            }
            else
            {
                return new DiagramNode(nodeModel);
            }
        }

        public static DiagramLink GetDiagramLink(BaseLinkModel linkModel)
        {
            if (linkModel.SourcePort is ColumnPort || linkModel.TargetPort is ColumnPort)
            {
                return new DatabaseDesignerTableLink(linkModel);
            }
            else
            {
                return new DiagramLink(linkModel);
            }
        }

        public static string ToJson (Diagram diagram)
        {
            var json = JsonConvert.SerializeObject(new
            {
                Nodes = diagram.Nodes.Select(p => DiagramHelper.GetDiagramNode(p)),
                Links = diagram.Links.Select(p => DiagramHelper.GetDiagramLink(p))
            }, new JsonSerializerSettings
            {
                ReferenceLoopHandling = ReferenceLoopHandling.Ignore
            });

            return json;
        }

        public static void ToObject(Diagram diagram, string json)
        {
            diagram.Nodes.Clear();
            diagram.Links.Clear();

            List<NodeModel> nodes = new List<NodeModel>();
            var data = JsonConvert.DeserializeObject<DiagramData>(json, new JsonConverter[] { new DiagramNodeConverter(), new DiagramLinkConverter() });
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

        /// <summary>  
        /// 获取枚举描述
        /// </summary>  
        /// <param name="en">枚举</param>  
        /// <returns>返回枚举的描述 </returns>  
        public static string GetDescription(Enum en)
        {
            Type type = en.GetType();   //获取类型  
            MemberInfo[] memberInfos = type.GetMember(en.ToString());   //获取成员  
            if (memberInfos != null && memberInfos.Length > 0)
            {
                DescriptionAttribute[] attrs = memberInfos[0].GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];   //获取描述特性  
                if (attrs != null && attrs.Length > 0)
                {
                    return attrs[0].Description;    //返回当前描述
                }
            }
            return en.ToString();
        }
    }
}
