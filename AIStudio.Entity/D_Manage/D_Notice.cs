using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.D_Manage
{
    /// <summary>
    /// 通告
    /// </summary>
    [Table("D_Notice")]
    public class D_Notice : BaseEntity
    {
        /// <summary>
        /// Mode=0，对应ALL,Mode=1,对应用户Id,Mode=2,对应角色Id，Mode=3，对应部门Id
        /// </summary>
        public string AnyId { get; set; }

        /// <summary>
        /// 内容
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// 类型=0，通告
        /// </summary>
        public NoticeType Type { get; set; }

        /// <summary>
        /// 类型=0，全部，=1，发给指定用户，=2，发给指定角色，=3，发给指定部门
        /// </summary>
        public NoticeMode Mode { get; set; }

        /// <summary>
        /// 附件
        /// </summary>
        public string Appendix { get; set; }

        /// <summary>
        /// 状态 =0草稿中，=1已发布，=2撤回
        /// </summary>
        public NoticeStatus Status { get; set; }
    }
}
