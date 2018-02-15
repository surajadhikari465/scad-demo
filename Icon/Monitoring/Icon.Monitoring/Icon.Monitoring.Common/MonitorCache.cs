using System;
using System.Runtime.Caching;

namespace Icon.Monitoring.Common
{
    public class MonitorCache : IMonitorCache
    {
        private ObjectCache cache;

        public MonitorCache()
        {
            cache = MemoryCache.Default;
        }

        public bool Contains(string key)
        {
            return cache.Contains(key);
        }

        public object Get(string key)
        {
            return cache.Get(key);
        }

        public void Set(string key, object item, DateTimeOffset absoluteExpiration)
        {
            cache.Set(key, item, absoluteExpiration);
        }
    }
}
