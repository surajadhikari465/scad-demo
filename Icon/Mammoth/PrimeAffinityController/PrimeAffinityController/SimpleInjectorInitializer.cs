using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Esb;
using Icon.Esb.Factory;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.Commands;
using Mammoth.PrimeAffinity.Library.Esb;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using Mammoth.PrimeAffinity.Library.Processors;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace PrimeAffinityController
{
    public static class SimpleInjectorInitializer
    {
        public static Container CreateContainer()
        {
            Container container = new Container();

            container.Register<PrimeAffinityController>();
            container.Register(() => PrimeAffinityControllerSettings.Load(), Lifestyle.Singleton);
            container.Register(() => PrimeAffinityPsgProcessorSettings.Load());
            container.Register(() => PrimeAffinityMessageBuilderSettings.Load());
            container.Register<IMessageBuilder<PrimeAffinityMessageBuilderParameters>, PrimeAffinityMessageBuilder>();
            container.Register<IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>, PrimeAffinityPsgProcessor>();
            container.Register<IEsbConnectionFactory>(() => new EsbConnectionFactory { Settings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("R10") });
            container.Register<IEsbConnectionCacheFactory, EsbConnectionCacheFactory>();
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
            container.Register<ISerializer<items>, Serializer<items>>();
            container.Register(typeof(ILogger<>), typeof(NLogLogger<>));
            container.Register(typeof(IQueryHandler<,>), new[] { Assembly.GetExecutingAssembly() });
            container.Register(typeof(ICommandHandler<>), new[] { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(ArchivePrimeAffinityMessageCommandHandler)) });

            Registration registration = container.GetRegistration(typeof(IDbConnection)).Registration;
            registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "DbConnection does not need to be disposed.");

            EsbConnectionCache.EsbConnectionSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("R10");
            EsbConnectionCache.InitializeConnectionFactoryAndConnection();

            return container;
        }
    }
}