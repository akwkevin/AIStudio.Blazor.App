using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_DictionaryBusiness : IBaseBusiness<Base_Dictionary>
    {
        Task<List<Base_DictionaryTree>> GetTreeDataListAsync(SearchInput input);
    }


}