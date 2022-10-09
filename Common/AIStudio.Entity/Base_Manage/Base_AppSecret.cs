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
        public string? AppId { get; set; }

        /// <summary>
        /// 应用密钥
        /// </summary>
        [Required(ErrorMessage = "请输入密钥")]
        public string? AppSecret { get; set; }

        /// <summary>
        /// 应用名
        /// </summary>
        [Required(ErrorMessage = "请输入应用名")]
        public string? AppName { get; set; }

    }
}