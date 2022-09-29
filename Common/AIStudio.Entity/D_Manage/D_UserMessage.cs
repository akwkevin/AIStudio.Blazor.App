using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.D_Manage
{
    /// <summary>
    /// 系统消息表
    /// </summary>
    [Table("D_UserMessage")]
    public class D_UserMessage : MessageBaseEntity
    {
        public UserMessageType Type { get; set; }
        public string ReadingMarks { get; set; }
        public string GroupId { get; set; }

        public string GroupName { get; set; }

        /// <summary>
        /// 状态 =0草稿中，=1已发送，=2废弃撤回，=3发送失败
        /// </summary>
        public UserMessageStatus Status { get; set; }
    }
}
