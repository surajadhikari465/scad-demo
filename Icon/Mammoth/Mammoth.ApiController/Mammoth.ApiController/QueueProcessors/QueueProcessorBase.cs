using Icon.ApiController.Common;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Logging;
using System;
using System.Collections.Generic;

namespace Mammoth.ApiController.QueueProcessors
{
    public abstract class QueueProcessorBase<TMessageQueue, TContract, TMessageHistory> : IQueueProcessor
    {
        protected ILogger logger;
        protected IQueueReader<TMessageQueue, TContract> queueReader;
        protected ISerializer<TContract> serializer;
        protected ICommandHandler<SaveToMessageHistoryCommand<TMessageHistory>> saveToMessageHistoryCommandHandler;
        protected ICommandHandler<AssociateMessageToQueueCommand<TMessageQueue, TMessageHistory>> associateMessageToQueueCommandHandler;
        protected ICommandHandler<UpdateMessageQueueProcessedDateCommand<TMessageQueue>> setProcessedDateCommandHandler;
        protected ICommandHandler<UpdateMessageHistoryStatusCommand<TMessageHistory>> updateMessageHistoryCommandHandler;
        protected ICommandHandler<UpdateMessageQueueStatusCommand<TMessageQueue>> updateMessageQueueStatusCommandHandler;
        private ICommandHandler<MarkQueuedEntriesAsInProcessCommand<TMessageQueue>> markQueuedEntriesAsInProcessCommandHandler;
        protected IEsbProducer producer;
        protected int messageReadyStatusId;
        protected int messageSentStatusId;
        protected int messageFailedStatusId;
        protected ApiControllerSettings settings;

        /// <summary>
        /// MessageTypeId (Mammoth.Common.DataAccess.MessageTypes)
        /// ItemLocale=1,Price=2,PrimePsg=3,Processbod=4,Confirmbod=5
        /// Deriving classes will set this value 
        /// (1 for ItemLocaleProcessor, 2 for PriceProcessor...)
        /// </summary>
        internal abstract int MessageType { get; }

        public QueueProcessorBase(ILogger logger,
            IQueueReader<TMessageQueue, TContract> queueReader,
            ISerializer<TContract> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<TMessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<TMessageQueue, TMessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<TMessageQueue>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<TMessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<TMessageQueue>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<TMessageQueue>> markQueuedEntriesAsInProcessCommandHandler,
            IEsbProducer producer)
        {
            this.logger = logger;
            this.queueReader = queueReader;
            this.serializer = serializer;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.associateMessageToQueueCommandHandler = associateMessageToQueueCommandHandler;
            this.setProcessedDateCommandHandler = setProcessedDateCommandHandler;
            this.updateMessageHistoryCommandHandler = updateMessageHistoryCommandHandler;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
            this.markQueuedEntriesAsInProcessCommandHandler = markQueuedEntriesAsInProcessCommandHandler;
            this.producer = producer;
            SetMessageIds();
        }

        public void ProcessMessageQueue()
        {
            Initialize();
            MarkQueuedMessagesAsInProcess();
            var messagesReadyToProcess = GetQueuedMessages();

            while (messagesReadyToProcess.Count > 0)
            {
                var messagesReadyForMiniBulk = GroupMessagesForMiniBulk(messagesReadyToProcess);

                if (messagesReadyForMiniBulk.Count > 0)
                {
                    var miniBulk = PrepareMiniBulk(messagesReadyForMiniBulk);

                    if(ShouldSendMessage(miniBulk))
                    {
                        List<TMessageQueue> messagesReadyToSerialize = GetMessagesReadyToSerialize(messagesReadyForMiniBulk, miniBulk);
                                                
                        TMessageHistory message = BuildMessageFromMiniBulk(miniBulk, messagesReadyToSerialize);
                        if (message != null)
                        {
                            bool messageSent = PublishMessage(message);

                            ProcessResponse(messageSent, message);
                        }                            
                    }
                }

                logger.Info("Ending the main processing loop.  Now preparing to retrieve a new set of queued messages.");

                MarkQueuedMessagesAsInProcess();
                messagesReadyToProcess = GetQueuedMessages();
            }

            logger.Info("Ending the queue processor.  No further queued messages were found in Ready status.");

            producer.Dispose();
        }

