using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.Quartz.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class JobExcuteResult
    {
        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="JobExcuteResult"/> is success.
        /// </summary>
        /// <value>
        ///   <c>true</c> if success; otherwise, <c>false</c>.
        /// </value>
        public bool Success { get; set; }
        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JobExcuteResult"/> class.
        /// </summary>
        /// <param name="success">if set to <c>true</c> [success].</param>
        /// <param name="message">The message.</param>
        public JobExcuteResult(bool success, string message)
        {
            Success = success;
            Message = message;
        }
    }
}
