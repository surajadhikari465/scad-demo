using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.ActiveMQ.Producer;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Icon.ApiController.Controller.ControllerConstants;
using Newtonsoft.Json;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.ApiController.Controller.QueueProcessors
{
    public class ProductSelectionGroupQueueProcessor : IQueueProcessor
    {
        private ApiControllerSettings settings;
        private ILogger<ProductSelectionGroupQueueProcessor> logger;
        private IQueueReader<MessageQueueProductSelectionGroup, Contracts.SelectionGroupsType> queueReader;
        private ISerializer<Contracts.SelectionGroupsType> serializer;
        private ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler;
        private ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProductSelectionGroup, MessageHistory>> associateMessageToQueueCommandHandler;
        private ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProductSelectionGroup>> setProcessedDateCommandHandler;
        private ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>> updateMessageQueueStatusCommandHandler;
        private ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProductSelectionGroup>> markQueuedEntriesAsInProcessCommandHandler;
        private IActiveMQProducer activeMqProducer;
        private Dictionary<string, string> messageProperties;
        private IMessageProcessorMonitor monitor;
        private APIMessageProcessorLogEntry monitorData;

        public ProductSelectionGroupQueueProcessor(
            ApiControllerSettings settings,
            ILogger<ProductSelectionGroupQueueProcessor> logger,
            IQueueReader<MessageQueueProductSelectionGroup, Contracts.SelectionGroupsType> queueReader,
            ISerializer<Contracts.SelectionGroupsType> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProductSelectionGroup, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProductSelectionGroup>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProductSelectionGroup>> markQueuedEntriesAsInProcessCommandHandler,
            IMessageProcessorMonitor monitor,
            IActiveMQProducer activeMQProducer)
        {
            this.settings = settings;
            this.logger = logger;
            this.queueReader = queueReader;
            this.serializer = serializer;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.associateMessageToQueueCommandHandler = associateMessageToQueueCommandHandler;
            this.setProcessedDateCommandHandler = setProcessedDateCommandHandler;
            this.updateMessageHistoryCommandHandler = updateMessageHistoryCommandHandler;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
            this.markQueuedEntriesAsInProcessCommandHandler = markQueuedEntriesAsInProcessCommandHandler;
            this.activeMqProducer = activeMQProducer;
            this.monitor = monitor;
            this.monitorData = new APIMessageProcessorLogEntry()
            {
                MessageTypeID = MessageTypes.ProductSelectionGroup
            };
        }

        public void ProcessMessageQueue()
        {
            monitorData.StartTime = DateTime.UtcNow;
            SetMessageProperties();
            MarkQueuedMessagesAsInProcess();

            var messagesReadyToProcess = GetQueuedMessages();

            var shouldRecordMonitorData = messagesReadyToProcess.Count > 0;
            while (messagesReadyToProcess.Count > 0)
            {
                var messagesReadyForMiniBulk = GroupMessagesForMiniBulk(messagesReadyToProcess);

                if (messagesReadyForMiniBulk.Count > 0)
                {
                    var miniBulk = PrepareMiniBulk(messagesReadyForMiniBulk);

                    if (miniBulk.group.Length > 0)
                    {
                        string xml = SerializeMiniBulk(miniBulk);

                        if (string.IsNullOrEmpty(xml))
                        {
                            MarkQueuedMessagesAsFailed(messagesReadyForMiniBulk);
                            monitorData.CountFailedMessages = monitorData.CountFailedMessages.GetValueOrDefault(0) + messagesReadyToProcess.Count;
                        }
                        else
                        {
                            var message = BuildXmlMessage(xml, messageProperties);

                            SaveXmlMessageToMessageHistory(message);

                            AssociateSavedMessageToMessageQueue(messagesReadyForMiniBulk, message);

                            SetProcessedDate(messagesReadyForMiniBulk);

                            int messageStatusId = PublishMessage(message.Message, message.MessageHistoryId);

                            ProcessResponse(messageStatusId, message);

                            if (messageStatusId == MessageStatusTypes.Sent)
                            {
                                monitorData.CountProcessedMessages = monitorData.CountProcessedMessages.GetValueOrDefault(0) + messagesReadyToProcess.Count;
                            }
                            else
                            {
                                monitorData.CountFailedMessages = monitorData.CountFailedMessages.GetValueOrDefault(0) + messagesReadyToProcess.Count;
                            }
                        }
                    }
                }

                logger.Debug("Ending the main processing loop.  Now preparing to retrieve a new set of queued messages.");

                MarkQueuedMessagesAsInProcess();
                messagesReadyToProcess = GetQueuedMessages();
            }

            logger.Debug("Ending the PSG queue processor.  No further queued messages were found in Ready status.");

            

            monitorData.EndTime = DateTime.UtcNow;
            if (shouldRecordMonitorData)
            {
                monitor.RecordResults(monitorData);
            }
        }

        private void SetMessageProperties()
        {
            messageProperties = new Dictionary<string, string>();
            messageProperties.Add("IconMessageID", string.Empty);
            messageProperties.Add("Source", "Icon");
			messageProperties.Add("TransactionType", "Item/Locale"); //PSG

			if (!string.IsNullOrWhiteSpace(settings.NonReceivingSystemsAll))
            {
                messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, settings.NonReceivingSystemsAll);
            }
        }

        private void ProcessResponse(int messageStatusId, MessageHistory message)
        {
            if (messageStatusId == MessageStatusTypes.Sent)
            {
                logger.Info(string.Format("Message {0} has been sent successfully.", message.MessageHistoryId));

                var updateMessageHistoryCommand = new UpdateMessageHistoryStatusCommand<MessageHistory>
                {
                    Message = message,
                    MessageStatusId = MessageStatusTypes.Sent
                };

                updateMessageHistoryCommandHandler.Execute(updateMessageHistoryCommand);
            }
            else
            {
                logger.Error(String.Format("Message {0} failed to send.  Message will remain in Ready state for re-processing during the next controller execution.", message.MessageHistoryId));
            }
        }

        private int PublishMessage(string xml, int messageHistoryId)
        {
            logger.Info(String.Format("Preparing to send message {0}.", messageHistoryId));
            bool sentToActiveMq = false;
            messageProperties["IconMessageID"] = messageHistoryId.ToString();
            sentToActiveMq = SendToActiveMq(xml, messageProperties);
            
            // Determining MessageStatus
            int messageStatusId;
            if(sentToActiveMq)
                messageStatusId = MessageStatusTypes.Sent;
            else
                messageStatusId = MessageStatusTypes.Ready;
            return messageStatusId;
        }

        private bool SendToActiveMq(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            bool sent = false;
            String utf8XmlMessage = new StringBuilder(xmlMessage).Replace("utf-16", "utf-8").ToString();
            try{
                activeMqProducer.Send(utf8XmlMessage, messageProperties);
                sent = true;
            }
            catch(Exception ex)
            {
                logger.Error(string.Format("Failed to send message {0} to ActiveMQ.  Error: {1}", messageProperties["IconMessageID"], ex.ToString()));
                sent = false;
            }
            return sent;
        }

        private void SetProcessedDate(List<MessageQueueProductSelectionGroup> messagesToUpdate)
        {
            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueProductSelectionGroup>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = messagesToUpdate
            };

            setProcessedDateCommandHandler.Execute(command);
        }

        private void AssociateSavedMessageToMessageQueue(List<MessageQueueProductSelectionGroup> associatedMessages, MessageHistory messageHistory)
        {
            var command = new AssociateMessageToQueueCommand<MessageQueueProductSelectionGroup, MessageHistory>
            {
                QueuedMessages = associatedMessages,
                MessageHistory = messageHistory
            };

            associateMessageToQueueCommandHandler.Execute(command);
        }

        private void SaveXmlMessageToMessageHistory(MessageHistory message)
        {
            var command = new SaveToMessageHistoryCommand<MessageHistory>
            {
                Message = message
            };

            saveToMessageHistoryCommandHandler.Execute(command);
        }

        private MessageHistory BuildXmlMessage(string xml, Dictionary<string, string> messageProperties)
        {
            // DVS wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
            // the database will happily store it.
            xml = new StringBuilder(xml).Replace("utf-8", "utf-16").ToString();

            return new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.ProductSelectionGroup,
                Message = xml,
                MessageHeader = JsonConvert.SerializeObject(messageProperties),
                InsertDate = DateTime.Now,
                InProcessBy = ControllerType.Instance,
                ProcessedDate = null
            };
        }

        private void MarkQueuedMessagesAsFailed(List<MessageQueueProductSelectionGroup> failedMessages)
        {
            var command = new UpdateMessageQueueStatusCommand<MessageQueueProductSelectionGroup>
            {
                QueuedMessages = failedMessages,
                MessageStatusId = MessageStatusTypes.Failed,
                ResetInProcessBy = true
            };

            updateMessageQueueStatusCommandHandler.Execute(command);
        }

        private string SerializeMiniBulk(Contracts.SelectionGroupsType miniBulk)
        {
            return serializer.Serialize(miniBulk, new Utf8StringWriter());
        }

        private Contracts.SelectionGroupsType PrepareMiniBulk(List<MessageQueueProductSelectionGroup> messagesToBundle)
        {
            return queueReader.BuildMiniBulk(messagesToBundle);
        }

        private List<MessageQueueProductSelectionGroup> GroupMessagesForMiniBulk(List<MessageQueueProductSelectionGroup> messagesToGroup)
        {
            return queueReader.GroupMessagesForMiniBulk(messagesToGroup);
        }

        private List<MessageQueueProductSelectionGroup> GetQueuedMessages()
        {
            return queueReader.GetQueuedMessages();
        }

        private void MarkQueuedMessagesAsInProcess()
        {
            // LookAhead can default to 1 since PSG messages aren't bundled.
            int lookAhead = 1;

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueProductSelectionGroup>
            {
                LookAhead = lookAhead,
                Instance = ControllerType.Instance
            };

            markQueuedEntriesAsInProcessCommandHandler.Execute(command);
        }
    }
}
