using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Common.Cache
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
