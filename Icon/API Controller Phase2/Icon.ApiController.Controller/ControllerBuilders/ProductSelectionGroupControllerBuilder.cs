using System;
using Icon.ApiController.Common;
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
    public class ProductSelectionGroupControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController()
        {
            ControllerType.Type = "ProductSelectionGroup";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings()); 
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("ProductSelectionGroupQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);
            var computedClientId = $"{settings.Source}ApiController.Type-{settings.ControllerType}.{Environment.MachineName}.{Guid.NewGuid().ToString()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            producer.OpenConnection(clientId);

            IconDbContextFactory iconContextFactory = new IconDbContextFactory();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.ProductSelectionGroup, producer, iconContextFactory);

            var processorLogger = new NLogLoggerInstance<ProductSelectionGroupQueueProcessor>(instance);
            var queueReader = new ProductSelectionGroupQueueReader(new NLogLoggerInstance<ProductSelectionGroupQueueReader>(instance),
                new GetMessageQueueQuery<MessageQueueProductSelectionGroup>(new NLogLoggerInstance<GetMessageQueueQuery<MessageQueueProductSelectionGroup>>(instance), iconContextFactory),
                new UpdateMessageQueueStatusCommandHandler<MessageQueueProductSelectionGroup>(new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueProductSelectionGroup>>(instance), iconContextFactory));
            var serializer = new Serializer<Contracts.SelectionGroupsType>(
                new NLogLoggerInstance<Serializer<Contracts.SelectionGroupsType>>(instance),
                emailClient);
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance), iconContextFactory);
            var associateMessageToQueueBulkCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueProductSelectionGroup>(new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueueProductSelectionGroup>>(instance), iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProductSelectionGroup>(new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueProductSelectionGroup>>(instance), iconContextFactory);
            var updateMessageHistoryStatusCommandHandler = new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueProductSelectionGroup>(new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueProductSelectionGroup>>(instance), iconContextFactory);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueProductSelectionGroup>(new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueProductSelectionGroup>>(instance), iconContextFactory);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), iconContextFactory);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var psgQueueProcessor = new ProductSelectionGroupQueueProcessor(
                settings,
                processorLogger,
                queueReader,
                serializer,
                saveToMessageHistoryCommandHandler,
                associateMessageToQueueBulkCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryStatusCommandHandler,
                updateMessageQueueStatusCommandHandler,
                markQueuedEntriesAsInProcessCommandHandler,
                producer,
                monitor);

            return new ApiControllerBase(
                new NLogLoggerInstance<ApiControllerBase>(ControllerType.Instance.ToString()), 
                emailClient,
                messageHistoryProcessor, 
                psgQueueProcessor, 
                producer);
        }
    }
}
