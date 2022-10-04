using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util.Common;
using SqlSugar;
using System.Linq.Dynamic.Core;

namespace AIStudio.Business.Base_Manage
{
    public class Base_DictionaryBusiness : BaseBusiness<Base_Dictionary>, IBase_DictionaryBusiness, ITransientDependency
    {
        public Base_DictionaryBusiness(ISqlSugarClient db)
            : base(db)
        {
        }

        #region 外部接口

        public async Task<List<Base_Dictionary>> GetDataListAsync(Base_DictionaryInputDTO input)
        {
            var q = GetIQueryable();
            //q = q
            //    .WhereIf(!input.parentId.IsNullOrEmpty(), x => x.ParentId == input.parentId)
            //    .WhereIf(input.types?.Length > 0, x => input.types.Contains(x.Type))
            //    .WhereIf(input.ActionIds?.Length > 0, x => input.ActionIds.Contains(x.Id))
            //    ;

            return await q.OrderBy(x => x.Sort).ToListAsync();
        }

        public async Task<List<Base_DictionaryTree>> GetTreeDataListAsync(Base_DictionaryInputDTO input)
        {
            var qList = await GetDataListAsync(input);

            var treeList = qList.Select(x => new Base_DictionaryTree
            {
                Id = x.Id,
                Code = x.Code,
                ParentId = x.ParentId,
                //Type = x.Type,
                ControlType = x.ControlType,
                Text = x.Text,
                Value = x.Value,  
                Sort = x.Sort,
                Remark = x.Remark,
                //selectable = input.selectable
            }).ToList();

            //菜单节点中,若子节点为空则移除父节点
            if (input.checkEmptyChildren)
                treeList = treeList.Where(x => x.Type != 0 || TreeHelper.GetChildren(treeList, x, false).Count > 0).ToList();

            return TreeHelper.BuildTree(treeList);
        }

        public override async Task AddDataAsync(Base_Dictionary data)
        {
            if (data.Type == Entity.DictionaryType.字典项)
            {
                //权限值必须唯一
                var repeatCount = GetIQueryable()
                    .Where(x => x.Type == Entity.DictionaryType.字典项 && x.Value == data.Value)
                    .Count();
                if (repeatCount > 0)
                    throw new Exception($"以下字典项值重复:{data.Value}");
            }
            await InsertAsync(data);
        }

        public async Task UpdateDataAsync(Base_Dictionary data)
        {
            if (data.Type == Entity.DictionaryType.字典项)
            {
                //权限值必须唯一
                var repeatCount = GetIQueryable()
                    .Where(x => x.Type == Entity.DictionaryType.字典项 && x.Value == data.Value && x.Id != data.Id)
                    .Count();
                if (repeatCount > 0)
                    throw new Exception($"以下字典项值重复:{data.Value}");
            }
            await UpdateAsync(data);
        }
        #endregion

        #region 私有成员

        #endregion
    }
}