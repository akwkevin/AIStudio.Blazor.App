using AIStudio.Common.Cache;
using Microsoft.Extensions.Caching.Distributed;
using Quartz.Util;
using SqlSugar;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.SqlSuger
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="SqlSugar.ICacheService" />
    public class SqlSugarCache : ICacheService
    {
        /// <summary>
        /// The cache
        /// </summary>
        private IDistributedCache _cache;
        /// <summary>
        /// The keys
        /// </summary>
        private SynchronizedCollection<string> _keys = new SynchronizedCollection<string>();
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlSugarCache"/> class.
        /// </summary>
        /// <param name="cache">The cache.</param>
        public SqlSugarCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="TV">The type of the v.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        public void Add<TV>(string key, TV value)
        {
            _cache.Set(key, value);
            _keys.Add(key);
        }

        /// <summary>
        /// Adds the specified key.
        /// </summary>
        /// <typeparam name="TV">The type of the v.</typeparam>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        /// <param name="cacheDurationInSeconds">The cache duration in seconds.</param>
        public void Add<TV>(string key, TV value, int cacheDurationInSeconds)
        {
            _cache.Set(key, value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds) });
            _keys.Add(key);
        }

        /// <summary>
        /// Determines whether the specified key contains key.
        /// </summary>
        /// <typeparam name="TV">The type of the v.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns>
        ///   <c>true</c> if the specified key contains key; otherwise, <c>false</c>.
        /// </returns>
        public bool ContainsKey<TV>(string key)
        {
            return _keys.Contains(key);
        }

        /// <summary>
        /// Gets the specified key.
        /// </summary>
        /// <typeparam name="TV">The type of the v.</typeparam>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        public TV Get<TV>(string key)
        {
            return _cache.Get<TV>(key);
        }

        /// <summary>
        /// Gets all key.
        /// </summary>
        /// <typeparam name="TV">The type of the v.</typeparam>
        /// <returns></returns>
        public IEnumerable<string> GetAllKey<TV>()
        {
            return _keys.ToList();
        }

        /// <summary>
        /// Gets the or create.
        /// </summary>
        /// <typeparam name="TV">The type of the v.</typeparam>
        /// <param name="cacheKey">The cache key.</param>
        /// <param name="create">The create.</param>
        /// <param name="cacheDurationInSeconds">The cache duration in seconds.</param>
        /// <returns></returns>
        public TV GetOrCreate<TV>(string cacheKey, Func<TV> create, int cacheDurationInSeconds = int.MaxValue)
        {
            if (ContainsKey<TV>(cacheKey))
            {
                return Get<TV>(cacheKey);
            }
            else
            {
                var result = create();
                Add(cacheKey, result, cacheDurationInSeconds);
                return result;
            }
        }

        /// <summary>
        /// Removes the specified key.
        /// </summary>
        /// <typeparam name="TV">The type of the v.</typeparam>
        /// <param name="key">The key.</param>
        public void Remove<TV>(string key)
        {
            _cache.Remove(key);
            _keys.Remove(key);
        }
    }
}
