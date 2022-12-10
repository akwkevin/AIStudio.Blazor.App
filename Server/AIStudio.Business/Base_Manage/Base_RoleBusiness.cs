using AIStudio.Business.AOP;
using AIStudio.Common.DI;
using AIStudio.Common.IdGenerator;
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
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="AIStudio.Business.BaseBusiness&lt;AIStudio.Entity.Base_Manage.Base_Role&gt;" />
    /// <seealso cref="AIStudio.IBusiness.Base_Manage.IBase_RoleBusiness" />
    /// <seealso cref="AIStudio.Common.DI.ITransientDependency" />
    public class Base_RoleBusiness : BaseBusiness<Base_Role>, IBase_RoleBusiness, ITransientDependency
    {
        /// <summary>
        /// The mapper
        /// </summary>
        private readonly IMapper _mapper;
        /// <summary>
        /// Initializes a new instance of the <see cref="Base_RoleBusiness"/> class.
        /// </summary>
        /// <param name="mapper">The mapper.</param>
        /// <param name="db">The database.</param>
        public Base_RoleBusiness(IMapper mapper, ISqlSugarClient db) : base(db)
        {
            _mapper = mapper;
        }

        /// <summary>
        /// Gets the text field.
        /// </summary>
        /// <value>
        /// The text field.
        /// </value>
        protected override string _textField => "RoleName";

        #region 外部接口

        /// <summary>
        /// Gets the data list asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        /// <returns></returns>
        public async Task<PageResult<Base_RoleEditInputDTO>> GetDataListAsync(PageInput input)
        {
            RefAsync<int> total = 0;
            var data = await GetIQueryable(input.SearchKeyValues)
                .Select<Base_RoleEditInputDTO>()
                .ToPageListAsync(input.PageIndex, input.PageRows, total);
            var page = new PageResult<Base_RoleEditInputDTO> { Data = data, Total = total };
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

        /// <summary>
        /// Gets the data asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public new async Task<Base_RoleEditInputDTO> GetTheDataAsync(string id)
        {
            return (await GetDataListAsync(new PageInput { SearchKeyValues = new Dictionary<string, object> { { "Id", id } } })).Data?.FirstOrDefault();
        }

        /// <summary>
        /// Adds the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        [DataRepeatValidate(new string[] { "RoleName" }, new string[] { "角色名" })]
        [Transactional]
        public async Task AddDataAsync(Base_RoleEditInputDTO input)
        {
            await InsertAsync(_mapper.Map<Base_Role>(input));
            await SetRoleActionAsync(input.Id, input.Actions);
        }

        /// <summary>
        /// Updates the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        [DataRepeatValidate(new string[] { "RoleName" }, new string[] { "角色名" })]
        [Transactional]
        public async Task UpdateDataAsync(Base_RoleEditInputDTO input)
        {
            await UpdateAsync(_mapper.Map<Base_Role>(input));
            await SetRoleActionAsync(input.Id, input.Actions);
        }

        /// <summary>
        /// Saves the data asynchronous.
        /// </summary>
        /// <param name="input">The input.</param>
        [DataRepeatValidate(new string[] { "RoleName" }, new string[] { "角色名" })]
        [Transactional]
        public async Task SaveDataAsync(Base_RoleEditInputDTO input)
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

        /// <summary>
        /// Deletes the data asynchronous.
        /// </summary>
        /// <param name="ids">The ids.</param>
        [Transactional]
        public override async Task DeleteDataAsync(List<string> ids)
        {
            await DeleteAsync(ids);
            await Db.Deleteable<Base_RoleAction>().Where(x => ids.Contains(x.RoleId)).ExecuteCommandAsync();
        }

        #endregion

        #region 私有成员

        /// <summary>
        /// Sets the role action asynchronous.
        /// </summary>
        /// <param name="roleId">The role identifier.</param>
        /// <param name="actions">The actions.</param>
        private async Task SetRoleActionAsync(string roleId, List<string> actions)
        {
            var roleActions = (actions ?? new List<string>())
                .Select(x => new Base_RoleAction
                {
                    Id = IdHelper.GetId(),
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