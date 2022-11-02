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
        Task<List<OA_DefFormTree>> GetTreeDataListAsync(string type, List<string> roleidlist);
        Task<PageResult<OA_DefFormDTO>> GetDataListAsync(PageInput pagination);
        Task<OA_DefFormDTO> GetTheDataAsync(string id);
    }


}