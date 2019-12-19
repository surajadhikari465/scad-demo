using Icon.Common;

namespace AttributePublisher.Services
{
    public class AttributePublisherServiceSettings
    {
        public int RecordsPerQuery { get; set; }
        public int RecordsPerMessage { get; set; }
        public string NonReceivingSystems { get; set; }

        public static AttributePublisherServiceSettings Load()
        {
            return new AttributePublisherServiceSettings
            {
                RecordsPerQuery = AppSettingsAccessor.GetIntSetting(nameof(RecordsPerQuery), defaultValue: 1),
                RecordsPerMessage = AppSettingsAccessor.GetIntSetting(nameof(RecordsPerMessage), defaultValue: 1),
                NonReceivingSystems = AppSettingsAccessor.GetStringSetting(nameof(NonReceivingSystems))
            };
        }
    }
}
