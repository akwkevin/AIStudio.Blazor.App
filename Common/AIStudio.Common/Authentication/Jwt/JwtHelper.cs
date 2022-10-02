using AIStudio.Common.Helpers;
using AIStudio.Common.Jwt;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AIStudio.Common.Authentication.Jwt;

public class JwtHelper
{
    private readonly IConfiguration _configuration;

    public JwtHelper(IConfiguration configuration)
    {
        _configuration = configuration;
    }
    /// <summary>
    /// 生成 JWT Token
    /// </summary>
    /// <param name="tokenModel"></param>
    /// <returns></returns>
    public string CreateToken(JwtTokenModel tokenModel)
    {
        // 获取配置
        var secret = _configuration["Jwt:SecretKey"];
        var issuer = _configuration["Jwt:Issuer"];
        var audience = _configuration["Jwt:Audience"];

        var claims = new List<Claim>()
        {
            new Claim(SimpleClaimTypes.UserName, tokenModel.UserName),
            new Claim(SimpleClaimTypes.JwtId, tokenModel.UserName),
            new Claim(SimpleClaimTypes.IssuedAt, DateTime.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(SimpleClaimTypes.NotBefore, DateTime.Now.ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(SimpleClaimTypes.Expiration, DateTime.Now.AddSeconds(tokenModel.Expiration).ToUnixTimeSeconds().ToString(), ClaimValueTypes.Integer64),
            new Claim(SimpleClaimTypes.Issuer, issuer),
            new Claim(SimpleClaimTypes.Audience, audience),
        };

        foreach (var role in tokenModel.Roles)
        {
            if (string.IsNullOrEmpty(role)) continue;
            claims.Add(new Claim(SimpleClaimTypes.Role, role));
        }

        if (tokenModel.Claims.Count > 0)
        {
            claims.AddRange(tokenModel.Claims);
        }

        // 密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 生成 token
        var jwt = new JwtSecurityToken(claims: claims, signingCredentials: creds);
        var jwtHandler = new JwtSecurityTokenHandler();
        var encodedJwt = jwtHandler.WriteToken(jwt);

        return encodedJwt;
    }

    #region
    /// <summary>
    /// 测试使用的，别用
    /// </summary>
    /// <returns></returns>
    public string CreateToken()
    {
        // 1. 定义需要使用到的Claims
        var claims = new[]
        {
            new Claim(ClaimTypes.Name, "u_admin"), //HttpContext.User.Identity.Name
            new Claim(ClaimTypes.Role, "r_admin"), //HttpContext.User.IsInRole("r_admin")
            new Claim(JwtRegisteredClaimNames.Jti, "admin"),
            new Claim("Username", "Admin"),
            new Claim("Name", "超级管理员"),
            new Claim("Permission", Permissions.UserCreate),
            new Claim("Permission", Permissions.UserUpdate)
        };

        // 2. 从 appsettings.json 中读取SecretKey
        var secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:SecretKey"]));

        // 3. 选择加密算法
        var algorithm = SecurityAlgorithms.HmacSha256;

        // 4. 生成Credentials
        var signingCredentials = new SigningCredentials(secretKey, algorithm);

        // 5. 根据以上，生成token
        var jwtSecurityToken = new JwtSecurityToken(
            _configuration["Jwt:Issuer"],     //Issuer
            _configuration["Jwt:Audience"],   //Audience
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
