using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 系统权限表
    /// </summary>
    [Table("Base_Action")]
    public class Base_Action : BaseEntity
    {

        /// <summary>
        /// 父级Id
        /// </summary>
        public string? ParentId { get; set; }

        /// <summary>
        /// 类型,菜单=0,页面=1,权限=2
        /// </summary>
        [Required]
        public ActionType Type { get; set; }

        /// <summary>
        /// 权限名/菜单名
        /// </summary>
        [Required(ErrorMessage = "请输入菜单名")]
        public string? Name { get; set; }

        /// <summary>
        /// 菜单地址
        /// </summary>
        public string? Url { get; set; }

        /// <summary>
        /// 权限值
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 是否需要权限(仅页面有效)
        /// </summary>
        [Required]
        public bool NeedAction { get; set; }

        /// <summary>
        /// 图标
        /// </summary>
        public string? Icon { get; set; }

        /// <summary>
        /// 排序
        /// </summary>
        public int Sort { get; set; }
    }


}