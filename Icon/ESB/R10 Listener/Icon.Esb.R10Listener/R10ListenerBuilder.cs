using Icon.Common.Email;
using Icon.Esb.Factory;
using Icon.Esb.Subscriber;
using Icon.Esb.ListenerApplication;
using Icon.Esb.R10Listener.Commands;
using Icon.Logging;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.R10Listener.Context;
using Icon.Framework;
using Icon.Esb.R10Listener.Infrastructure.Cache;

namespace Icon.Esb.R10Listener
{
    public static class R10ListenerBuilder
    {
        public static R10Listener Build()
        {
            var applicationSettings = R10ListenerApplicationSettings.CreateDefaultSettings<R10ListenerApplicationSettings>("R10 Listener");
            var connectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();
            var globalContext = new GlobalContext(new IconContext());

            R10Listener listener = new R10Listener(
                applicationSettings,
                connectionSettings,
                new EsbSubscriber(connectionSettings),
                new ProcessR10MessageResponseCommandHandler(
                    globalContext,
                    new AddMessageResponseCommandHandler(globalContext),
                    new ProcessFailedR10MessageResponseCommandHandler(
                        globalContext,
                        new ResendMessageQueueEntriesCommandHandler(globalContext, new MessageQueueResendStatusCache(), applicationSettings),
                        new ResendMessageCommandHandler(globalContext, applicationSettings)
                    )
                ),
                new R10MessageResponseParser(),
                EmailClient.CreateFromConfig(),
                new NLogLogger<R10Listener>());

            return listener;
        }
    }
}
