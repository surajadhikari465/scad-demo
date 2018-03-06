using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Decorators;
using Mammoth.Esb.ProductListener.Cache;
using Mammoth.Esb.ProductListener.Managers;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.MessageParsers;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.PrimeAffinity.Library.Commands;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace Mammoth.Esb.ProductListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();

            container.Register<ProductListener>();
            container.Register(() => ProductListenerSettings.Load());
            container.Register<ListenerApplicationSettings>(() => ListenerApplicationSettings.CreateDefaultSettings("Mammoth Product Listener"));
            container.Register<EsbConnectionSettings>(() => EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("ESB"));
            container.Register<IEsbSubscriber>(() => new EsbSubscriber(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("ESB")));
            container.Register<ILogger<ProductListener>, NLogLogger<ProductListener>>();
            container.Register<IMessageParser<List<ItemModel>>, ProductMessageParser>();
            container.Register<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.Register<IHierarchyClassIdMapper, HierarchyClassIdMapper>();
            container.Register<IHierarchyClassCache, HierarchyClassCache>();

            //PrimeAffinity
            container.Register<IPrimeAffinityManager, PrimeAffinityManager>();
            container.Register<IMessageBuilder<PrimeAffinityMessageBuilderParameters>, PrimeAffinityMessageBuilder>();
            container.Register<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>, PrimeAffinityPsgProcessor>();
            container.Register<IEsbConnectionFactory>(() => new EsbConnectionFactory { Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("R10") });
            container.Register<ISerializer<items>, Serializer<items>>();
            container.Register<ILogger<PrimeAffinityPsgProcessor>, NLogLogger<PrimeAffinityPsgProcessor>>();

            //Data Access
            container.RegisterSingleton<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
            container.Register(typeof(ICommandHandler<>), new[] { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(ArchivePrimeAffinityMessageCommandHandler)) }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), new[] { Assembly.GetExecutingAssembly() }, Lifestyle.Singleton);
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>), Lifestyle.Singleton);

            Registration subscriberRegistration = container.GetRegistration(typeof(IEsbSubscriber)).Registration;
            subscriberRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
            Registration dbConnectionRegistration = container.GetRegistration(typeof(IDbConnection)).Registration;
            dbConnectionRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IDbConnection is taken care of by the application.");

            return container;
        }
    }
}
