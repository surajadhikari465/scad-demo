using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using Irma.Framework;
using IrmaPriceListenerService.Archive;
using IrmaPriceListenerService.DataAccess;
using IrmaPriceListenerService.Parser;
using IrmaPriceListenerService.Serializer;
using Mammoth.Framework;
using SimpleInjector;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.S3.Settings;
using Wfm.Aws.S3;
using Wfm.Aws.SQS.Settings;
using Wfm.Aws.SQS;
using IrmaPriceListenerService.Service.Parser;
using IrmaPriceListenerService.Listener;
using Wfm.Aws.ExtendedClient.Serializer;

namespace IrmaPriceListenerService
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var serviceSettings = IrmaPriceListenerServiceSettings.CreateFromConfig();
            var listenerSettings = serviceSettings.GetGpmBridgeListenerSettings();
            var emailSettings = EmailClientSettings.CreateFromConfig();

            container.RegisterSingleton(() => serviceSettings);
            container.RegisterSingleton<IDbContextFactory<IrmaContext>, IrmaDbContextFactory>();
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton(() => emailSettings);
            container.RegisterSingleton<IEmailClient, EmailClient>();
            container.RegisterSingleton(() => listenerSettings);
            container.RegisterSingleton<ISQSExtendedClient, SQSExtendedClient>();
            container.RegisterSingleton(() => S3FacadeSettings.CreateSettingsFromConfig());
            container.RegisterSingleton(() => SQSFacadeSettings.CreateSettingsFromConfig());
            container.RegisterSingleton<IS3Facade, S3Facade>();
            container.RegisterSingleton<ISQSFacade, SQSFacade>();
            container.RegisterSingleton<IExtendedClientMessageSerializer, ExtendedClientMessageSerializer>();
            container.RegisterSingleton<ILogger<IrmaPriceListener>, NLogLogger<IrmaPriceListener>>();
            container.RegisterSingleton<IErrorEventPublisher, ErrorEventPublisher>();
            container.RegisterSingleton<IMessageArchiver, MessageArchiver>();
            container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
            container.RegisterSingleton<IMessageParser<MammothPricesType>, MammothPriceParser>();
            container.RegisterSingleton<ISerializer<ErrorMessage>, Serializer<ErrorMessage>>();
            container.RegisterSingleton<IIrmaPriceDAL, IrmaPriceDAL>();
            container.RegisterSingleton<IrmaPriceListener>();

            container.Verify();
            return container;
        }
    }
}
