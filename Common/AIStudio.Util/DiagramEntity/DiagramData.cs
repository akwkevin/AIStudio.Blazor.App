using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.DiagramEntity
{
    public class DiagramData
    {
        public DiagramNode[] Nodes { get; set; }
        public DiagramLink[] Links { get; set; }
        public DiagramGroup[] Groups { get; set; }

        public DiagramData()
        {
            Nodes = new DiagramNode[0];
            Links = new DiagramLink[0];
            Groups = new DiagramGroup[0];
        }
    }
}
