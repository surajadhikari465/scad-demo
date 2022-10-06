using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GPMService.Producer.ESB.Listener.NearRealTime
{
    internal class NearRealTimeMessageListenerBuilder
    {
        public static NearRealTimeMessageListener Build()
        {
            EsbConnectionSettings connectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();
            NearRealTimeMessageListenerSettings listenerSettings = NearRealTimeMessageListenerSettings.CreateSettings();

            NearRealTimeMessageListener listener = new NearRealTimeMessageListener(
                ListenerApplicationSettings.CreateDefaultSettings<ListenerApplicationSettings>("GPM NearRealTimeMessage Listener"),
                connectionSettings,
                new EsbSubscriber(connectionSettings),
                EmailClient.CreateFromConfig(),
                new NLogLogger<NearRealTimeMessageListener>(),
                listenerSettings
                );

            return listener;
        }
    }
}
