using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Common.IdGenerator;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
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
        private readonly IMapper _mapper;
        public Base_ActionBusiness(IMapper mapper, ISqlSugarClient db) : base(db)
        {
            _mapper = mapper;
        }

        #region 外部接口

        public async Task<List<Base_Action>> GetDataListAsync(Base_ActionsInputDTO input)
        {
            var q = await GetIQueryable(input.SearchKeyValues)
                 .WhereIF(!input.ParentId.IsNullOrEmpty(), x => x.ParentId == input.ParentId)
                 .WhereIF(input.Types?.Length > 0, x => input.Types.Contains(x.Type))
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
                Sort = x.Sort
            }).OrderBy(p => p.Sort).ToList();

            //菜单节点中,若子节点为空则移除父节点
            //treeList = treeList.Where(x => x.Type != 0 || TreeHelper.GetChildren(treeList, x, false).Count > 0).ToList();

            await SetProperty(treeList);

            return TreeHelper.BuildGenericsTree(treeList);

            async Task SetProperty(List<Base_ActionTree> _list)
            {
                var ids = _list.Select(x => x.Id).ToList();
                var allPermissions = await GetIQueryable()
                    .Where(x => ids.Contains(x.ParentId) && (int)x.Type == (int)ActionType.权限)
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

        public async Task<List<Base_Action>> GetAllActionListAsync()
        {
            return await GetDataListAsync(new Base_ActionsInputDTO
            {
                Types = new ActionType[] { ActionType.菜单, ActionType.页面, ActionType.权限 }
            });
        }

        public async Task<List<Base_ActionTree>> GetMenuTreeListAsync(Base_ActionsInputDTO input)
        {
            input.Types = new ActionType[] { ActionType.菜单, ActionType.页面 };

            return await GetTreeDataListAsync(input);
        }

        public async Task<List<Base_Action>> GetPermissionListAsync(Base_ActionsInputDTO input)
        {
            input.Types = new ActionType[] { Entity.ActionType.权限 };

            return await GetDataListAsync(input);
        }
        [Transactional]
        public async Task AddDataAsync(Base_ActionEditInputDTO input)
        {
            var action = _mapper.Map<Base_Action>(input);
            await InsertAsync(action);
            await SavePermissionAsync(action.Id, input.permissionList);
        }      

        [Transactional]
        public async Task UpdateDataAsync(Base_ActionEditInputDTO input)
        {
            var action = _mapper.Map<Base_Action>(input);
            await UpdateAsync(action);
            await SavePermissionAsync(action.Id, input.permissionList);
        }

        public async Task SaveDataAsync(Base_ActionEditInputDTO input)
        {
            if (input.Id.IsNullOrEmpty())
            {
                await AddDataAsync(input);
            }
            else
            {
                await UpdateDataAsync(input);
            }
        }

        [Transactional]
        public override async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
            await DeleteAsync(x => ids.Contains(x.ParentId));
        }

        public async Task SavePermissionAsync(string parentId, List<Base_Action> permissionList)
        {
            permissionList.ForEach(aData =>
            {
                aData.Id = IdHelper.GetId();
                aData.CreateTime = DateTime.Now;
                aData.CreatorId = null;
                aData.ParentId = parentId;
                aData.NeedAction = true;
            });
            //删除原来
            await DeleteAsync(x => x.ParentId == parentId && (int)x.Type == 2);
            //新增
            await InsertAsync(permissionList);

            //权限值必须唯一
            var repeatValues = await GetIQueryable()
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