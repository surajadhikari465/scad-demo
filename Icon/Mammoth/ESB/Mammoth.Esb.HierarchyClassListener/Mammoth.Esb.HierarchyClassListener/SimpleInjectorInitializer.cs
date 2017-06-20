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
using Mammoth.Common.DataAccess.Models;
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.MessageParsers;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Queries;
using Mammoth.Esb.HierarchyClassListener.Services;
using SimpleInjector;
using System.Collections.Generic;

namespace Mammoth.Esb.HierarchyClassListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container Initialize()
        {
            var container = new Container();

            container.Register<MammothHierarchyClassListener>();
            container.RegisterSingleton(() => ListenerApplicationSettings.CreateDefaultSettings("Mammoth Hierarchy Class Listener"));
            container.RegisterSingleton(() => EsbConnectionSettings.CreateSettingsFromConfig());
            container.RegisterSingleton<IEsbSubscriber, EsbSubscriber>();
            container.RegisterSingleton(() => EmailClientSettings.CreateFromConfig());
            container.RegisterSingleton<IEmailClient, EmailClient>();
            container.RegisterSingleton<ILogger<MammothHierarchyClassListener>, NLogLogger<MammothHierarchyClassListener>>();
            container.RegisterSingleton<IMessageParser<List<HierarchyClassModel>>, HierarchyClassMessageParser>();
            container.Register<IHierarchyClassService<AddOrUpdateHierarchyClassRequest>, AddOrUpdateHierarchyClassService>();
            container.Register<IHierarchyClassService<DeleteBrandRequest>, DeleteBrandService>();
            container.Register<ICommandHandler<AddOrUpdateHierarchyClassesCommand>, AddOrUpdateHierarchyClassesCommandHandler>();
            container.Register<ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand>, AddOrUpdateMerchandiseHierarchyLineageCommandHandler>();
            container.Register<ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand>, AddOrUpdateFinancialHierarchyClassCommandHandler>();
            container.Register<ICommandHandler<DeleteBrandsCommand>, DeleteBrandsCommandHandler>();
            container.Register<IQueryHandler<GetItemsByBrandIdQuery, IEnumerable<Item>>, GetItemsByBrandIdQueryHandler>();
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>));
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>));
            container.RegisterDecorator(typeof(IHierarchyClassService<DeleteBrandRequest>), typeof(ValidateItemAssociationDeleteBrandServiceDecorator));

            container.Verify();
            return container;
        }
    }
}
