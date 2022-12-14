using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.Common.Email;
using Icon.DbContextFactory;
using Icon.Esb;
using Icon.Esb.MessageParsers;
using Icon.Esb.ListenerApplication;
using Icon.Logging;
using SimpleInjector;
using MammothR10Price.Esb.Listener;
using MammothR10Price.Publish;
using MammothR10Price.Message.Processor;
using MammothR10Price.Service;
using MammothR10Price.Mapper;
using System.Collections.Generic;
using Icon.Esb.Schemas.Mammoth;
using Icon.Esb.Schemas.Wfm.Contracts;
using MammothR10Price.Serializer;
using MammothR10Price.Esb.Subscriber;
using MammothR10Price.Message.Parser;
using Icon.Esb.Producer;
using Icon.Esb.Subscriber;
using Mammoth.Framework;

namespace MammothR10Price
{
    internal class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var serviceSettings = MammothR10PriceServiceSettings.CreateSettingsFromConfig();
            var sb1Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("MammothR10PriceSb1Queue");
            var esbSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("MammothR10PriceEsbQueue");

            var sb1Subscriber = new Sb1EsbConsumer(sb1Settings);
            var esbProducer = new EsbProducer(esbSettings);

            container.RegisterSingleton<IEsbSubscriber>(() => sb1Subscriber);
            container.RegisterSingleton<IEsbProducer>(() => esbProducer);

            container.RegisterSingleton(() => ActiveMQConnectionSettings.CreateSettingsFromConfig());
            container.RegisterSingleton<IActiveMQProducer, ActiveMQProducer>();
            container.RegisterSingleton<IDbContextFactory<MammothContext>, MammothContextFactory>();
            container.RegisterSingleton<IEmailClient>(() => { return EmailClient.CreateFromConfig(); });
            container.RegisterSingleton<IErrorEventPublisher, ErrorEventPublisher>();
            container.RegisterSingleton<IMessageProcessor, MammothR10PriceProcessor>();
            container.RegisterSingleton<MammothR10PriceListener>();
            container.RegisterSingleton(() => serviceSettings);
            container.RegisterSingleton(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.RegisterSingleton(() => ListenerApplicationSettings.CreateDefaultSettings(serviceSettings.ApplicationName));
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
            container.RegisterSingleton<IMessagePublisher, MessagePublisher>();
            container.RegisterSingleton<IProducerService, MammothR10PriceService>();
            container.RegisterSingleton<IListenerApplication, MammothR10PriceListener>();
            
            return container;
        }
    }
}
