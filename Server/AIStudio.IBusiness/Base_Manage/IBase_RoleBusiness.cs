﻿using AIStudio.Common.Mapper;
using AIStudio.Entity;
using AIStudio.Entity.Base_Manage;
using AIStudio.Util;
using AIStudio.Util.Common;

namespace AIStudio.IBusiness.Base_Manage
{
    public interface IBase_RoleBusiness : IBaseBusiness<Base_Role>
    {
        Task<PageResult<Base_RoleInfoDTO>> GetDataListAsync(PageInput<RolesInputDTO> input);
        Task<Base_RoleInfoDTO> GetTheDataDTOAsync(string id);
        Task AddDataAsync(Base_RoleInfoDTO input);
        Task UpdateDataAsync(Base_RoleInfoDTO input);
    }

    public class RolesInputDTO
    {
        public string roleId { get; set; }
        public string roleName { get; set; }
    }

    [Map(typeof(Base_Role))]
    public class Base_RoleInfoDTO : Base_Role
    {
        public RoleTypes? RoleType { get { try { return RoleName?.ToEnum<RoleTypes>(); } catch { return null; } } }
        public List<string> Actions { get; set; } = new List<string>();
    }
}