using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetTemplate.CommonService
{
    public class CacheService : ICacheService
    {
        private IMemoryCache _cache;
        public CacheService(IMemoryCache memoryCache)
        {
            _cache = memoryCache;
        }

        public object Get(string key)
        {
            // Look for cache key.
            object cacheEntry = null;
            _cache.TryGetValue(key, out cacheEntry);
            return cacheEntry;
        }

        public void Set(string key, object value)
        {
            object cacheEntry = null;
            if (!_cache.TryGetValue(key, out cacheEntry))
            {
                // Key not in cache, so get data.
                cacheEntry = value;

                // Set cache options.
                var cacheEntryOptions = new MemoryCacheEntryOptions()
                    // Keep in cache for this time, reset time if accessed.
                    .SetSlidingExpiration(TimeSpan.FromSeconds(3600));

                _cache.Set(key, cacheEntry, cacheEntryOptions);
            }
        }
    }
}
