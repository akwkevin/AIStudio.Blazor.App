using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Quartz_Manage
{
    /// <summary>
    /// 任务管理表
    /// </summary>
    [Table("Quartz_Task")]

    public class Quartz_Task : BaseEntity
    {
        /// <summary>
        /// 作业名称
        /// </summary>
        [MaxLength(255)]
        public string? TaskName { get; set; }

        [MaxLength(255)]
        public string? GroupName { get; set; }

        /// <summary>
        /// 启用状态
        /// </summary>
        public bool IsEnabled { get; set; } = true;

        /// <summary>
        /// Cron表达式
        /// </summary>
        [MaxLength(50)]
        public string? Cron { get; set; }

        /// <summary>
        /// 任务类名
        /// </summary>
        [MaxLength(500)]
        public string? ActionClass { get; set; }

        [MaxLength(500)]
        public string? Remark { get; set; }

        #region HttpResultfulJob 使用
        [MaxLength(50)]
        public string? RequestType { get; set; }

        [MaxLength(500)]
        public string? ApiUrl { get; set; }

        [MaxLength(50)]
        public string? AuthKey { get; set; }

        [MaxLength(500)]
        public string? AuthValue { get; set; }
        #endregion

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }
}
