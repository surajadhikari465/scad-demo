using Icon.Common;

namespace Mammoth.PrimeAffinity.Library.Processors
{
    public class PrimeAffinityPsgProcessorSettings
    {
        public string NonReceivingSystems { get; set; }

        public static PrimeAffinityPsgProcessorSettings Load()
        {
            return new PrimeAffinityPsgProcessorSettings
            {
                NonReceivingSystems = AppSettingsAccessor.GetStringSetting(nameof(NonReceivingSystems))
            };
        }
    }
}
