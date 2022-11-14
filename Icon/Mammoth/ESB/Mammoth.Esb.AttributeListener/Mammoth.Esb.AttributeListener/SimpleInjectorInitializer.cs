using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Esb.Core.Serializer;
using Icon.Common.Email;
using Icon.Dvs;
using Icon.Dvs.MessageParser;
using Icon.Dvs.Subscriber;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Icon.Esb.Schemas.Mammoth.ContractTypes;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Decorators;
using Mammoth.Esb.AttributeListener.MessageParsers;
using Mammoth.Esb.AttributeListener.Commands;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using Amazon.S3;
using Amazon.SQS;

namespace Mammoth.Esb.AttributeListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();
            var listenerSettings = DvsListenerSettings.CreateSettingsFromConfig();

            container.Register<AttributeListener>();

            container.Register(() => listenerSettings);
            container.Register(() => DvsClientUtil.GetS3Client(listenerSettings));
            container.Register(() => DvsClientUtil.GetSqsClient(listenerSettings));
            container.Register<IDvsSubscriber, DvsSqsSubscriber>();

            container.Register<ISerializer<ErrorMessage>, SerializerWithoutNamepaceAliases<ErrorMessage>>();
            container.Register<ILogger<AttributeListener>, NLogLogger<AttributeListener>>();
            container.Register<IMessageParser<AttributesType>, AttributeMessageParser>();
            container.Register<IEmailClient>(() => EmailClient.CreateFromConfig());
            container.Register<ErrorMessageHandler>();

            //Data Access
            container.RegisterSingleton<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
            container.Register(typeof(ICommandHandler<>), Assembly.GetExecutingAssembly(), Lifestyle.Singleton);
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();

            Registration amazonS3Registration = container.GetRegistration(typeof(IAmazonS3)).Registration;
            amazonS3Registration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonS3 is taken care of by the application.");
            Registration amazonSqsRegistration = container.GetRegistration(typeof(IAmazonSQS)).Registration;
            amazonSqsRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IAmazonSQS is taken care of by the application.");
            Registration dbConnectionRegistration = container.GetRegistration(typeof(IDbConnection)).Registration;
            dbConnectionRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IDbConnection is taken care of by the application.");

            container.Verify();
            return container;
        }
    }
}
