using AIStudio.Common.Mapper;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;
using Newtonsoft.Json;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_ActionBusiness : IBaseBusiness<Base_Action>
    {
        Task<List<Base_Action>> GetDataListAsync(Base_ActionsInputDTO input);
        Task<List<Base_ActionTree>> GetTreeDataListAsync(Base_ActionsInputDTO input);
        Task<List<Base_Action>> GetAllActionListAsync();
        Task<List<Base_ActionTree>> GetMenuTreeListAsync(Base_ActionsInputDTO input);
        Task<List<Base_Action>> GetPermissionListAsync(Base_ActionsInputDTO input);
        Task AddDataAsync(Base_ActionEditInputDTO input);
        Task UpdateDataAsync(Base_ActionEditInputDTO input);
        Task SaveDataAsync(Base_ActionEditInputDTO input);
    }


}