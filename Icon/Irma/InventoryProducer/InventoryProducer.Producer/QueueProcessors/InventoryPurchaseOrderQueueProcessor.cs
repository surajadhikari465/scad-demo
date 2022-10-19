using Icon.Esb.Schemas.Wfm.Contracts;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Helpers;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Producer.Publish;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;

namespace InventoryProducer.Producer.QueueProcessors
{
    public class InventoryPurchaseOrderQueueProcessor : IQueueProcessor
    {
        private readonly InventoryProducerSettings settings;
        private readonly IInventoryLogger<InventoryPurchaseOrderQueueProcessor> inventoryLogger;
        private readonly IInstockDequeueService instockDequeueService;
        private readonly RetryPolicy retrypolicy;
        private readonly IArchiveInventoryEvents archiveInventoryEvents;
        private readonly IPurchaseOrdersDAL purchaseOrdersDAL;
        private readonly IPurchaseOrderCanonicalMapper purchaseOrderXmlCanonicalMapper;
        private readonly IMessagePublisher messagePublisher;
        private readonly IErrorEventPublisher errorEventPublisher;

        public InventoryPurchaseOrderQueueProcessor(
            InventoryProducerSettings settings,
            IInventoryLogger<InventoryPurchaseOrderQueueProcessor> inventoryLogger,
            IInstockDequeueService instockDequeueService,
            IPurchaseOrdersDAL purchaseOrdersDAL,
            IPurchaseOrderCanonicalMapper purchaseOrderXmlCanonicalMapper,
            IMessagePublisher messagePublisher,
            IArchiveInventoryEvents archiveInventoryEvents,
            IErrorEventPublisher errorEventPublisher
            )
        {
            this.settings = settings;
            this.inventoryLogger = inventoryLogger;
            this.instockDequeueService = instockDequeueService;
            this.purchaseOrdersDAL = purchaseOrdersDAL;
            this.purchaseOrderXmlCanonicalMapper = purchaseOrderXmlCanonicalMapper;
            this.messagePublisher = messagePublisher;
            this.archiveInventoryEvents = archiveInventoryEvents;
            this.errorEventPublisher = errorEventPublisher;

            this.retrypolicy = RetryPolicy
                .Handle<Exception>()
                .WaitAndRetry(
                    settings.ServiceMaxRetryCount,
                    retryAttempt => TimeSpan.FromMilliseconds(settings.ServiceMaxRetryDelayInMilliseconds)
                );
        }

        public void ProcessMessageQueue()
        {
            inventoryLogger.LogInfo($"Starting {settings.TransactionType} producer.");
            IList<InstockDequeueResult> dequeuedMessages = instockDequeueService.GetDequeuedMessages();

            foreach (InstockDequeueResult dequeuedMessage in dequeuedMessages)
            {
                try
                {
                    this.retrypolicy.Execute(
                        () =>
                        {
                            this.ProcessInstockDequeueEvent(dequeuedMessage);
                        }
                    );
                }
                catch (Exception ex)
                {
                    // this will happen after all retries
                    inventoryLogger.LogError(ex.Message, ex.StackTrace);
                    this.errorEventPublisher.PublishErrorEventToMammoth(dequeuedMessage, ex);
                }
            }
            inventoryLogger.LogInfo($"Ending {settings.TransactionType} producer.");
        }

        private void ProcessInstockDequeueEvent(InstockDequeueResult dequeuedMessage)
        {
            try
            {
                InstockDequeueModel instockDequeueEvent = dequeuedMessage.InstockDequeueModel;
                inventoryLogger.LogInfo($"Processing event{dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber]} with EventTypeCode: {instockDequeueEvent.EventTypeCode}.");

                IList<PurchaseOrdersModel> purchaseOrdersList = purchaseOrdersDAL.GetPurchaseOrders(
                    instockDequeueEvent.EventTypeCode,
                    instockDequeueEvent.KeyID,
                    instockDequeueEvent.SecondaryKeyID
                );
                inventoryLogger.LogInfo($"Retrieved purchase orders Count: {purchaseOrdersList.Count}.");

                if (purchaseOrdersList != null && purchaseOrdersList.Count > 0)
                {
                    inventoryLogger.LogInfo("Transforming DB model to XML model");
                    PurchaseOrders purchaseOrderCanonicalList =
                        this.purchaseOrderXmlCanonicalMapper.TransformToXmlCanonical(
                            purchaseOrdersList,
                            dequeuedMessage
                        );

                    inventoryLogger.LogInfo("Serializing XML model to XML string");
                    string xmlMessage = this.purchaseOrderXmlCanonicalMapper.SerializeToXml(purchaseOrderCanonicalList);

                    Dictionary<string, string> messageHeaders = this.CreateMessageHeaders(dequeuedMessage,
                        purchaseOrdersList[0]);

                    inventoryLogger.LogInfo("Sending message.");
                    messagePublisher.PublishMessage(
                        xmlMessage,
                        messageHeaders,
                        onSuccess: () =>
                        {
                            inventoryLogger.LogInfo($"Message: {messageHeaders["TransactionID"]} is sent.");
                            this.ArchiveInventoryEvent(xmlMessage, purchaseOrdersList[0], dequeuedMessage);
                        },
                        onFailure: (Exception ex) =>
                        {
                            inventoryLogger.LogError(
                                $"Message: {messageHeaders["TransactionID"]} failed due to Exception: {ex.Message}.",
                                ex.StackTrace
                            );
                            this.ArchiveInventoryEvent(xmlMessage, purchaseOrdersList[0], dequeuedMessage, ex);
                        }
                    );
                }
                else
                {
                    inventoryLogger.LogInfo($"Skipping transformation and publish, couldn't find purchase orders.");
                }
            }
            catch (Exception ex)
            {
                inventoryLogger.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        private void ArchiveInventoryEvent(string xmlMessage, PurchaseOrdersModel purchaseOrdersModel, InstockDequeueResult dequeuedMessage, Exception ex = null)
        {
            throw new NotImplementedException();
        }

        private Dictionary<string, string> CreateMessageHeaders(InstockDequeueResult dequeuedMessage, PurchaseOrdersModel purchaseOrdersModel)
        {
            throw new NotImplementedException();
        }
    }
}
