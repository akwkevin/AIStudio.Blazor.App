using AIStudio.Common.CurrentUser;
using AIStudio.Common.DI;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Entity.DTO.Base_Manage.InputDTO;
using AIStudio.IBusiness.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;
using AutoMapper;
using Simple.Common;
using SqlSugar;
using System.Linq.Dynamic.Core;
using Yitter.IdGenerator;

namespace AIStudio.Business.Base_Manage
{
    public class Base_UserBusiness : BaseBusiness<Base_User>, IBase_UserBusiness, ITransientDependency
    {
        readonly IOperator _operator;
        readonly IMapper _mapper;

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
            var q = GetIQueryable(input.SearchKeyValues);//按字典筛选

            var data = await q.LeftJoin<Base_Department>((x, d) => x.DepartmentId == d.Id).Select(x => new Base_UserDTO
                {
                    UserName = x.UserName,
                    Id = x.Id,
                    DepartmentId = x.DepartmentId,
                    Birthday = x.Birthday,
                    Avatar = x.Avatar,
                    CreateTime = x.CreateTime,
                    CreatorId = x.CreatorId,
                    CreatorName = x.CreatorName,
                    ModifyId = x.ModifyId,
                    ModifyName = x.ModifyName,
                    ModifyTime = x.ModifyTime,
                    Password = x.Password,
                    PhoneNumber = x.PhoneNumber,
                    RealName = x.RealName,
                    Sex = x.Sex,
                    TenantId = x.TenantId
                }).ToPageListAsync(input.PageIndex, input.PageRows, total);
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
                RefAsync<int> total = 0;
                var data = await GetIQueryable().Where(x => x.Id.Equals(id)).Select<Base_UserDTO>().ToListAsync();
                return new PageResult<Base_UserDTO> { Data = data, Total = total }.Data.FirstOrDefault();
            }
        }

        [DataRepeatValidate( new string[] { "UserName" }, new string[] { "用户名" })]
        public async Task AddDataAsync(Base_UserEditInputDTO input)
        {
            await Db.Insertable<Base_User>(_mapper.Map<Base_User>(input)).ExecuteCommandAsync();
            await SetUserRoleAsync(input.Id, input.RoleIdList);
        }

        [DataRepeatValidate( new string[] { "UserName" }, new string[] { "用户名" })]
        public async Task UpdateDataAsync(Base_UserEditInputDTO input)
        {
            if (input.Id == AdminTypes.Admin.ToString() && _operator?.UserId != input.Id)
                throw AjaxResultException.Status403Forbidden("禁止更改超级管理员！");

            await Db.Updateable<Base_User>(_mapper.Map<Base_User>(input)).ExecuteCommandAsync();
            await SetUserRoleAsync(input.Id, input.RoleIdList);
        }

        public override async Task DeleteDataAsync(List<string> ids)
        {
            if (ids.Contains(AdminTypes.Admin.ToString()))
                throw AjaxResultException.Status403Forbidden("超级管理员是内置账号,禁止删除！");

            await base.DeleteDataAsync(ids);
        }

        #endregion

        #region 私有成员

        public async Task SetUserRoleAsync(string userId, List<string> roleIds)
        {
            roleIds = roleIds ?? new List<string>();
            var userRoleList = roleIds.Select(x => new Base_UserRole
            {
                Id = YitIdHelper.NextId().ToString(),
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