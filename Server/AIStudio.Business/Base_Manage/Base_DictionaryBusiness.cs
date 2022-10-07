using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
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
        public async Task<List<Base_DictionaryTree>> GetTreeDataListAsync(SearchInput input)
        {
            input.SortField = "Sort";
            var qList = await GetDataListAsync(input);

            var treeList = qList.Select(x => new Base_DictionaryTree
            {
                Id = x.Id,
                Code = x.Code,
                ParentId = x.ParentId,
                Type = x.Type,
                ControlType = x.ControlType,
                Text = x.Text,
                Value = x.Value,  
                Sort = x.Sort,
                Remark = x.Remark,
            }).ToList();

            return TreeHelper.BuildGenericsTree(treeList);
        }

        [DataRepeatValidate(new string[] { "Type", "Text", "Value" }, new string[] { "类型", "文本", "值" }, false)]
        public override async Task AddDataAsync(Base_Dictionary data)
        {
            //if (data.Type == Entity.DictionaryType.字典项)
            //{
            //    //权限值必须唯一
            //    var repeatCount = GetIQueryable()
            //        .Where(x => x.Type == Entity.DictionaryType.字典项 && x.Value == data.Value)
            //        .Count();
            //    if (repeatCount > 0)
            //        throw new Exception($"以下字典项值重复:{data.Value}");
            //}
            await base.AddDataAsync(data);
        }

        [DataRepeatValidate(new string[] { "Type", "Text", "Value" }, new string[] { "类型", "文本", "值" }, false)]
        public override async Task UpdateDataAsync(Base_Dictionary data)
        {
            //if (data.Type == Entity.DictionaryType.字典项)
            //{
            //    //权限值必须唯一
            //    var repeatCount = GetIQueryable()
            //        .Where(x => x.Type == Entity.DictionaryType.字典项 && x.Value == data.Value && x.Id != data.Id)
            //        .Count();
            //    if (repeatCount > 0)
            //        throw new Exception($"以下字典项值重复:{data.Value}");
            //}
            await base.UpdateDataAsync(data);
        }
        #endregion

        #region 私有成员

        #endregion
    }
}