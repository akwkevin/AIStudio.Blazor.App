using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    /// <summary>
    /// 通用表单查询配置表
    /// </summary>
    [Table("Base_CommonFormConfig")]
    public class Base_CommonFormConfig : BaseEntity
    {
        /// <summary>
        /// 表名
        /// </summary>
        [Required(ErrorMessage = "请输入表名")]
        public string Table { get; set; }
        /// <summary>
        /// 列头
        /// </summary>
        [Required(ErrorMessage = "请输入列头")]
        public string Header { get; set; }
        /// <summary>
        /// 属性名
        /// </summary>
        [Required(ErrorMessage = "请输入属性名")]
        public string PropertyName { get; set; }
        /// <summary>
        /// 属性类型
        /// </summary>
        [Required(ErrorMessage = "请输入属性类型")]
        public string PropertyType { get; set; }
        /// <summary>
        /// 显示索引
        /// </summary>
        [Required(ErrorMessage = "请输入索引")]
        public int DisplayIndex { get; set; }

        /// <summary>
        /// 配置类型 查询=0，列表=1
        /// </summary>
        [Required(ErrorMessage = "请输入配置类型")]
        public int Type { get; set; }

        /// <summary>
        /// 格式化
        /// </summary>
        public string? StringFormat { get; set; }
        /// <summary>
        /// 是否显示 Visible = 0,Hidden = 1,Collapsed = 2
        /// </summary>
        public int Visibility { get; set; }

        #region 编辑项和列表使用
        /// <summary>
        /// 控件类型，仅控件框使用
        /// </summary>
        public ControlType ControlType { get; set; }

        /// <summary>
        /// 只读
        /// </summary>
        public bool IsReadOnly { get; set; }

        /// <summary>
        /// 必输项
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        /// 字典名
        /// </summary>
        public string? ItemSource { get; set; }

        /// <summary>
        /// 默认值
        /// </summary>
        public string? Value { get; set; }

        /// <summary>
        /// 正则校验表达式
        /// </summary>
        public string? Regex { get; set; }

        /// <summary>
        /// 错误信息
        /// </summary>
        public string? ErrorMessage { get; set; }
        #endregion

        #region 列表专用

        /// <summary>
        /// 排序名
        /// </summary>
        public string? SortMemberPath { get; set; }

        /// <summary>
        /// 转换器
        /// </summary>
        public string? Converter { get; set; }

        /// <summary>
        /// 转换参数
        /// </summary>
        public string? ConverterParameter { get; set; }

        /// <summary>
        /// 对齐方式 Left = 0,Center = 1,Right = 2,Stretch = 3
        /// </summary>
        public int HorizontalAlignment { get; set; }

        /// <summary>
        /// 最大宽度
        /// </summary>
        public double MaxWidth { get; set; }

        /// <summary>
        /// 最小宽度
        /// </summary>
        public double MinWidth { get; set; }

        /// <summary>
        /// 列表宽度
        /// </summary>
        public string? Width { get; set; }

        /// <summary>
        /// 是否可以重排
        /// </summary>
        public bool CanUserReorder { get; set; }

        /// <summary>
        /// 是否可以调整大小
        /// </summary>
        public bool CanUserResize { get; set; }

        /// <summary>
        /// 是否可以排序
        /// </summary>
        public bool CanUserSort { get; set; }

        /// <summary>
        /// 单元格样式，暂未实现
        /// </summary>
        public string? CellStyle { get; set; }

        /// <summary>
        /// 列头样式，赞未实现
        /// </summary>
        public string? HeaderStyle { get; set; }

        /// <summary>
        /// 背景颜色触发公式
        /// </summary>
        public string? BackgroundExpression { get; set; }
        /// <summary>
        /// 前景颜色触发公式
        /// </summary>
        public string? ForegroundExpression { get; set; }
        #endregion



    }
}
