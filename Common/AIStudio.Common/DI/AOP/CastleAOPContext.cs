using Castle.DynamicProxy;
using System;
using System.Reflection;

namespace AIStudio.Common.DI.AOP
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Common.DI.AOP.IAOPContext" />
    public class CastleAOPContext : IAOPContext
    {
        /// <summary>
        /// The invocation
        /// </summary>
        private readonly IInvocation _invocation;
        /// <summary>
        /// Initializes a new instance of the <see cref="CastleAOPContext"/> class.
        /// </summary>
        /// <param name="invocation">The invocation.</param>
        /// <param name="serviceProvider">The service provider.</param>
        public CastleAOPContext(IInvocation invocation, IServiceProvider serviceProvider)
        {
            _invocation = invocation;
            ServiceProvider = serviceProvider;
        }
        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        public IServiceProvider ServiceProvider { get; }

        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        public object[] Arguments => _invocation.Arguments;

        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <value>
        /// The generic arguments.
        /// </value>
        public Type[] GenericArguments => _invocation.GenericArguments;

        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        public MethodInfo Method => _invocation.Method;

        /// <summary>
        /// Gets the method invocation target.
        /// </summary>
        /// <value>
        /// The method invocation target.
        /// </value>
        public MethodInfo MethodInvocationTarget => _invocation.MethodInvocationTarget;

        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        public object Proxy => _invocation.Proxy;

        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        /// <value>
        /// The return value.
        /// </value>
        public object ReturnValue { get => _invocation.ReturnValue; set => _invocation.ReturnValue = value; }

        /// <summary>
        /// Gets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        public Type TargetType => _invocation.TargetType;

        /// <summary>
        /// Gets the invocation target.
        /// </summary>
        /// <value>
        /// The invocation target.
        /// </value>
        public object InvocationTarget => _invocation.InvocationTarget;
    }
}
