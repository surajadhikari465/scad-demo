using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GlobalEventController.Common
{
    public class GlobalControllerSettings : IGlobalControllerSettings
    {
        public bool SendRetailUomChangeEmailAlerts { get; set; }
        public string[] GlobalEvents { get; set; }
        public int DbContextConnectionTimeout { get; set; }
        public int MaxQueueEntriesToProcess { get; set; }
        public string EmailSubjectEnvironment { get; set; }
        public bool EnableInforUpdates { get; set; }

        public static GlobalControllerSettings CreateFromConfig()
        {
            GlobalControllerSettings settings = new GlobalControllerSettings();
            settings.GlobalEvents = AppSettingsAccessor.GetStringSetting("GlobalEvents")?.Split(',');
            settings.MaxQueueEntriesToProcess = AppSettingsAccessor.GetIntSetting("MaxQueueEntriesToProcess");
            settings.DbContextConnectionTimeout = AppSettingsAccessor.GetIntSetting("DbContextConnectionTimeout");
            settings.EmailSubjectEnvironment = AppSettingsAccessor.GetStringSetting("EmailSubjectEnvironment");
            settings.EnableInforUpdates = AppSettingsAccessor.GetBoolSetting("EnableInforUpdates", false);
            return settings;
        }

        public static GlobalControllerSettings CreateFromConfig(string region)
        {
            GlobalControllerSettings settings = new GlobalControllerSettings();
            settings.GlobalEvents = AppSettingsAccessor.GetStringSetting("GlobalEvents")?.Split(',');
            settings.MaxQueueEntriesToProcess = AppSettingsAccessor.GetIntSetting("MaxQueueEntriesToProcess");
            settings.DbContextConnectionTimeout = AppSettingsAccessor.GetIntSetting("DbContextConnectionTimeout");
            settings.SendRetailUomChangeEmailAlerts = AppSettingsAccessor.GetBoolSetting(String.Format("SendUomChangeEmails_{0}", region));
            settings.EmailSubjectEnvironment = AppSettingsAccessor.GetStringSetting("EmailSubjectEnvironment");
            return settings;
        }
    }
}
