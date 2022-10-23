using AIStudio.Util.Mapper;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;
using System.Collections.Generic;

namespace AIStudio.Entity.DTO.Base_Manage
{
    [Map(typeof(Base_Test))]
    public class Base_TestDTO : Base_Test, IIdObject
    {

    }
}
