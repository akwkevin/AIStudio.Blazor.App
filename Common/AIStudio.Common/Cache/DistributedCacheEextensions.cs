using AIStudio.Util;
using Microsoft.Extensions.Caching.Distributed;
using System.Collections;
using System.Text;

namespace AIStudio.Common.Cache;

public static class DistributedCacheEextensions
{
    public static TCacheItem? Get<TCacheItem>(this IDistributedCache cache, string key)
    {
        var cachedBytes = cache.Get(key);
        var cached = System.Text.UTF8Encoding.Default.GetString(cachedBytes);
        var value = cached.ToObject<TCacheItem>();
        return value;
    }

    public static async Task<TCacheItem?> GetAsync<TCacheItem>(this IDistributedCache cache, string key, CancellationToken token = default)
    {
        var cachedBytes = await cache.GetAsync(key, token);
        string json =  System.Text.UTF8Encoding.Default.GetString(cachedBytes);
        var value = json.ToObject<TCacheItem>();
        return value;
    }

    public static void Set<TCacheItem>(this IDistributedCache cache, string key, TCacheItem value)
    {
        string json = value.ToJson();
        var bytes = System.Text.UTF8Encoding.Default.GetBytes(json);
        cache.Set(key, bytes);
    }

    public static void Set<TCacheItem>(this IDistributedCache cache, string key, TCacheItem value, DistributedCacheEntryOptions options)
    {
        string json = value.ToJson();
        var bytes = System.Text.UTF8Encoding.Default.GetBytes(json);
        cache.Set(key, bytes, options);
    }

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
