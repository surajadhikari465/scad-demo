using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Helpers;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Publish;
using Polly;
using Polly.Retry;
using System;

namespace InventoryProducer.Producer.QueueProcessors
{
    public abstract class QueueProcessor<CanonicalType, InventoryModel> : IQueueProcessor
    {
        protected readonly InventoryProducerSettings settings;
        protected readonly IInventoryLogger<InventoryPurchaseOrderQueueProcessor> inventoryLogger;
        protected readonly IInstockDequeueService instockDequeueService;
        protected readonly RetryPolicy retrypolicy;
        protected readonly IArchiveInventoryEvents archiveInventoryEvents;
        protected readonly IDAL<InventoryModel> dataAccessLayer;
        protected readonly ICanonicalMapper<CanonicalType, InventoryModel> canonicalMapper;
        protected readonly IMessagePublisher messagePublisher;
        protected readonly IErrorEventPublisher errorEventPublisher;

        public QueueProcessor(
            InventoryProducerSettings settings,
            IInventoryLogger<InventoryPurchaseOrderQueueProcessor> inventoryLogger,
            IInstockDequeueService instockDequeueService,
            IArchiveInventoryEvents archiveInventoryEvents,
            IDAL<InventoryModel> dataAccess,
            ICanonicalMapper<CanonicalType, InventoryModel> canonicalMapper,
            IMessagePublisher messagePublisher,
            IErrorEventPublisher errorEventPublisher)
        {
            this.settings = settings;
            this.inventoryLogger = inventoryLogger;
            this.instockDequeueService = instockDequeueService;
            this.archiveInventoryEvents = archiveInventoryEvents;
            this.dataAccessLayer = dataAccess;
            this.canonicalMapper = canonicalMapper;
            this.messagePublisher = messagePublisher;
            this.errorEventPublisher = errorEventPublisher;
            this.retrypolicy = RetryPolicy
                .Handle<Exception>()
                .WaitAndRetry(
                    settings.ServiceMaxRetryCount,
                    retryAttempt => TimeSpan.FromMilliseconds(settings.ServiceMaxRetryDelayInMilliseconds)
                );
        }

        public abstract void ProcessMessageQueue();
    }
}
