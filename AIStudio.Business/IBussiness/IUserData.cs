using AIStudio.Util.Common;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIStudio.Business
{
    public interface IUserData
    {
        public string[] ReadOnlySource { get; }
        public string[] IgnoreSource { get; }
        Task<List<SelectOption>> GetBase_User();
        void ClearBase_User();
        Task<List<SelectOption>> GetBase_Role();
        void ClearBase_Role();
        Task<List<TreeModel>> GetBase_DepartmentTree();
        Task<List<SelectOption>> GetBase_Department();
        void ClearBase_Department();
        Task<List<TreeModel>> GetBase_ActionTree();
        Task<List<SelectOption>> GetBase_Action();
        void ClearBase_Action();
        Task<List<DictionaryTreeModel>> GetBase_Dictionary();
        void ClearBase_Dictionary();
        Dictionary<string, List<SelectOption>> ItemSource { get; }
        Dictionary<string, DictionaryTreeModel> Base_Dictionary { get;  }
        Task Init();
    }
}
