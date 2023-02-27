using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb;
using Icon.Logging;
using SimpleInjector;
using MammothR10Price.Listener;
using MammothR10Price.Publish;
using MammothR10Price.Message.Processor;
using MammothR10Price.Service;
using MammothR10Price.Mapper;
using System.Collections.Generic;
using Icon.Esb.Schemas.Mammoth;
using Icon.Esb.Schemas.Wfm.Contracts;
using MammothR10Price.Serializer;
using MammothR10Price.Message.Parser;
using Icon.Esb.Producer;
using Mammoth.Framework;
using MammothR10Price.Message.Archive;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.Serializer;
using Wfm.Aws.S3.Settings;
using Wfm.Aws.S3;
using Wfm.Aws.SQS.Settings;
using Wfm.Aws.SQS;

namespace MammothR10Price
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var serviceSettings = MammothR10PriceServiceSettings.CreateSettingsFromConfig();
            var esbSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("MammothR10PriceEsbProducer");
            var esbProducer = new EsbProducer(esbSettings);
            container.RegisterSingleton<IEsbProducer>(() => esbProducer);
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton<IEmailClient>(() => { return EmailClient.CreateFromConfig(); });
            container.RegisterSingleton<IErrorEventPublisher, ErrorEventPublisher>();
            container.RegisterSingleton(() => S3FacadeSettings.CreateSettingsFromConfig());
            container.RegisterSingleton(() => SQSFacadeSettings.CreateSettingsFromConfig());
            container.RegisterSingleton(() => SQSExtendedClientListenerSettings.CreateSettingsFromConfig());
            container.RegisterSingleton<IS3Facade, S3Facade>();
            container.RegisterSingleton<ISQSFacade, SQSFacade>();
            container.RegisterSingleton<IExtendedClientMessageSerializer, ExtendedClientMessageSerializer>();
            container.RegisterSingleton<ISQSExtendedClient, SQSExtendedClient>();
            container.RegisterSingleton<IMessageProcessor, MammothR10PriceProcessor>();
            container.RegisterSingleton(() => serviceSettings);
            container.RegisterSingleton(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.RegisterSingleton<IMapper<IList<MammothPriceType>, items>, ItemPriceCanonicalMapper>();
            container.RegisterSingleton<ISerializer<items>, Serializer<items>>();
            container.RegisterSingleton<ISerializer<ErrorMessage>, Serializer<ErrorMessage>>();
            container.RegisterSingleton<IMessageParser<MammothPricesType>, MammothPriceParser>();
            container.RegisterSingleton<ISerializer<MammothPricesType>, Serializer<MammothPricesType>>();
            container.RegisterSingleton<ILogger<ItemPriceCanonicalMapper>, NLogLogger<ItemPriceCanonicalMapper>>();
            container.RegisterSingleton<ILogger<MammothR10PriceProcessor>, NLogLogger<MammothR10PriceProcessor>>();
            container.RegisterSingleton<ILogger<MammothR10PriceService>, NLogLogger<MammothR10PriceService>>();
            container.RegisterSingleton<ILogger<MammothR10PriceListener>, NLogLogger<MammothR10PriceListener>>();
            container.RegisterSingleton<ILogger<MessagePublisher>, NLogLogger<MessagePublisher>>();
            container.RegisterSingleton<ILogger<MessageArchiver>, NLogLogger<MessageArchiver>>();
            container.RegisterSingleton<IMessagePublisher, MessagePublisher>();
            container.RegisterSingleton<IMessageArchiver, MessageArchiver>();
            container.RegisterSingleton<IProducerService, MammothR10PriceService>();
            container.RegisterSingleton<ISQSExtendedClientListener, MammothR10PriceListener>();
            container.Verify();
            return container;
        }
    }
}
