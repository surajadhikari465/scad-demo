using System;
using System.Runtime.Caching;

namespace Icon.Caching
{
    /// <summary>
    /// Creates, Sets, and Gets from a cache.
    /// </summary>
    public class Cache : ICache
    {
        private MemoryCache cache;

        public Cache()
        {
            this.cache = MemoryCache.Default;
        }

        public void Initialize()
        {
            if (this.cache == null)
            {
                this.cache = MemoryCache.Default;
            }
        }

        public void Set(string key, object value)
        {
            var policy = new CacheItemPolicy();
            AddToCache(key, value, policy);

        }

        public void Set(string key, object value, DateTime expiresAt)
        {
            var policy = new CacheItemPolicy();
            policy.AbsoluteExpiration = expiresAt;
            AddToCache(key, value, policy);
        }

        public void Set(string key, object value, TimeSpan timespan)
        {
            var policy = new CacheItemPolicy();
            policy.SlidingExpiration = timespan;
            AddToCache(key, value, policy);
        }

        private void AddToCache(string key, object value, CacheItemPolicy policy)
        {
            this.cache.Set(key, value, policy);
        }

        public object Get(string key)
        {
            return this.cache[key];
        }

        public void Remove(string key)
        {
            this.cache.Remove(key);
        }

        public bool Exists(string key)
        {
            return this.cache.Contains(key);
        }
    }
}
