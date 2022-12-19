using AIStudio.Business.AOP;
using AIStudio.Common.CurrentUser;
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
using Newtonsoft.Json.Linq;
using Simple.Common;
using SqlSugar;
using System.Linq.Dynamic.Core;
using Yitter.IdGenerator;
using static Google.Protobuf.WireFormat;

namespace AIStudio.Business.Base_Manage
{
    public class Base_UserBusiness : BaseBusiness<Base_User>, IBase_UserBusiness, ITransientDependency
    {
        private readonly IOperator _operator;
        private readonly IMapper _mapper;

        public Base_UserBusiness(
            IOperator @operator,
            IMapper mapper,
            ISqlSugarClient db
            ) : base(db)
        {
            _operator = @operator;
            _mapper = mapper;
        }
        protected override string _textField => "UserName";

        #region 外部接口

        public new async Task<PageResult<Base_UserDTO>> GetDataListAsync(PageInput input)
        {
            RefAsync<int> total = 0;
            //var q = Db.Queryable<Base_User, Base_Department>((o, i) => new JoinQueryInfos( JoinType.Left, o.DepartmentId == i.Id)).Select<Base_UserDTO>();
            var q = GetIQueryable().LeftJoin<Base_Department>((o, i) => o.DepartmentId == i.Id).Select<Base_UserDTO>();

            //因为联表查找，where条件写在后面，避免列名 'Id' 不明确等错误
            var conModels = new List<IConditionalModel>();
            if (input.SearchKeyValues != null)
            {
                foreach (var keyValuePair in input.SearchKeyValues.Where(p => !string.IsNullOrEmpty(p.Key) && !string.IsNullOrEmpty(p.Value?.ToString())))
                {
                    conModels.Add(new ConditionalModel() { FieldName = keyValuePair.Key, ConditionalType = ConditionalType.Like, FieldValue = keyValuePair.Value.ObjToString() });
                }
            }
            q = q.Where(conModels);

            var data = await q.ToPageListAsync(input.PageIndex, input.PageRows, total);
            var list = new PageResult<Base_UserDTO> { Data = data, Total = total };
            await SetProperty(list.Data);
            return list;

            async Task SetProperty(List<Base_UserDTO> users)
            {
                //补充用户角色属性
                List<string> userIds = users.Select(x => x.Id).ToList();

                var userRoles = await Db.Queryable<Base_UserRole, Base_Role>((a, b) => new object[] {
               JoinType.Left,a.RoleId==b.Id})
                .Select((a, b) => new
                {
                    a.UserId,
                    RoleId = b.Id,
                    b.RoleName
                }).ToListAsync();
                users.ForEach(aUser =>
                {
                    var roleList = userRoles.Where(x => x.UserId == aUser.Id);
                    aUser.RoleIdList = roleList.Select(x => x.RoleId).ToList();
                    aUser.RoleNameList = roleList.Select(x => x.RoleName).ToList();
                });
            }
        }

        public new async Task<Base_UserDTO> GetTheDataAsync(string id)
        {
            if (id.IsNullOrEmpty())
                return null;
            else
            {
                PageInput input = new PageInput
                {
                    SearchKeyValues = new Dictionary<string, object>()
                    {
                        {"o.Id", id }
                    }
                    ,PageRows=1
                };
                return (await GetDataListAsync(input)).Data.FirstOrDefault();
            }
        }

        [DataRepeatValidate(new string[] { "UserName" }, new string[] { "用户名" })]
        [Transactional]
        public async Task AddDataAsync(Base_UserEditInputDTO input)
        {
            var user = _mapper.Map<Base_User>(input);
            await InsertAsync(user);
            await SetUserRoleAsync(user.Id, input.RoleIdList);
        }

        [DataRepeatValidate(new string[] { "UserName" }, new string[] { "用户名" })]
        [Transactional]
        public async Task UpdateDataAsync(Base_UserEditInputDTO input)
        {
            if (input.Id == AdminTypes.Admin.ToString() && _operator?.UserId != input.Id)
                throw AjaxResultException.Status403Forbidden("禁止更改超级管理员！");

            var user = _mapper.Map<Base_User>(input);
            await UpdateAsync(user);
            await SetUserRoleAsync(user.Id, input.RoleIdList);
        }

        [DataRepeatValidate(new string[] { "UserName" }, new string[] { "用户名" })]
        [Transactional]
        public async Task SaveDataAsync(Base_UserEditInputDTO input)
        {
            if (!input.newPwd.IsNullOrEmpty())
                input.Password = input.newPwd.ToMD5String();
            else if (!input.Password.IsNullOrEmpty() && !input.Password.IsMd5())
            {
                input.Password = input.Password.ToMD5String();
            }

            if (input.Id.IsNullOrEmpty())
            {
                await AddDataAsync(input);
            }
            else
            {
                await UpdateDataAsync(input);
            }
        }

        public override async Task DeleteDataAsync(List<string> ids)
        {
            if (ids.Contains(AdminTypes.Admin.ToString()))
                throw AjaxResultException.Status403Forbidden("超级管理员是内置账号,禁止删除！");

            await DeleteAsync(ids);
        }

        #endregion

        #region 私有成员

        public async Task SetUserRoleAsync(string userId, List<string> roleIds)
        {
            roleIds = roleIds ?? new List<string>();
            var userRoleList = roleIds.Select(x => new Base_UserRole
            {
                Id = IdHelper.GetId(),
                CreateTime = DateTime.Now,
                UserId = userId,
                RoleId = x
            }).ToList();

            await Db.Deleteable<Base_UserRole>().Where(x => x.UserId == userId).ExecuteCommandAsync();
            await Db.Insertable<Base_UserRole>(userRoleList).ExecuteCommandAsync();
        }

        #endregion

        public async Task<string> GetAvatar(string userId)
        {
            if (string.IsNullOrEmpty(userId))
                return null;

            var user = await base.GetTheDataAsync(userId);

            return user?.Avatar;
        }
    }
}