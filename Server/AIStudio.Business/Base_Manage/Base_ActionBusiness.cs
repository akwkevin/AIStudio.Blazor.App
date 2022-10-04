using AIStudio.Common.DI;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AutoMapper;
using SqlSugar;
using Yitter.IdGenerator;

namespace AIStudio.Business.Base_Manage
{
    public class Base_ActionBusiness : BaseBusiness<Base_Action>, IBase_ActionBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public Base_ActionBusiness(IMapper mapper, ISqlSugarClient db) : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<List<Base_Action>> GetDataListAsync(Base_ActionsInputDTO input)
        {
            var q = await Db.Queryable<Base_Action>()
               .WhereIF(!input.parentId.IsNullOrEmpty(), x => x.ParentId == input.parentId)
                .WhereIF(input.types?.Length > 0, x => input.types.Contains(x.Type))
                 .WhereIF(input.ActionIds?.Length > 0, x => input.ActionIds.Contains(x.Id))
                 .OrderBy(x => x.CreateTime).ToListAsync();
            return q;
        }

        public async Task<List<Base_ActionTree>> GetTreeDataListAsync(Base_ActionsInputDTO input)
        {
            var qList = await GetDataListAsync(input);
            var treeList = qList.Select(x => new Base_ActionTree
            {
                Id = x.Id,
                NeedAction = x.NeedAction,
                Text = x.Name,
                ParentId = x.ParentId,
                Type = x.Type,
                Url = x.Url,
                Value = x.Id,
                Icon = x.Icon,
                //selectable = input.selectable
            }).ToList();

            //菜单节点中,若子节点为空则移除父节点
            if (input.checkEmptyChildren)
                treeList = treeList.Where(x => x.Type != 0 || TreeHelper.GetChildren(treeList, x, false).Count > 0).ToList();

            await SetProperty(treeList);

            return TreeHelper.BuildTree2(treeList);

            async Task SetProperty(List<Base_ActionTree> _list)
            {
                var ids = _list.Select(x => x.Id).ToList();
                var allPermissions = await Db.Queryable<Base_Action>()
                    .Where(x => ids.Contains(x.ParentId) && (int)x.Type == 2)
                    .ToListAsync();

                _list.ForEach(aData =>
                {
                    var permissionValues = allPermissions
                        .Where(x => x.ParentId == aData.Id)
                        .Select(x => $"{x.Name}({x.Value})")
                        .ToList();

                    aData.PermissionValues = permissionValues;
                });
            }
        }


        public async Task AddDataAsync(ActionEditInputDTO input)
        {
            await Db.Insertable(_mapper.Map<Base_Action>(input)).ExecuteCommandAsync();
            await SavePermissionAsync(input.Id, input.permissionList);
        }


        public async Task UpdateDataAsync(ActionEditInputDTO input)
        {
            await Db.Updateable(_mapper.Map<Base_Action>(input)).ExecuteCommandAsync();
            await SavePermissionAsync(input.Id, input.permissionList);
        }    

        public async Task SavePermissionAsync(string parentId, List<Base_Action> permissionList)
        {
            permissionList.ForEach(aData =>
            {
                aData.Id = YitIdHelper.NextId().ToString();
                aData.CreateTime = DateTime.Now;
                aData.CreatorId = null;
                aData.ParentId = parentId;
                aData.NeedAction = true;
            });
            //删除原来
            await Db.Deleteable<Base_Action>().Where(x => x.ParentId == parentId && (int)x.Type == 2).ExecuteCommandAsync();
            //新增
            await Db.Insertable(permissionList).ExecuteCommandAsync();

            //权限值必须唯一
            var repeatValues = await Db.Queryable<Base_Action>()
                .GroupBy(x => x.Value)
                .Having(x => SqlFunc.AggregateCount(x.Value) > 1)
                .Select(x => x.Value)
                .ToListAsync();
            if (repeatValues.Count > 0)
                throw new Exception($"以下权限值重复:{string.Join(",", repeatValues)}");
        }

        #endregion

        #region 私有成员

        #endregion
    }

}