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

        [MaxLength(255)]
        public string TaskName { get; set; }

        [MaxLength(255)]
        public string GroupName { get; set; }

        [MaxLength(50)]
        public string Interval { get; set; }

        [MaxLength(500)]
        public string ApiUrl { get; set; }

        [MaxLength(50)]
        public string AuthKey { get; set; }

        [MaxLength(500)]
        public string AuthValue { get; set; }

        [MaxLength(500)]
        public string Describe { get; set; }

        [MaxLength(50)]
        public string RequestType { get; set; }
        public DateTime? LastRunTime { get; set; }
        public int Status { get; set; }

        public bool ForbidOperate { get; set; }
        public bool ForbidEdit { get; set; }
    }
}
