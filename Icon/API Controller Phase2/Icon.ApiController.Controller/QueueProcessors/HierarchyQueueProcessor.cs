using Icon.ApiController.Common;
using Icon.ApiController.Controller.ControllerConstants;
using Icon.ApiController.Controller.Monitoring;
using Icon.ApiController.Controller.QueueReaders;
using Icon.ApiController.Controller.Serializers;
using Icon.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Queries;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;
using Icon.ActiveMQ.Producer;

namespace Icon.ApiController.Controller.QueueProcessors
{
    public class HierarchyQueueProcessor : IQueueProcessor
    {
        private ApiControllerSettings settings;
        private ILogger<HierarchyQueueProcessor> logger;
        private IQueueReader<MessageQueueHierarchy, Contracts.HierarchyType> queueReader;
        private ISerializer<Contracts.HierarchyType> serializer;
        private IQueryHandler<GetFinancialHierarchyClassesParameters, List<HierarchyClass>> getFinancialHierarchyClassesQueryHandler;
        private ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler;
        private ICommandHandler<AssociateMessageToQueueCommand<MessageQueueHierarchy, MessageHistory>> associateMessageToQueueCommandHandler;
        private ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueHierarchy>> setProcessedDateCommandHandler;
        private ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>> updateMessageQueueStatusCommandHandler;
        private ICommandHandler<UpdateStagedProductStatusCommand> updateStagedProductStatusCommandHandler;
        private ICommandHandler<UpdateSentToEsbHierarchyTraitCommand> updateSentToEsbHierarchyTraitCommandHandler;
        private ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueHierarchy>> markQueuedEntriesAsInProcessCommandHandler;
        private IActiveMQProducer activeMqProducer;
        private Dictionary<string, string> messageProperties;
        private IMessageProcessorMonitor monitor;
        private APIMessageProcessorLogEntry monitorData;

        public HierarchyQueueProcessor(
            ApiControllerSettings settings,
            ILogger<HierarchyQueueProcessor> logger,
            IQueueReader<MessageQueueHierarchy, Contracts.HierarchyType> queueReader,
            ISerializer<Contracts.HierarchyType> serializer,
            IQueryHandler<GetFinancialHierarchyClassesParameters, List<HierarchyClass>> getFinancialHierarchyClassesQueryHandler,
            ICommandHandler<SaveToMessageHistoryCommand<MessageHistory>> saveToMessageHistoryCommandHandler,
            ICommandHandler<AssociateMessageToQueueCommand<MessageQueueHierarchy, MessageHistory>> associateMessageToQueueCommandHandler,
            ICommandHandler<UpdateMessageQueueProcessedDateCommand<MessageQueueHierarchy>> setProcessedDateCommandHandler,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageQueueStatusCommand<MessageQueueHierarchy>> updateMessageQueueStatusCommandHandler,
            ICommandHandler<UpdateStagedProductStatusCommand> updateStagedProductStatusCommandHandler,
            ICommandHandler<UpdateSentToEsbHierarchyTraitCommand> updateSentToEsbHierarchyTraitCommandHandler,
            ICommandHandler<MarkQueuedEntriesAsInProcessCommand<MessageQueueHierarchy>> markQueuedEntriesAsInProcessCommandHandler,
            IMessageProcessorMonitor monitor,
            IActiveMQProducer activeMqProducer)
        {
            this.settings = settings;
            this.logger = logger;
            this.queueReader = queueReader;
            this.serializer = serializer;
            this.getFinancialHierarchyClassesQueryHandler = getFinancialHierarchyClassesQueryHandler;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.associateMessageToQueueCommandHandler = associateMessageToQueueCommandHandler;
            this.setProcessedDateCommandHandler = setProcessedDateCommandHandler;
            this.updateMessageHistoryCommandHandler = updateMessageHistoryCommandHandler;
            this.updateMessageQueueStatusCommandHandler = updateMessageQueueStatusCommandHandler;
            this.updateStagedProductStatusCommandHandler = updateStagedProductStatusCommandHandler;
            this.updateSentToEsbHierarchyTraitCommandHandler = updateSentToEsbHierarchyTraitCommandHandler;
            this.markQueuedEntriesAsInProcessCommandHandler = markQueuedEntriesAsInProcessCommandHandler;
            this.monitor = monitor;
            this.activeMqProducer = activeMqProducer;
            this.monitorData = new APIMessageProcessorLogEntry()
            {
                MessageTypeID = MessageTypes.Hierarchy
            };
        }

