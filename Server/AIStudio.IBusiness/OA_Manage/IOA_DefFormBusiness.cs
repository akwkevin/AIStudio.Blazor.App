using AIStudio.Entity.DTO.OA_Manage;
using AIStudio.Entity.OA_Manage;
using AIStudio.IBusiness;
using AIStudio.Util;
using AIStudio.Util.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIStudio.Business.OA_Manage
{
    public interface IOA_DefFormBusiness : IBaseBusiness<OA_DefForm>
    {
        void LoadDefinition();
        Task LoadDefinitionAsync();
        Task<List<OA_DefFormTree>> GetTreeDataListAsync(SearchInput input);
        new Task<PageResult<OA_DefFormDTO>> GetDataListAsync(PageInput input);
        new Task<OA_DefFormDTO> GetTheDataAsync(string id);
        Task SaveDataAsync(OA_DefFormDTO theData);
        Task StartDataAsync(IdInputDTO input);
        Task StopDataAsync(IdInputDTO input);
    }


}