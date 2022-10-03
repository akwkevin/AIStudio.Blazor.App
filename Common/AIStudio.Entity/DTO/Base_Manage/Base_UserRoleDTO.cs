using AIStudio.Common.Mapper;
using AIStudio.Entity.DTO.Base_Manage;
using Org.BouncyCastle.Crypto;
using SqlSugar;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AIStudio.Entity.Base_Manage
{
    [Map(typeof(Base_UserRole))]
    public class Base_UserRoleDTO : Base_UserRole
    {
  
        public Base_User User { get; set; }
  
        public Base_Role Role { get; set; }

    }
}