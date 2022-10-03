using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.DI.AOP
{
    public class TestAOP : BaseAOPAttribute
    {
        public override async Task Befor(IAOPContext context)
        {
            Console.WriteLine("执行前");
            await Task.CompletedTask;
        }

        public override async Task After(IAOPContext context)
        {
            Console.WriteLine("执行后");
            await Task.CompletedTask;
        }
    }
}
