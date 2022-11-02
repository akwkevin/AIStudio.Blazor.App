using AIStudio.Util.Common;
using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity
{
    public abstract class KeyBaseEntity<TPrimaryKey> : IKeyBaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 自然主键
        /// </summary>
        [Key, Column(Order = 1)]
        [MaxLength(50)]
        public virtual TPrimaryKey? Id { get; set; }
    }

    public abstract class ReadOnlyBaseEntity<TPrimaryKey> : KeyBaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 创建时间
        /// </summary>
        [SplitField]
        public DateTime CreateTime { get; set; }

        /// <summary>
        /// 创建人Id
        /// </summary>
        [MaxLength(50)]
        public TPrimaryKey? CreatorId { get; set; }

        /// <summary>
        /// 创建人
        /// </summary>
        [MaxLength(255)]
        public string? CreatorName { get; set; }

        /// <summary>
        /// 租户Id
        /// </summary>
        [MaxLength(50)]
        public virtual TPrimaryKey? TenantId { get; set; }

        //[SqlSugar.SugarColumn(IsEnableUpdateVersionValidation = true)]//标识版本字段
        //public long Ver { get; set; }
    }

    /// <summary>
    /// 泛型实体基类(物理删除，无Deleted)
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class PhysicDeleteBaseEntity<TPrimaryKey> : ReadOnlyBaseEntity<TPrimaryKey>
    {       
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime? ModifyTime { get; set; }

        /// <summary>
        /// 修改人Id
        /// </summary>
        [MaxLength(50)]
        public TPrimaryKey? ModifyId { get; set; }

        /// <summary>
        /// 修改人
        /// </summary>
        [MaxLength(255)]
        public string? ModifyName { get; set; }     
    }

    /// <summary>
    /// 泛型实体基类，软删除
    /// </summary>
    /// <typeparam name="TPrimaryKey">主键类型</typeparam>
    public abstract class BaseEntity<TPrimaryKey> : PhysicDeleteBaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// 否已删除
        /// </summary>
        public bool Deleted { get; set; }      
    }

    /// <summary>
    /// 主键基类
    /// </summary>
    public abstract class KeyBaseEntity : KeyBaseEntity<string>
    {

    }

    /// <summary>
    /// 定义默认主键类型为String的只读实体基类
    /// </summary>

    public abstract class ReadOnlyBaseEntity : ReadOnlyBaseEntity<string>
    {


    }

    /// <summary>
    /// 定义默认主键类型为String的硬删除实体基类
    /// </summary>

    public abstract class PhysicDeleteBaseEntity : PhysicDeleteBaseEntity<string>
    {


    }

    /// <summary>
    /// 定义默认主键类型为String的实体基类
    /// </summary>

    public abstract class BaseEntity : BaseEntity<string>
    {


    }

}