        public void ProcessMessageQueue()
        {
            monitorData.StartTime = DateTime.UtcNow;
            SetMessageProperties();
            PopulateFinancialClassesCache();
            MarkQueuedMessagesAsInProcess();

            var messagesReadyToProcess = GetQueuedMessages();

            var shouldRecordMonitorData = messagesReadyToProcess.Count > 0;
            while (messagesReadyToProcess.Count > 0)
            {
                var messagesReadyForMiniBulk = GroupMessagesForMiniBulk(messagesReadyToProcess);

                if (messagesReadyForMiniBulk.Count > 0)
                {
                    var miniBulk = PrepareMiniBulk(messagesReadyForMiniBulk);

                    if (miniBulk.@class.Length > 0)
                    {
                        var hierarchyClassesInMiniBulk = miniBulk.@class.Select(hc => hc.id).ToList();
                        var messagesReadyToSerialize = messagesReadyForMiniBulk.Where(m => hierarchyClassesInMiniBulk.Contains(m.HierarchyClassId)).ToList();

                        string xml = SerializeMiniBulk(miniBulk);

                        if (string.IsNullOrEmpty(xml))
                        {
                            MarkQueuedMessagesAsFailed(messagesReadyToSerialize);
                            monitorData.CountFailedMessages = monitorData.CountFailedMessages.GetValueOrDefault(0) + messagesReadyToSerialize.Count;
                        }
                        else
                        {
                            var hierarchyMessage = BuildXmlMessage(xml, messageProperties);

                            SaveXmlMessageToMessageHistory(hierarchyMessage);

                            AssociateSavedMessageToMessageQueue(messagesReadyToSerialize, hierarchyMessage);

                            SetProcessedDate(messagesReadyToSerialize);
                            
                            int messageStatusId = PublishMessage(hierarchyMessage.Message, hierarchyMessage.MessageHistoryId);

                            ProcessResponse(messageStatusId, hierarchyMessage, hierarchyClassesInMiniBulk, miniBulk.name);

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

                logger.Debug("Ending the main processing loop.  Now preparing to retrieve a new set of queued messages.");

                MarkQueuedMessagesAsInProcess();
                messagesReadyToProcess = GetQueuedMessages();
            }

            logger.Debug("Ending the Hierarchy queue processor.  No further queued messages were found in Ready status.");

            

            monitorData.EndTime = DateTime.UtcNow;
            if (shouldRecordMonitorData)
            {
                monitor.RecordResults(monitorData);
            }
        }

        private void SetMessageProperties()
        {
            messageProperties = new Dictionary<string, string>
          {
            { "IconMessageID", string.Empty },
            { "Source", "Icon" },
            { "TransactionType", "Hierarchy" }
          };

            var nonNonReceiving = String.Format("{0},{1}", settings.NonReceivingSystemsAll, settings.NonReceivingSystemsHierarchy).Split(',')
              .Where(x => !String.IsNullOrWhiteSpace(x)).ToArray();

            if(nonNonReceiving.Any())
            {
                messageProperties.Add(EsbConstants.NonReceivingSystemsJmsProperty, String.Join(",", nonNonReceiving));
            }
        }

        private void ProcessResponse(int messageStatusId, MessageHistory message, List<string> publishedHierarchyClassesByIdAsString, string hierarchyName)
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

                // For the case of Financial hierarchy, we'll need to translate the 4-digit subteam number to the actual hierarchyClassID in order to properly
                // execute the next two commands.

                List<int> publishedHierarchyClassesByIdAsInteger = new List<int>();

                if (hierarchyName == HierarchyNames.Financial)
                {
                    foreach (string subteamNumber in publishedHierarchyClassesByIdAsString)
                    {
                        publishedHierarchyClassesByIdAsInteger.Add(Cache.financialSubteamToHierarchyClassId[subteamNumber]);
                    }
                }
                else
                {
                    publishedHierarchyClassesByIdAsInteger = publishedHierarchyClassesByIdAsString.Select(hc => int.Parse(hc)).ToList();
                }

                // Icon is not the source system for tax classes, so the Sent To ESB trait will not be maintained for them.
                if (hierarchyName != HierarchyNames.Tax)
                {
                    var updateSentToEsbHierarchyTraitCommand = new UpdateSentToEsbHierarchyTraitCommand
                    {
                        PublishedHierarchyClasses = publishedHierarchyClassesByIdAsInteger
                    };

                    updateSentToEsbHierarchyTraitCommandHandler.Execute(updateSentToEsbHierarchyTraitCommand);
                }

                var updateStagedProductStatusCommand = new UpdateStagedProductStatusCommand
                {
                    PublishedHierarchyClasses = publishedHierarchyClassesByIdAsInteger
                };

                updateStagedProductStatusCommandHandler.Execute(updateStagedProductStatusCommand);
            }
            else
            {
                logger.Error(string.Format("Message {0} failed to send.  Message will remain in Ready state for re-processing during the next controller execution.", message.MessageHistoryId));
            }
        }

        private int PublishMessage(string xml, int messageHistoryId)
        {
            logger.Info(string.Format("Preparing to send message {0}.", messageHistoryId));

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

        private bool SendToActiveMq(String xmlMessage, Dictionary<string, string> messageProperties)
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

        private void SetProcessedDate(List<MessageQueueHierarchy> messagesToUpdate)
        {
            var command = new UpdateMessageQueueProcessedDateCommand<MessageQueueHierarchy>
            {
                ProcessedDate = DateTime.Now,
                MessagesToUpdate = messagesToUpdate
            };

            setProcessedDateCommandHandler.Execute(command);
        }

        private void AssociateSavedMessageToMessageQueue(List<MessageQueueHierarchy> associatedMessages, MessageHistory messageHistory)
        {
            var command = new AssociateMessageToQueueCommand<MessageQueueHierarchy, MessageHistory>
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
                MessageTypeId = MessageTypes.Hierarchy,
                Message = xml,
                MessageHeader = JsonConvert.SerializeObject(messageProperties),
                InsertDate = DateTime.Now,
                InProcessBy = ControllerType.Instance,
                ProcessedDate = null
            };
        }

        private void MarkQueuedMessagesAsFailed(List<MessageQueueHierarchy> failedMessages)
        {
            var command = new UpdateMessageQueueStatusCommand<MessageQueueHierarchy>
            {
                QueuedMessages = failedMessages,
                MessageStatusId = MessageStatusTypes.Failed,
                ResetInProcessBy = true
            };

            updateMessageQueueStatusCommandHandler.Execute(command);
        }

        private string SerializeMiniBulk(Contracts.HierarchyType miniBulk)
        {
            return serializer.Serialize(miniBulk, new Utf8StringWriter());
        }

        private Contracts.HierarchyType PrepareMiniBulk(List<MessageQueueHierarchy> messagesToBundle)
        {
            return queueReader.BuildMiniBulk(messagesToBundle);
        }

        private List<MessageQueueHierarchy> GroupMessagesForMiniBulk(List<MessageQueueHierarchy> messagesToGroup)
        {
            return queueReader.GroupMessagesForMiniBulk(messagesToGroup);
        }

        private List<MessageQueueHierarchy> GetQueuedMessages()
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

            var command = new MarkQueuedEntriesAsInProcessCommand<MessageQueueHierarchy>
            {
                LookAhead = lookAhead,
                Instance = ControllerType.Instance
            };

            markQueuedEntriesAsInProcessCommandHandler.Execute(command);
        }

        private void PopulateFinancialClassesCache()
        {
            var financialClasses = getFinancialHierarchyClassesQueryHandler.Search(new GetFinancialHierarchyClassesParameters());

            foreach (var financialClass in financialClasses)
            {
                string subteamNumber = financialClass.hierarchyClassName.Split('(')[1].TrimEnd(')');
                int hierarchyClassId = financialClass.hierarchyClassID;

                Cache.financialSubteamToHierarchyClassId.Add(subteamNumber, hierarchyClassId);
            }
        }
    }
}
