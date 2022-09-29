using AIStudio.Entity;
using AIStudio.Entity.DTO.Base_Manage;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace AIStudio.Client.Business
{
    /// <summary>
    /// 操作者
    /// </summary>
    public class Operator : IOperator
    {
        public bool IsAuthenticated { get; set; }
        public Dictionary<string, string> Claims { get; set; }
        public List<string> Roles { get; set; }
        public bool IsExpired { get; set; }

        public string UserId { get { return Property?.Id; } }
        /// <summary>
        /// 当前操作者UserName
        /// </summary>
        public string UserName { get { return Property?.UserName; } }

        public string Avatar { get {return Property?.Avatar ?? "_content/AIStudio.Blazor.UI/images/Luffy.jpg"; } }

        public Base_UserDTO Property { get; set; }

        public List<string> Permissions { get; set; }

        //菜单树
        public List<Base_ActionTree> MenuTrees { get; set; }

        //打平用于查询的菜单
        public List<Base_ActionTree> Menus { get; set; }

        public bool HasPerm(string permiss)
        {
            var menu = Menus.FirstOrDefault(p => p.PageUrl != null && permiss.StartsWith(p.PageUrl));
            if (menu != null && menu.NeedAction == false)
            {
                return true;
            }

            return Permissions?.Contains(permiss) == true;
        }
    }
}
