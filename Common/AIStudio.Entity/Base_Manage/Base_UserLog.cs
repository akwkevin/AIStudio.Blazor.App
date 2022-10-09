using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 操作记录表
    /// </summary>
    [Table("Base_UserLog")]
    public class Base_UserLog
    {
        /// <summary>
        /// 自然主键
        /// </summary>
        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public string? Id { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        public string? CreatorId { get; set; }

        /// <summary>
        /// 创建人姓名
        /// </summary>
        public string? CreatorName { get; set; }

        /// <summary>
        /// 日志类型
        /// </summary>
        public string? LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string? LogContent { get; set; }
    }
}