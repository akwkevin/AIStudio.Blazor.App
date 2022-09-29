using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 应用密钥表
    /// </summary>
    [Table("Base_AppSecret")]
    public class Base_AppSecret : BaseEntity
    {
        /// <summary>
        /// 应用Id
        /// </summary>
        [Required(ErrorMessage = "请输入应用Id")]
        public String AppId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        [Required(ErrorMessage = "请输入密钥")]
        public String AppSecret { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        [Required(ErrorMessage = "请输入应用名")]
        public String AppName { get; set; }

    }
}