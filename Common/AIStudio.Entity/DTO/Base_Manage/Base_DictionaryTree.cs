using AIStudio.Util.Common;

namespace AIStudio.Entity.DTO.Base_Manage
{
    public class Base_DictionaryTree : TreeModel<Base_DictionaryTree>
    {
        public DictionaryType Type { get; set; }
        public ControlType ControlType { get; set; }
        public int Sort { get; set; }

        public string TypeText { get => ((DictionaryType)Type).ToString(); }
        public string? Code { get; set; }
 
        public string? Remark { get; set; }
    }
}
