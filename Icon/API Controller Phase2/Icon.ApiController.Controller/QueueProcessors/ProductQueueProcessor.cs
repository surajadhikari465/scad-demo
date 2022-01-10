using Icon.ApiController.Common;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Icon.ApiController.Controller.ControllerConstants;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using System.Configuration;
using Newtonsoft.Json;
using Icon.ActiveMQ.Producer;

namespace Icon.ApiController.Controller.QueueProcessors
{
    public class ProductQueueProcessor : IQueueProcessor
    {
        private ApiControllerSettings settings;
        private ILogger<ProductQueueProcessor> logger;
        private IQueueReader<MessageQueueProduct, Contracts.items> queueReader;
        private ISerializer<Contracts.items> serializer;
        private ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler;
        private ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProduct, MessageHistory>> associateMessageToQueueCommandHandler;
        private ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProduct>> setProcessedDateCommandHandler;
        private ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>> updateMessageQueueStatusCommandHandler;
        private ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProduct>> markQueuedEntriesAsInProcessCommandHandler;
        private IEsbProducer producer;
        private IActiveMQProducer activeMqProducer;
        private Dictionary<string, string> messageProperties;
        private IMessageProcessorMonitor monitor;
        private APIMessageProcessorLogEntry monitorData;

        public ProductQueueProcessor(
            ApiControllerSettings settings,
            ILogger<ProductQueueProcessor> logger,
            IQueueReader<MessageQueueProduct, Contracts.items> queueReader,
            ISerializer<Contracts.items> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<MessageQueueProduct, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueProduct>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueProduct>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueProduct>> markQueuedEntriesAsInProcessCommandHandler,
            IEsbProducer producer,
            IMessageProcessorMonitor monitor,
            IActiveMQProducer activeMqProducer)
        {
            this.settings = settings;
            this.logger = logger;
            this.serializer = serializer;
            this.queueReader = queueReader;
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
                MessageTypeID = MessageTypes.Product
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

                    if (miniBulk.item.Length > 0)
                    {
                        var itemsInMiniBulk = miniBulk.item.Select(i => i.id).ToList();
                        var messagesReadyToSerialize = messagesReadyForMiniBulk.Where(m => itemsInMiniBulk.Contains(m.ItemId)).ToList();

                        string xml = SerializeMiniBulk(miniBulk);

                        if (String.IsNullOrEmpty(xml))
                        {
                            MarkQueuedMessagesAsFailed(messagesReadyToSerialize);
                            monitorData.CountFailedMessages = monitorData.CountFailedMessages.GetValueOrDefault(0) + messagesReadyToSerialize.Count;
                        }
                        else
                        {
                            bool nonRetailMessages = messagesReadyToSerialize[0].ItemTypeCode == ItemTypeCodes.NonRetail;

                            if (!SetNonReceivingSystemsProperties(nonRetailMessages))
                            {
                                MarkQueuedMessagesAsFailed(messagesReadyToSerialize);
                            }
                            else
                            {
                                var productMessage = BuildXmlMessage(xml, messageProperties);
                                SaveXmlMessageToMessageHistory(productMessage);
                                AssociateSavedMessageToMessageQueue(messagesReadyToSerialize, productMessage);
                                SetProcessedDate(messagesReadyToSerialize);
                                int messageStatusId = PublishMessage(productMessage);
                                ProcessResponse(messageStatusId, productMessage);

                                if (messageStatusId == MessageStatusTypes.Sent)
                                {
                                    monitorData.CountProcessedMessages = monitorData.CountProcessedMessages.GetValueOrDefault(0) + messagesReadyToSerialize.Count;
                                }
                                else
                                {
                                    monitorData.CountFailedMessages = monitorData.CountFailedMessages.GetValueOrDefault(0) + messagesReadyToSerialize.Count;
                                }
                            }
                        }
                    }
                }

                logger.Debug("Ending the main processing loop.  Now preparing to retrieve a new set of queued messages.");

                MarkQueuedMessagesAsInProcess();
                messagesReadyToProcess = GetQueuedMessages();
            }

            logger.Debug("Ending the Product queue processor.  No further queued messages were found in Ready status.");

            

