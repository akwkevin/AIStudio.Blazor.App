using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.OA_Manage
{
    /// <summary>
    /// OA表单定义
    /// </summary>
    [Table("OA_DefForm")]
    public class OA_DefForm : BaseEntity
    {
        public string? WorkflowJSON { get; set; }
        [MaxLength(50)]
        public string? JSONId { get; set; }

        public int JSONVersion { get; set; }
        [MaxLength(50)]
        public string? Type { get; set; }
        [MaxLength(255)]
        public string? Name { get; set; }
        public string? Text { get; set; }
        public int Sort { get; set; }

        public int Status { get; set; }
        /// <summary>
        /// 权限值
        /// </summary>
        [MaxLength(500)]
        public string? Value { get; set; }
    }
}
