using Icon.ApiController.Common;
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
using Icon.ApiController.Controller.ControllerConstants;

namespace Icon.ApiController.Controller.HistoryProcessors
{
    public class MessageHistoryProcessor : IHistoryProcessor
    {
        private ApiControllerSettings settings;
        private ILogger<MessageHistoryProcessor> logger;
        private ICommandHandler<MarkUnsentMessagesAsInProcessCommand> markUnsentMessagesAsInProcessCommandHandler;
        private IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>> getMessageHistoryQuery;
        private ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler;
        private ICommandHandler<UpdateStagedProductStatusCommand> updateStagedProductStatusCommandHandler;
        private ICommandHandler<UpdateSentToEsbHierarchyTraitCommand> updateSentToEsbHierarchyTraitCommandHandler;
        private IEsbProducer producer;
        private int messageTypeId;

        public MessageHistoryProcessor(
            ApiControllerSettings settings,
            ILogger<MessageHistoryProcessor> logger,
            ICommandHandler<MarkUnsentMessagesAsInProcessCommand> markUnsentMessagesAsInProcessCommandHandler,
            IQueryHandler<GetMessageHistoryParameters, List<MessageHistory>> getMessageHistoryQuery,
            ICommandHandler<UpdateMessageHistoryStatusCommand<MessageHistory>> updateMessageHistoryCommandHandler,
            ICommandHandler<UpdateStagedProductStatusCommand> updateStagedProductStatusCommandHandler,
            ICommandHandler<UpdateSentToEsbHierarchyTraitCommand> updateSentToEsbHierarchyTraitCommandHandler,
            IEsbProducer producer,
            int messageTypeId)
        {
            this.settings = settings;
            this.logger = logger;
            this.markUnsentMessagesAsInProcessCommandHandler = markUnsentMessagesAsInProcessCommandHandler;
            this.getMessageHistoryQuery = getMessageHistoryQuery;
            this.updateMessageHistoryCommandHandler = updateMessageHistoryCommandHandler;
            this.updateStagedProductStatusCommandHandler = updateStagedProductStatusCommandHandler;
            this.updateSentToEsbHierarchyTraitCommandHandler = updateSentToEsbHierarchyTraitCommandHandler;
            this.producer = producer;
            this.messageTypeId = messageTypeId;
        }

        public void ProcessMessageHistory()
        {
            MarkUnsentMessagesAsInProcess();

            var messagesReadyToSend = GetUnsentMessages();

            while (messagesReadyToSend.Count > 0)
            {
                bool messageSent;

                foreach (var message in messagesReadyToSend)
                {
                    try
                    {
                        //set message properties
                        var messageProperties = SetMessageProperties(message);
                        
                        //send the message
                        producer.Send(message.Message, messageProperties);
                        messageSent = true;                    
                    }
                    catch (Exception ex)
                    {
                        logger.Error(string.Format("Failed to send message {0}.  Error: {1}", message.MessageHistoryId, ex.ToString()));
                        messageSent = false;
                    }

                    ProcessResponse(messageSent, message);
                }

                logger.Info("Ending the main processing loop.  Now preparing to retrieve a new set of unsent messages.");

                MarkUnsentMessagesAsInProcess();
                messagesReadyToSend = GetUnsentMessages();
            }

            logger.Info("Ending the MessageHistory processor.  No further unsent messages were found.");
        }

        private void EnsureNonReceivingSystemPropertiesAreSet(MessageHistory message,
            Dictionary<string, string> messageProperties, string nonReceivingSystemsJmsKey, string nonReceivingSystemsAllValue)
        {
            if (!string.IsNullOrWhiteSpace(nonReceivingSystemsAllValue))
            {
                messageProperties.Add(nonReceivingSystemsJmsKey, nonReceivingSystemsAllValue);
            }

            switch (message.MessageTypeId)
            {
                case MessageTypes.ItemLocale:
                    //is there a settings value for systems which should not receive item locale messages?
                    if (!string.IsNullOrWhiteSpace(settings.NonReceivingSystemsItemLocale))
                    {
                        //make sure systems which should not receive item locale messages are listed in the properties
                        PrependValueToDictionaryEntry(messageProperties, nonReceivingSystemsJmsKey, settings.NonReceivingSystemsItemLocale);
                    }
                    break;
                case MessageTypes.Price:
                    //is there a settings value for systems which should not receive price messages?
                    if (!string.IsNullOrWhiteSpace(settings.NonReceivingSystemsPrice))
                    {
                        //make sure systems which should not receive price messages are listed in the properties
                        PrependValueToDictionaryEntry(messageProperties, nonReceivingSystemsJmsKey, settings.NonReceivingSystemsPrice);
                    }
                    break;
                case MessageTypes.Product:
                    //is the message for a non-retail product?
                    if (message.MessageQueueProduct.FirstOrDefault() != default(MessageQueueProduct)
                        && message.MessageQueueProduct.FirstOrDefault().ItemTypeCode == ItemTypeCodes.NonRetail)
                    {
                        //make sure R10 is in the list of systems which do not want to receive the message
                        PrependValueToDictionaryEntry(messageProperties, nonReceivingSystemsJmsKey, "R10");
                    }
                    break;
                case MessageTypes.Locale:
                case MessageTypes.Hierarchy:
                case MessageTypes.DepartmentSale:
                case MessageTypes.CchTaxUpdate:
                case MessageTypes.ProductSelectionGroup:
                case MessageTypes.Ewic:
                case MessageTypes.InforNewItem:
                default:
                    break;
            }
        }

