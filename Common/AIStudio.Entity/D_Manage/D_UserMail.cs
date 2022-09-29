using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.D_Manage
{
    /// <summary>
    /// 系统邮件表
    /// </summary>
    [Table("D_UserMail")]
    public class D_UserMail : MessageBaseEntity
    {
        public string Title { get; set; }
        public UserMailType Type { get; set; }
        public string CCIds { get; set; }
        public string CCNames { get; set; }
        public string ReadingMarks { get; set; }
        public bool StarMark { get; set; }
        public string Appendix { get; set; }

        /// <summary>
        /// 状态 =0草稿中，=1已发布，=2撤回
        /// </summary>
        public EmailStatus Status { get; set; }
    }
}
