using System;
using System.Runtime.Caching;

namespace Icon.Caching
{
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
