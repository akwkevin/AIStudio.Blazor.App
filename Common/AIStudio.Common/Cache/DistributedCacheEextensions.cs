using AIStudio.Util;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Text;

namespace AIStudio.Common.Cache;

/// <summary>
/// 
/// </summary>
public static class DistributedCacheEextensions
{
    /// <summary>
    /// Gets the specified key.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="cache">The cache.</param>
    /// <param name="key">The key.</param>
    /// <returns></returns>
    public static TCacheItem? Get<TCacheItem>(this IDistributedCache cache, string key)
    {
        var cachedBytes = cache.Get(key);
        if (cachedBytes == null)
            return default(TCacheItem);
        var cached = System.Text.UTF8Encoding.Default.GetString(cachedBytes);
        var value = cached.ToObject<TCacheItem>();
        return value;
    }

    /// <summary>
    /// Gets the asynchronous.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="cache">The cache.</param>
    /// <param name="key">The key.</param>
    /// <param name="token">The token.</param>
    /// <returns></returns>
    public static async Task<TCacheItem?> GetAsync<TCacheItem>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        var cachedBytes = await cache.GetAsync(key, token);
        if (cachedBytes == null)
            return default(TCacheItem);
        string json =  System.Text.UTF8Encoding.Default.GetString(cachedBytes);
        var value = json.ToObject<TCacheItem>();
        return value;
    }

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="cache">The cache.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    public static void Set<TCacheItem>(this IDistributedCache cache, string key, TCacheItem value)
    {
        string json = value.ToJson();
        var bytes = System.Text.UTF8Encoding.Default.GetBytes(json);
        cache.Set(key, bytes);
    }

    /// <summary>
    /// Sets the specified key.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="cache">The cache.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    public static void Set<TCacheItem>(this IDistributedCache cache, string key, TCacheItem value, DistributedCacheEntryOptions options)
    {
        string json = value.ToJson();
        var bytes = System.Text.UTF8Encoding.Default.GetBytes(json);
        cache.Set(key, bytes, options);
    }

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="cache">The cache.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="token">The token.</param>
    public static async Task SetAsync<TCacheItem>(
            this IDistributedCache cache,
            string key,
            TCacheItem value,
            CancellationToken token = default)
    {
        string json = value.ToJson();
        var bytes = System.Text.UTF8Encoding.Default.GetBytes(json);
        await cache.SetAsync(key, bytes, token);
    }

    /// <summary>
    /// Sets the asynchronous.
    /// </summary>
    /// <typeparam name="TCacheItem">The type of the cache item.</typeparam>
    /// <param name="cache">The cache.</param>
    /// <param name="key">The key.</param>
    /// <param name="value">The value.</param>
    /// <param name="options">The options.</param>
    /// <param name="token">The token.</param>
    public static async Task SetAsync<TCacheItem>(
            this IDistributedCache cache,
            string key,
            TCacheItem value,
            DistributedCacheEntryOptions options,
            CancellationToken token = default)
    {
        string json = value.ToJson();
        var bytes = System.Text.UTF8Encoding.Default.GetBytes(json);
        await cache.SetAsync(key, bytes, options, token);
    }
}
