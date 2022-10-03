using AIStudio.Common.Cache;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace AIStudio.Common.SqlSuger
{
    public class SqlSugarCache : ICacheService
    {
        private ICache _cache;
        public SqlSugarCache(ICache cache)
        {
            _cache = cache;
        }

        public void Add<TV>(string key, TV value)
        {
            _cache.Set(key, value);
        }

        public void Add<TV>(string key, TV value, int cacheDurationInSeconds)
        {
            _cache.Set(key, value, TimeSpan.FromSeconds(cacheDurationInSeconds));
        }

        public bool ContainsKey<TV>(string key)
        {
            return _cache.Exists(key);
        }

        public TV Get<TV>(string key)
        {
            return _cache.Get<TV>(key);
        }

        public IEnumerable<string> GetAllKey<TV>()
        {

            return _cache.GetAllKeys();
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
            _cache.Del(key);
        }
    }
}
