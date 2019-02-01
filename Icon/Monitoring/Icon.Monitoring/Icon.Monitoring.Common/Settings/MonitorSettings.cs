namespace Icon.Monitoring.Common.Settings
{
    using Enums;
    using NodaTime;
    using NodaTime.Text;
    using System;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Linq;
    using System.Reflection;

    public class MonitorSettings : IMonitorSettings
    {
        public string IntegrationKey { get; set; }
        public bool SendOpsGenieAlert { get; set; }
        public string OpsGenieUri { get; set; }
        public List<string> ApiControllerMonitorRegions { get; set; }
        public string Environment { get; set; }
        public double MonitorServiceTimer { get; set; }
        public LocalTime PosPushStartTime_FL { get; set; }
        public LocalTime PosPushStartTime_MA { get; set; }
        public LocalTime PosPushStartTime_MW { get; set; }
        public LocalTime PosPushStartTime_NA { get; set; }
        public LocalTime PosPushStartTime_NC { get; set; }
        public LocalTime PosPushStartTime_NE { get; set; }
        public LocalTime PosPushStartTime_PN { get; set; }
        public LocalTime PosPushStartTime_RM { get; set; }
        public LocalTime PosPushStartTime_SO { get; set; }
        public LocalTime PosPushStartTime_SP { get; set; }
        public LocalTime PosPushStartTime_SW { get; set; }
        public LocalTime PosPushStartTime_UK { get; set; }
        public LocalTime StoreOpenCentralTime_FL { get; set; }
        public LocalTime StoreOpenCentralTime_MA { get; set; }
        public LocalTime StoreOpenCentralTime_MW { get; set; }
        public LocalTime StoreOpenCentralTime_NA { get; set; }
        public LocalTime StoreOpenCentralTime_NC { get; set; }
        public LocalTime StoreOpenCentralTime_NE { get; set; }
        public LocalTime StoreOpenCentralTime_PN { get; set; }
        public LocalTime StoreOpenCentralTime_RM { get; set; }
        public LocalTime StoreOpenCentralTime_SO { get; set; }
        public LocalTime StoreOpenCentralTime_SP { get; set; }
        public LocalTime StoreOpenCentralTime_SW { get; set; }
        public LocalTime StoreOpenCentralTime_UK { get; set; }
        public int NumberOfMinutesBeforeStoreOpens { get; set; }
        public LocalTime MaintenanceStartTime { get; set; }
        public LocalTime MaintenanceEndTime { get; set; }
        public string MaintenanceDay { get; set; }
        public Dictionary<string, TimeSpan> MonitorTimers { get; set; }

        public static MonitorSettings CreateFromConfig()
        {
            MonitorSettings settings = new MonitorSettings();
            var pattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss");
            settings.IntegrationKey = AppSettingsAccessor.GetStringSetting("IntegrationKey");
            settings.SendOpsGenieAlert = AppSettingsAccessor.GetBoolSetting("SendOpsGenieAlert");
            settings.OpsGenieUri = AppSettingsAccessor.GetStringSetting("OpsGenieUri");
            settings.Environment = AppSettingsAccessor.GetStringSetting("Environment");
            settings.MonitorServiceTimer = AppSettingsAccessor.GetDoubleSetting("MonitorServiceTimer");
            settings.ApiControllerMonitorRegions = AppSettingsAccessor.GetStringSetting(nameof(ApiControllerMonitorRegions)).Split(',').ToList();
            settings.NumberOfMinutesBeforeStoreOpens = AppSettingsAccessor.GetIntSetting(nameof(NumberOfMinutesBeforeStoreOpens));
            // All monitor settings in app.config need to be <ControllerMonitorName>Timer.
            settings.MonitorTimers = ConfigurationManager.AppSettings.AllKeys.Where(k => k.EndsWith("Timer"))
                    .Select(k => new { Key = k, Value = ConfigurationManager.AppSettings[k] })
                    .ToDictionary(x => x.Key, x => TimeSpan.FromMilliseconds(int.Parse(x.Value)));

            settings.MaintenanceStartTime = pattern.Parse(AppSettingsAccessor.GetStringSetting(nameof(MaintenanceStartTime))).Value;
            settings.MaintenanceEndTime = pattern.Parse(AppSettingsAccessor.GetStringSetting(nameof(MaintenanceEndTime))).Value;
            settings.MaintenanceDay = AppSettingsAccessor.GetStringSetting("MaintenanceDay");


            string config = String.Empty;
            string storeOpenTimeConfigSetting = String.Empty;
            foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
            {
                config = String.Format("PosPushStartTime_{0}", region.ToString());
                string configTime = AppSettingsAccessor.GetStringSetting(config);
                LocalTime time = pattern.Parse(configTime).Value;
                PropertyInfo propertyInfo = settings.GetType().GetProperty(config);
                propertyInfo.SetValue(settings, time);

                storeOpenTimeConfigSetting = String.Format("StoreOpenCentralTime_{0}", region.ToString());
                string configOpenTime = AppSettingsAccessor.GetStringSetting(storeOpenTimeConfigSetting);
                LocalTime openTime = pattern.Parse(configOpenTime).Value;
                PropertyInfo openTimePropertyInfo = settings.GetType().GetProperty(storeOpenTimeConfigSetting);
                openTimePropertyInfo.SetValue(settings, openTime);
            }
            return settings;
        }
    }
}