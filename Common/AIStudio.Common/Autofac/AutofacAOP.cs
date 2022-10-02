using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Autofac
{
    public class AutofacAOP : IInterceptor
    {
        public async void Intercept(IInvocation invocation)
        {
            await Befor(invocation);
            invocation.Proceed();
            await After(invocation);
        }

        public virtual async Task Befor(IInvocation invocation)
        {
            Console.WriteLine("执行前");
            await Task.CompletedTask;
        }

        public virtual async Task After(IInvocation invocation)
        {
            Console.WriteLine("执行后");
            await Task.CompletedTask;
        }
    }
}
