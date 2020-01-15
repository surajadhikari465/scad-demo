using Icon.ApiController.Common;
using Icon.ApiController.Controller.ControllerConstants;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Esb.Producer;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Newtonsoft.Json;

namespace Icon.ApiController.Controller.QueueProcessors
{
    public class ItemLocaleQueueProcessor : IQueueProcessor
    {
        private ApiControllerSettings settings;
        private ILogger<ItemLocaleQueueProcessor> logger;
        private IQueueReader<MessageQueueItemLocale, Contracts.items> queueReader;
        private ISerializer<Contracts.items> serializer;
        private ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler;
        private ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>> associateMessageToQueueCommandHandler;
        private ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>> setProcessedDateCommandHandler;
        private ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>> updateMessageQueueStatusCommandHandler;
        private ICommandHandler<UpdateInProcessBusinessUnitCommand> updateInProcessBusinessUnitCommandHandler;
        private ICommandHandler<ClearBusinessUnitInProcessCommand> clearBusinessUnitInProcessCommandHandler;
        private IQueryHandler<GetNextAvailableBusinessUnitParameters, int?> getNextAvailableBusinessUnitQueryHandler;
        private ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>> markQueuedEntriesAsInProcessCommandHandler;
        private IEsbProducer producer;
        private Dictionary<string, string> messageProperties;
        private IMessageProcessorMonitor monitor;
        private APIMessageProcessorLogEntry monitorData;

        public ItemLocaleQueueProcessor(
            ApiControllerSettings settings,
            ILogger<ItemLocaleQueueProcessor> logger,
            IQueueReader<MessageQueueItemLocale, Contracts.items> queueReader,
            ISerializer<Contracts.items> serializer,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueItemLocale>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<UpdateInProcessBusinessUnitCommand> updateInProcessBusinessUnitCommandHandler,
            ICommandHandler<ClearBusinessUnitInProcessCommand> clearBusinessUnitInProcessCommandHandler,
            IQueryHandler<GetNextAvailableBusinessUnitParameters, int?> getNextAvailableBusinessUnitQueryHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>> markQueuedEntriesAsInProcessCommandHandler,
            IEsbProducer producer,
            IMessageProcessorMonitor monitor)
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
            this.updateInProcessBusinessUnitCommandHandler = updateInProcessBusinessUnitCommandHandler;
            this.clearBusinessUnitInProcessCommandHandler = clearBusinessUnitInProcessCommandHandler;
            this.getNextAvailableBusinessUnitQueryHandler = getNextAvailableBusinessUnitQueryHandler;
            this.markQueuedEntriesAsInProcessCommandHandler = markQueuedEntriesAsInProcessCommandHandler;
            this.producer = producer;
            this.monitor = monitor;
            this.monitorData = new APIMessageProcessorLogEntry()
            {
                MessageTypeID = MessageTypes.ItemLocale
            };
        }

        public void ProcessMessageQueue()
        {
            SetMessageProperties();

            int? businessUnit = GetNextAvailableBusinessUnit(typeof(MessageQueueItemLocale).Name, settings.Instance);
            while (businessUnit.HasValue)
            {
                ProcessMessageQueue(businessUnit);

                businessUnit = GetNextAvailableBusinessUnit(typeof(MessageQueueItemLocale).Name, settings.Instance);
            }

            logger.Debug("Ending the ItemLocale queue processor.  No further queued messages were found in Ready status.");

            producer.Dispose();
        }

        private void ProcessMessageQueue(int? businessUnit)
        {
            monitorData.StartTime = DateTime.UtcNow;
            UpdateInProcessBusinessUnit(settings.Instance, businessUnit.Value, MessageTypes.ItemLocale);
            MarkQueuedMessagesAsInProcess(businessUnit.Value);

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
                            var itemLocaleMessage = BuildXmlMessage(xml, messageProperties);

                            SaveXmlMessageToMessageHistory(itemLocaleMessage);

                            AssociateSavedMessageToMessageQueue(messagesReadyToSerialize, itemLocaleMessage);

                            SetProcessedDate(messagesReadyToSerialize);

                            bool messageSent = PublishMessage(itemLocaleMessage.Message, itemLocaleMessage.MessageHistoryId);

                            ProcessResponse(messageSent, itemLocaleMessage);

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

                logger.Debug("Ending the main processing loop.  Now preparing to retrieve a new set of queued messages.");

                MarkQueuedMessagesAsInProcess(businessUnit.Value);
                messagesReadyToProcess = GetQueuedMessages();
            }

