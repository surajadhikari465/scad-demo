using Icon.Logging;
using InventoryProducer.Producer.DataAccess;
using InventoryProducer.Producer.Model.DBModel;
using InventoryProducer.Producer.Publish;
using System;
using System.Collections.Generic;
using Polly;
using Polly.Retry;

namespace InventoryProducer.Producer.QueueProcessors
{
    internal class RepublishInventoryQueueProcessor : IQueueProcessor
    {
        IRepublishInventoryDAL republishInventoryDal;
        IErrorEventPublisher errorEventPublisher;
        ILogger<RepublishInventoryQueueProcessor> logger;
        RetryPolicy retryPolicy;

        const int DB_ERROR_RETRY_COUNT = 6;
        const int DB_ERROR_RETRY_INTERVAL_MILLISECONDS = 10000;

        public RepublishInventoryQueueProcessor()
        {
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
                            // TODO: Update null with compatible object or add another method in ErrorEventPublisher
                            errorEventPublisher.PublishErrorEventToMammoth(null, ex);
                            logger.Error(ex.ToString());
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
                // TODO: Update null with compatible object or add another method in ErrorEventPublisher
                errorEventPublisher.PublishErrorEventToMammoth(null, ex);
                logger.Error(ex.ToString());
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
            // TODO: sent to ESB topic and proper ActiveMQ queues based on EventType
            throw new NotImplementedException();
        }
    }
}
