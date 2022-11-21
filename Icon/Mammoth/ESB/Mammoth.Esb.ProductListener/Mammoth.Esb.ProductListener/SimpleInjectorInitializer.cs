using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Subscriber;
using Amazon.S3;
using Amazon.SQS;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Decorators;
using Mammoth.Esb.ProductListener.Cache;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.MessageParsers;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Commands;
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
            var listenerSettings = DvsListenerSettings.CreateSettingsFromConfig();

            container.Register<ProductListener>();
            container.RegisterSingleton(() => listenerSettings);
            container.RegisterSingleton(() => DvsClientUtil.GetS3Client(listenerSettings));
            container.RegisterSingleton(() => DvsClientUtil.GetSqsClient(listenerSettings));
            container.Register<IDvsSubscriber, DvsSqsSubscriber>();

            container.Register<ILogger<ProductListener>, NLogLogger<ProductListener>>();
            container.Register<IMessageParser<List<ItemModel>>, ProductMessageParser>();
            container.Register<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.Register<IHierarchyClassIdMapper, HierarchyClassIdMapper>();
            container.Register<IHierarchyClassCache, HierarchyClassCache>();

            //Data Access
            container.RegisterSingleton<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
			container.Register(typeof(ICommandHandler<>), new[] { Assembly.GetExecutingAssembly(), Assembly.GetAssembly(typeof(AddOrUpdateProductsCommandHandler)) }, Lifestyle.Singleton);
            container.Register(typeof(IQueryHandler<,>), new[] { Assembly.GetExecutingAssembly() }, Lifestyle.Singleton);
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterDecorator(typeof(IQueryHandler<,>), typeof(DbProviderQueryHandlerDecorator<,>), Lifestyle.Singleton);

            Registration dbConnectionRegistration = container.GetRegistration(typeof(IDbConnection)).Registration;
            dbConnectionRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IDbConnection is taken care of by the application.");
            Registration amazonS3Registration = container.GetRegistration(typeof(IAmazonS3)).Registration;
            amazonS3Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonS3 is taken care of by the application.");
            Registration amazonSqsRegistration = container.GetRegistration(typeof(IAmazonSQS)).Registration;
            amazonSqsRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonSQS is taken care of by the application.");

            container.Verify();
            return container;
        }
    }
}
