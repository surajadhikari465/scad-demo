using Icon.Common;

namespace AttributePublisher.Infrastructure
{
    public class RecurringServiceSettings
    {
        public double RunInterval { get; set; }

        public static RecurringServiceSettings Load()
        {
            return new RecurringServiceSettings
            {
                RunInterval = AppSettingsAccessor.GetIntSetting(nameof(RunInterval), defaultValue: 30000)
            };
        }
    }
}