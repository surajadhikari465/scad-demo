using System.Collections.Generic;

namespace GlobalEventController.Common
{
    public static class StartupOptions
    {
        public static int Instance { get; set; }
        public static List<string> NutritionEnabledRegions { get; set; }
    }
}
