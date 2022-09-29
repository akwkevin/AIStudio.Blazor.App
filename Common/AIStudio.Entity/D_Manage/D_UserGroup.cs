using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.D_Manage
{
    /// <summary>
    /// 用户群组
    /// </summary>
    [Table("D_UserGroup")]
    public class D_UserGroup : BaseEntity
    {
        public string UserIds { get; set; }
        public string UserNames { get; set; }
        [MaxLength(255)]
        public string Title { get; set; }
        public int Type { get; set; }
        public string Remark { get; set; }
        public string Appendix { get; set; }
        [MaxLength(500)]
        public string Avatar { get; set; }
        public string ManagerIds { get; set; }
        public string ManagerNames { get; set; }
        [MaxLength(50)]
        public string Name { get; set; }

    }
}
