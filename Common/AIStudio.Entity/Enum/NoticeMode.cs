using System.ComponentModel;

namespace AIStudio.Entity
{
    public enum NoticeMode
    {
        /// <summary>
        /// 所有
        /// </summary>        
        [Description("所有")]
        All = 0,
        /// <summary>
        /// 用户
        /// </summary>
        [Description("用户")]
        User = 1,
        /// <summary>
        /// 角色
        /// </summary>
        [Description("角色")]
        Role = 2,
        /// <summary>
        /// 部门
        /// </summary>
        [Description("部门")]
        Department = 3,
    }
}
