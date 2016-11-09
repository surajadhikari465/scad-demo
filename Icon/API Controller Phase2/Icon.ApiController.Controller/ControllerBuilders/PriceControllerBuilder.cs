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
    public class PriceControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController(IRenewableContext<IconContext> globalContext)
        {
            ControllerType.Type = "Price";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("ItemQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            producer.OpenConnection();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.Price, producer, globalContext);

            var queueProcessorLogger = new NLogLoggerInstance<PriceQueueProcessor>(instance);
            var serializer = new Serializer<Contracts.items>(
                new NLogLoggerInstance<Serializer<Contracts.items>>(instance),
                emailClient);
            var queueReader = new PriceQueueReader(
                new NLogLoggerInstance<PriceQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueuePrice>(
                    new NLogLoggerInstance<GetMessageQueueQuery<MessageQueuePrice>>(instance),
                    globalContext),
                new UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>>(instance), 
                    globalContext));
            var saveToMessageHistoryCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                globalContext);
            var associateMessageToQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueuePrice>(
                new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueuePrice>>(instance),
                globalContext);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueuePrice>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueuePrice>>(instance), 
                globalContext);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(
                new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), 
                globalContext);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueuePrice>>(instance), 
                globalContext);

            var updateInProcessBusinessUnitCommandHandler = new UpdateInProcessBusinessUnitCommandHandler(globalContext);
            var clearBusinessUnitInProcessCommandHandler = new ClearBusinessUnitInProcessCommandHandler(globalContext);
            var getNextAvailableBusinessUnitQueryHandler = new GetNextAvailableBusinessUnitQuery(globalContext);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueuePrice>(
                    new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueuePrice>>(instance),
                    globalContext);

            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), globalContext);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var priceQueueProcessor = new PriceQueueProcessor(
                settings,
                queueProcessorLogger,
                globalContext,
                queueReader,
                serializer,
                saveToMessageHistoryCommandHandler,
                associateMessageToQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                updateInProcessBusinessUnitCommandHandler,
                clearBusinessUnitInProcessCommandHandler,
                getNextAvailableBusinessUnitQueryHandler,
                markQueuedEntriesAsInProcessCommandHandler,
                producer,
                monitor);

            return new ApiControllerBase(baseLogger, emailClient, messageHistoryProcessor, priceQueueProcessor, producer);
        }
    }
}
