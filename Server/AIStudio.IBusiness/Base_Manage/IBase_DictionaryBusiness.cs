using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_DictionaryBusiness : IBaseBusiness<Base_Dictionary>
    {
        Task<List<Base_Dictionary>> GetDataListAsync(Base_DictionaryInputDTO input);
        Task<List<Base_DictionaryTree>> GetTreeDataListAsync(Base_DictionaryInputDTO input);
    }

    public class Base_DictionaryInputDTO
    {
        public string[] ActionIds { get; set; }
        public string parentId { get; set; }
        public DictionaryType[] types { get; set; }
        public bool selectable { get; set; }
     
    }
}