using System;
using System.Threading.Tasks;

namespace AIStudio.Common.DI.AOP
{
    /// <summary>
    /// AOP基类
    /// 注:不支持控制器,需要定义接口并实现接口,自定义AOP特性放到接口实现类上
    /// </summary>
    /// <seealso cref="System.Attribute" />
    public abstract class BaseAOPAttribute : Attribute
    {
        /// <summary>
        /// Befors the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual async Task Befor(IAOPContext context)
        {
            await Task.CompletedTask;
        }

        /// <summary>
        /// Afters the specified context.
        /// </summary>
        /// <param name="context">The context.</param>
        public virtual async Task After(IAOPContext context)
        {
            await Task.CompletedTask;
        }
    }
}
