using AIStudio.Util.Common;
using AIStudio.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;

namespace AIStudio.Business
{
    public class UserData : IUserData
    {
        IDataProvider _dataProvider { get; }
        public UserData(IDataProvider dataProvider)
        {
            _dataProvider = dataProvider;

        }

        public string[] ReadOnlySource { get; } = new string[] { "Id", "CreateTime", "ModifyTime", "CreatorName", "ModifyName" };
        public string[] IgnoreSource { get; } = new string[] { "IsChecked", "Deleted", "CreatorId", "ModifyId", };

        private List<SelectOption> base_user { get; set; } = new List<SelectOption>();

        private List<SelectOption> base_role { get; set; } = new List<SelectOption>();

        private List<SelectOption> base_department { get; set; } = new List<SelectOption>();

        private List<TreeModel> base_departmenttree { get; set; } = new List<TreeModel>();

        private List<SelectOption> base_action { get; set; } = new List<SelectOption>();

        private List<TreeModel> base_actiontree { get; set; } = new List<TreeModel>();

        private List<DictionaryTreeModel> base_dictionary { get; set; } = new List<DictionaryTreeModel>();

        public async Task<List<SelectOption>> GetBase_User()
        {
            if (base_user.Count == 0)
            {
                var result = await _dataProvider.GetData<List<SelectOption>>("/Base_Manage/Base_User/GetOptionList");
                if (!result.Success)
                {
                    throw new Exception(result.Msg);
                }
                else
                {
                    base_user.AddRange(result.Data);
                }
            }

            return base_user;
        }

        public void ClearBase_User()
        {
            base_user.Clear();
        }

        public async Task<List<SelectOption>> GetBase_Role()
        {
            if (base_role.Count == 0)
            {
                var result = await _dataProvider.GetData<List<SelectOption>>("/Base_Manage/Base_Role/GetOptionList");
                if (!result.Success)
                {
                    throw new Exception(result.Msg);
                }
                else
                {
                    base_role.AddRange(result.Data);
                }
            }

            return base_role;
        }

        public void ClearBase_Role()
        {
            base_role.Clear();
        }

        public async Task<List<TreeModel>> GetBase_DepartmentTree()
        {
            if (base_departmenttree.Count == 0)
            {
                var result = await _dataProvider.GetData<List<TreeModel>>("/Base_Manage/Base_Department/GetTreeDataList");
                if (!result.Success)
                {
                    throw new Exception(result.Msg);
                }
                else
                {
                    base_departmenttree.AddRange(result.Data);
                }
            }

            return base_departmenttree;
        }

        public async Task<List<SelectOption>> GetBase_Department()
        {
            if (base_department.Count == 0)
            {
                var tree = await GetBase_DepartmentTree();
                base_department.AddRange(TreeHelper.GetTreeToList(tree.Select(p => p as TreeModel)).OfType<SelectOption>());
            }

            return base_department;
        }

        public void ClearBase_Department()
        {
            base_departmenttree.Clear();
            base_department.Clear();
        }

        public async Task<List<TreeModel>> GetBase_ActionTree()
        {
            if (base_actiontree.Count == 0)
            {
                var result = await _dataProvider.GetData<List<TreeModel>>("/Base_Manage/Base_Action/GetActionTreeList");
                if (!result.Success)
                {
                    throw new Exception(result.Msg);
                }
                else
                {
                    base_actiontree.AddRange(result.Data);
                }
            }

            return base_actiontree;
        }

        public async Task<List<SelectOption>> GetBase_Action()
        {
            if (base_action.Count == 0)
            {
                var tree = await GetBase_ActionTree();
                base_action.AddRange(TreeHelper.GetTreeToList(tree.Select(p => p as TreeModel)).OfType<SelectOption>());
            }

            return base_action;
        }

        public void ClearBase_Action()
        {
            base_actiontree.Clear();
            base_action.Clear();
        }

        public async Task<List<DictionaryTreeModel>> GetBase_Dictionary()
        {
            if (base_dictionary.Count == 0)
            {
                var result = await _dataProvider.GetData<List<DictionaryTreeModel>>("/Base_Manage/Base_Dictionary/GetTreeDataList");
                if (!result.Success)
                {
                    throw new Exception(result.Msg);
                }
                else
                {
                    base_dictionary.AddRange(result.Data);
                }
            }

            return base_dictionary;
        }

        public void ClearBase_Dictionary()
        {
            base_dictionary.Clear();
        }

        public Dictionary<string, List<SelectOption>> ItemSource { get; private set; } = new Dictionary<string, List<SelectOption>>();
        public Dictionary<string, DictionaryTreeModel> Base_Dictionary { get; private set; } = new Dictionary<string, DictionaryTreeModel>();

        public async Task Init()
        {
            ClearBase_User(); 
            ClearBase_Role();
            ClearBase_Department();
            ClearBase_Dictionary();
            ClearBase_Action();

            ItemSource.Clear();
            Base_Dictionary.Clear();
            var user = await GetBase_User();
            ItemSource.Add("Base_User", user);
            var role = await GetBase_Role();
            ItemSource.Add("Base_Role", role);
            var department = await GetBase_Department();    
            ItemSource.Add("Base_Department", department);
            var departmenttree = await GetBase_DepartmentTree();
            ItemSource.Add("Base_DepartmentTree", departmenttree.OfType<SelectOption>().ToList());
            var action = await GetBase_Action();
            ItemSource.Add("Base_Action", action);
            var actiontree = await GetBase_ActionTree();
            ItemSource.Add("Base_ActionTree", actiontree.OfType<SelectOption>().ToList());
            var datas = await GetBase_Dictionary();
            BuildDictionary(ItemSource, Base_Dictionary, datas);
        }

        public static void BuildDictionary(Dictionary<string, List<SelectOption>> items, Dictionary<string, DictionaryTreeModel> dics, IEnumerable<DictionaryTreeModel> trees)
        {
            foreach (var tree in trees)
            {
                if (tree.Type == 0)
                {
                    dics.Add(tree.Value, tree);

                    if (tree.Children != null)
                    {
                        var datas = tree.Children.Where(p => p.Type == 1);
                        if (datas.Count() > 0)
                        {
                            items.Add(tree.Value, new List<SelectOption>(datas.Select(p => new SelectOption() { Value = p.Value, Text = p.Text })));
                        }

                        var subtrees = tree.Children.Where(p => p.Type == 0);
                        if (subtrees.Count() > 0)
                        {
                            BuildDictionary(items, dics, subtrees);
                        }
                    }
                }
            }
        }
    }
}