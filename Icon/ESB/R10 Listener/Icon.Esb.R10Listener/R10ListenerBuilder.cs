using Icon.Common.Email;
using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Infrastructure.DataAccess;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;

namespace Icon.Esb.R10Listener
{
    public static class R10ListenerBuilder
    {
        public static R10Listener Build()
        {
            var applicationSettings = R10ListenerApplicationSettings.CreateDefaultSettings<R10ListenerApplicationSettings>("R10 Listener");
            var connectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();

            R10Listener listener = new R10Listener(
                applicationSettings,
                connectionSettings,
                new EsbSubscriber(connectionSettings),
                new SaveR10MessageResponseCommandHandler(
                    new DbFactory()),
                new R10MessageResponseParser(),
                EmailClient.CreateFromConfig(),
                new NLogLogger<R10Listener>());

            return listener;
        }
    }
}
