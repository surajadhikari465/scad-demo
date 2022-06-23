﻿using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.DataAccess;
using Icon.ActiveMQ.Producer;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Icon.ApiController.Controller.ControllerConstants;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Newtonsoft.Json;

namespace Icon.ApiController.Controller.QueueProcessors
{
    public class LocaleQueueProcessor : IQueueProcessor
    {
        private ApiControllerSettings settings;
        private ILogger<LocaleQueueProcessor> logger;
        private IQueueReader<MessageQueueLocale, Contracts.LocaleType> queueReader;
        private ISerializer<Contracts.LocaleType> serializer;
        private ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler;
        private ICommandHandler<AssociateMessageToQueueCommand<MessageQueueLocale, MessageHistory>> associateMessageToQueueCommandHandler;
        private ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueLocale>> setProcessedDateCommandHandler;
        private ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>> updateMessageQueueStatusCommandHandler;
        private ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueLocale>> markQueuedEntriesAsInProcessCommandHandler;
        private IEsbProducer producer;
        private IActiveMQProducer activeMqProducer;
        private Dictionary<string, string> messageProperties;
        private IMessageProcessorMonitor monitor;
        private APIMessageProcessorLogEntry monitorData;

        public LocaleQueueProcessor(
            ApiControllerSettings settings,
            ILogger<LocaleQueueProcessor> logger,
            IQueueReader<MessageQueueLocale, Contracts.LocaleType> queueReader,
            ISerializer<Contracts.LocaleType> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<MessageQueueLocale, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueLocale>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueLocale>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueLocale>> markQueuedEntriesAsInProcessCommandHandler,
            IEsbProducer producer,
            IMessageProcessorMonitor monitor,
            IActiveMQProducer activeMqProducer)
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
            this.producer = producer;
            this.monitor = monitor;
            this.activeMqProducer = activeMqProducer;
            this.monitorData = new APIMessageProcessorLogEntry()
            {
                MessageTypeID = MessageTypes.Locale
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

                    if (miniBulk != null)
                    {
                        var xml = SerializeMiniBulk(miniBulk);

                        if (String.IsNullOrEmpty(xml))
                        {
                            MarkQueuedMessagesAsFailed(messagesReadyForMiniBulk);
                            monitorData.CountFailedMessages = monitorData.CountFailedMessages.GetValueOrDefault(0) + messagesReadyForMiniBulk.Count;
                        }
                        else
                        {
                            var localeMessage = BuildXmlMessage(xml, messageProperties);

                            SaveXmlMessageToMessageHistory(localeMessage);

                            AssociateSavedMessageToMessageQueue(messagesReadyForMiniBulk, localeMessage);

                            SetProcessedDate(messagesReadyForMiniBulk);

                            int messageStatusId = PublishMessage(localeMessage.Message, localeMessage.MessageHistoryId);

                            ProcessResponse(messageStatusId, localeMessage);

                            if (messageStatusId == MessageStatusTypes.Sent)
                            {
                                monitorData.CountProcessedMessages = monitorData.CountProcessedMessages.GetValueOrDefault(0) + messagesReadyForMiniBulk.Count;
                            }
                            else
                            {
                                monitorData.CountFailedMessages = monitorData.CountFailedMessages.GetValueOrDefault(0) + messagesReadyForMiniBulk.Count;
                            }
                        }
                    }
                }

                logger.Debug("Ending the main processing loop.  Now preparing to retrieve a new set of queued messages.");

                MarkQueuedMessagesAsInProcess();
                messagesReadyToProcess = GetQueuedMessages();
            }

            logger.Debug("Ending the Locale queue processor.  No further queued messages were found in Ready status.");

            

