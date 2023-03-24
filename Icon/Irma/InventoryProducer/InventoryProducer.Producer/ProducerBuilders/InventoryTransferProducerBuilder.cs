using System;
using Icon.ActiveMQ;
using Icon.ActiveMQ.Producer;
using Icon.Logging;
using Irma.Framework;
using InventoryProducer.Common;
using InventoryProducer.Producer.QueueProcessors;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Helpers;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Publish;
using InventoryProducer.Common.InstockDequeue;
using Icon.DbContextFactory;
using Mammoth.Framework;
using InventoryProducer.Common.Serializers;
using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Common.InstockDequeue.Schemas;

namespace InventoryProducer.Producer.ProducerBuilders
{
    public class InventoryTransferProducerBuilder : IProducerBuilder
    {
        public InventoryProducerBase ComposeProducer(InventoryProducerSettings inventoryProducerSettings)
        {
            ProducerType.Type = "InventoryTransfer";
            string applicationName = "PublishTransferOrderService";
            var instance = ProducerType.Instance.ToString();
            InventoryProducerSettings settings = InventoryProducerSettings.CreateFromConfig("IRMA", ProducerType.Instance);
            IDbContextFactory<IrmaContext> irmaContextFactory = new IrmaDbContextFactory();
            IDbContextFactory<MammothContext> mammothContextFactory = new MammothContextFactory();
            ISerializer<transferOrders> serializer = new Serializer<transferOrders>();
            ISerializer<EventTypes> instockDequeueSerializer = new Serializer<EventTypes>();
            var activeMqProducer = new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqInventoryTransferQueueName"));
            var computedClientId = $"{settings.Source}InventoryProducer.Type-{settings.ProducerType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            var baseLogger = new InventoryLogger<InventoryProducerBase>(new NLogLoggerInstance<InventoryProducerBase>(instance), irmaContextFactory, settings);
            baseLogger.LogInfo("Running InventoryTransferProducerBuilder.ComposeProducer");

            baseLogger.LogInfo("Opening ActiveMQ Connection");
            activeMqProducer.OpenConnection(clientId);
            baseLogger.LogInfo("ActiveMQ Connection Opened");

            var queueProcessorLogger = new InventoryLogger<InventoryTransferQueueProcessor>(
                new NLogLoggerInstance<InventoryTransferQueueProcessor>(instance),
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

            var transferOrdersDal = new TransferOrdersDAL(irmaContextFactory, settings);
            var transferOrderXmlCanonicalMapper = new TransferOrderXmlCanonicalMapper(
                new Serializer<transferOrders>()
            );

            var messagePublisher = new MessagePublisher(activeMqProducer);
            var archiveInventoryEvents = new ArchiveInventoryEvents(irmaContextFactory, settings);
            var errorEventPublisher = new ErrorEventPublisher(mammothContextFactory, instockDequeueSerializer, applicationName);
            
            
            var inventoryTransferQueueProcessor = new InventoryTransferQueueProcessor(
                settings,
                queueProcessorLogger,
                instockDequeueService,
                transferOrdersDal,
                transferOrderXmlCanonicalMapper,
                messagePublisher,
                archiveInventoryEvents,
                errorEventPublisher
            );

            return new InventoryProducerBase(baseLogger, inventoryTransferQueueProcessor);
        }
    }
}
