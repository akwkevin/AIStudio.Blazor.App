using Castle.DynamicProxy;

namespace AIStudio.Common.DI.AOP
{
    public class TestAOP : Castle.DynamicProxy.IInterceptor
    {
        public void Intercept(IInvocation invocation)
        {
            Console.WriteLine("Before target call");
            try
            {
                invocation.Proceed();
            }
            catch (Exception)
            {
                Console.WriteLine("Target threw an exception!");
                throw;
            }
            finally
            {
                Console.WriteLine("After target call");
            }
        }
    }
}
