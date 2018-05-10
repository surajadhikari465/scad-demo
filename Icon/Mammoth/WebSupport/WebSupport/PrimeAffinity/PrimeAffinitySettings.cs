using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebSupport.PrimeAffinity
{
    public class PrimeAffinitySettings
    {
        public List<int> ExcludedPSNumbers { get; set; }
        public List<string> PriceTypes { get; set; }

        public static PrimeAffinitySettings Load()
        {
            return new PrimeAffinitySettings
            {
                ExcludedPSNumbers = AppSettingsAccessor.GetStringSetting(nameof(ExcludedPSNumbers)).Split(',').Select(ps => int.Parse(ps)).ToList(),
                PriceTypes = AppSettingsAccessor.GetStringSetting(nameof(PriceTypes)).Split(',').Select(pt => pt.Trim()).ToList()
            };
        }
    }
}