using AIStudio.Common.Mapper;
using AIStudio.Entity.Quartz_Manage;
using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Quartz_Manage
{
    [Map(typeof(Quartz_Task))]
    public class Quartz_TaskDTO : Quartz_Task, IIdObject
    {

    }
}