            logger.Info(String.Format("All marked message queue entries for business unit {0} have been processed.  Getting the next business unit in the queue...",
                businessUnit.HasValue ? businessUnit.Value.ToString() : "error"));

            ClearBusinessUnitInProcess(settings.Instance, MessageTypes.ItemLocale);

            monitorData.EndTime = DateTime.UtcNow;
            if (shouldRecordMonitorData)
            {
                monitor.RecordResults(monitorData);
            }
        }

        private void ClearBusinessUnitInProcess(int instance, int messageTypeId)
        {
            clearBusinessUnitInProcessCommandHandler.Execute(new ClearBusinessUnitInProcessCommand { InstanceId = instance, MessageTypeId = messageTypeId });
        }

        private void MarkQueuedMessagesAsInProcess(int businessUnit)
        {
            int lookAhead;
            if (!int.TryParse(ConfigurationManager.AppSettings["QueueLookAhead"], out lookAhead))
            {
                lookAhead = 1000;
            }

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>
            {
                LookAhead = lookAhead,
                Instance = ControllerType.Instance,
                BusinessUnit = businessUnit
            };

            markQueuedEntriesAsInProcessCommandHandler.Execute(command);
        }

        private void UpdateInProcessBusinessUnit(int instance, int businessUnit, int messageTypeId)
        {
            updateInProcessBusinessUnitCommandHandler.Execute(new UpdateInProcessBusinessUnitCommand
            {
                InstanceId = instance,
                BusinessUnitId = businessUnit,
                MessageTypeId = messageTypeId
            });
        }

        private int? GetNextAvailableBusinessUnit(string messageQueueName, int instance)
        {
            return getNextAvailableBusinessUnitQueryHandler.Search(new GetNextAvailableBusinessUnitParameters { MessageQueueName = messageQueueName, InstanceId = instance });
        }

        private void SetMessageProperties()
        {
            messageProperties = new Dictionary<string, string>();
            messageProperties.Add("IconMessageID", String.Empty);
            messageProperties.Add("Source", "Icon");
			messageProperties.Add("TransactionType", "Item/Locale");

			if (!String.IsNullOrWhiteSpace(settings.NonReceivingSystemsAll) && !String.IsNullOrWhiteSpace(settings.NonReceivingSystemsItemLocale))
            {
                messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, settings.NonReceivingSystemsAll + "," + settings.NonReceivingSystemsItemLocale);
            }
            else if (!String.IsNullOrWhiteSpace(settings.NonReceivingSystemsAll))
            {
                messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, settings.NonReceivingSystemsAll);
            }
            else if (!String.IsNullOrWhiteSpace(settings.NonReceivingSystemsItemLocale))
            {
                messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, settings.NonReceivingSystemsItemLocale);
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

        private bool PublishMessage(string xml, int messageHistoryId)
        {
            logger.Info(String.Format("Preparing to send message {0}.", messageHistoryId));

            try
            {
                messageProperties["IconMessageID"] = messageHistoryId.ToString();
                producer.Send(xml, messageProperties);
                return true;
            }
            catch (Exception ex)
            {
                logger.Error(String.Format("Failed to send message {0}.  Error details: {1}", messageHistoryId, ex.ToString()));
                return false;
            }
        }

        private void SetProcessedDate(List<MessageQueueItemLocale> messagesToUpdate)
        {
            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = messagesToUpdate
            };

            setProcessedDateCommandHandler.Execute(command);
        }

        private void AssociateSavedMessageToMessageQueue(List<MessageQueueItemLocale> associatedMessages, MessageHistory messageHistory)
        {
            var command = new AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>
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
                MessageTypeId = MessageTypes.ItemLocale,
                Message = xml,
                MessageHeader = JsonConvert.SerializeObject(messageProperties),
                InsertDate = DateTime.Now,
                InProcessBy = ControllerType.Instance,
                ProcessedDate = null
            };
        }

        private void MarkQueuedMessagesAsFailed(List<MessageQueueItemLocale> failedMessages)
        {
            var command = new UpdateMessageQueueStatusCommand<MessageQueueItemLocale>
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

        private Contracts.items PrepareMiniBulk(List<MessageQueueItemLocale> messagesToBundle)
        {
            return queueReader.BuildMiniBulk(messagesToBundle);
        }

        private List<MessageQueueItemLocale> GroupMessagesForMiniBulk(List<MessageQueueItemLocale> messagesToGroup)
        {
            return queueReader.GroupMessagesForMiniBulk(messagesToGroup);
        }

        private List<MessageQueueItemLocale> GetQueuedMessages()
        {
            return queueReader.GetQueuedMessages();
        }
    }
}
