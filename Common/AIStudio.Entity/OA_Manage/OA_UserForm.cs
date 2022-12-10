using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.OA_Manage
{
    /// <summary>
    /// OA表单流程
    /// </summary>
    [Table("OA_UserForm")]
    public class OA_UserForm : BaseEntity
    {
        [MaxLength(50)]
        public string? DefFormId { get; set; }
        [MaxLength(255)]
        public string? DefFormName { get; set; }
        [MaxLength(50)]
        public string? JsonId { get; set; }      
        public int JsonVersion { get; set; }
        public string? WorkflowJSON { get; set; }
        public int Grade { get; set; }
        public double Flag { get; set; }
        public string? Remarks { get; set; }
        public string? Appendix { get; set; }
        public string? ExtendJSON { get; set; }
        [MaxLength(255)]
        public string? ApplicantUser { get; set; }
        [MaxLength(50)]
        public string? ApplicantUserId { get; set; }
        [MaxLength(255)]
        public string? ApplicantDepartment { get; set; }
        [MaxLength(50)]
        public string? ApplicantDepartmentId { get; set; }
        [MaxLength(255)]
        public string? ApplicantRole { get; set; }
        [MaxLength(50)]
        public string? ApplicantRoleId { get; set; }

        public string? UserRoleNames { get; set; }
        public string? UserRoleIds { get; set; }
        public string? AlreadyUserNames { get; set; }
        public string? AlreadyUserIds { get; set; }
        public int Status { get; set; }
        [MaxLength(50)]
        public string? Type { get; set; }
        [MaxLength(50)]
        public string? SubType { get; set; }
        [MaxLength(50)]
        public string? Unit { get; set; }
        public DateTime? ExpectedDate { get; set; }
        [MaxLength(500)]
        public string? CurrentNode { get; set; }

        public string? UserIds { get; set; }
        public string? UserNames { get; set; }
        public string? Text { get; set; }
    }
}
