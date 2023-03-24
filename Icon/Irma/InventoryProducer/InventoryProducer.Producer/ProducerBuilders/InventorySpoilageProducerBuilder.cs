using System;
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Irma.Framework;
using InventoryProducer.Common;
using InventoryProducer.Producer.QueueProcessors;
using InventoryProducer.Common.InstockDequeue;
using Icon.DbContextFactory;
using Mammoth.Framework;
using InventoryProducer.Common.Serializers;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Common.InstockDequeue.Schemas;

namespace InventoryProducer.Producer.ProducerBuilders
{
    public class InventorySpoilageProducerBuilder : IProducerBuilder
    {
        public InventoryProducerBase ComposeProducer(InventoryProducerSettings inventoryProducerSettings)
        {
            ProducerType.Type = "InventorySpoilage";
            var instance = ProducerType.Instance.ToString();
            InventoryProducerSettings settings = InventoryProducerSettings.CreateFromConfig("IRMA", ProducerType.Instance);
            IDbContextFactory<IrmaContext> irmaContextFactory = new IrmaDbContextFactory();
            IDbContextFactory<MammothContext> mammothContextFactory = new MammothContextFactory();
            ISerializer<inventoryAdjustments> serializer = new Serializer<inventoryAdjustments>();
            ISerializer<EventTypes> instockDequeueSerializer = new Serializer<EventTypes>();
            var activeMqProducer = new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqInventorySpoilageQueueName"));
            var computedClientId = $"{settings.Source}InventoryProducer.Type-{settings.ProducerType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            var baseLogger = new InventoryLogger<InventoryProducerBase>(new NLogLoggerInstance<InventoryProducerBase>(instance), irmaContextFactory, settings);
            baseLogger.LogInfo("Running InventorySpoilageProducerBuilder.ComposeProducer");

            baseLogger.LogInfo("Opening ActiveMQ Connection");
            activeMqProducer.OpenConnection(clientId);
            baseLogger.LogInfo("ActiveMQ Connection Opened");

            var queueProcessorLogger = new InventoryLogger<InventorySpoilageQueueProcessor>(
                new NLogLoggerInstance<InventorySpoilageQueueProcessor>(instance), 
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

            var inventorySpoilageQueueProcessor = new InventorySpoilageQueueProcessor(
                irmaContextFactory,
                mammothContextFactory,
                settings,
                queueProcessorLogger,
                instockDequeueService,
                serializer,
                instockDequeueSerializer,
                activeMqProducer);

            return new InventoryProducerBase(baseLogger, inventorySpoilageQueueProcessor);
        }
    }
}
