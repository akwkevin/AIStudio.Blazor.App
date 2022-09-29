using System.ComponentModel;

namespace AIStudio.Entity
{
    public enum NoticeStatus
    {
        /// <summary>
        /// 草稿中
        /// </summary>
        [Description("草稿中")]
        Draft = 0,
        /// <summary>
        /// 已发布
        /// </summary>
        [Description("已发布")]
        Published = 1,
        /// <summary>
        /// 撤回
        /// </summary>
        [Description("撤回")]
        Withdraw = 2,
    }
}
