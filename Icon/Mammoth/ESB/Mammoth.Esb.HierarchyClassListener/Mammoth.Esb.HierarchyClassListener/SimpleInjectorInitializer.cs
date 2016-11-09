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
using Mammoth.Esb.HierarchyClassListener.Commands;
using Mammoth.Esb.HierarchyClassListener.MessageParsers;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using SimpleInjector;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            container.Register<IHierarchyClassService, HierarchyClassService>();
            container.Register<ICommandHandler<AddOrUpdateHierarchyClassesCommand>, AddOrUpdateHierarchyClassesCommandHandler>();
            container.Register<ICommandHandler<AddOrUpdateMerchandiseHierarchyLineageCommand>, AddOrUpdateMerchandiseHierarchyLineageCommandHandler>();
            container.Register<ICommandHandler<AddOrUpdateFinancialHierarchyClassCommand>, AddOrUpdateFinancialHierarchyClassCommandHandler>();
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>));

            return container;
        }
    }
}
