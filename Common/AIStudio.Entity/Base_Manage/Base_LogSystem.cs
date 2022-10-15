using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 操作记录表
    /// </summary>
    [Table("Base_LogSystem")]
    public class Base_LogSystem : ReadOnlyBaseEntity
    {

        /// <summary>
        /// 日志类型
        /// </summary>
        public string? LogType { get; set; }

        /// <summary>
        /// 日志内容
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTimeOffset LogTime { get; set; }
    }
}