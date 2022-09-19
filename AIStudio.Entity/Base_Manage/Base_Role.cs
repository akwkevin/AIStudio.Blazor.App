using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 系统角色表
    /// </summary>
    [Table("Base_Role")]
    public class Base_Role : BaseEntity
    {

        /// <summary>
        /// 角色名
        /// </summary    
        [Required(ErrorMessage = "角色名不能为空")]
        public String RoleName { get; set; }

    }
}