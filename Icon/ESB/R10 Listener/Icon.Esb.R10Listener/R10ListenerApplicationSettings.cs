using Icon.Common;
using Icon.Esb.ListenerApplication;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener
{
    public class R10ListenerApplicationSettings : ListenerApplicationSettings, IListenerApplicationSettings
    {
        public int ResendMessageCount { get; set; }
        
        protected override void LoadSubSettings()
        {
            ResendMessageCount = AppSettingsAccessor.GetIntSetting("ResendMessageCount", 1);
            ListenerApplicationName = "R10Listener";
        }
    }
}
