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

using TransferOrdersCanonical = Icon.Esb.Schemas.Wfm.Contracts.transferOrders;


namespace InventoryProducer.Producer.QueueProcessors
{
    public class InventoryTransferQueueProcessor : IQueueProcessor
    {
        private readonly InventoryProducerSettings settings;
        private readonly IInventoryLogger<InventoryTransferQueueProcessor> inventoryLogger;
        private readonly IInstockDequeueService instockDequeueService;
        private readonly RetryPolicy retrypolicy;
        private readonly IArchiveInventoryEvents archiveInventoryEvents;
        private readonly ITransferOrdersDAL transferOrdersDal;
        private readonly ITransferOrderCanonicalMapper transferOrderXmlCanonicalMapper;
        private readonly IMessagePublisher messagePublisher;
        private readonly IErrorEventPublisher errorEventPublisher;

        public InventoryTransferQueueProcessor(
            InventoryProducerSettings settings,
            IInventoryLogger<InventoryTransferQueueProcessor> inventoryLogger,
            IInstockDequeueService instockDequeueService,
            ITransferOrdersDAL transferOrdersDal,
            ITransferOrderCanonicalMapper transferOrderXmlCanonicalMapper,
            IMessagePublisher messagePublisher,
            IArchiveInventoryEvents archiveInventoryEvents,
            IErrorEventPublisher errorEventPublisher
            )
        {
            this.settings = settings;
            this.inventoryLogger = inventoryLogger;
            this.instockDequeueService = instockDequeueService;
            this.transferOrdersDal = transferOrdersDal;
            this.transferOrderXmlCanonicalMapper = transferOrderXmlCanonicalMapper;
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
                
                IList<TransferOrdersModel> transferOrdersList = this.transferOrdersDal.GetTransferOrders(
                    instockDequeueEvent.EventTypeCode,
                    instockDequeueEvent.KeyID,
                    instockDequeueEvent.SecondaryKeyID
                );
                inventoryLogger.LogInfo($"Retrieved transferOrders Count: {transferOrdersList.Count}.");

                if (transferOrdersList != null && transferOrdersList.Count > 0)
                {
                    inventoryLogger.LogInfo("Transforming DB model to XML model");
                    TransferOrdersCanonical transferOrdersCanonical = 
                        this.transferOrderXmlCanonicalMapper.TransformToXmlCanonical(
                            transferOrdersList, 
                            dequeuedMessage
                        );

                    inventoryLogger.LogInfo("Serializing XML model to XML string");
                    string xmlMessage = this.transferOrderXmlCanonicalMapper.SerializeToXml(transferOrdersCanonical);

                    Dictionary<string, string> messageHeaders = this.CreateMessageHeaders(dequeuedMessage,
                        transferOrdersList[0]);

                    inventoryLogger.LogInfo("Sending message.");
                    messagePublisher.PublishMessage(
                        xmlMessage,
                        messageHeaders,
                        onSuccess: () =>
                        {
                            inventoryLogger.LogInfo($"Message: {messageHeaders["TransactionID"]} is sent.");
                            this.ArchiveInventoryEvent(xmlMessage, transferOrdersList[0], dequeuedMessage);
                        },
                        onFailure: (Exception ex) =>
                        {
                            inventoryLogger.LogError(
                                $"Message: {messageHeaders["TransactionID"]} failed due to Exception: {ex.Message}.",
                                ex.StackTrace
                            );
                            this.ArchiveInventoryEvent(xmlMessage, transferOrdersList[0], dequeuedMessage, ex);
                        }
                    );
                }
                else
                {
                    inventoryLogger.LogInfo($"Skipping transformation and publish, couldn't find transferOrders.");
                }
            }
            catch(Exception ex)
            {
                inventoryLogger.LogError(ex.Message, ex.StackTrace);
                throw ex;
            }
        }

        private Dictionary<string, string> CreateMessageHeaders(InstockDequeueResult dequeuedMessage, TransferOrdersModel transferOrdersDbItem)
        {
            Dictionary<string, string> messageHeaders = dequeuedMessage.Headers;
            // skipping RegionCode, Source attributes since they are already in the headers of InstockDequeueResult
            messageHeaders[Constants.MessageProperty.TransactionID] = 
                this.GetBusinessUnitId(transferOrdersDbItem)
                + this.settings.TransactionType
                + dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber];
            messageHeaders[Constants.MessageProperty.MessageType] = "Text";
            messageHeaders[Constants.MessageProperty.NonReceivingSystems] = this.settings.NonReceivingSystemsTransferOrder;
            return messageHeaders;
        }

        private void ArchiveInventoryEvent(string xmlMessage, TransferOrdersModel transferOrdersDbItem,
            InstockDequeueResult dequeuedMessage, Exception exception = null)
        {
            archiveInventoryEvents.Archive(
                xmlMessage,
                dequeuedMessage.InstockDequeueModel.EventTypeCode,
                Int32.Parse(
                    this.GetBusinessUnitId(transferOrdersDbItem)
                ),
                dequeuedMessage.InstockDequeueModel.KeyID,
                0,
                exception == null ? 'P' : 'U',
                exception?.Message,
                dequeuedMessage.Headers[Constants.MessageProperty.MessageNumber],
                null
            );
        }


        private string GetBusinessUnitId(TransferOrdersModel transferOrdersDbItem)
        {
            return transferOrdersDbItem.ToLocationNumber.ToString();
        }
    }
}
