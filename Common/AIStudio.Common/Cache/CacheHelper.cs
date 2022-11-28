using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.Extensions.Caching.Distributed;

namespace AIStudio.Common.Cache;

/// <summary>
/// 
/// </summary>
public static class CacheHelper
{
    /// <summary>
    /// The cache
    /// </summary>
    private static IDistributedCache? _cache;

    /// <summary>
    /// Gets the cache.
    /// </summary>
    /// <value>
    /// The cache.
    /// </value>
    /// <exception cref="System.NullReferenceException">Cache</exception>
    private static IDistributedCache Cache
    {
        get
        {
            if (_cache == null) throw new NullReferenceException(nameof(Cache));
            return _cache;
        }
    }

    /// <summary>
    /// 缓存所有索引键（Key）
    /// </summary>
    public const string KeySetCacheKey = "key_set";

    /// <summary>
    /// Configures the specified cache.
    /// </summary>
    /// <param name="cache">The cache.</param>
    /// <exception cref="System.Exception"></exception>
    /// <exception cref="System.ArgumentNullException">cache</exception>
    public static void Configure(IDistributedCache? cache)
    {
        if(_cache != null)
        {
            throw new Exception($"{nameof(Cache)}不可修改！");
        }
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    #region 缓存操作方法

    /// <summary>
    /// Gets the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static byte[] Get(string key)
    {
        return Cache.Get(key);
    }

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public static Task<byte[]> GetAsync(string key, CancellationToken token = default)
    {
        return Cache.GetAsync(key, token);
    }

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public static Task<TCacheItem?> GetAsync<TCacheItem>(string key, CancellationToken token = default)
        where TCacheItem : class
    {
        return Cache.GetAsync<TCacheItem>(key, token);
    }

    /// <summary>
    /// Refreshes the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    public static void Refresh(string key)
    {
        Cache.Refresh(key);
    }

    /// <summary>
    /// Refreshes the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public static Task RefreshAsync(string key, CancellationToken token = default)
    {
        return Cache.RefreshAsync(key, token);
    }

    /// <summary>
    /// Removes the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    public static void Remove(string key)
    {
        Cache.Remove(key);

        RemoveCacheKey(key);
    }

    /// <summary>
    /// Removes the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    public static async Task RemoveAsync(string key, CancellationToken token = default)
    {
        await Cache.RemoveAsync(key, token);

        await RemoveCacheKeyAsync(key);
    }

    /// <summary>
    /// Removes the range.
    /// </summary>
    /// <param name="keys">The keys.</param>
    public static void RemoveRange(IEnumerable<string> keys)
    {
        foreach(string key in keys)
        {
            Cache.Remove(key);
        }

        RemoveRangeCacheKey(keys);
    }

    /// <summary>
    /// Removes the range asynchronous.
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <param name="token">The token.</param>
    public static async Task RemoveRangeAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        foreach (string key in keys)
        {
            await Cache.RemoveAsync(key, token);
        }

        await RemoveRangeCacheKeyAsync(keys, token);
    }

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public static void Set(string key, byte[] value)
    {
        Cache.Set(key, value);

        AddCacheKey(key);
    }

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    public static void Set(string key, byte[] value, DistributedCacheEntryOptions options)
    {
        Cache.Set(key, value, options);

        AddCacheKey(key);
    }

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="token">The token.</param>
    public static async Task SetAsync(string key, byte[] value, CancellationToken token = default)
    {
        await Cache.SetAsync(key, value, token);

        await AddCacheKeyAsync(key);
    }

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    /// <param name="token">The token.</param>
    public static async Task SetAsync(string key, byte[] value, DistributedCacheEntryOptions options, CancellationToken token = default)
    {
        await Cache.SetAsync(key, value, options, token);

        await AddCacheKeyAsync(key);
    }

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="token">The token.</param>
    public static async Task SetAsync<TCacheItem>(string key, TCacheItem value, CancellationToken token = default)
        where TCacheItem : class
    {
        await Cache.SetAsync<TCacheItem>(key, value, token);

        await AddCacheKeyAsync(key);
    }

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    /// <param name="token">The token.</param>
    public static async Task SetAsync<TCacheItem>(string key, TCacheItem value, DistributedCacheEntryOptions options, CancellationToken token = default)
        where TCacheItem : class
    {
        await Cache.SetAsync<TCacheItem>(key, value, options, token);

        await AddCacheKeyAsync(key);
    }

