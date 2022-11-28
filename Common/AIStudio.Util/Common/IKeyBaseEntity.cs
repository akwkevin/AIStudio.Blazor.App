namespace AIStudio.Util.Common
{
    /// <summary>
    /// 实力类主键
    /// </summary>
    /// <typeparam name="TPrimaryKey">The type of the primary key.</typeparam>
    public interface IKeyBaseEntity<TPrimaryKey>
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        TPrimaryKey? Id { get; set; }
    }

    /// <summary>
    /// 字符串实力类主键
    /// </summary>
    public interface IKeyBaseEntity : IKeyBaseEntity<string>
    {
       
    }
}
