using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;
using Esb.Core.Serializer;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Producer;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Icon.Esb.Schemas.Mammoth.ContractTypes;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.ConnectionBuilders;
using Mammoth.Common.DataAccess.DbProviders;
using Mammoth.Common.DataAccess.Decorators;
using Mammoth.Esb.AttributeListener.Infrastructure.Esb;
using Mammoth.Esb.AttributeListener.MessageParsers;
using SimpleInjector;
using SimpleInjector.Diagnostics;

namespace Mammoth.Esb.AttributeListener
{
    public static class SimpleInjectorInitializer
    {
        public static Container InitializeContainer()
        {
            var container = new Container();

            container.Register<AttributeListener>();
            container.Register(() => ListenerApplicationSettings.CreateDefaultSettings("Mammoth Attribute Listener"));
            container.Register(() => EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("SB1"));
            container.Register<IEsbSubscriber>(() => new Sb1EsbConsumer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("SB1")));
            container.Register<IEsbProducer>(() => new Sb1EsbProducer(EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("SB1Error")));
            container.Register<ISerializer<ErrorMessage>, SerializerWithoutNamepaceAliases<ErrorMessage>>();
            container.Register<ILogger<AttributeListener>, NLogLogger<AttributeListener>>();
            container.Register<IMessageParser<AttributesType>, AttributeMessageParser>();
            container.Register<IEmailClient>(() => EmailClient.CreateFromConfig());

            //Data Access
            container.RegisterSingleton<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Mammoth"].ConnectionString));
            container.Register(typeof(ICommandHandler<>), Assembly.GetExecutingAssembly(), Lifestyle.Singleton);
            container.RegisterSingleton<IConnectionBuilder>(() => new ConnectionBuilder("Mammoth"));
            container.RegisterDecorator(typeof(ICommandHandler<>), typeof(DbProviderCommandHandlerDecorator<>), Lifestyle.Singleton);
            container.RegisterSingleton<IDbProvider, SqlDbProvider>();

            Registration subscriberRegistration = container.GetRegistration(typeof(IEsbSubscriber)).Registration;
            subscriberRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the subscriber is taken care of by the application.");
            Registration producerRegistration = container.GetRegistration(typeof(IEsbProducer)).Registration;
            producerRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing of the producer is taken care of by the application.");
            Registration dbConnectionRegistration = container.GetRegistration(typeof(IDbConnection)).Registration;
            dbConnectionRegistration.SuppressDiagnosticWarning(DiagnosticType.DisposableTransientComponent, "Disposing IDbConnection is taken care of by the application.");

            container.Verify();
            return container;
        }
    }
}
