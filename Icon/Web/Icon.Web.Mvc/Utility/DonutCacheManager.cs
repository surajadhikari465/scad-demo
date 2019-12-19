using DevTrends.MvcDonutCaching;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Icon.Web.Mvc.Utility
{
    /// <summary>
    /// This class is responsible for wrapping the donut cache manager so we can inject it into our controllers
    /// </summary>
    public class DonutCacheManager : IDonutCacheManager
    {
        public void ClearCacheForController(string controllerName)
        {
            var cacheManager = new OutputCacheManager();
            cacheManager.RemoveItems(controllerName);
        }
    }
}