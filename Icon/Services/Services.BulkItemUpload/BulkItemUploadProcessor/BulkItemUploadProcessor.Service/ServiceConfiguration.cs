using BulkItemUploadProcessor.Service.Interfaces;
using Icon.Common;

namespace BulkItemUploadProcessor.Service
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
        }

        public int TimerInterval { get; set; }
        public string IconConnectionString { get; set; }

        public static ServiceConfiguration LoadFromAppSettings()
        {
            return new ServiceConfiguration
            {
                TimerInterval = AppSettingsAccessor.GetIntSetting("ServiceTimerInterval", 60000),
                IconConnectionString = AppSettingsAccessor.GetStringSetting("IconConnectionString")
            };
        }
    }
}