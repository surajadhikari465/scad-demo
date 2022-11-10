using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.DbContextFactory;
using Icon.Esb;
using Icon.Esb.Producer;
using Icon.Logging;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue;
using InventoryProducer.Common.InstockDequeue.Schemas;
using InventoryProducer.Common.Serializers;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Helpers;
using InventoryProducer.Producer.Publish;
using InventoryProducer.Producer.QueueProcessors;
using Irma.Framework;
using Mammoth.Framework;
using System;

namespace InventoryProducer.Producer.ProducerBuilders
{
    public abstract class ProducerBuilder<CanonicalType, InventoryModel, QueueProcessor> : IProducerBuilder
        where QueueProcessor: class
    {
        protected abstract string ActiveMQQueueConfigName { get; }
        protected abstract string ApplicationName { get; }
        public InventoryProducerBase ComposeProducer(InventoryProducerSettings settings)
        {
            ProducerType.Type = settings.ProducerType;
            var instance = ProducerType.Instance.ToString();

            IDbContextFactory<IrmaContext> irmaContextFactory = new IrmaDbContextFactory();
            IDbContextFactory<MammothContext> mammothContextFactory = new MammothContextFactory();
            ISerializer<CanonicalType> serializer = new Serializer<CanonicalType>();
            ISerializer<EventTypes> instockDequeueSerializer = new Serializer<EventTypes>();
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("InventoryTopicName"));
            var activeMqProducer = new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig(this.ActiveMQQueueConfigName));
            var computedClientId = $"{settings.Source}InventoryProducer.Type-{settings.ProducerType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            var logger = new InventoryLogger<InventoryProducerBase>(new NLogLoggerInstance<InventoryProducerBase>(instance), irmaContextFactory, settings);
            logger.LogInfo($"Running {ApplicationName}.ComposeProducer");

            logger.LogInfo("Opening ESB Connection");
            producer.OpenConnection(clientId);
            logger.LogInfo("ESB Connection Opened");

            logger.LogInfo("Opening ActiveMQ Connection");
            activeMqProducer.OpenConnection(clientId);
            logger.LogInfo("ActiveMQ Connection Opened");

            var queueProcessorLogger = new InventoryLogger<QueueProcessor<CanonicalType, InventoryModel>>(
                new NLogLoggerInstance<QueueProcessor<CanonicalType, InventoryModel>>(instance),
                irmaContextFactory,
                settings
                );
            var dequeueEvents = new DequeueEvents(irmaContextFactory, settings);
            var instockDequeueService = new InstockDequeueService(
                settings,
                irmaContextFactory,
                mammothContextFactory,
                dequeueEvents,
                instockDequeueSerializer,
                new InventoryLogger<InstockDequeueService>(
                    new NLogLoggerInstance<InstockDequeueService>(instance),
                    irmaContextFactory,
                    settings
                    )
                );

            var messagePublisher = new MessagePublisher(activeMqProducer, producer);
            var archiveInventoryEvents = new ArchiveInventoryEvents(irmaContextFactory, settings);
            var errorEventPublisher = new ErrorEventPublisher(mammothContextFactory, instockDequeueSerializer, ApplicationName);

            var inventoryProducerQueueProcessor = CreateQueueProcessor(
                settings,
                queueProcessorLogger,
                instockDequeueService,
                messagePublisher,
                archiveInventoryEvents,
                errorEventPublisher,
                irmaContextFactory,
                serializer
                );

            return new InventoryProducerBase(logger, inventoryProducerQueueProcessor);
        }
        public abstract IQueueProcessor CreateQueueProcessor(
            InventoryProducerSettings settings,
            InventoryLogger<QueueProcessor<CanonicalType, InventoryModel>> logger,
            InstockDequeueService instockDequeueService,
            MessagePublisher messagePublisher,
            ArchiveInventoryEvents archiveInventoryEvents,
            ErrorEventPublisher errorEventPublisher,
            IDbContextFactory<IrmaContext> irmaContext,
            ISerializer<CanonicalType> serializer
        );
    }
}
