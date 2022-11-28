using Quartz.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AIStudio.Common.Quartz.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="Quartz.Logging.ILogProvider" />
    public class CustomConsoleLogProvider : ILogProvider
    {
        /// <summary>
        /// Gets the specified named logger.
        /// </summary>
        /// <param name="name">Name of the logger.</param>
        /// <returns>
        /// The logger reference.
        /// </returns>
        public Logger GetLogger(string name)
        {
            return (level, func, exception, parameters) =>
            {
                if (level >= LogLevel.Info && func != null)
                {
                    Console.WriteLine($"[{ DateTime.Now.ToLongTimeString()}] [{ level}] { func()} {string.Join(";", parameters.Select(p => p == null ? " " : p.ToString()))}  自定义日志{name}");
                }
                return true;
            };
        }
        /// <summary>
        /// Opens a nested diagnostics context. Not supported in EntLib logging.
        /// </summary>
        /// <param name="message">The message to add to the diagnostics context.</param>
        /// <returns>
        /// A disposable that when disposed removes the message from the context.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IDisposable OpenNestedContext(string message)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens the mapped context.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IDisposable OpenMappedContext(string key, string value)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Opens a mapped diagnostics context. Not supported in EntLib logging.
        /// </summary>
        /// <param name="key">A key.</param>
        /// <param name="value">A value.</param>
        /// <param name="destructure">Determines whether to call the destructor or not.</param>
        /// <returns>
        /// A disposable that when disposed removes the map from the context.
        /// </returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public IDisposable OpenMappedContext(string key, object value, bool destructure = false)
        {
            throw new NotImplementedException();
        }
    }
}