    #endregion


    #region 索引键操作方法

    /// <summary>
    /// Gets the key set.
    /// </summary>
    /// <returns></returns>
    public static HashSet<string> GetKeySet()
    {
        var keySet = Cache.Get<HashSet<string>>(KeySetCacheKey);
        return keySet ?? new HashSet<string>();
    }

    /// <summary>
    /// Gets the key set asynchronous.
    /// </summary>
    /// <returns></returns>
    public static async Task<HashSet<string>> GetKeySetAsync()
    {
        var keySet = await Cache.GetAsync<HashSet<string>>(KeySetCacheKey);
        return keySet ?? new HashSet<string>();
    }

    /// <summary>
    /// Adds the cache key.
    /// </summary>
    /// <param name="key">The key.</param>
    public static void AddCacheKey(string key)
    {
        // 获取缓存键集合
        var keySet = GetKeySet();

        // 成功添加，则写入新的缓存
        if (keySet.Add(key))
        {
            Cache.Set(KeySetCacheKey, keySet);
        }
    }

    /// <summary>
    /// Adds the cache key asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    public static async Task AddCacheKeyAsync(string key, CancellationToken token = default)
    {
        // 获取缓存键集合
        var keySet = await GetKeySetAsync();

        // 成功添加，则写入新的缓存
        if (keySet.Add(key))
        {
            await Cache.SetAsync(KeySetCacheKey, keySet, token);
        }
    }

    /// <summary>
    /// Adds the range cache key.
    /// </summary>
    /// <param name="keys">The keys.</param>
    public static void AddRangeCacheKey(IEnumerable<string> keys)
    {
        // 获取缓存键集合
        var keySet = GetKeySet();

        // 添加
        foreach (var key in keys)
        {
            keySet.Add(key);
        }
        // 写入缓存
        Cache.Set(KeySetCacheKey, keySet);
    }

    /// <summary>
    /// Adds the range cache key asynchronous.
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <param name="token">The token.</param>
    public static async Task AddRangeCacheKeyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        // 获取缓存键集合
        var keySet = await GetKeySetAsync();

        // 添加
        foreach (var key in keys)
        {
            keySet.Add(key);
        }
        // 写入缓存
        await Cache.SetAsync(KeySetCacheKey, keySet, token);
    }

    /// <summary>
    /// Removes the cache key.
    /// </summary>
    /// <param name="key">The key.</param>
    public static void RemoveCacheKey(string key)
    {
        // 读缓存
        var keySet = GetKeySet();

        // 如果缓存存在，且成功删除，则更新缓存
        if (keySet.Remove(key))
        {
            Cache.Set(KeySetCacheKey, keySet);
        }
    }

    /// <summary>
    /// Removes the cache key asynchronous.
    /// </summary>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    public static async Task RemoveCacheKeyAsync(string key, CancellationToken token = default)
    {
        // 读缓存
        var keySet = await GetKeySetAsync();

        // 如果缓存存在，且成功删除，则更新缓存
        if (keySet.Remove(key))
        {
            await Cache.SetAsync(KeySetCacheKey, keySet, token);
        }
    }

    /// <summary>
    /// Removes the range cache key.
    /// </summary>
    /// <param name="keys">The keys.</param>
    public static void RemoveRangeCacheKey(IEnumerable<string> keys)
    {
        // 读缓存
        var keySet = GetKeySet();

        foreach (string key in keys)
        {
            keySet.Remove(key);
        }
        Cache.Set(KeySetCacheKey, keySet);
    }

    /// <summary>
    /// Removes the range cache key asynchronous.
    /// </summary>
    /// <param name="keys">The keys.</param>
    /// <param name="token">The token.</param>
    public static async Task RemoveRangeCacheKeyAsync(IEnumerable<string> keys, CancellationToken token = default)
    {
        // 读缓存
        var keySet = await GetKeySetAsync();

        foreach (string key in keys)
        {
            keySet.Remove(key);
        }
        await Cache.SetAsync(KeySetCacheKey, keySet, token);
    }

    #endregion


    #region 辅助方法

    /// <summary>
    /// Gets the key.
    /// </summary>
    /// <param name="partialKeys">The partial keys.</param>
    /// <returns></returns>
    public static string GetKey(params string[] partialKeys)
    {
        return string.Join("", partialKeys);
    }

    #endregion
}