            monitorData.EndTime = DateTime.UtcNow;
            if (shouldRecordMonitorData)
            {
                monitor.RecordResults(monitorData);
            }
        }

        private bool SetNonReceivingSystemsProperties(bool nonRetailMessage)
        {
            try
            {
                if (nonRetailMessage)
                {
                    if (!messageProperties.ContainsKey(EsbConstants.NonReceivingSystemsJmsProperty))
                    {
                        messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, "R10");
                    }
                    else if (!messageProperties[EsbConstants.NonReceivingSystemsJmsProperty].Contains("R10"))
                    {
                        messageProperties[EsbConstants.NonReceivingSystemsJmsProperty] = "R10," + messageProperties[EsbConstants.NonReceivingSystemsJmsProperty];
                    }
                }
                else
                {
                    messageProperties[EsbConstants.NonReceivingSystemsJmsProperty] = settings.NonReceivingSystemsAll;
                }

            }
            catch(Exception ex)
            {
                logger.Error(String.Format("Failed when setting NonReceivingSystemsJmsProperty properties  Error: {0}", ex.ToString()));
                return false;
            }
            return true;
        }

        private void SetMessageProperties()
        {
          messageProperties = new Dictionary<string, string>
          {
            { "IconMessageID", String.Empty },
            { "Source", "Icon" },
            { "TransactionType", "Global Item" }
          };

          var nonNonReceiving = String.Format("{0},{1}", settings.NonReceivingSystemsAll, settings.NonReceivingSystemsProduct).Split(',')
            .Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();
          
          if(nonNonReceiving.Any())
          {
            messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, String.Join(",", nonNonReceiving));
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
            else if(messageStatusId == MessageStatusTypes.Ready)
            {
                logger.Error(String.Format("Message {0} failed to send.  Message will remain in Ready state for re-processing during the next controller execution.", message.MessageHistoryId));
            }
            else
            {
                // either sent to ESB or ActiveMQ not both
                logger.Error(String.Format("Message {0} has not been sent to one of the two Brokers. Message will be resend to that Broker during the next controller execution", message.MessageHistoryId));

                var updateMessageHistoryCommand = new UpdateMessageHistoryStatusCommand<MessageHistory>
                {
                    Message = message,
                    MessageStatusId = messageStatusId
                };

                updateMessageHistoryCommandHandler.Execute(updateMessageHistoryCommand);
            }
        }

        private int PublishMessage(MessageHistory messageHistory)
        {
            logger.Info(String.Format("Preparing to send message {0}.", messageHistory.MessageHistoryId));
            bool sentToEsb = false;
            bool sentToActiveMq = false;
            messageProperties["IconMessageID"] = messageHistory.MessageHistoryId.ToString();   
            sentToEsb = SendToEsb(messageHistory, messageProperties);
            sentToActiveMq = SendToActiveMq(messageHistory, messageProperties);
            
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

        private bool SendToEsb(MessageHistory message, Dictionary<string, string> messageProperties)
        {
            bool sent = false;
            try{
                producer.Send(message.Message, messageProperties);
                sent = true;
            }
            catch(Exception ex)
            {
                logger.Error(string.Format("Failed to send message {0} to ESB.  Error: {1}", message.MessageHistoryId, ex.ToString()));
                sent = false;
            }
            return sent;
        }

        private bool SendToActiveMq(MessageHistory message, Dictionary<string, string> messageProperties)
        {
            bool sent = false;
            try{
                activeMqProducer.Send(message.Message, messageProperties);
                sent = true;
            }
            catch(Exception ex)
            {
                logger.Error(string.Format("Failed to send message {0} to ActiveMQ.  Error: {1}", message.MessageHistoryId, ex.ToString()));
                sent = false;
            }
            return sent;
        }

        private void SetProcessedDate(List<MessageQueueProduct> messagesToUpdate)
        {
            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueProduct>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = messagesToUpdate
            };

            setProcessedDateCommandHandler.Execute(command);
        }

        private void AssociateSavedMessageToMessageQueue(List<MessageQueueProduct> associatedMessages, MessageHistory messageHistory)
        {
            var command = new AssociateMessageToQueueCommand<MessageQueueProduct, MessageHistory>
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
            // ESB wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
            // the database will happily store it.
            xml = new StringBuilder(xml).Replace("utf-8", "utf-16").ToString();

            return new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.Product,
                Message = xml,
                MessageHeader = JsonConvert.SerializeObject(messageProperties),
                InsertDate = DateTime.Now,
                InProcessBy = ControllerType.Instance,
                ProcessedDate = null
            };
        }

        private void MarkQueuedMessagesAsFailed(List<MessageQueueProduct> failedMessages)
        {
            var command = new UpdateMessageQueueStatusCommand<MessageQueueProduct>
            {
                QueuedMessages = failedMessages,
                MessageStatusId = MessageStatusTypes.Failed,
                ResetInProcessBy = true
            };

            updateMessageQueueStatusCommandHandler.Execute(command);
        }

        private string SerializeMiniBulk(Contracts.items miniBulk)
        {
            return serializer.Serialize(miniBulk, new Utf8StringWriter());
        }

        private Contracts.items PrepareMiniBulk(List<MessageQueueProduct> messagesToBundle)
        {
            return queueReader.BuildMiniBulk(messagesToBundle);
        }

        private List<MessageQueueProduct> GroupMessagesForMiniBulk(List<MessageQueueProduct> messagesToGroup)
        {
            return queueReader.GroupMessagesForMiniBulk(messagesToGroup);
        }

        private List<MessageQueueProduct> GetQueuedMessages()
        {
            return queueReader.GetQueuedMessages();
        }

        private void MarkQueuedMessagesAsInProcess()
        {
            int lookAhead;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["QueueLookAhead"], out lookAhead))
            {
                lookAhead = 1000;
            }

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueProduct>
            {
                LookAhead = lookAhead,
                Instance = ControllerType.Instance
            };

            markQueuedEntriesAsInProcessCommandHandler.Execute(command);
        }
    }
}