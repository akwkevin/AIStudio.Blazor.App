using System;
using System.Reflection;

namespace AIStudio.Common.DI.AOP
{
    /// <summary>
    /// 
    /// </summary>
    public interface IAOPContext
    {
        /// <summary>
        /// Gets the service provider.
        /// </summary>
        /// <value>
        /// The service provider.
        /// </value>
        IServiceProvider ServiceProvider { get; }
        /// <summary>
        /// Gets the arguments.
        /// </summary>
        /// <value>
        /// The arguments.
        /// </value>
        object[] Arguments { get; }
        /// <summary>
        /// Gets the generic arguments.
        /// </summary>
        /// <value>
        /// The generic arguments.
        /// </value>
        Type[] GenericArguments { get; }
        /// <summary>
        /// Gets the method.
        /// </summary>
        /// <value>
        /// The method.
        /// </value>
        MethodInfo Method { get; }
        /// <summary>
        /// Gets the method invocation target.
        /// </summary>
        /// <value>
        /// The method invocation target.
        /// </value>
        MethodInfo MethodInvocationTarget { get; }
        /// <summary>
        /// Gets the proxy.
        /// </summary>
        /// <value>
        /// The proxy.
        /// </value>
        object Proxy { get; }
        /// <summary>
        /// Gets or sets the return value.
        /// </summary>
        /// <value>
        /// The return value.
        /// </value>
        object ReturnValue { get; set; }
        /// <summary>
        /// Gets the type of the target.
        /// </summary>
        /// <value>
        /// The type of the target.
        /// </value>
        Type TargetType { get; }
        /// <summary>
        /// Gets the invocation target.
        /// </summary>
        /// <value>
        /// The invocation target.
        /// </value>
        object InvocationTarget { get; }
    }
}
