using AIStudio.Client.Business;
using AIStudio.Util.Common;
using Microsoft.AspNetCore.Components.Authorization;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace AIStudio.Blazor.UI.Services.Auth
{
    public class JwtAuthenticationStateProvider : AuthenticationStateProvider
    {
        private readonly IAuthService api;

        public JwtAuthenticationStateProvider(IAuthService api) { this.api = api; }

        public override async Task<AuthenticationState> GetAuthenticationStateAsync()
        {
            var identity = new ClaimsIdentity();
            var currentUser = await GetCurrentUser();
            if (currentUser.IsAuthenticated)
            {
                var claims = new List<Claim> { new Claim(ClaimTypes.Name, currentUser.UserName) };

                //好像客户端也没有用到角色的信息，先注释掉
                //if (currentUser.RoleNameList != null)
                //{
                //    foreach (var rolename in currentUser.RoleNameList)
                //    {
                //        claims.Add(new Claim(ClaimTypes.Actor, rolename));
                //    }
                //}
                //if (currentUser.RoleIdList != null)
                //{
                //    foreach (var roleid in currentUser.RoleIdList)
                //    {
                //        claims.Add(new Claim(ClaimTypes.Role, roleid));
                //    }
                //}
                identity = new ClaimsIdentity(claims, "Password");
            }

            var state = new AuthenticationState(new ClaimsPrincipal(identity));
            return state;
        }

        private async Task<IOperator> GetCurrentUser()
        {
            return await api.CurrentUserInfo();
        }

        public async Task<AjaxResult> Logout()
        {
            var response = await api.Logout();
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return response;
        }

        public async Task<AjaxResult> Login(string userName, string password)
        {
            var response = await api.Login(userName, password);
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
            return response;
        }

        public void Refresh(string token)
        {
            if (string.IsNullOrEmpty(token)) return;
            NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
        }


    }
}
