using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Esb.EwicErrorResponseListener.DataAccess.Commands;
using Icon.Esb.EwicErrorResponseListener.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Logging;

namespace Icon.Esb.EwicErrorResponseListener
{
    public class EwicErrorResponseListenerBuilder
    {
        private static IRenewableContext<IconContext> globalContext;

        public static EwicErrorResponseListener Build()
        {
            globalContext = new GlobalIconContext(new IconContext());

            var applicationSettings = EwicErrorResponseListenerApplicationSettings.CreateDefaultSettings<EwicErrorResponseListenerApplicationSettings>("eWIC Error Response Listener");
            var connectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();
            var messageParser = new ErrorResponseMessageParser();
            var saveToMessageResponseCommand = new SaveToMessageResponseCommand(globalContext);
            var updateMessageHistoryStatusCommand = new UpdateMessageHistoryStatusCommand(globalContext);

            var listener = new EwicErrorResponseListener(
                globalContext,
                applicationSettings,
                connectionSettings,
                new EsbSubscriber(connectionSettings),
                EmailClient.CreateFromConfig(),
                new NLogLogger<EwicErrorResponseListener>(),
                messageParser,
                saveToMessageResponseCommand,
                updateMessageHistoryStatusCommand);

            return listener;
        }
    }
}
