#region "Imports"

using System;
using System.Collections.Concurrent;
using System.Runtime.Caching;

#endregion

namespace CacheStore
{
    public static class Cache
    {
        private const string DefaultCacheName = "default";

        public static ConcurrentDictionary<string, MemoryCache> _caches;

        #region "Constructors"

        static Cache()
        {
            _caches = new ConcurrentDictionary<string, MemoryCache>();
        }

        #endregion

        public static T Get<T>(CacheKey key)
        {
            object o = GetCache(key.CacheName).Get(key.GenerateKey());
            if (o is T)
            {
                return (T)o;
            }
            else
            {
                return default(T);
            }

        }

        public static bool TryGetValue<T>(CacheKey key, out T value)
        {
            object o = GetCache(key.CacheName).Get(key.GenerateKey());
            if (o is T)
            {
                value = (T)o;
                return true;
            }
            value = default(T);
            return false;
        }

        public static void Set<T>(CacheKey key, T value)
        {
            CacheItemPolicy policy = new CacheItemPolicy();
            if (key.AbsoluteExpiration != DateTimeOffset.MaxValue) policy.AbsoluteExpiration = key.AbsoluteExpiration;
            if (key.SlidingExpiration != TimeSpan.MaxValue) policy.SlidingExpiration = key.SlidingExpiration;
            GetCache(key.CacheName).Set(key.GenerateKey(), value, policy);

        }

        private static MemoryCache GetCache(string cacheName)
        {
            MemoryCache c = null;
            if (!_caches.TryGetValue(cacheName, out c))
            {
                // Use the default cache manager if a manger isn't defined
                c = (cacheName == DefaultCacheName || cacheName == string.Empty) ? MemoryCache.Default : new MemoryCache(cacheName);
                _caches[cacheName] = c;
            }
            return c;
        }

    }
}
