using AIStudio.Entity.Base_Manage;
using AIStudio.Util.Common;
using AIStudio.Util.Mapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Entity.DTO.Base_Manage
{
    [Map(typeof(Base_LogException))]
    public class Base_LogExceptionDTO : Base_LogException, IIdObject
    {
    }
}
