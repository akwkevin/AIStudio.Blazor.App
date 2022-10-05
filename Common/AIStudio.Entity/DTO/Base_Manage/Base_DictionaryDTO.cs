using AIStudio.Common.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Base_Manage
{
    [Map(typeof(Base_Dictionary))]
    public class Base_DictionaryDTO : Base_Dictionary, IIdObject
    {
    }
}
