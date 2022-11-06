using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.OA_Manage
{
    /// <summary>
    /// OA表单流程
    /// </summary>
    [Table("OA_UserFormStep")]
    public class OA_UserFormStep : BaseEntity
    {
        [MaxLength(50)]
        public string? UserFormId { get; set; }
        public string? RoleIds { get; set; }
        public string? RoleNames { get; set; }
        public string? Remarks { get; set; }
        public int Status { get; set; }
        public string? StepName { get; set; }

        public string? Avatar { get; set; }
    }
}
