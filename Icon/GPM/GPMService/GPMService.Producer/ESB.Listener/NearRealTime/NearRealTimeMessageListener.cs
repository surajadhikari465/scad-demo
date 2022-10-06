using GPMService.Producer.Message.Parser;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using TIBCO.EMS;

namespace GPMService.Producer.ESB.Listener.NearRealTime
{
    internal class NearRealTimeMessageListener : ListenerApplication<NearRealTimeMessageListener, ListenerApplicationSettings>
    {
        private readonly NearRealTimeMessageParser messageParser;
        private readonly NearRealTimeMessageListenerSettings listenerSettings;
        public NearRealTimeMessageListener(
            ListenerApplicationSettings listenerApplicationSettings,
            EsbConnectionSettings esbConnectionSettings,
            IEsbSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<NearRealTimeMessageListener> logger,
            NearRealTimeMessageListenerSettings listenerSettings
            )
            : base(listenerApplicationSettings, esbConnectionSettings, subscriber, emailClient, logger)
        {
            this.listenerSettings = listenerSettings;
            this.messageParser = new NearRealTimeMessageParser();
        }

        public override void HandleMessage(object sender, EsbMessageEventArgs args)
        {
            if ((esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge ||
                esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge))
            {
                args.Message.Acknowledge();
            }
        }
    }
}
