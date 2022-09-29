using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.D_Manage
{
    /// <summary>
    /// 通告读取标记
    /// </summary>
    [Table("D_NoticeReadingMarks")]
    public class D_NoticeReadingMarks : BaseEntity
    {
        public string NoticeId { get; set; }
    }
}
