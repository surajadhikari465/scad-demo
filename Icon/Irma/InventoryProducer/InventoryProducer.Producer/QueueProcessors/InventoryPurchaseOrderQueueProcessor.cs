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
    public class InventoryPurchaseOrderQueueProcessor : QueueProcessor<PurchaseOrders, PurchaseOrdersModel>
    {
        public InventoryPurchaseOrderQueueProcessor(
            InventoryProducerSettings settings,
            IInventoryLogger<QueueProcessor<PurchaseOrders, PurchaseOrdersModel>> inventoryLogger,
            IInstockDequeueService instockDequeueService,
            IDAL<PurchaseOrdersModel> dataAccessLayer,
            ICanonicalMapper<PurchaseOrders, PurchaseOrdersModel> canonicalMapper,
            IMessagePublisher messagePublisher,
            IArchiveInventoryEvents archiveInventoryEvents,
            IErrorEventPublisher errorEventPublisher
            ) : base(settings, inventoryLogger, instockDequeueService, archiveInventoryEvents, dataAccessLayer, canonicalMapper, messagePublisher, errorEventPublisher) { }

        public override void ProcessMessageQueue()
        {
            inventoryLogger.LogInfo($"Starting {settings.TransactionType} producer.");
            IList<InstockDequeueResult> dequeuedMessages = instockDequeueService.GetDequeuedMessages();
            while (dequeuedMessages != null && dequeuedMessages.Count > 0)
            {
                foreach (InstockDequeueResult dequeuedMessage in dequeuedMessages)
                {
                    List<InstockDequeueModel> instockDequeueModelList = new List<InstockDequeueModel>();
                    InstockDequeueModel instockDequeueModelToValidate = new InstockDequeueModel();
                    try
                    {
                        this.retrypolicy.Execute(
                            () =>
                            {
                                instockDequeueModelToValidate = this.ProcessInstockDequeueEvent(dequeuedMessage);
                                if (instockDequeueModelToValidate != null)
                                {
                                    instockDequeueModelList.Add(instockDequeueModelToValidate);
                                }
                            });
                        dataAccessLayer.Insert(instockDequeueModelList);
                    }
                    catch (Exception ex)
                    {
                        // this will happen after all retries
                        inventoryLogger.LogError(ex.Message, ex.StackTrace);
                        this.errorEventPublisher.PublishErrorEventToMammoth(dequeuedMessage, ex);
                    }
                }
                dequeuedMessages = instockDequeueService.GetDequeuedMessages();
            }
            inventoryLogger.LogInfo($"Ending {settings.TransactionType} producer.");
        }

        private InstockDequeueModel ProcessInstockDequeueEvent(InstockDequeueResult dequeuedMessage)
        {
            try
            {
                InstockDequeueModel instockDequeueEvent = dequeuedMessage.InstockDequeueModel;
                inventoryLogger.LogInfo($"Processing event{dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber]} with EventTypeCode: {instockDequeueEvent.EventTypeCode}.");

                IList<PurchaseOrdersModel> purchaseOrdersList = dataAccessLayer.Get(
                    instockDequeueEvent.EventTypeCode,
                    instockDequeueEvent.KeyID,
                    instockDequeueEvent.SecondaryKeyID
                );
                inventoryLogger.LogInfo($"Retrieved purchase orders Count: {purchaseOrdersList.Count}.");

                if (purchaseOrdersList != null && purchaseOrdersList.Count > 0)
                {

                    if ((DateTime.UtcNow - instockDequeueEvent.MessageTimestampUtc).TotalMinutes < settings.PurchaseOrdersProcessingBufferTimeInMinutes)
                    {
                        foreach (PurchaseOrdersModel purchaseOrder in purchaseOrdersList)
                        {
                            if (purchaseOrder.EInvoiceQuantity == null)
                            {
                                inventoryLogger.LogInfo($"Re-inserted purchase order: {purchaseOrder.OrderHeaderId} into the queue.");
                                return instockDequeueEvent;
                            }
                        }
                    }

                    inventoryLogger.LogInfo("Transforming DB model to XML model");
                    PurchaseOrders purchaseOrderCanonicalList =
                        this.canonicalMapper.TransformToXmlCanonical(
                            purchaseOrdersList,
                            dequeuedMessage
                        );

                    inventoryLogger.LogInfo("Serializing XML model to XML string");
                    string xmlMessage = this.canonicalMapper.SerializeToXml(purchaseOrderCanonicalList);

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
                return null;
            }
            catch (Exception ex)
            {
                inventoryLogger.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        private void ArchiveInventoryEvent(string xmlMessage, PurchaseOrdersModel purchaseOrdersModel, InstockDequeueResult dequeuedMessage, Exception ex = null)
        {
            archiveInventoryEvents.Archive(
                xmlMessage,
                dequeuedMessage.InstockDequeueModel.EventTypeCode,
                this.GetBusinessUnitId(purchaseOrdersModel),
                dequeuedMessage.InstockDequeueModel.KeyID,
                0,
                ex == null ? 'P' : 'U',
                ex?.Message,
                dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber],
                null
            );
        }

        private int GetBusinessUnitId(PurchaseOrdersModel purchaseOrdersModel)
        {
            return purchaseOrdersModel.LocationNumber;
        }

        private Dictionary<string, string> CreateMessageHeaders(InstockDequeueResult dequeuedMessage, PurchaseOrdersModel purchaseOrdersModel)
        {
            Dictionary<string, string> messageHeaders = dequeuedMessage.Headers;
            // skipping RegionCode, Source attributes since they are already in the headers of InstockDequeueResult
            messageHeaders[Constants.MessageProperty.MessageType] = "Text";
            messageHeaders[Constants.MessageProperty.NonReceivingSystems] = this.settings.NonReceivingSystemsTransferOrder;
            messageHeaders[Constants.MessageProperty.TransactionID] =
               this.GetBusinessUnitId(purchaseOrdersModel)
               + this.settings.TransactionType
               + dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber];
            return messageHeaders;
        }
    }
}
