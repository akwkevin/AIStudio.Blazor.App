﻿using AIStudio.Common.AppSettings;
using Microsoft.Extensions.Configuration;

namespace AIStudio.Common.Cache;

/// <summary>
/// Redis缓存
/// </summary>
public class RedisCache : ICache
{

    public RedisCache()
    {
        var csredis = new CSRedis.CSRedisClient(AppSettingsConfig.RedisOptions.ConnectionString);
        RedisHelper.Initialization(csredis);
    }

    public long Del(params string[] key)
    {
        return RedisHelper.Del(key);
    }

    public Task<long> DelAsync(params string[] key)
    {
        return RedisHelper.DelAsync(key);
    }

    public async Task<long> DelByPatternAsync(string pattern)
    {
        if (string.IsNullOrEmpty(pattern))
            return default;

        //pattern = Regex.Replace(pattern, @"\{.*\}", "*");
        var keys = await RedisHelper.KeysAsync(pattern + "*");
        if (keys != null && keys.Length > 0)
        {
            return await RedisHelper.DelAsync(keys);
        }

        return default;
    }

    public bool Exists(string key)
    {
        return RedisHelper.Exists(key);
    }

    public Task<bool> ExistsAsync(string key)
    {
        return RedisHelper.ExistsAsync(key);
    }

    public string Get(string key)
    {
        return RedisHelper.Get(key);
    }

    public T Get<T>(string key)
    {
        return RedisHelper.Get<T>(key);
    }

    public Task<string> GetAsync(string key)
    {
        return RedisHelper.GetAsync(key);
    }

    public Task<T> GetAsync<T>(string key)
    {
        return RedisHelper.GetAsync<T>(key);
    }

    public bool Set(string key, object value)
    {
        return RedisHelper.Set(key, value);
    }

    public bool Set(string key, object value, TimeSpan expire)
    {
        return RedisHelper.Set(key, value, expire);
    }

    public Task<bool> SetAsync(string key, object value)
    {
        return RedisHelper.SetAsync(key, value);
    }

    public Task<bool> SetAsync(string key, object value, TimeSpan expire)
    {
        return RedisHelper.SetAsync(key, value, expire);
    }

    public List<string> GetAllKeys()
    {
        return RedisHelper.Keys("*").ToList();
    }
}