        private TMessageHistory BuildMessageFromMiniBulk(TContract miniBulk, List<TMessageQueue> messagesReadyToSerialize)
        {
            TMessageHistory message = default(TMessageHistory);

            string xml = SerializeMiniBulk(miniBulk);
            if (string.IsNullOrEmpty(xml))
            {
                MarkQueuedMessagesAsFailed(messagesReadyToSerialize);
            }
            else
            {
                try
                {
                    message = BuildXmlMessage(xml);
                    SaveXmlMessageToMessageHistory(message);
                    AssociateSavedMessageToMessageQueue(messagesReadyToSerialize, message);
                    SetProcessedDate(messagesReadyToSerialize);
                }
                catch (Exception ex)
                {
                    logger.Error(string.Format("Unexpected error occurred when building the message. Failing message rows. Error {0}", ex));

                    MarkQueuedMessagesAsFailed(messagesReadyToSerialize);
                }
            }

            return message;
        }

        private void ProcessResponse(bool messageSent, TMessageHistory message)
        {
            if (messageSent)
            {
                logger.Info(string.Format("Message {0} has been sent successfully.", GetMessageHistoryId(message)));

                var updateMessageHistoryCommand = new UpdateMessageHistoryStatusCommand<TMessageHistory>
                {
                    Message = message,
                    MessageStatusId = messageSentStatusId
                };

                updateMessageHistoryCommandHandler.Execute(updateMessageHistoryCommand);
            }
            else
            {
                logger.Error(string.Format("Message {0} failed to send.  Message will remain in Ready state for re-processing during the next controller execution.", GetMessageHistoryId(message)));
            }
        }

        private void SetProcessedDate(List<TMessageQueue> messagesToUpdate)
        {
            var command = new UpdateMessageQueueProcessedDateCommand<TMessageQueue>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = messagesToUpdate
            };

            setProcessedDateCommandHandler.Execute(command);
        }

        private void AssociateSavedMessageToMessageQueue(List<TMessageQueue> associatedMessages, TMessageHistory messageHistory)
        {
            var command = new AssociateMessageToQueueCommand<TMessageQueue, TMessageHistory>
            {
                QueuedMessages = associatedMessages,
                MessageHistory = messageHistory
            };

            associateMessageToQueueCommandHandler.Execute(command);
        }

        private void SaveXmlMessageToMessageHistory(TMessageHistory message)
        {
            var command = new SaveToMessageHistoryCommand<TMessageHistory>
            {
                Message = message
            };

            saveToMessageHistoryCommandHandler.Execute(command);
        }
        
        private void MarkQueuedMessagesAsFailed(List<TMessageQueue> failedMessages)
        {
            var command = new UpdateMessageQueueStatusCommand<TMessageQueue>
            {
                QueuedMessages = failedMessages,
                MessageStatusId = messageFailedStatusId,
                ResetInProcessBy = true
            };

            updateMessageQueueStatusCommandHandler.Execute(command);
        }

        private string SerializeMiniBulk(TContract miniBulk)
        {
            try
            {
                return serializer.Serialize(miniBulk, new Utf8StringWriter());
            }
            catch(Exception ex)
            {
                logger.Error(string.Format("Failed to serialize minibulk. Will mark messages as failed. Error {0}", ex));
                return null;
            }
        }

        private TContract PrepareMiniBulk(List<TMessageQueue> messagesToBundle)
        {
            return queueReader.BuildMiniBulk(messagesToBundle);
        }

        private List<TMessageQueue> GroupMessagesForMiniBulk(List<TMessageQueue> messagesToGroup)
        {
            return queueReader.GroupMessagesForMiniBulk(messagesToGroup);
        }

        private List<TMessageQueue> GetQueuedMessages()
        {
            return queueReader.GetQueuedMessages();
        }

        private void MarkQueuedMessagesAsInProcess()
        {
            var command = new MarkQueuedEntriesAsInProcessCommand<TMessageQueue>
            {
                LookAhead = settings.QueueLookAhead,
                Instance = settings.Instance
            };

            markQueuedEntriesAsInProcessCommandHandler.Execute(command);
        }

        public virtual void Initialize()
        {
            if (!producer.IsConnected)
            {
                producer.OpenConnection();
            }
        }

        protected abstract List<TMessageQueue> GetMessagesReadyToSerialize(List<TMessageQueue> messagesReadyForMiniBulk, TContract miniBulk);

        protected abstract bool ShouldSendMessage(TContract miniBulk);

        protected abstract TMessageHistory BuildXmlMessage(string xml);

        protected abstract string GetMessageHistoryId(TMessageHistory messageHistory);

        protected abstract bool PublishMessage(TMessageHistory messageHistory);
        
        protected abstract void SetMessageIds();
    }
}
