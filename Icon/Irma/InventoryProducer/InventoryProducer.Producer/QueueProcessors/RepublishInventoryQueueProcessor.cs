using InventoryProducer.Common.Helpers;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Producer.Publish;
using System;
using System.Collections.Generic;
using Polly;
using Polly.Retry;
using Icon.DbContextFactory;
using Mammoth.Framework;
using InventoryProducer.Common;

namespace InventoryProducer.Producer.QueueProcessors
{
    internal class RepublishInventoryQueueProcessor : IQueueProcessor
    {
        InventoryProducerSettings settings;
        IRepublishInventoryDAL republishInventoryDal;
        IMessagePublisher messagePublisher;
        IDbContextFactory<MammothContext> mammothDbContextFactory;
        InventoryLogger<RepublishInventoryQueueProcessor> logger;
        RetryPolicy retryPolicy;

        const int DB_ERROR_RETRY_COUNT = 6;
        const int DB_ERROR_RETRY_INTERVAL_MILLISECONDS = 10000;
        const string APPLICATION_NAME = "RePublishInventoryMessagesService";

        public RepublishInventoryQueueProcessor(
            InventoryProducerSettings settings,
            IRepublishInventoryDAL republishInventoryDal,
            IMessagePublisher messagePublisher,
            IDbContextFactory<MammothContext> mammothDbContextFactory,
            InventoryLogger<RepublishInventoryQueueProcessor> logger
        )
        {
            this.settings = settings;
            this.republishInventoryDal = republishInventoryDal;
            this.messagePublisher = messagePublisher;
            this.mammothDbContextFactory = mammothDbContextFactory;
            this.logger = logger;
            this.retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    DB_ERROR_RETRY_COUNT, 
                    retryAttempt => TimeSpan.FromMilliseconds(DB_ERROR_RETRY_INTERVAL_MILLISECONDS)
                );
        }


        public void ProcessMessageQueue()
        {
            try
            {
                IList<ArchivedMessageModel> unsentMessages = GetUnsentMessages();
                if (unsentMessages.Count > 0)
                {
                    foreach (var unsentMessage in unsentMessages)
                    {
                        try
                        {
                            Publish(unsentMessage);
                        }
                        catch (Exception ex)
                        {
                            PublishErrorEvents.SendToMammoth(
                                mammothDbContextFactory,
                                APPLICATION_NAME,
                                unsentMessage.MessageNumber.ToString(),
                                new Dictionary<string, string>()
                                {
                                    { "", "" } // Referred from Existing DB records created by TIBCO app
                                },
                                unsentMessage.Message,
                                ex.Message,
                                ex.ToString(),
                                "Fatal"
                            );
                            logger.LogError(ex.Message, ex.ToString());
                            republishInventoryDal.UpdateMessageArchiveWithError(
                                unsentMessage.MessageArchiveID, 
                                DB_ERROR_RETRY_COUNT, 
                                ex.Message
                            );
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                logger.LogError(ex.Message, ex.ToString());
                PublishErrorEvents.SendToMammoth(
                    mammothDbContextFactory,
                    APPLICATION_NAME,
                    settings.InstanceGUID,
                    new Dictionary<string, string>()
                    {
                        { "", "" } // Referred from Existing DB records created by TIBCO app
                    },
                    ex.GetType().ToString(),
                    ex.Message,
                    ex.ToString(),
                    "Fatal"
                );
            }
        }

        private IList<ArchivedMessageModel> GetUnsentMessages()
        {
            return this.retryPolicy.Execute(() =>
            {
                return republishInventoryDal.GetUnsentMessages();
            });
        }

        private void Publish(ArchivedMessageModel message)
        {
            this.retryPolicy.Execute(() =>
            {
                messagePublisher.PublishMessage(
                    message.Message,
                    new Dictionary<string, string>()
                    {
                    { Constants.MessageProperty.TransactionID, GetTransactionId(message) },
                    { Constants.MessageProperty.TransactionType, GetTransactionType(message) },
                    { Constants.MessageProperty.Source, "IRMA" },
                    { Constants.MessageProperty.MessageType, "TEXT" },
                    { Constants.MessageProperty.MessageNumber, message.MessageNumber.ToString() },
                    { Constants.MessageProperty.RegionCode, message.BusinessUnitID.ToString() }
                    },
                    null,
                    null
                );
            });
        }

        private string GetTransactionId(ArchivedMessageModel message)
        {
            string transactionType = GetTransactionType(message);
            return message.BusinessUnitID + transactionType + message.MessageNumber;
        }

        private string GetTransactionType(ArchivedMessageModel message)
        {
            if (message.EventType.ToUpper().StartsWith("INV"))
            {
                return Constants.TransactionType.InventorySpoilage;
            }
            else if (message.EventType.ToUpper().StartsWith("PO_"))
            {
                return Constants.TransactionType.PurchaseOrders;
            }
            else if (message.EventType.ToUpper().StartsWith("RCPT"))
            {
                return Constants.TransactionType.ReceiptOrders;
            }
            else
            {
                return Constants.TransactionType.TransferOrders;
            }
        }
    }
}
