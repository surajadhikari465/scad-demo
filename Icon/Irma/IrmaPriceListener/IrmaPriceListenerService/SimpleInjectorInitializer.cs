using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Dvs;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Subscriber;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using Irma.Framework;
using IrmaPriceListenerService.Archive;
using IrmaPriceListenerService.DataAccess;
using IrmaPriceListenerService.Listener;
using IrmaPriceListenerService.Parser;
using IrmaPriceListenerService.Serializer;
using Mammoth.Framework;
using SimpleInjector;

namespace IrmaPriceListenerService
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var serviceSettings = IrmaPriceListenerServiceSettings.CreateFromConfig();
            var listenerSettings = serviceSettings.GetDvsListenerSettings();
            var emailSettings = EmailClientSettings.CreateFromConfig();

            container.RegisterSingleton(() => serviceSettings);
            container.RegisterSingleton<IDbContextFactory<IrmaContext>, IrmaDbContextFactory>();
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton(() => emailSettings);
            container.RegisterSingleton<IEmailClient, EmailClient>();
            container.RegisterSingleton(() => listenerSettings);
            container.RegisterSingleton<IDvsSubscriber, DvsSqsSubscriber>();
            container.RegisterSingleton(() => DvsClientUtil.GetS3Client(listenerSettings));
            container.RegisterSingleton(() => DvsClientUtil.GetSqsClient(listenerSettings));

            container.RegisterSingleton<ILogger<IrmaPriceListener>, NLogLogger<IrmaPriceListener>>();
            container.RegisterSingleton<IErrorEventPublisher, ErrorEventPublisher>();
            container.RegisterSingleton<IMessageArchiver, MessageArchiver>();
            container.RegisterSingleton<MessageParserBase<MammothPricesType, MammothPricesType>, MammothPriceParser>();
            container.RegisterSingleton<ISerializer<ErrorMessage>, Serializer<ErrorMessage>>();
            container.RegisterSingleton<IIrmaPriceDAL, IrmaPriceDAL>();
            container.RegisterSingleton<IrmaPriceListener>();

            container.Verify();
            return container;
        }
    }
}
