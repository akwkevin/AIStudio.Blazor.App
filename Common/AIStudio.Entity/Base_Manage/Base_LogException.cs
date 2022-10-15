using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 异常记录表
    /// </summary>
    [Table("Base_LogException")]
    public class Base_LogException : ReadOnlyBaseEntity
    {
        /// <summary>
        /// 异常事件Id
        /// </summary>
        public string EventId { get; set; }

        /// <summary>
        /// 异常名称
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 异常消息
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// 类名称
        /// </summary>
        [MaxLength(256)]
        public string ClassName { get; set; }

        /// <summary>
        /// 方法名称
        /// </summary>
        [MaxLength(256)]
        public string MethodName { get; set; }

        /// <summary>
        /// 异常源
        /// </summary>
        public string ExceptionSource { get; set; }

        /// <summary>
        /// 堆栈信息
        /// </summary>
        public string StackTrace { get; set; }

        /// <summary>
        /// 请求参数
        /// </summary>
        public string Parameters { get; set; }

        /// <summary>
        /// 异常时间
        /// </summary>
        public DateTimeOffset LogTime { get; set; }
    }
}
