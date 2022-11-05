namespace AIStudio.Util.Common
{
    public interface IKeyBaseEntity<TPrimaryKey>
    {
        TPrimaryKey? Id { get; set; }
    }
    public interface IKeyBaseEntity : IKeyBaseEntity<string>
    {
       
    }
}
