using System;
using System.Collections.Generic;

namespace TlogController.Common
{
    public static class Cache
    {
        public static DateTime CacheCreatedDate = default(DateTime);
        public static Dictionary<int, string> BusinessUnitToRegionCode = new Dictionary<int, string>();
    }
}
