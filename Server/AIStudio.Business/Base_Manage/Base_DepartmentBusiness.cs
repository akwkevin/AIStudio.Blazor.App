using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Simple.Common;
using SqlSugar;
using System.Linq;
using System.Linq.Dynamic.Core;

namespace AIStudio.Business.Base_Manage
{
    public class Base_DepartmentBusiness : BaseBusiness<Base_Department>, IBase_DepartmentBusiness, ITransientDependency
    {

        public Base_DepartmentBusiness(ISqlSugarClient db) : base(db)
        {

        }

        #region 外部接口

        public async Task<List<Base_DepartmentTree>> GetTreeDataListAsync(SearchInput input)
        {
            var data = await GetIQueryable().ToListAsync();
            var treeList = data
                .Select(x => new Base_DepartmentTree
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    Name = x.Name,
                    Text = x.Name,
                    Value = x.Id
                }).ToList();

            var tree = TreeHelper.BuildGenericsTree(treeList);
            
            //按字典筛选
            if (input.SearchKeyValues?.Count > 0)
            {
                IEnumerable<Base_DepartmentTree> treeList2 = TreeHelper.GetTreeToList(tree);
                foreach (var keyValuePair in input.SearchKeyValues.Where(p => !string.IsNullOrEmpty(p.Key) && !string.IsNullOrEmpty(p.Value?.ToString())))
                {
                    var newWhere = DynamicExpressionParser.ParseLambda<Base_DepartmentTree, bool>(
                        ParsingConfig.Default, false, $@"{keyValuePair.Key}.Contains(@0)", keyValuePair.Value);
                    treeList2 = treeList2.Where(newWhere.Compile());
                }

                tree = treeList2.ToList();
            }

            return tree;
        }

        public async Task<List<string>> GetChildrenIdsAsync(string departmentId)
        {
            var allNode = await GetIQueryable().Select(x => new TreeModel
            {
                Id = x.Id,
                ParentId = x.ParentId,
                Text = x.Name,
                Value = x.Id
            }).ToListAsync();

            var children = TreeHelper
                .GetChildren(allNode, allNode.Where(x => x.Id == departmentId).FirstOrDefault(), true)
                .Select(x => x.Id)
                .ToList();

            return children;
        }

        public override async Task<Base_Department> GetTheDataAsync(string id)
        {
            return await GetIQueryable().FirstAsync(x => x.Id.Equals(id));
        }

        [DataRepeatValidate(new string[] { "Name" }, new string[] { "部门名" })]
        public override async Task AddDataAsync(Base_Department newData)
        {
            await base.AddDataAsync(newData);
        }

        [DataRepeatValidate(new string[] { "Name" }, new string[] { "部门名" })]
        public override async Task UpdateDataAsync(Base_Department theData)
        {
            await base.UpdateDataAsync(theData);
        }

        [DataRepeatValidate(new string[] { "Name" }, new string[] { "部门名" })]
        public override async Task SaveDataAsync(Base_Department theData)
        {
            await base.SaveDataAsync(theData);
        }

        public override async Task DeleteDataAsync(List<string> ids)
        {
            if (await GetIQueryable().AnyAsync(x => ids.Contains(x.ParentId)))
                throw AjaxResultException.Status403Forbidden("禁止删除！请先删除所有子级！");

            await DeleteAsync(ids);
        }

        #endregion
    }
}