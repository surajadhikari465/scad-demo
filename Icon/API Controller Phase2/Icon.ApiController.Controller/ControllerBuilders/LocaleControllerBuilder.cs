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
    public class LocaleControllerBuilder : IControllerBuilder
    {
        public ApiControllerBase ComposeController()
        {
            ControllerType.Type = "Locale";

            var instance = ControllerType.Instance.ToString();
            var baseLogger = new NLogLoggerInstance<ApiControllerBase>(instance);
            var emailClient = new EmailClient(EmailHelper.BuildEmailClientSettings());
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("LocaleQueueName"));
            var settings = ApiControllerSettings.CreateFromConfig("Icon", ControllerType.Instance);

            producer.OpenConnection();

            IconDbContextFactory iconContextFactory = new IconDbContextFactory();

            var messageHistoryProcessor = BuilderHelpers.BuildMessageHistoryProcessor(instance, MessageTypes.Locale, producer, iconContextFactory);

            var queueProcessorLogger = new NLogLoggerInstance<LocaleQueueProcessor>(instance);
            var serializer = new Serializer<Contracts.LocaleType>(
                new NLogLoggerInstance<Serializer<Contracts.LocaleType>>(instance),
                emailClient);
            var queueReader = new LocaleQueueReader(
                new NLogLoggerInstance<LocaleQueueReader>(instance),
                emailClient,
                new GetMessageQueueQuery<MessageQueueLocale>(new NLogLoggerInstance<GetMessageQueueQuery<MessageQueueLocale>>(instance), iconContextFactory),
                new GetLocaleLineageQuery(iconContextFactory),
                new UpdateMessageQueueStatusCommandHandler<MessageQueueLocale>(
                    new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueLocale>>(instance), 
                    iconContextFactory));
            var saveXmlMessageCommandHandler = new SaveToMessageHistoryCommandHandler(
                new NLogLoggerInstance<SaveToMessageHistoryCommandHandler>(instance),
                iconContextFactory);
            var associateMessageToMessageQueueCommandHandler = new AssociateMessageToQueueCommandHandler<MessageQueueLocale>(
                new NLogLoggerInstance<AssociateMessageToQueueCommandHandler<MessageQueueLocale>>(instance),
                iconContextFactory);
            var setProcessedDateCommandHandler = new UpdateMessageQueueProcessedDateCommandHandler<MessageQueueLocale>(
                new NLogLoggerInstance<UpdateMessageQueueProcessedDateCommandHandler<MessageQueueLocale>>(instance), 
                iconContextFactory);
            var updateMessageHistoryCommandHandler = new UpdateMessageHistoryStatusCommandHandler(new NLogLoggerInstance<UpdateMessageHistoryStatusCommandHandler>(instance), iconContextFactory);
            var updateMessageQueueStatusCommandHandler = new UpdateMessageQueueStatusCommandHandler<MessageQueueLocale>(
                new NLogLoggerInstance<UpdateMessageQueueStatusCommandHandler<MessageQueueLocale>>(instance), 
                iconContextFactory);
            var markQueuedEntriesAsInProcessCommandHandler = new MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueLocale>(new NLogLoggerInstance<MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueLocale>>(instance), iconContextFactory);
            
            var monitorCommandHandler = new SaveMessageProcessorJobSummaryCommandHandler(
                new NLogLoggerInstance<SaveMessageProcessorJobSummaryCommandHandler>(instance), iconContextFactory);
            var monitor = new MessageProcessorMonitor(monitorCommandHandler);

            var localeQueueProcessor = new LocaleQueueProcessor(
                settings,
                queueProcessorLogger,
                queueReader,
                serializer,
                saveXmlMessageCommandHandler,
                associateMessageToMessageQueueCommandHandler,
                setProcessedDateCommandHandler,
                updateMessageHistoryCommandHandler,
                updateMessageQueueStatusCommandHandler,
                markQueuedEntriesAsInProcessCommandHandler,
                producer,
                monitor);

            return new ApiControllerBase(baseLogger, emailClient, messageHistoryProcessor, localeQueueProcessor, producer);
        }
    }
}
