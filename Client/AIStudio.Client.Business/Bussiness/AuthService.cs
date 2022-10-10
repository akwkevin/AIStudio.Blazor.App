using AIStudio.Entity;
using AIStudio.Entity.DTO.Base_Manage;
using AIStudio.Util.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AIStudio.Client.Business
{
    public class AuthService : IAuthService
    {
        private const string TOKEN = "Token";

        private readonly Blazored.LocalStorage.ILocalStorageService _storage;
        private readonly IDataProvider _dataProvider;
        private readonly IOperator _operator;

        public AuthService(Blazored.LocalStorage.ILocalStorageService storage, IDataProvider dataProvider, IOperator @operator)
        {
            _storage = storage;
            _dataProvider = dataProvider;
            _operator = @operator;
        }

        public async Task<IOperator> CurrentUserInfo()
        {
            try
            {
                var userinfo = await _dataProvider.GetData<UserInfoPermissions>("/Base_Manage/Home/GetOperatorInfo");
                if (userinfo.Success)
                {
                    _operator.IsAuthenticated = true;
                    _operator.Property = userinfo.Data.UserInfo;
                    _operator.Permissions = userinfo.Data.Permissions;
                    _operator.RoleNameList = userinfo.Data.UserInfo.RoleNameList;
                    _operator.RoleIdList = userinfo.Data.UserInfo.RoleIdList;

                    _operator.MenuTrees = await GetMenus();// 初始化接收菜单
                    _operator.Menus = TreeHelper.GetTreeToList(_operator.MenuTrees);
                    //把介绍当主页
                    var main = _operator.Menus.FirstOrDefault(p => p.Url == "/Home/Introduce");
                    if (main != null)
                    {
                        main.Url = "/";
                    }
                }
                else
                {
                    _operator.IsAuthenticated = false;
                }
            }
            catch (Exception ex)
            {
                _operator.IsAuthenticated = false;
                System.Diagnostics.Debug.WriteLine(ex.Message);
            }

            return _operator;
        }

        public async Task<List<Base_ActionTree>> GetMenus()
        {
            List<Base_ActionTree> menus = new List<Base_ActionTree>();
            var result = await _dataProvider.GetData<List<Base_ActionTree>>("/Base_Manage/Home/GetOperatorMenuList");
            if (result.Success)
            {
                menus = result.Data;
            }

#if DEBUG
            Base_ActionTree code = new Base_ActionTree() { Icon = "code", Text = "开发", Url = "Demo", Type = ActionType.菜单 };
            code.Children = new List<Base_ActionTree>();
            code.Children.Add(new Base_ActionTree() { Text = "数据库连接", Url = "/Base_Manage/Base_DbLink/List", Type = ActionType.页面 });
            code.Children.Add(new Base_ActionTree() { Text = "代码生成", Url = "/Base_Manage/BuildCode/List", Type = ActionType.页面 });
            code.Children.Add(new Base_ActionTree() { Text = "Swagger", Url = "/Develop/Swagger/List", Type = ActionType.页面 });
            code.Children.Add(new Base_ActionTree() { Text = "文件上传", Url = "/Develop/FileUpload/List", Type = ActionType.页面 });
            code.Children.Add(new Base_ActionTree() { Text = "图片上传", Url = "/Develop/ImageUpload/List", Type = ActionType.页面 });
            menus.Add(code);
#endif

            return menus;
        }

        public async Task<AjaxResult> Login(string userName, string password)
        {
            var result = await _dataProvider.GetToken(userName, password);
            return result;
        }

        public async Task<AjaxResult> Logout()
        {
            var result = await _dataProvider.ClearToken();
            return result;
        }
    }
}
