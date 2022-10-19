using System;
using Icon.Esb;
using Icon.Esb.Producer;
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
    public class InventoryPurchaseOrderProducerBuilder : IProducerBuilder
    {
        public InventoryProducerBase ComposeProducer(InventoryProducerSettings inventoryProducerSettings)
        {
            ProducerType.Type = "PurchaseOrder";
            string applicationName = "PublishPurchaseOrderService";
            var instance = ProducerType.Instance.ToString();
            
            InventoryProducerSettings settings = InventoryProducerSettings.CreateFromConfig("IRMA", ProducerType.Instance);
            IDbContextFactory<IrmaContext> irmaContextFactory = new IrmaDbContextFactory();
            IDbContextFactory<MammothContext> mammothContextFactory = new MammothContextFactory();
            ISerializer<transferOrders> serializer = new Serializer<transferOrders>();
            ISerializer<EventTypes> instockDequeueSerializer = new Serializer<EventTypes>();
            var producer = new EsbProducer(EsbConnectionSettings.CreateSettingsFromConfig("InventoryTopicName"));
            var activeMqProducer = new ActiveMQProducer(ActiveMQConnectionSettings.CreateSettingsFromConfig("ActiveMqInventoryPurchaseOrderQueueName"));
            var computedClientId = $"{settings.Source}InventoryProducer.Type-{settings.ProducerType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));

            var baseLogger = new InventoryLogger<InventoryProducerBase>(new NLogLoggerInstance<InventoryProducerBase>(instance), irmaContextFactory, settings);
            baseLogger.LogInfo("Running InventoryPurchaseOrderProducerBuilder.ComposeProducer");

            baseLogger.LogInfo("Opening ESB Connection");
            producer.OpenConnection(clientId);
            baseLogger.LogInfo("ESB Connection Opened");

            baseLogger.LogInfo("Opening ActiveMQ Connection");
            activeMqProducer.OpenConnection(clientId);
            baseLogger.LogInfo("ActiveMQ Connection Opened");

            var queueProcessorLogger = new InventoryLogger<InventoryPurchaseOrderQueueProcessor>(
                new NLogLoggerInstance<InventoryPurchaseOrderQueueProcessor>(instance),
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

            var purchaseOrdersDAL = new PurchaseOrdersDAL(irmaContextFactory, settings);
            var transferOrderXmlCanonicalMapper = new PurchaseOrderXmlCanonicalMapper(
                new Serializer<PurchaseOrders>()
            );

            var messagePublisher = new MessagePublisher(activeMqProducer, producer);
            var archiveInventoryEvents = new ArchiveInventoryEvents(irmaContextFactory, settings);
            var errorEventPublisher = new ErrorEventPublisher(mammothContextFactory, instockDequeueSerializer, applicationName);


            var inventoryProducerQueueProcessor = new InventoryPurchaseOrderQueueProcessor(
                settings,
                queueProcessorLogger,
                instockDequeueService,
                purchaseOrdersDAL,
                transferOrderXmlCanonicalMapper,
                messagePublisher,
                archiveInventoryEvents,
                errorEventPublisher
            );

            return new InventoryProducerBase(baseLogger, inventoryProducerQueueProcessor);
        }
    }
}
