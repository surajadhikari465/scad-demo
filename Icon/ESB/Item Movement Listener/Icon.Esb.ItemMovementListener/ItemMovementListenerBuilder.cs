using Icon.Common.Email;
using Icon.Esb.ItemMovement;
using Icon.Esb.ItemMovementListener.Commands;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;

namespace Icon.Esb.ItemMovementListener
{
    public static class ItemMovementListenerBuilder
    {
        public static ItemMovementListener Build()
        {
            var connectionSettings = EsbConnectionSettings.CreateSettingsFromConfig();
            var listenerSettings = ItemMovementListenerSettings.CreateSettings();
            
            ItemMovementListener listener = new ItemMovementListener(
                ListenerApplicationSettings.CreateDefaultSettings<ListenerApplicationSettings>("Item Movement Listener"),
                connectionSettings,
                new EsbSubscriber(connectionSettings),
                EmailClient.CreateFromConfig(),
                new NLogLogger<ItemMovementListener>(),
                new SaveItemMovementTransactionCommandHandler(),
                listenerSettings);

            return listener;
        }
    }
}
