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
            IMessageProcessorMonitor monitor)
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
                                var productMessage = BuildXmlMessage(xml);
                                SaveXmlMessageToMessageHistory(productMessage);
                                AssociateSavedMessageToMessageQueue(messagesReadyToSerialize, productMessage);
                                SetProcessedDate(messagesReadyToSerialize);
                                bool messageSent = PublishMessage(productMessage);
                                ProcessResponse(messageSent, productMessage);

                                if (messageSent)
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

                logger.Info("Ending the main processing loop.  Now preparing to retrieve a new set of queued messages.");

                MarkQueuedMessagesAsInProcess();
                messagesReadyToProcess = GetQueuedMessages();
            }

            logger.Info("Ending the Product queue processor.  No further queued messages were found in Ready status.");

            producer.Dispose();

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
            messageProperties = new Dictionary<string, string>();
            messageProperties.Add("IconMessageID", String.Empty);
            messageProperties.Add("Source", "Icon");
			messageProperties.Add("TransactionType", "Global Item");

			if (!String.IsNullOrWhiteSpace(settings.NonReceivingSystemsAll))
            {
                messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, settings.NonReceivingSystemsAll);
            }
        }

        private void ProcessResponse(bool messageSent, MessageHistory message)
        {
            if (messageSent)
            {
                logger.Info(String.Format("Message {0} has been sent successfully.", message.MessageHistoryId));

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

        private bool PublishMessage(MessageHistory messageHistory)
        {
            logger.Info(String.Format("Preparing to send message {0}.", messageHistory.MessageHistoryId));
            try
            {
                messageProperties["IconMessageID"] = messageHistory.MessageHistoryId.ToString();
                
                producer.Send(messageHistory.Message, messageProperties);

                return true;
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Failed to send message {0}.  Error: {1}", messageHistory.MessageHistoryId, ex.ToString()));
                return false;
            }
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

        private MessageHistory BuildXmlMessage(string xml)
        {
            // ESB wants the xml in utf-8 encoding, but SQL Server wants it as utf-16.  This will replace the encoding in the xml header so that
            // the database will happily store it.
            xml = new StringBuilder(xml).Replace("utf-8", "utf-16").ToString();

            return new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.Product,
                Message = xml,
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
