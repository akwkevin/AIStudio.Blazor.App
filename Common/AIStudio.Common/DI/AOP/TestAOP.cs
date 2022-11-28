using Castle.DynamicProxy;

namespace AIStudio.Common.DI.AOP
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Castle.DynamicProxy.IInterceptor" />
    public class TestAOP : Castle.DynamicProxy.IInterceptor
    {
        /// <summary>
        /// Intercepts the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
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
