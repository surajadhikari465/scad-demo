﻿using Icon.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass
{
    public class HierarchyClassListenerSettings : IHierarchyClassListenerSettings
    {
        public bool EnableNationalClassEventGeneration { get; set; }

        public static HierarchyClassListenerSettings CreateFromConfig()
        {
            return new HierarchyClassListenerSettings
            {
                EnableNationalClassEventGeneration = AppSettingsAccessor
                    .GetBoolSetting(nameof(EnableNationalClassEventGeneration))
            };
        }
    }
}
