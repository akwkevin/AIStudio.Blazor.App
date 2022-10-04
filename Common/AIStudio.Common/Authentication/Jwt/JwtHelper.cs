using AIStudio.Common.AppSettings;
using AIStudio.Common.Jwt;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AIStudio.Common.Authentication.Jwt;

public class JwtHelper
{
    /// <summary>
    /// 生成 JWT Token
    /// </summary>
    /// <param name="tokenModel"></param>
    /// <returns></returns>
    public static string CreateToken(List<Claim> claims)
    {
        // 获取配置
        var secret = AppSettingsConfig.JwtOptions.SecretKey;
        var issuer = AppSettingsConfig.JwtOptions.Issuer;
        var audience = AppSettingsConfig.JwtOptions.Audience;
        var expireHours = AppSettingsConfig.JwtOptions.AccessExpireHours;

        // 密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 生成 token
        var jwt = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, signingCredentials: creds, expires: DateTime.Now.AddHours(expireHours));
        var jwtHandler = new JwtSecurityTokenHandler();
        var encodedJwt = jwtHandler.WriteToken(jwt);

        return encodedJwt;
    }

    #region
    /// <summary>
    /// 测试使用的，别用
    /// </summary>
    /// <returns></returns>
    public static string CreateToken()
    { 
        // 获取配置
        var secret = AppSettingsConfig.JwtOptions.SecretKey;
        var issuer = AppSettingsConfig.JwtOptions.Issuer;
        var audience = AppSettingsConfig.JwtOptions.Audience;

        // 1. 定义需要使用到的Claims
        var claims = new[]
        {
            new Claim(SimpleClaimTypes.Name, "u_admin"), //HttpContext.User.Identity.Name
            new Claim(SimpleClaimTypes.Role, "r_admin"), //HttpContext.User.IsInRole("r_admin")
            new Claim(JwtRegisteredClaimNames.Jti, "admin"),
            new Claim("Username", "Admin"),
            new Claim("Name", "超级管理员"),
            new Claim("Permission", Permissions.UserCreate),
            new Claim("Permission", Permissions.UserUpdate)
        };

        // 2. 从 appsettings.json 中读取SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));

        // 3. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        // 5. 根据以上，生成token
        var jwtSecurityToken = new JwtSecurityToken(
            issuer,     //Issuer
            audience,   //Audience
            claims,                          //Claims,
            DateTime.Now,                    //notBefore
            DateTime.Now.AddSeconds(30),    //expires
            signingCredentials               //Credentials
        );

        // 6. 将token变为string
        var token = new JwtSecurityTokenHandler().WriteToken(jwtSecurityToken);

        return token;
    }

    public static class Permissions
    {
        public const string User = "User";
        public const string UserCreate = User + ".Create";
        public const string UserDelete = User + ".Delete";
        public const string UserUpdate = User + ".Update";
    }
    #endregion
}
