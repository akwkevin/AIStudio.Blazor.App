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
        public bool checkEmptyChildren { get; set; }
    }

    public class Base_DictionaryDTO : TreeModel
    {
        public DictionaryType Type { get; set; }
        public ControlType ControlType { get; set; }
        public string TypeText { get => ((DictionaryType)Type).ToString(); }
        public object children { get => Children; }
        public string Code { get; set; }
        public string Remark { get; set; }
        //public string title { get => Text; }
        //public string value { get => Id; }
        //public string key { get => Id; }
        public bool selectable { get; set; }
        public int Sort { get; set; }
    }
}