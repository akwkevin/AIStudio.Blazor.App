using AntDesign;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.BlazorUI.Core
{
    public class FeedbackBase : FeedbackComponent<string>, ILoading
    {
        public bool Loading { get; set; }
    }
}
