using System;
using System.Collections.Generic;

namespace Icon.ApiController.Controller
{
    public static class StartupOptions
    {
        public static HashSet<string> ValidArgs = new HashSet<string>(StringComparer.InvariantCultureIgnoreCase) 
        {
            "l", // locale
            "h", // hierarchy
            "i", // item-locale
            "r", // price
            "p", // product
            "g"  // product selection group
        };
    }
}
