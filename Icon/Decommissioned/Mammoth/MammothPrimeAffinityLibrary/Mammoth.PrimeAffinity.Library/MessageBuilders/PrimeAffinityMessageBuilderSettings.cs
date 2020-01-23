using Icon.Common;

namespace Mammoth.PrimeAffinity.Library.MessageBuilders
{
    public class PrimeAffinityMessageBuilderSettings
    {
        public string PrimeAffinityPsgName { get; set; }
        public string PrimeAffinityPsgType { get; set; }

        public static PrimeAffinityMessageBuilderSettings Load()
        {
            return new PrimeAffinityMessageBuilderSettings
            {
                PrimeAffinityPsgName = AppSettingsAccessor.GetStringSetting(nameof(PrimeAffinityPsgName)),
                PrimeAffinityPsgType = AppSettingsAccessor.GetStringSetting(nameof(PrimeAffinityPsgType))
            };
        }
    }
}