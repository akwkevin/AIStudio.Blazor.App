namespace Simple.Common.Filters;

/// <summary>
/// 禁用请求记录过滤器,废弃，改成RequestRecordAttribute，有标记才记录
/// </summary>
[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
public class DisabledRequestRecordAttribute : Attribute
{
}
