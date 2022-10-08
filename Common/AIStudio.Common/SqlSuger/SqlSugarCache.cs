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
    public class SqlSugarCache : ICacheService
    {
        private IDistributedCache _cache;
        private SynchronizedCollection<string> _keys = new SynchronizedCollection<string>();
        public SqlSugarCache(IDistributedCache cache)
        {
            _cache = cache;
        }

        public void Add<TV>(string key, TV value)
        {
            _cache.Set(key, value);
            _keys.Add(key);
        }

        public void Add<TV>(string key, TV value, int cacheDurationInSeconds)
        {
            _cache.Set(key, value, new DistributedCacheEntryOptions() { AbsoluteExpirationRelativeToNow = TimeSpan.FromSeconds(cacheDurationInSeconds) });
            _keys.Add(key);
        }

        public bool ContainsKey<TV>(string key)
        {
            return _keys.Contains(key);
        }

        public TV Get<TV>(string key)
        {
            return _cache.Get<TV>(key);
        }

        public IEnumerable<string> GetAllKey<TV>()
        {
            return _keys.ToList();
        }

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

        public void Remove<TV>(string key)
        {
            _cache.Remove(key);
            _keys.Remove(key);
        }
    }
}
