using AttributePublisher.DataAccess.Models;
using AttributePublisher.Infrastructure;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.MessageBuilders;
using AttributePublisher.Operations;
using AttributePublisher.Operations.Decorators;
using AttributePublisher.Services;
using Esb.Core.MessageBuilders;
using Esb.Core.Serializer;
using Icon.Common.DataAccess;
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Schemas.Attributes.ContractTypes;
using Icon.Logging;
using SimpleInjector;
using SimpleInjector.Diagnostics;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Reflection;

namespace AttributePublisher
{
    public static class SimpleInjectorInitializer
    {
        public static Container Initialize()
        {
            Container container = new Container(); 

            container.Register<AttributePublisherService>();
            container.Register<RecurringServiceSettings>(() => RecurringServiceSettings.Load());
            container.Register<AttributePublisherServiceSettings>(() => AttributePublisherServiceSettings.Load());
            container.Register<AttributePublisherServiceParameters>(Lifestyle.Singleton);

            container.RegisterConditional<IOperation<AttributePublisherServiceParameters>, GetAttributesOperation>(
                c => c.Consumer.ImplementationType == typeof(AttributePublisherService));
            container.RegisterConditional<IOperation<AttributePublisherServiceParameters>, BuildMessageOperation>(
                c => c.Consumer.ImplementationType == typeof(GetAttributesOperation));
            container.RegisterConditional<IOperation<AttributePublisherServiceParameters>, SendAttributesToConsumerOperation>(
                c => c.Consumer.ImplementationType == typeof(BuildMessageOperation));
            container.RegisterConditional<IOperation<AttributePublisherServiceParameters>, ArchiveAttributeMessagesOperation>(
                c => c.Consumer.ImplementationType == typeof(SendAttributesToConsumerOperation));
            container.RegisterConditional<IOperation<AttributePublisherServiceParameters>, ClearAttributePublisherServiceParametersOperation>(
                c => c.Consumer.ImplementationType == typeof(ArchiveAttributeMessagesOperation));

            container.Register<ActiveMQConnectionSettings>(() => ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqAttributeQueueName"), Lifestyle.Singleton);
            container.Register<IDbConnection>(() => new SqlConnection(ConfigurationManager.ConnectionStrings["Icon"].ConnectionString));
            container.Register<IActiveMQProducer, ActiveMQProducer>(Lifestyle.Singleton);
            container.Register<ILogger>(() => new NLogLogger(typeof(AttributePublisherService)), Lifestyle.Transient);
            container.Register<IMessageBuilder<List<AttributeModel>>, AttributeMessageBuilder>();
            container.Register<IMessageHeaderBuilder, AttributeMessageHeaderBuilder>();
            container.Register<ISerializer<AttributesType>, SerializerWithoutNamepaceAliases<AttributesType>>();

            container.RegisterDecorator(typeof(IOperation<AttributePublisherServiceParameters>), typeof(ManageConsumerConnectionOperationDecorator),
                c => c.ImplementationType == typeof(SendAttributesToConsumerOperation));
            container.RegisterDecorator<IOperation<AttributePublisherServiceParameters>, ErrorHandlingOperationDecorator>();
            container.RegisterDecorator<IOperation<AttributePublisherServiceParameters>, CheckContinueProcessingOperationDecorator>();

            container.Register(typeof(ICommandHandler<>), new[] { Assembly.Load("AttributePublisher.DataAccess") });
            container.Register(typeof(IQueryHandler<,>), new[] { Assembly.Load("AttributePublisher.DataAccess") });

            container.GetRegistration(typeof(IDbConnection)).Registration
                .SuppressDiagnosticWarning(
                    DiagnosticType.DisposableTransientComponent,
                    "Reason of suppression");

            container.Verify();

            return container;
        }
    }
}
