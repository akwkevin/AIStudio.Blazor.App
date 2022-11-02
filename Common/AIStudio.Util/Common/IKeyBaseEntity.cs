namespace AIStudio.Util.Common
{
    public interface IKeyBaseEntity<TPrimaryKey>
    {
        TPrimaryKey? Id { get; set; }
    }
}
