using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Castle.DynamicProxy;

namespace AIStudio.Common.DI.AOP
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Castle.DynamicProxy.AsyncInterceptorBase" />
    public class CastleInterceptor : AsyncInterceptorBase
    {
        /// <summary>
        /// The service provider
        /// </summary>
        private readonly IServiceProvider _serviceProvider;
        /// <summary>
        /// Initializes a new instance of the <see cref="CastleInterceptor"/> class.
        /// </summary>
        /// <param name="serviceProvider">The service provider.</param>
        public CastleInterceptor(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        /// <summary>
        /// The aop context
        /// </summary>
        private IAOPContext _aopContext;
        /// <summary>
        /// The aops
        /// </summary>
        private List<BaseAOPAttribute> _aops;
        /// <summary>
        /// Befors this instance.
        /// </summary>
        private async Task Befor()
        {
            foreach (var aAop in _aops)
            {
                await aAop.Befor(_aopContext);
            }
        }
        /// <summary>
        /// Afters this instance.
        /// </summary>
        private async Task After()
        {
            foreach (var aAop in _aops)
            {
                await aAop.After(_aopContext);
            }
        }
        /// <summary>
        /// Initializes the specified invocation.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        private void Init(IInvocation invocation)
        {
            _aopContext = new CastleAOPContext(invocation, _serviceProvider);

            _aops = invocation.MethodInvocationTarget.GetCustomAttributes(typeof(BaseAOPAttribute), true)
                .Concat(invocation.InvocationTarget.GetType().GetCustomAttributes(typeof(BaseAOPAttribute), true))
                .Select(x => (BaseAOPAttribute)x)
                .ToList();
        }


        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="T:Castle.DynamicProxy.IInvocationProceedInfo" />.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo" />.</param>
        protected override async Task InterceptAsync(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task> proceed)
        {
            Init(invocation);

            await Befor();
            await proceed(invocation, proceedInfo);
            await After();
        }

        /// <summary>
        /// Override in derived classes to intercept method invocations.
        /// </summary>
        /// <typeparam name="TResult">The type of the <see cref="T:System.Threading.Tasks.Task`1" /><see cref="P:System.Threading.Tasks.Task`1.Result" />.</typeparam>
        /// <param name="invocation">The method invocation.</param>
        /// <param name="proceedInfo">The <see cref="T:Castle.DynamicProxy.IInvocationProceedInfo" />.</param>
        /// <param name="proceed">The function to proceed the <paramref name="proceedInfo" />.</param>
        /// <returns>
        /// A <see cref="T:System.Threading.Tasks.Task" /> object that represents the asynchronous operation.
        /// </returns>
        protected override async Task<TResult> InterceptAsync<TResult>(IInvocation invocation, IInvocationProceedInfo proceedInfo, Func<IInvocation, IInvocationProceedInfo, Task<TResult>> proceed)
        {
            Init(invocation);

            TResult result;

            await Befor();
            result = await proceed(invocation, proceedInfo);
            await After();

            return result;
        }
    }
}
