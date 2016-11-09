using Icon.Framework;
using System.Collections.Generic;

namespace Icon.ApiController.Common
{
    public class Cache
    {
        public static Dictionary<string, Item> scanCodeToItem = new Dictionary<string, Item>();
        public static Dictionary<string, int> financialSubteamToHierarchyClassId = new Dictionary<string,int>();
    }
}
