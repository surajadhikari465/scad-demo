using Vim.Common;

namespace Vim.Locale.Controller
{
    public class ControllerApplicationSettings
    {
        public int Instance { get; set; }
        public int MaxNumberOfRowsToMark { get; set; }
        public int FirstAttemptWaitTimeInMinute { get; set; }
        public int SecondAttemptWaitTimeInMinute { get; set; }
        public int ThirdAttemptWaitTimeInMinute { get; set; }

        public static ControllerApplicationSettings CreateFromConfig(int instance)
        {
            return new ControllerApplicationSettings
            {
                Instance = instance,
                MaxNumberOfRowsToMark = AppSettingsAccessor.GetIntSetting("MaxNumberOfRowsToMark"),
                FirstAttemptWaitTimeInMinute = AppSettingsAccessor.GetIntSetting("FirstAttemptWaitTimeInMinute"),
                SecondAttemptWaitTimeInMinute = AppSettingsAccessor.GetIntSetting("SecondAttemptWaitTimeInMinute"),
                ThirdAttemptWaitTimeInMinute = AppSettingsAccessor.GetIntSetting("ThirdAttemptWaitTimeInMinute"),
            };
        }
    }
}