        /// <summary>
        /// Adds the provided string to the dictionary entry matching the key
        /// If the key is not found the key will be created & the value added
        /// If the key already exists, the new value will be pre-pended to the existing value
        /// </summary>
        /// <param name="stringDictionary"></param>
        /// <param name="key"></param>
        /// <param name="valueToPrepend"></param>
        private void PrependValueToDictionaryEntry(Dictionary<string, string> stringDictionary, string key, string valueToPrepend)
        {
            if (!stringDictionary.ContainsKey(key))
            {
                stringDictionary.Add(key, valueToPrepend);
            }
            else if (!stringDictionary[key].Contains(valueToPrepend))
            {
                stringDictionary[key] = valueToPrepend + "," + stringDictionary[key];
            }
        }

        /// <summary>
        /// Sets the Message Properties for IconMessageID, Source(Icon), and Non-Receiving Systems (if any)
        /// </summary>
        private Dictionary<string, string> SetMessageProperties(MessageHistory message)
        {
            var messageProperties = new Dictionary<string, string>();
            messageProperties.Add("IconMessageID", message.MessageHistoryId.ToString());
            messageProperties.Add("Source", "Icon");
 
            //make sure the non-receiving systems message property is set correctly per message type
            EnsureNonReceivingSystemPropertiesAreSet(message, messageProperties, EsbConstants.NonReceivingSystemsJmsProperty, settings.NonReceivingSystemsAll);

            return messageProperties;
        }

        private void ProcessResponse(bool messageSent, MessageHistory message)
        {
            if (messageSent)
            {
                logger.Info(string.Format("Message {0} has been sent successfully.", message.MessageHistoryId));

                var updateMessageHistoryCommand = new UpdateMessageHistoryStatusCommand<MessageHistory>
                {
                    Message = message,
                    MessageStatusId = MessageStatusTypes.Sent
                };

                updateMessageHistoryCommandHandler.Execute(updateMessageHistoryCommand);

                if(messageTypeId == MessageTypes.Hierarchy)
                {
                    var publishedHierarchyClassesByIdAsInteger = message.MessageQueueHierarchy
                        .Select(mqh => int.Parse(mqh.HierarchyClassId))
                        .ToList();

                    var updateSentToEsbHierarchyTraitCommand = new UpdateSentToEsbHierarchyTraitCommand
                    {
                        PublishedHierarchyClasses = publishedHierarchyClassesByIdAsInteger
                    };
                    updateSentToEsbHierarchyTraitCommandHandler.Execute(updateSentToEsbHierarchyTraitCommand);

                    var updateStagedProductStatusCommand = new UpdateStagedProductStatusCommand
                    {
                        PublishedHierarchyClasses = publishedHierarchyClassesByIdAsInteger
                    };
                    updateStagedProductStatusCommandHandler.Execute(updateStagedProductStatusCommand);
                }
            }
            else
            {
                logger.Error(string.Format("Message {0} failed to send.  The message will remain in Ready state for re-processing during the next controller execution.", message.MessageHistoryId));
            }
        }

        private List<MessageHistory> GetUnsentMessages()
        {
            var parameters = new GetMessageHistoryParameters
            {
                MessageTypeId = messageTypeId,
                Instance = ControllerType.Instance
            };

            return getMessageHistoryQuery.Search(parameters);
        }

        private void MarkUnsentMessagesAsInProcess()
        {
            int miniBulkLimitMessageHistory;
            if (!Int32.TryParse(ConfigurationManager.AppSettings["MiniBulkLimitMessageHistory"], out miniBulkLimitMessageHistory))
            {
                miniBulkLimitMessageHistory = 100;
            }

            var command = new MarkUnsentMessagesAsInProcessCommand
            {
                MiniBulkLimitMessageHistory = miniBulkLimitMessageHistory,
                MessageTypeId = messageTypeId,
                Instance = ControllerType.Instance
            };

            markUnsentMessagesAsInProcessCommandHandler.Execute(command);
        }
    }
}
