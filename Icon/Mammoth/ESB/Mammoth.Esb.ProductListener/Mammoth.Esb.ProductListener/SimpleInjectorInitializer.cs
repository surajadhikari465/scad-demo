using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Decorators;
using Mammoth.Esb.ProductListener.Cache;
using Mammoth.Esb.ProductListener.Commands;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.MessageParsers;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Queries;
using SimpleInjector;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();

            container.RegisterSingleton<ProductListener>();
            container.RegisterSingleton<ListenerApplicationSettings>(() => ListenerApplicationSettings.CreateDefaultSettings("Mammoth Product Listener"));
            container.RegisterSingleton<EsbConnectionSettings>(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.RegisterSingleton<IEsbSubscriber>(() => new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromConfig()));
            container.RegisterSingleton<ILogger<ProductListener>, NLogLogger<ProductListener>>();
            container.RegisterSingleton<IMessageParser<List<ProductModel>>, ProductMessageParser>();
            container.RegisterSingleton<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.RegisterSingleton<ICommandHandler<AddOrUpdateProductsCommand>, AddOrUpdateProductsCommandHandler>();
            container.RegisterSingleton<IQueryHandler<GetHierarchyClassesParameters, List<HierarchyClassModel>>, GetHierarchyClassesQueryHandler>();
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IHierarchyClassIdMapper, HierarchyClassIdMapper>();
            container.RegisterSingleton<IHierarchyClassCache, HierarchyClassCache>();
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>), Lifestyle.Singleton);

            return container;
        }
    }
}
