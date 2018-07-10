using Icon.ApiController.Common;
using Icon.ApiController.Controller.Mappers;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.ControllerBuilders
{
    public class ProductControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController()
        {
            ControllerType.Type = "Product";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("ItemQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            producer.OpenConnection();

            IconDbContextFactory iconContextFactory = new IconDbContextFactory();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.Product, producer, iconContextFactory);

            var queueProcessorLogger = new NLogLoggerInstance<ProductQueueProcessor>(instance);
            var serializer = new Serializer<Contracts.items>(
                new NLogLoggerInstance<Serializer<Contracts.items>>(instance),
                emailClient);
            var queueReader = new ProductQueueReader(
                new NLogLoggerInstance<ProductQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueueProduct>(
                    new NLogLoggerInstance<GetMessageQueueQuery<MessageQueueProduct>>(instance), 
                    iconContextFactory),
                new UpdateMessageQueueStatusCommandHandler<MessageQueueProduct>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueProduct>>(instance), 
                    iconContextFactory),
                new ProductSelectionGroupsMapper(new GetProductSelectionGroupsQuery(iconContextFactory)),
                new UomMapper(new NLogLoggerInstance<UomMapper>(instance)),
                settings);
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                iconContextFactory);
            var associateMessageToQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueProduct>(
                new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueueProduct>>(instance),
                iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProduct>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProduct>>(instance), 
                iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueProduct>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueProduct>>(instance), 
                iconContextFactory);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueProduct>(
                    new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueProduct>>(instance),
                    iconContextFactory);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), iconContextFactory);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var productQueueProcessor = new ProductQueueProcessor(
                settings,
                queueProcessorLogger,
                queueReader,
                serializer,
                saveXmlMessageCommandHandler,
                associateMessageToQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                markQueuedEntriesAsInProcessCommandHandler,
                producer,
                monitor);

            return new ApiControllerBase(baseLogger, emailClient, messageHistoryProcessor, productQueueProcessor, producer);
        }
    }
}
