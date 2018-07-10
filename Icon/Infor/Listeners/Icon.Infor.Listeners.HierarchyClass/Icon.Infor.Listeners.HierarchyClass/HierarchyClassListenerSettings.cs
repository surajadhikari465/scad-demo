﻿using Icon.Common;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class HierarchyClassListenerSettings : IHierarchyClassListenerSettings
    {
        public bool EnableNationalClassMessageGeneration { get; set; }
        public bool ValidateSequenceId { get; set; }
        public bool EnableConfirmBods { get; set; }
        public int MaxNumberOfRetries { get; set; }
        public int RetryDelayInMilliseconds { get; set; }

        public static HierarchyClassListenerSettings CreateFromConfig()
        {
            return new HierarchyClassListenerSettings
            {
                EnableNationalClassMessageGeneration = AppSettingsAccessor
                    .GetBoolSetting(nameof(EnableNationalClassMessageGeneration)),
                ValidateSequenceId = AppSettingsAccessor
                    .GetBoolSetting(nameof(ValidateSequenceId)),
                EnableConfirmBods = AppSettingsAccessor
                    .GetBoolSetting(nameof(EnableConfirmBods)),
                MaxNumberOfRetries = AppSettingsAccessor
                    .GetIntSetting(nameof(MaxNumberOfRetries)),
                RetryDelayInMilliseconds = AppSettingsAccessor
                    .GetIntSetting(nameof(RetryDelayInMilliseconds))
            };
        }
    }
}
