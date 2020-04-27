using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Service.Interfaces;
using Icon.Common;

namespace BrandUploadProcessor.Service
{
    public class ServiceConfiguration : IServiceConfiguration
    {
        public ServiceConfiguration()
        {
        }

        public int TimerInterval { get; set; }
        public string IconConnectionString { get; set; }

        public List<string> BrandRefreshEventConfiguredRegions { get; set; }

        public static ServiceConfiguration LoadFromAppSettings()
        {
            var regions = AppSettingsAccessor.GetStringSetting("BrandRefreshEventConfiguredRegions", true);
            return new ServiceConfiguration
            {
                TimerInterval = AppSettingsAccessor.GetIntSetting("ServiceTimerInterval", 60000),
                IconConnectionString = AppSettingsAccessor.GetStringSetting("IconConnectionString", true),
                BrandRefreshEventConfiguredRegions = regions.Split(',').ToList()

            };
        }
    }
}