using AIStudio.Common.EventBus.Abstract;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.EventBus.Models
{
    public class SystemEvent : EventModel
    {    /// <summary>
         /// 操作人
         /// </summary>
        [MaxLength(64)]
        public string? CreatorId { get; set; }

        /// <summary>
        /// 操作人名称
        /// </summary>
        [MaxLength(64)]
        public string? CreatorName { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        [MaxLength(64)]
        public string? TenantId { get; set; }
        /// <summary>
        /// 日志类型
        /// </summary>
        public string? LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string? Message { get; set; }
    }
}
