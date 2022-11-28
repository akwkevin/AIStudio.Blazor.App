using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Util.DiagramEntity
{
    /// <summary>
    /// 
    /// </summary>
    public class DiagramData
    {
        /// <summary>
        /// Gets or sets the nodes.
        /// </summary>
        /// <value>
        /// The nodes.
        /// </value>
        public DiagramNode[] Nodes { get; set; }
        /// <summary>
        /// Gets or sets the links.
        /// </summary>
        /// <value>
        /// The links.
        /// </value>
        public DiagramLink[] Links { get; set; }
        /// <summary>
        /// Gets or sets the groups.
        /// </summary>
        /// <value>
        /// The groups.
        /// </value>
        public DiagramGroup[] Groups { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DiagramData"/> class.
        /// </summary>
        public DiagramData()
        {
            Nodes = new DiagramNode[0];
            Links = new DiagramLink[0];
            Groups = new DiagramGroup[0];
        }
    }
}
