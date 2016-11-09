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
        public bool SendPagerDutyNotifications { get; set; }
        public string PagerDutyUri { get; set; }
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

        public Dictionary<string, TimeSpan> MonitorTimers { get; set; }

        public static MonitorSettings CreateFromConfig()
        {
            MonitorSettings settings = new MonitorSettings();

            settings.IntegrationKey = AppSettingsAccessor.GetStringSetting("IntegrationKey");
            settings.SendPagerDutyNotifications = AppSettingsAccessor.GetBoolSetting("SendPagerDutyNotifications");
            settings.PagerDutyUri = AppSettingsAccessor.GetStringSetting("PagerDutyUri");
            settings.Environment = AppSettingsAccessor.GetStringSetting("Environment");
            settings.MonitorServiceTimer = AppSettingsAccessor.GetDoubleSetting("MonitorServiceTimer");

            // All monitor settings in app.config need to be <ControllerMonitorName>Timer.
            settings.MonitorTimers = ConfigurationManager.AppSettings.AllKeys.Where(k => k.EndsWith("Timer"))
                .Select(k => new { Key = k, Value = ConfigurationManager.AppSettings[k] })
                .ToDictionary(x => x.Key, x => TimeSpan.FromMilliseconds(int.Parse(x.Value)));

            var pattern = LocalTimePattern.CreateWithInvariantCulture("HH:mm:ss");
            string config = String.Empty;
            foreach (IrmaRegions region in Enum.GetValues(typeof(IrmaRegions)))
            {
                config = String.Format("PosPushStartTime_{0}", region.ToString());
                string configTime = AppSettingsAccessor.GetStringSetting(config);
                LocalTime time = pattern.Parse(configTime).Value;
                PropertyInfo propertyInfo = settings.GetType().GetProperty(config);
                propertyInfo.SetValue(settings, time);
            }

            return settings;
        }
    }
}
