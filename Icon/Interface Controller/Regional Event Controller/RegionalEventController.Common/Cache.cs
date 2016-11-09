using Icon.Framework;
using System.Collections.Generic;

namespace RegionalEventController.Common
{
    public static class Cache
    {
        public static Dictionary<string, int> taxCodeToTaxId = new Dictionary<string, int>();
        public static List<string> itemSbTeamEventEnabledRegions = null;
        public static Dictionary<int, int> nationalClassCodeToNationalClassId = new Dictionary<int, int>();
        public static Dictionary<string, string> brandNameToBrandAbbreviationMap = new Dictionary<string, string>();
        public static Dictionary<string, int> defaultCertificationAgencies = new Dictionary<string, int>();
    }
}
