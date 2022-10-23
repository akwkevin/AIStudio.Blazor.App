using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 
    /// </summary>
    [Table("Base_Test")]
    public class Base_Test
    {

        /// <summary>
        /// Id
        /// </summary>
        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public String? Id { get; set; }

        /// <summary>
        /// TenantId
        /// </summary>
        public String TenantId { get; set; }

        /// <summary>
        /// ParentId
        /// </summary>
        public String ParentId { get; set; }

        /// <summary>
        /// Type
        /// </summary>
        public Int32? Type { get; set; }

        /// <summary>
        /// Name
        /// </summary>
        public String Name { get; set; }

        /// <summary>
        /// Url
        /// </summary>
        public String Url { get; set; }

        /// <summary>
        /// Value
        /// </summary>
        public String Value { get; set; }

        /// <summary>
        /// NeedTest
        /// </summary>
        public Boolean? NeedTest { get; set; }

        /// <summary>
        /// Icon
        /// </summary>
        public String Icon { get; set; }

        /// <summary>
        /// Sort
        /// </summary>
        public Int32? Sort { get; set; }

        /// <summary>
        /// Deleted
        /// </summary>
        public Boolean? Deleted { get; set; }

        /// <summary>
        /// CreateTime
        /// </summary>
        public DateTime? CreateTime { get; set; }

        /// <summary>
        /// ModifyTime
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// CreatorId
        /// </summary>
        public String CreatorId { get; set; }

        /// <summary>
        /// CreatorName
        /// </summary>
        public String CreatorName { get; set; }

        /// <summary>
        /// ModifyId
        /// </summary>
        public String ModifyId { get; set; }

        /// <summary>
        /// ModifyName
        /// </summary>
        public String ModifyName { get; set; }

    }
}