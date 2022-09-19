using AIStudio.Entity.DTO;
using AIStudio.Entity.DTO.Base_Manage;
using System.Collections.Generic;

namespace AIStudio.Business
{
    public interface IOperator
    {
        bool IsAuthenticated { get; set; }
        Dictionary<string, string> Claims { get; set; }
        List<string> Roles { get; set; }
        bool IsExpired { get; set; }

        /// <summary>
        /// 当前操作者UserId
        /// </summary>
        string UserId { get; }
        string UserName { get; }
        string Avatar { get; }
        /// <summary>
        /// 当前操作者
        /// </summary>
        Base_UserDTO Property { get; set; }

        List<string> Permissions { get; set; }

        bool HasPerm(string permiss);

        //菜单树
        List<Base_ActionTree> MenuTrees { get; set; }

        //打平用于查询的菜单
        List<Base_ActionTree> Menus { get; set; }

        #region 操作方法
        public void Reset()
        {
            IsAuthenticated = false;
            Claims.Clear();
            Roles.Clear();
        }      
        #endregion
    }
}