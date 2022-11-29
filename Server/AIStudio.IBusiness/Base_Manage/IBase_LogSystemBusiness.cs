using AIStudio.Common.EventBus.Abstract;
using AIStudio.Common.EventBus.Models;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.IBusiness.ISplitTableBaseBusiness&lt;AIStudio.Entity.Base_Manage.Base_LogSystem&gt;" />
    /// <seealso cref="AIStudio.Common.EventBus.Abstract.IEventHandler&lt;AIStudio.Common.EventBus.Models.SystemEvent&gt;" />
    public interface IBase_LogSystemBusiness : ISplitTableBaseBusiness<Base_LogSystem>, IEventHandler<SystemEvent>
    {
 
    }


}