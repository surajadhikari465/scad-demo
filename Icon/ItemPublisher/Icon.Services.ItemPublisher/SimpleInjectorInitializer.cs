using Esb.Core.Serializer;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Esb;
using Icon.Services.ItemPublisher.Infrastructure.Models.Builders;
using Icon.Services.ItemPublisher.Infrastructure.Models.Mappers;
using Icon.Services.ItemPublisher.Infrastructure.Repositories;
using Icon.Services.ItemPublisher.Services;
using Icon.Shared.DataAccess.Dapper.ConnectionBuilders;
using SimpleInjector;
using System.Configuration;
using Icon.Services.ItemPublisher.Infrastructure.Esb.Communication;

namespace Icon.Services.ItemPublisher.Application
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            Container container = new Container();

            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register<IConnectionBuilder>(() => new ConnectionBuilder("Icon"), Lifestyle.Singleton);
            container.Register<IProviderFactory>(() =>
            {
                return new ProviderFactory(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString);
            }, Lifestyle.Singleton);
            container.Register<IMessageQueueItemModelBuilder, MessageQueueItemModelBuilder>();
            container.Register<IItemMapper, ItemMapper>();
            container.Register<IItemPublisherRepository, ItemPublisherRepository>();
            container.Register<IItemPublisherService, ItemPublisherService>();
            container.Register<IEsbClient, EsbClient>();
            container.Register<IEsbService, EsbService>();
            container.Register<IEsbMessageBuilder, EsbMessageBuilder>();
            container.Register<IEsbHeaderBuilder, EsbHeaderBuilder>();
            container.Register<IEsbServiceCache, EsbServiceCache>(Lifestyle.Singleton);
            container.Register<ICacheRepository, CacheRepository>(Lifestyle.Singleton);
            container.Register<ITraitMessageBuilder, TraitMessageBuilder>();
            container.Register<ISystemListBuilder, SystemListBuilder>();
            container.Register<IItemProcessor, ItemProcessor>();
            container.Register<IHierarchyValueParser, HierarchyValueParser>();
            container.Register<IValueFormatter, ValueFormatter>();
            container.Register<IUomMapper, UomMapper>();

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
            container.Register<IItemPublisherApplication, ItemPublisherApplication>();
            container.Register<ISerializer<items>, SerializerWithoutEncodingType<items>>();

            return container;
        }
    }
}