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
using InventoryProducer.Producer.Model.DBModel;

namespace InventoryProducer.Producer.ProducerBuilders
{
    public class InventoryPurchaseOrderProducerBuilder :
        ProducerBuilder<PurchaseOrders, PurchaseOrdersModel, InventoryPurchaseOrderQueueProcessor>
    {
        protected override string ActiveMQQueueConfigName { get; } = "ActiveMqInventoryPurchaseOrderQueueName";
        protected override string ApplicationName { get; } = "PublishPurchaseOrderService";

        public override IQueueProcessor CreateQueueProcessor(
            InventoryProducerSettings settings,
            InventoryLogger<InventoryPurchaseOrderQueueProcessor> logger,
            InstockDequeueService instockDequeueService,
            MessagePublisher messagePublisher,
            ArchiveInventoryEvents archiveInventoryEvents,
            ErrorEventPublisher errorEventPublisher,
            IDbContextFactory<IrmaContext> irmaContext,
            ISerializer<PurchaseOrders> serializer)
        {
            var purchaseOrdersDAL = new PurchaseOrdersDAL(irmaContext, settings);
            var purchaseOrderXmlCanonicalMapper = new PurchaseOrderXmlCanonicalMapper(
                serializer
            );
            return new InventoryPurchaseOrderQueueProcessor(
                settings,
                logger,
                instockDequeueService,
                purchaseOrdersDAL,
                purchaseOrderXmlCanonicalMapper,
                messagePublisher,
                archiveInventoryEvents,
                errorEventPublisher
            );
        }
    }
}
