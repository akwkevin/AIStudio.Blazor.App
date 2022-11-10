namespace AIStudio.Util.Common
{
    /// <summary>
    /// 实力类主键
    /// </summary>
    /// <typeparam name="TPrimaryKey"></typeparam>
    public interface IKeyBaseEntity<TPrimaryKey>
    {
        TPrimaryKey? Id { get; set; }
    }

    /// <summary>
    /// 字符串实力类主键
    /// </summary>
    public interface IKeyBaseEntity : IKeyBaseEntity<string>
    {
       
    }
}
