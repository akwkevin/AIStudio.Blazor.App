using AIStudio.Business;
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
                var claims = new List<Claim> {
                    new Claim(ClaimTypes.Name, currentUser.Claims[ClaimTypes.Name])
                };

                if (currentUser.Roles != null)
                {
                    for (int i = 0; i < currentUser.Roles.Count; i++)
                    {
                        claims.Add(new Claim(ClaimTypes.Role, currentUser.Roles[i]));
                    }
                }
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
