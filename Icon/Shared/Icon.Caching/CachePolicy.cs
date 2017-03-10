using System;
using System.Runtime.Caching;

namespace Icon.Caching
{
    /// <summary>
    /// Sets how long the cache should hold the data.
    /// </summary>
    /// <typeparam name="TQuery"></typeparam>
    public class CachePolicy<TQuery> : ICachePolicy<TQuery>
    {
        private CacheItemPolicy policy;

        public DateTime AbsoluteExpiration { get; set; }
        public TimeSpan SlidingExpiration { get; set; }

        public CachePolicy()
        {
            this.policy = new CacheItemPolicy();
        }
    }
}
