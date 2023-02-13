using System;
using System.Collections.Generic;
using InventoryProducer.Common.InstockDequeue;
using InventoryProducer.Common;
using InventoryProducer.Common.InstockDequeue.Model;
using InventoryProducer.Common.InstockDequeue.Model.DBModel;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Producer.Helpers;
using InventoryProducer.Producer.Mapper;
using InventoryProducer.Producer.Publish;
using Polly.Retry;
using Polly;

using OrderReceipts = Icon.Esb.Schemas.Wfm.Contracts.orderReceipts;


namespace InventoryProducer.Producer.QueueProcessors
{
    public class InventoryReceiveQueueProcessor : QueueProcessor<OrderReceipts, ReceiveModel>
    {
        public InventoryReceiveQueueProcessor(
            InventoryProducerSettings settings,
            IInventoryLogger<QueueProcessor<OrderReceipts, ReceiveModel>> inventoryLogger,
            IInstockDequeueService instockDequeueService,
            IDAL<ReceiveModel> dataAccessLayer,
            ICanonicalMapper<OrderReceipts, ReceiveModel> canonicalMapper,
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
                dequeuedMessages = instockDequeueService.GetDequeuedMessages();
            }
            inventoryLogger.LogInfo($"Ending {settings.TransactionType} producer.");
        }

        private void ProcessInstockDequeueEvent(InstockDequeueResult dequeuedMessage)
        {
            try
            {
                InstockDequeueModel instockDequeueEvent = dequeuedMessage.InstockDequeueModel;
                inventoryLogger.LogInfo($"Processing event{dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber]} with EventTypeCode: {instockDequeueEvent.EventTypeCode}.");

                IList<ReceiveModel> receiveList = dataAccessLayer.Get(
                    instockDequeueEvent.EventTypeCode,
                    instockDequeueEvent.KeyID,
                    instockDequeueEvent.SecondaryKeyID
                );
                inventoryLogger.LogInfo($"Retrieved Receives Count: {receiveList.Count}.");

                if (receiveList != null && receiveList.Count > 0)
                {
                    inventoryLogger.LogInfo("Transforming DB model to XML model");
                    OrderReceipts orderReceiptsCanonical =
                        canonicalMapper.TransformToXmlCanonical(
                            receiveList,
                            dequeuedMessage
                        );

                    inventoryLogger.LogInfo("Serializing XML model to XML string");
                    string xmlMessage = canonicalMapper.SerializeToXml(orderReceiptsCanonical);

                    Dictionary<string, string> messageHeaders = this.CreateMessageHeaders(dequeuedMessage,
                        receiveList[0]);

                    inventoryLogger.LogInfo("Sending message.");
                    messagePublisher.PublishMessage(
                        xmlMessage,
                        messageHeaders,
                        onSuccess: () =>
                        {
                            inventoryLogger.LogInfo($"Message: {messageHeaders[Constants.MessageProperty.TransactionID]} is sent.");
                            this.ArchiveInventoryEvent(xmlMessage, receiveList[0], dequeuedMessage);
                        },
                        onFailure: (Exception ex) =>
                        {
                            inventoryLogger.LogError(
                                $"Message: {messageHeaders[Constants.MessageProperty.TransactionID]} failed due to Exception: {ex.Message}.",
                                ex.StackTrace
                            );
                            this.ArchiveInventoryEvent(xmlMessage, receiveList[0], dequeuedMessage, ex);
                        }
                    );
                }
                else
                {
                    inventoryLogger.LogInfo($"Skipping transformation and publish, couldn't find receive orders.");
                }
            }
            catch (Exception ex)
            {
                inventoryLogger.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        private Dictionary<string, string> CreateMessageHeaders(InstockDequeueResult dequeuedMessage, ReceiveModel receiveDbItem)
        {   // TransactionType, Source and RegionCode are already present in dequeuedMessage.Headers
            Dictionary<string, string> messageHeaders = dequeuedMessage.Headers;
            messageHeaders[Constants.MessageProperty.TransactionID] =
                receiveDbItem.StoreNumber
                + this.settings.TransactionType
                + dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber];
            messageHeaders[Constants.MessageProperty.MessageType] = "Text";
            messageHeaders[Constants.MessageProperty.MessageNumber] = dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber];
            messageHeaders[Constants.MessageProperty.NonReceivingSystems] = this.settings.NonReceivingSystemsReceive;
            return messageHeaders;
        }

        private void ArchiveInventoryEvent(string xmlMessage, ReceiveModel receiveDbItem,
            InstockDequeueResult dequeuedMessage, Exception exception = null)
        {
            archiveInventoryEvents.Archive(
                xmlMessage,
                dequeuedMessage.InstockDequeueModel.EventTypeCode,
                receiveDbItem.StoreNumber != null ? (int)receiveDbItem.StoreNumber : -1,
                dequeuedMessage.InstockDequeueModel.KeyID,
                0,
                exception == null ? 'P' : 'U',
                exception?.Message,
                dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber],
                null
            );
        }
    }
}