            monitorData.EndTime = DateTime.UtcNow;
            if (shouldRecordMonitorData)
            {
                monitor.RecordResults(monitorData);
            }
        }

        private void SetMessageProperties()
        {
            messageProperties = new Dictionary<string, string>();
            messageProperties.Add("IconMessageID", String.Empty);
            messageProperties.Add("Source", "Icon");
			messageProperties.Add("TransactionType", "Locale");

			if (!String.IsNullOrWhiteSpace(settings.NonReceivingSystemsAll))
            {
                messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, settings.NonReceivingSystemsAll);
            }
        }

        private void ProcessResponse(int messageStatusId, MessageHistory message)
        {
            if (messageStatusId == MessageStatusTypes.Sent)
            {
                logger.Info(String.Format("Message {0} has been sent successfully.", message.MessageHistoryId));

                var updateMessageHistoryCommand = new UpdateMessageHistoryStatusCommand<MessageHistory>
                {
                    Message = message,
                    MessageStatusId = MessageStatusTypes.Sent
                };

                updateMessageHistoryCommandHandler.Execute(updateMessageHistoryCommand);
            }
            else if (messageStatusId == MessageStatusTypes.Ready)
            {
                logger.Error(String.Format("Message {0} failed to send.  Message will remain in Ready state for re-processing during the next controller execution.", message.MessageHistoryId));
            }
            else
            {
                // either sent to ESB or ActiveMQ not both
                logger.Error(String.Format("Message {0} has not been sent to one of the two Brokers(ESB,ActiveMQ). Message will be resend to that Broker during the next controller execution", message.MessageHistoryId));
                var updateMessageHistoryCommand = new UpdateMessageHistoryStatusCommand<MessageHistory>
                {
                    Message = message,
                    MessageStatusId = messageStatusId
                };
                updateMessageHistoryCommandHandler.Execute(updateMessageHistoryCommand);

            }
        }

        private int PublishMessage(string xml, int messageHistoryId)
        {
            logger.Info(String.Format("Preparing to send message {0}.", messageHistoryId));
            bool sentToEsb = false;
            bool sentToActiveMq = false;
            messageProperties["IconMessageID"] = messageHistoryId.ToString();   
            sentToEsb = SendToEsb(xml, messageProperties);
            sentToActiveMq = SendToActiveMq(xml, messageProperties);
            
            // Determining MessageStatus
            int messageStatusId;
            if(sentToEsb && sentToActiveMq)
                messageStatusId = MessageStatusTypes.Sent;
            else if(sentToEsb && !sentToActiveMq)
                messageStatusId = MessageStatusTypes.SentToEsb;
            else if(!sentToEsb && sentToActiveMq)
                messageStatusId = MessageStatusTypes.SentToActiveMq;
            else
                messageStatusId = MessageStatusTypes.Ready;
            return messageStatusId;
        }

        private bool SendToEsb(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            bool sent = false;
            try{
                producer.Send(xmlMessage, messageProperties);
                sent = true;
            }
            catch(Exception ex)
            {
                logger.Error(string.Format("Failed to send message {0} to ESB.  Error: {1}", messageProperties["IconMessageID"], ex.ToString()));
                sent = false;
            }
            return sent;
        }

        private bool SendToActiveMq(string xmlMessage, Dictionary<string, string> messageProperties)
        {
            bool sent = false;
            try{
                activeMqProducer.Send(xmlMessage, messageProperties);
                sent = true;
            }
            catch(Exception ex)
            {
                logger.Error(string.Format("Failed to send message {0} to ActiveMQ.  Error: {1}", messageProperties["IconMessageID"], ex.ToString()));
                sent = false;
            }
            return sent;
        }

        private void SetProcessedDate(List<MessageQueueLocale> messagesToUpdate)
        {
            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueLocale>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = messagesToUpdate
            };

            setProcessedDateCommandHandler.Execute(command);
        }

        private void AssociateSavedMessageToMessageQueue(List<MessageQueueLocale> associatedMessages, MessageHistory messageHistory)
        {
            var command = new AssociateMessageToQueueCommand<MessageQueueLocale, MessageHistory>
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

        private MessageHistory BuildXmlMessage(string xml, Dictionary<string, string>MessageProperties)
        {
            // ESB wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
            // the database will happily store it.
            xml = new StringBuilder(xml).Replace("utf-8", "utf-16").ToString();

            return new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.Locale,
                Message = xml,
                MessageHeader = JsonConvert.SerializeObject(messageProperties),
                InsertDate = DateTime.Now,
                InProcessBy = ControllerType.Instance,
                ProcessedDate = null
            };
        }

        private void MarkQueuedMessagesAsFailed(List<MessageQueueLocale> failedMessages)
        {
            var command = new UpdateMessageQueueStatusCommand<MessageQueueLocale>
            {
                QueuedMessages = failedMessages,
                MessageStatusId = MessageStatusTypes.Failed,
                ResetInProcessBy = true
            };

            updateMessageQueueStatusCommandHandler.Execute(command);
        }

        private string SerializeMiniBulk(Contracts.LocaleType miniBulk)
        {
            return serializer.Serialize(miniBulk, new Utf8StringWriter());
        }

        private Contracts.LocaleType PrepareMiniBulk(List<MessageQueueLocale> messagesToBundle)
        {
            return queueReader.BuildMiniBulk(messagesToBundle);
        }

        private List<MessageQueueLocale> GroupMessagesForMiniBulk(List<MessageQueueLocale> messagesToGroup)
        {
            return queueReader.GroupMessagesForMiniBulk(messagesToGroup);
        }

        private List<MessageQueueLocale> GetQueuedMessages()
        {
            return queueReader.GetQueuedMessages();
        }

        private void MarkQueuedMessagesAsInProcess()
        {
            // LookAhead can default to 1 since locale messages aren't bundled.
            int lookAhead = 1;

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueLocale>
            {
                LookAhead = lookAhead,
                Instance = ControllerType.Instance
            };

            markQueuedEntriesAsInProcessCommandHandler.Execute(command);
        }
    }
}