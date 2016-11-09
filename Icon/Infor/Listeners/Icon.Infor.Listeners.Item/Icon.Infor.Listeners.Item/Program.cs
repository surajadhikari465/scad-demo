using Icon.Common;
using Icon.Common.Context;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.MessageParsers;
using Icon.Infor.Listeners.Item.Notifiers;
using Icon.Infor.Listeners.Item.Services;
using Icon.Infor.Listeners.Item.Validators;
using Icon.Logging;
using Topshelf;

namespace Icon.Infor.Listeners.Item
{
    class Program
    {
        static void Main(string[] args)
        {
            GlobalContext<IconContext> globalContext = new GlobalContext<IconContext>();

            HostFactory.Run(r =>
            {
                r.Service<IListenerApplication>(s =>
                {
                    s.ConstructUsing(c => new ItemListener(
                        new ItemMessageParser(new NLogLogger<ItemMessageParser>()),
                        new ItemModelValidator(new ValidateItemsCommandHandler(globalContext)),
                        globalContext,
                        new ItemService(
                                new ItemAddOrUpdateCommandHandler(globalContext),
                                new GenerateItemMessagesCommandHandler(globalContext),
                                new ArchiveItemsCommandHandler(),
                                new ArchiveMessageCommandHandler(globalContext)
                            ),
                            ListenerApplicationSettings.CreateDefaultSettings("Infor Item Listener"),
                            EsbConnectionSettings.CreateSettingsFromConfig(),
                            new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()),
                            EmailClient.CreateFromConfig(),
                            new ItemListenerNotifier(EmailClient.CreateFromConfig()),
                            new NLogLogger<ItemListener>())
                        );
                    s.WhenStarted(cm => cm.Run());
                    s.WhenStopped(cm => cm.Close());
                });
                r.SetServiceName(AppSettingsAccessor.GetStringSetting("ServiceName"));
                r.SetDisplayName(AppSettingsAccessor.GetStringSetting("ServiceDisplayName"));
                r.SetDescription(AppSettingsAccessor.GetStringSetting("ServiceDescription"));
            });
        }
    }
}
