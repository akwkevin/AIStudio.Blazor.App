using SqlSugar;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 操作记录表
    /// </summary> 
    [SplitTable(SplitType.Month)]
    [SugarTable("Base_LogSystem_{year}{month}{day}")]//生成表名格式 3个变量必须要有
    public class Base_LogSystem : ReadOnlyBaseEntity
    {
        /// <summary>
        /// 日志类型
        /// </summary>
        public string? LogType { get; set; }

        /// <summary>
        /// 日志名称
        /// </summary>
        [MaxLength(128)]
        public string Name { get; set; } = "";

        /// <summary>
        /// 日志内容
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 日志时间
        /// </summary>
        public DateTime LogTime { get; set; }
    }
}