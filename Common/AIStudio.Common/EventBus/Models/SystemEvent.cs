using AIStudio.Common.EventBus.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.EventBus.Models
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Common.EventBus.Abstract.EventModel" />
    public class SystemEvent : EventModel
    {    /// <summary>
         /// 操作人
         /// </summary>
        [MaxLength(64)]
        public string? CreatorId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        /// <value>
        /// The name of the creator.
        /// </value>
        [MaxLength(64)]
        public string? CreatorName { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        /// <value>
        /// The tenant identifier.
        /// </value>
        [MaxLength(64)]
        public string? TenantId { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        /// <value>
        /// The type of the log.
        /// </value>
        public string? LogType { get; set; }

        /// <summary>
        /// 日志名称
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [MaxLength(128)]
        public string Name { get; set; } = "";

        /// <summary>
        /// 日志内容
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string? Message { get; set; }
    }
}
