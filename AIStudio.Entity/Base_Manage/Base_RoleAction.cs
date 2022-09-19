using System;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 角色权限表
    /// </summary>
    [Table("Base_RoleAction")]
    public class Base_RoleAction : BaseEntity
    {
        /// <summary>
        /// 用户Id
        /// </summary>
        public String RoleId { get; set; }

        /// <summary>
        /// 权限Id
        /// </summary>
        public String ActionId { get; set; }

    }
}