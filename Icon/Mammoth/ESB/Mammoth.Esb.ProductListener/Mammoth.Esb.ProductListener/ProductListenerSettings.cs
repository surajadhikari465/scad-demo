using Icon.Common;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.ProductListener
{
    public class ProductListenerSettings
    {
        public bool EnablePrimeAffinityMessages { get; set; }
        public List<int> ExcludedPSNumbers { get; set; }
        public List<string> EligiblePriceTypes { get; set; }

        public static ProductListenerSettings Load()
        {
            return new ProductListenerSettings
            {
                EnablePrimeAffinityMessages = AppSettingsAccessor.GetBoolSetting(nameof(EnablePrimeAffinityMessages), false),
                ExcludedPSNumbers = AppSettingsAccessor.GetStringSetting(nameof(ExcludedPSNumbers)).Split(',').Select(ps => int.Parse(ps)).ToList(),
                EligiblePriceTypes = AppSettingsAccessor.GetStringSetting(nameof(EligiblePriceTypes)).Split(',').Select(pt => pt.Trim()).ToList(),
            };
        }
    }
}
