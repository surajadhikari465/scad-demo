using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Icon.Shared.DataAccess.Dapper.ConnectionBuilders;
using Icon.Shared.DataAccess.Dapper.DbProviders;
using Icon.Shared.DataAccess.Dapper.Decorators;
using Mammoth.Esb.LocaleListener.Commands;
using Mammoth.Esb.LocaleListener.MessageParsers;
using Mammoth.Esb.LocaleListener.Models;
using SimpleInjector;
using System.Collections.Generic;

namespace Mammoth.Esb.LocaleListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();

            container.RegisterSingleton<MammothLocaleListener>();
            container.RegisterSingleton<ListenerApplicationSettings>(() => ListenerApplicationSettings.CreateDefaultSettings("Mammoth Locale Listener"));
            container.RegisterSingleton<EsbConnectionSettings>(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.RegisterSingleton<IEsbSubscriber>(() => new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()));
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton<ILogger<MammothLocaleListener>, NLogLogger<MammothLocaleListener>>();
            container.RegisterSingleton<IMessageParser<List<LocaleModel>>, LocaleMessageParser>();
            container.RegisterSingleton<ICommandHandler<AddOrUpdateLocalesCommand>, AddOrUpdateLocalesCommandHandler>();
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();

            container.RegisterDecorator<ICommandHandler<AddOrUpdateLocalesCommand>, DbProviderCommandHandlerDecorator<AddOrUpdateLocalesCommand>>(Lifestyle.Singleton);

            return container;
        }
    }
}
