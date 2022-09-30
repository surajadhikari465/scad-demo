using Esb.Core.Serializer;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Filters;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Services;
using Icon.Shared.DataAccess.Dapper.ConnectionBuilders;
using SimpleInjector;
using System.Configuration;
using Icon.Common;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue.Communication;
using Icon.ActiveMQ.Factory;
using Icon.ActiveMQ;

namespace Icon.Services.ItemPublisher.Application
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            Container container = new Container();

            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.RegisterSingleton<IClientIdManager>(() => {
                var client = new ClientIdManager();
                client.Initialize(AppSettingsAccessor.GetStringSetting("AppName", "ItemPublisher"));
                return client;
            });
            container.Register<IConnectionBuilder>(() => new ConnectionBuilder("Icon"), Lifestyle.Singleton);
            container.Register<IProviderFactory>(() =>
            {
                return new ProviderFactory(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            }, Lifestyle.Singleton);
            container.Register<IMessageQueueItemModelBuilder, MessageQueueItemModelBuilder>();
            container.Register<IItemMapper, ItemMapper>();
            container.Register<IItemPublisherRepository, ItemPublisherRepository>();
            container.Register<IItemPublisherService, ItemPublisherService>();
            container.Register<IMessageQueueClient, MessageQueueClient>();
            container.Register<IMessageQueueService, MessageQueueService>();
            container.Register<IMessageBuilder, MessageBuilder>();
            container.Register<IMessageHeaderBuilder, MessageHeaderBuilder>();
            container.Register<IMessageServiceCache, MessageServiceCache>(Lifestyle.Singleton);
            container.Register<ICacheRepository, CacheRepository>(Lifestyle.Singleton);
            container.Register<ITraitMessageBuilder, TraitMessageBuilder>();
            container.Register<ISystemListBuilder, SystemListBuilder>();
            container.Register<IItemProcessor, ItemProcessor>();
            container.Register<IHierarchyValueParser, HierarchyValueParser>();
            container.Register<IValueFormatter, ValueFormatter>();
            container.Register<IUomMapper, UomMapper>();
            container.Register<IFilter, UKItemFilter>();

            container.Register(() =>
            {
                var settings = new ServiceSettings();
                settings.LoadSettings();
                return settings;
            });
            container.Register<IEsbConnectionFactory>(() =>
            {
                return new EsbConnectionFactory { Settings = EsbConnectionSettings.CreateSettingsFromConfig("QueueName") };
            });
            container.Register<IActiveMQConnectionFactory>(() =>
            {
                return new ActiveMQConnectionFactory(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqQueueName"));
            });
            container.Register<IItemPublisherApplication, ItemPublisherApplication>();
            container.Register<ISerializer<items>, SerializerWithoutEncodingType<items>>();

            return container;
        }
    }
}