using Microsoft.AspNetCore.Http;
using System.Net.Http.Headers;
using System.Text;


namespace AIStudio.Common.Authentication.BasicAuth
{
    public class BasicAuthMiddleware
    {
        private readonly RequestDelegate _next;

        public BasicAuthMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context, IBaseUserService userService)
        {
            try
            {
                var authHeader = AuthenticationHeaderValue.Parse(context.Request.Headers["Authorization"]);
                var credentialBytes = Convert.FromBase64String(authHeader.Parameter);
                var credentials = Encoding.UTF8.GetString(credentialBytes).Split(':', 2);
                var username = credentials[0];
                var password = credentials[1];

                // authenticate credentials with user service and attach user to http context
                context.Items["User"] = await userService.Authenticate(username, password);
            }
            catch
            {
                // do nothing if invalid auth header
                // user is not attached to context so request won't have access to secure routes
            }

            await _next(context);
        }
    }

    //使用方法，需要在启动项添加下面两个方法
    // configure DI for application services
    //services.AddScoped<IUserService, UserService>();
    // custom basic auth middleware
    //app.UseMiddleware<BasicAuthMiddleware>();


}