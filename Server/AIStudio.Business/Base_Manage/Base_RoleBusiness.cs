using AIStudio.Common.DI;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AutoMapper;
using SqlSugar;
using Yitter.IdGenerator;

namespace AIStudio.Business.Base_Manage
{
    public class Base_RoleBusiness : BaseBusiness<Base_Role>, IBase_RoleBusiness, ITransientDependency
    {
        readonly IMapper _mapper;
        public Base_RoleBusiness(IMapper mapper, ISqlSugarClient db) :base(db)
        {
            _mapper = mapper;
        }

        protected override string _textField => "RoleName";

        #region 外部接口

        public async Task<PageResult<Base_RoleEditInputDTO>> GetDataListAsync(PageInput<Base_RoleInputDTO> input)
        {
            var search = input.Search;
            RefAsync<int> total = 0;
            var P = await Db.Queryable<Base_Role>()
                .WhereIF(!search.roleId.IsNullOrEmpty(), x => x.Id == search.roleId)
                .WhereIF(!search.roleName.IsNullOrEmpty(), x => x.RoleName.Contains(search.roleName))
                .Select<Base_RoleEditInputDTO>()
                .ToPageListAsync(input.PageIndex, input.PageRows, total);
            var page = new PageResult<Base_RoleEditInputDTO> { Data = P, Total = total };
            await SetProperty(page.Data);
            return page;

            async Task SetProperty(List<Base_RoleEditInputDTO> _list)
            {
                var allActionIds = await Db.Queryable<Base_Action>().Select(x => x.Id).ToListAsync();

                var ids = _list.Select(x => x.Id).ToList();
                var roleActions = await Db.Queryable<Base_RoleAction>()
                    .Where(x => ids.Contains(x.RoleId))
                    .ToListAsync();
                _list.ForEach(aData =>
                {
                    if (aData.RoleName == RoleTypes.超级管理员.ToString())
                        aData.Actions = allActionIds;
                    else
                        aData.Actions = roleActions.Where(x => x.RoleId == aData.Id).Select(x => x.ActionId).ToList();
                });
            }
        }

        public async Task<Base_RoleEditInputDTO> GetTheDataDTOAsync(string id)
        {
            return (await GetDataListAsync(new PageInput<Base_RoleInputDTO> { Search = new Base_RoleInputDTO { roleId = id } })).Data.FirstOrDefault();
        }
        public async Task AddDataAsync(Base_RoleEditInputDTO input)
        {

            await Db.Insertable(_mapper.Map<Base_Role>(input)).ExecuteCommandAsync();
            await SetRoleActionAsync(input.Id, input.Actions);
        }

        public async Task UpdateDataAsync(Base_RoleEditInputDTO input)
        {
            await Db.Updateable<Base_Role>().SetColumns(x => new Base_Role { RoleName = input.RoleName }).Where(x => x.Id.Equals(input.Id)).ExecuteCommandAsync();
            await SetRoleActionAsync(input.Id, input.Actions);
        }


        public override async Task DeleteDataAsync(List<string> ids)
        {
            await Db.Deleteable<Base_Role>().Where(x => ids.Contains(x.Id)).ExecuteCommandAsync();
            await Db.Deleteable<Base_RoleAction>().Where(x => ids.Contains(x.Id)).ExecuteCommandAsync();
        }

        #endregion

        #region 私有成员

        private async Task SetRoleActionAsync(string roleId, List<string> actions)
        {
            var roleActions = (actions ?? new List<string>())
                .Select(x => new Base_RoleAction
                {
                    Id = YitIdHelper.NextId().ToString(),
                    ActionId = x,
                    CreateTime = DateTime.Now,
                    RoleId = roleId
                }).ToList();
            await Db.Deleteable<Base_RoleAction>().Where(x => x.RoleId == roleId).ExecuteCommandAsync();
            await Db.Insertable(roleActions).ExecuteCommandAsync();
        }

        #endregion

        #region 数据模型

        #endregion
    }
}