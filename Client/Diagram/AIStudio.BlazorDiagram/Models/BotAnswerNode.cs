using AIStudio.BlazorDiagram.Components;
using Blazor.Diagrams.Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorDiagram.Models
{
    public class BotAnswerNode : DiagramNode
    {
        public string Answer { get; set; }

        public BotAnswerNode() : base()
        {

        }
        /// <summary>
        /// 将diagram信息转换成自己的类
        /// </summary>
        /// <param name="nodeModel"></param>
        public BotAnswerNode(NodeModel nodeModel) : base(nodeModel)
        {
            if (nodeModel is BotAnswerNodeModel botAnswerNodeModel)
            {
                Answer = botAnswerNodeModel.Answer;
            }
        }

        public override NodeModel ToNodelModel()
        {
            BotAnswerNodeModel botAnswerNodeModel = new BotAnswerNodeModel(Id);
            botAnswerNodeModel.Answer = Answer;

            return ToNodelModel(botAnswerNodeModel);

        }
    }
}
