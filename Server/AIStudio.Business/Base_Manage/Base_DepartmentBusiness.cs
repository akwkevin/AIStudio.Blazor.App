using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using Simple.Common;
using SqlSugar;

namespace AIStudio.Business.Base_Manage
{
    public class Base_DepartmentBusiness : BaseBusiness<Base_Department>, IBase_DepartmentBusiness, ITransientDependency
    {

        public Base_DepartmentBusiness(ISqlSugarClient db) : base(db)
        {

        }

        #region 外部接口

        public async Task<List<Base_DepartmentTreeDTO>> GetTreeDataListAsync(DepartmentsTreeInputDTO input)
        {
            var P = await Db.Queryable<Base_Department>().WhereIF(!input.parentId.IsNullOrEmpty(), x => x.ParentId == input.parentId).ToListAsync();
            var treeList = P
                .Select(x => new Base_DepartmentTreeDTO
                {
                    Id = x.Id,
                    ParentId = x.ParentId,
                    ParentIds = x.ParentIds,
                    Text = x.Name,
                    Value = x.Id
                }).ToList();

            return TreeHelper.BuildTree(treeList);
        }

        public async Task<List<string>> GetChildrenIdsAsync(string departmentId)
        {
            var allNode = await Db.Queryable<Base_Department>().Select(x => new TreeModel
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

        public async Task<Base_Department> GetTheDataAsync(string id)
        {
            return await Db.Queryable<Base_Department>().FirstAsync(x => x.Id.Equals(id));
        }
        public async Task AddDataAsync(Base_Department newData)
        {
            await Db.Insertable(newData).ExecuteCommandAsync();
        }
        public async Task UpdateDataAsync(Base_Department theData)
        {
            await Db.Updateable<Base_Department>().SetColumns(x => new Base_Department { Name = theData.Name, ParentId = theData.ParentId }).Where(x => x.Id.Equals(theData.Id)).ExecuteCommandAsync();
        }
        public async Task DeleteDataAsync(List<string> ids)
        {
            if (await Db.Queryable<Base_Department>().AnyAsync(x => ids.Contains(x.ParentId)))
                throw AjaxResultException.Status403Forbidden("禁止删除！请先删除所有子级！");

            await Db.Deleteable<Base_Department>().Where(x => ids.Contains(x.Id)).ExecuteCommandAsync();
        }

        #endregion
    }
}