using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Web.Common.Cache
{
    public static class CacheExtensions
    {
        public static T Get<T>(this ICache cache, string key)
        {
            return (T)cache.Get(key);
        }
    }
}
