using AIStudio.Common.AppSettings;
using AIStudio.Common.Jwt;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace AIStudio.Common.Authentication.Jwt;

/// <summary>
/// JWT Helper
/// </summary>
public class JwtHelper
{
    /// <summary>
    /// 创建Token
    /// </summary>
    /// <param name="claims"></param>
    /// <param name="isRefresh"></param>
    /// <returns></returns>
    public static string CreateToken(List<Claim> claims, bool isRefresh = false)
    {
        // 获取配置
        var secret = isRefresh ? AppSettingsConfig.JwtOptions.RefreshSecretKey : AppSettingsConfig.JwtOptions.SecretKey;
        var issuer = AppSettingsConfig.JwtOptions.Issuer;
        var audience = AppSettingsConfig.JwtOptions.Audience;
        var expireHours = isRefresh ? AppSettingsConfig.JwtOptions.RefreshExpireHours : AppSettingsConfig.JwtOptions.AccessExpireHours;

        if (string.IsNullOrEmpty(secret))
            return string.Empty;

        // 密钥
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secret));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 生成 token
        var jwt = new JwtSecurityToken(issuer: issuer, audience: audience, claims: claims, signingCredentials: creds, expires: DateTime.Now.AddHours(expireHours));
        var jwtHandler = new JwtSecurityTokenHandler();
        var encodedJwt = jwtHandler.WriteToken(jwt);

        return encodedJwt;
    }

    /// <summary>
    /// 验证 Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="secretKey"></param>
    /// <returns></returns>
    public static (bool IsValid, JsonWebToken Token, TokenValidationResult validationResult) Validate(string accessToken, string secretKey)
    {
        if (string.IsNullOrWhiteSpace(secretKey)) return (false, default, default);

        // 加密Key
        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(secretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        // 创建Token验证参数
        var tokenValidationParameters = CreateTokenValidationParameters(secretKey);
        tokenValidationParameters.IssuerSigningKey ??= creds.Key;

        // 验证 Token
        var tokenHandler = new JsonWebTokenHandler();
        try
        {
            var tokenValidationResult = tokenHandler.ValidateToken(accessToken, tokenValidationParameters);
            if (!tokenValidationResult.IsValid) return (false, null, tokenValidationResult);

            var jsonWebToken = tokenValidationResult.SecurityToken as JsonWebToken;
            return (true, jsonWebToken, tokenValidationResult);
        }
        catch
        {
            return (false, default, default);
        }
    }
    /// <summary>
    /// 生成Token验证参数
    /// </summary>
    /// <param name="secretKey"></param>
    /// <returns></returns>
    public static TokenValidationParameters CreateTokenValidationParameters(string secretKey)
    {
        // 读取配置
        var symmetricKeyAsBase64 = secretKey;
        var issuer = AppSettingsConfig.JwtOptions.Issuer;
        var audience = AppSettingsConfig.JwtOptions.Audience;

        // 获取密钥
        var keyByteArray = Encoding.UTF8.GetBytes(symmetricKeyAsBase64);
        var signingKey = new SymmetricSecurityKey(keyByteArray);
        var signingCredentials = new SigningCredentials(signingKey, SecurityAlgorithms.HmacSha256);

        // 令牌验证参数
        return new TokenValidationParameters()
        {
            ValidateIssuerSigningKey = true, // 是否验证SecurityKey
            IssuerSigningKey = signingKey, // 拿到SecurityKey
            ValidateIssuer = true, // 是否验证Issuer
            ValidIssuer = issuer, // 发行人Issuer
            ValidateAudience = true, // 是否验证Audience
            ValidAudience = audience, // 订阅人Audience
            ValidateLifetime = true, // 是否验证失效时间
            ClockSkew = TimeSpan.FromSeconds(30), // 过期时间容错值，解决服务器端时间不同步问题（秒）
            RequireExpirationTime = true,
        };
    }
    /// <summary>
    /// 通过过期Token 和 刷新Token 换取新的 Token
    /// </summary>
    /// <param name="expiredToken"></param>
    /// <param name="refreshToken"></param>
    /// <param name="secretKey"></param>
    /// <param name="refreshSecretKey"></param>
    /// <param name="httpContext"></param>
    /// <param name="expiredTime">过期时间（分钟）</param>
    /// <param name="clockSkew">刷新token容差值，秒做单位</param>
    /// <returns></returns>
    public static string Exchange(string expiredToken, string refreshToken, string secretKey, string refreshSecretKey, HttpContext httpContext, double expiredTime, long clockSkew = 5)
    {
        // 交换刷新Token 必须原Token 已过期
        var (_isValid, _, _) = Validate(expiredToken, secretKey);
        if (_isValid) return default;

        // 判断刷新Token 是否过期
        var (isValid, refreshTokenObj, _) = Validate(refreshToken, refreshSecretKey);
        if (!isValid) return default;

        // 判断这个刷新Token 是否已刷新过
        var blacklistRefreshKey = "BLACKLIST_REFRESH_TOKEN:" + refreshToken;
        var distributedCache = httpContext?.RequestServices?.GetService<IDistributedCache>();

        // 处理token并发容错问题
        //var nowTime = DateTimeOffset.UtcNow;
        //var cachedValue = distributedCache?.GetString(blacklistRefreshKey);
        //var isRefresh = !string.IsNullOrWhiteSpace(cachedValue);    // 判断是否刷新过
        //if (isRefresh)
        //{
        //    var refreshTime = new DateTimeOffset(long.Parse(cachedValue), TimeSpan.Zero);
        //    // 处理并发时容差值
        //    if ((nowTime - refreshTime).TotalSeconds > clockSkew) return default;
        //}

        // 分割过期Token
        var tokenParagraphs = expiredToken.Split('.', StringSplitOptions.RemoveEmptyEntries);
        if (tokenParagraphs.Length < 3) return default;

        // 判断各个部分是否匹配
        //if (!refreshTokenObj.GetPayloadValue<string>("exp").Equals(tokenParagraphs[0])) return default;
        //if (!refreshTokenObj.GetPayloadValue<string>("e").Equals(tokenParagraphs[2])) return default;
        //if (!tokenParagraphs[1].Substring(refreshTokenObj.GetPayloadValue<int>("s"), refreshTokenObj.GetPayloadValue<int>("l")).Equals(refreshTokenObj.GetPayloadValue<string>("k"))) return default;

        // 获取过期 Token 的存储信息
        var jwtSecurityToken = SecurityReadJwtToken(expiredToken);
        var payload = jwtSecurityToken.Payload;

        // 移除 Iat，Nbf，Exp
        foreach (var innerKey in DateTypeClaimTypes)
        {
            if (!payload.ContainsKey(innerKey)) continue;
            payload.Remove(innerKey);
        }

        // 交换成功后登记刷新Token，标记失效
        //if (!isRefresh)
        //{
        //    distributedCache?.SetString(blacklistRefreshKey, nowTime.Ticks.ToString(), new DistributedCacheEntryOptions
        //    {
        //        AbsoluteExpiration = DateTimeOffset.FromUnixTimeSeconds(refreshTokenObj.GetPayloadValue<long>(JwtRegisteredClaimNames.Exp))
        //    });
        //}

        return Encrypt(payload, secretKey, expiredTime);
    }
    /// <summary>
    /// 生成 Token
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="expiredTime">过期时间（分钟）</param>
    /// <returns></returns>
    public static string Encrypt(IDictionary<string, object> payload, string secretKey, double expiredTime)
    {
        var Payload = CombinePayload(payload, expiredTime);
        return Encrypt(secretKey, Payload, SecurityAlgorithms.HmacSha256);
    }
    /// <summary>
    /// 生成 Token
    /// </summary>
    /// <param name="issuerSigningKey"></param>
    /// <param name="payload"></param>
    /// <param name="algorithm"></param>
    /// <returns></returns>
    public static string Encrypt(string issuerSigningKey, IDictionary<string, object> payload, string algorithm = SecurityAlgorithms.HmacSha256)
    {
        // 处理 JwtPayload 序列化不一致问题
        var stringPayload = payload is JwtPayload jwtPayload ? jwtPayload.SerializeToJson() : JsonSerializer.Serialize(payload, new JsonSerializerOptions
        {
            Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
        });
        return Encrypt(issuerSigningKey, stringPayload, algorithm);
    }
    /// <summary>
    /// 生成 Token
    /// </summary>
    /// <param name="issuerSigningKey"></param>
    /// <param name="payload"></param>
    /// <param name="algorithm"></param>
    /// <returns></returns>
    public static string Encrypt(string issuerSigningKey, string payload, string algorithm = SecurityAlgorithms.HmacSha256)
    {
        SigningCredentials credentials = null;

        if (!string.IsNullOrWhiteSpace(issuerSigningKey))
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(issuerSigningKey));
            credentials = new SigningCredentials(securityKey, algorithm);
        }

        var tokenHandler = new JsonWebTokenHandler();
        return credentials == null ? tokenHandler.CreateToken(payload) : tokenHandler.CreateToken(payload, credentials);
    }
    /// <summary>
    /// 组合 Claims 负荷
    /// </summary>
    /// <param name="payload"></param>
    /// <param name="expiredTime">过期时间，单位：分钟</param>
    /// <returns></returns>
    private static IDictionary<string, object> CombinePayload(IDictionary<string, object> payload, double expiredTime)
    {
        var datetimeOffset = DateTimeOffset.Now;

        TimeSpan ts = DateTime.Now.ToUniversalTime() - new DateTime(1970, 1, 1);
        long time = (long)ts.TotalSeconds;

        TimeSpan ts1 = DateTime.Now.AddHours(expiredTime).ToUniversalTime() - new DateTime(1970, 1, 1);

        if (!payload.ContainsKey(JwtRegisteredClaimNames.Iat))
        {
            payload.Add(JwtRegisteredClaimNames.Iat, time);
        }

        if (!payload.ContainsKey(JwtRegisteredClaimNames.Nbf))
        {
            payload.Add(JwtRegisteredClaimNames.Nbf, time);
        }

        if (!payload.ContainsKey(JwtRegisteredClaimNames.Exp))
        {
            var minute = expiredTime;
            payload.Add(JwtRegisteredClaimNames.Exp, (long)ts1.TotalSeconds);
        }

        if (!payload.ContainsKey(JwtRegisteredClaimNames.Iss))
        {
            payload.Add(JwtRegisteredClaimNames.Iss, AppSettingsConfig.JwtOptions.Issuer);
        }

        if (!payload.ContainsKey(JwtRegisteredClaimNames.Aud))
        {
            payload.Add(JwtRegisteredClaimNames.Aud, AppSettingsConfig.JwtOptions.Audience);
        }

        return payload;
    }

    /// <summary>
    /// 读取 Token，不含验证
    /// </summary>
    /// <param name="accessToken"></param>
    /// <returns></returns>
    public static JwtSecurityToken SecurityReadJwtToken(string accessToken)
    {
        var jwtSecurityTokenHandler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = jwtSecurityTokenHandler.ReadJwtToken(accessToken);
        return jwtSecurityToken;
    }
    /// <summary>
    /// 日期类型的 Claim 类型
    /// </summary>
    private static readonly string[] DateTypeClaimTypes = new[] { JwtRegisteredClaimNames.Iat, JwtRegisteredClaimNames.Nbf, JwtRegisteredClaimNames.Exp };
    /// <summary>
    /// 生成刷新 Token
    /// </summary>
    /// <param name="accessToken"></param>
    /// <param name="expiredTime">刷新 Token 有效期（分钟）</param>
    /// <returns></returns>
    public static string GenerateRefreshToken(string accessToken, string SecretKey, long expiredTime = 43200)
    {
        // 分割Token
        var tokenParagraphs = accessToken.Split('.', StringSplitOptions.RemoveEmptyEntries);

        var s = RandomNumberGenerator.GetInt32(10, tokenParagraphs[1].Length / 2 + 2);
        var l = RandomNumberGenerator.GetInt32(3, 13);

        var payload = new Dictionary<string, object>
            {
                { "f",tokenParagraphs[0] },
                { "e",tokenParagraphs[2] },
                { "s",s },
                { "l",l },
                { "k",tokenParagraphs[1].Substring(s,l) }
            };

        return Encrypt(payload, SecretKey, expiredTime);
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

    /// <summary>
    /// 测试权限类型
    /// </summary>
    public static class Permissions
    {
        /// <summary>
        /// User
        /// </summary>
        public const string User = "User";
        /// <summary>
        /// UserCreate
        /// </summary>
        public const string UserCreate = User + ".Create";
        /// <summary>
        /// UserDelete
        /// </summary>
        public const string UserDelete = User + ".Delete";
        /// <summary>
        /// UserUpdate
        /// </summary>
        public const string UserUpdate = User + ".Update";
    }
    #endregion


}
