using AIStudio.Common.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Base_Manage
{
    [Map(typeof(Base_UserLog))]
    public class Base_UserLogDTO : Base_UserLog, IIdObject
    {

    }
}
