using Icon.Common.DataAccess;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Framework;
using Icon.Web.Common;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;

namespace Icon.Web.DataAccess.Managers
{
    public class RemoveEwicExclusionManagerHandler : IManagerHandler<RemoveEwicExclusionManager>
    {
        private ISerializer<EwicExclusionMessageModel> exclusionSerializer;
        private IQueryHandler<GetEwicAgenciesWithExclusionParameters, List<Agency>> getEwicAgenciesWithExclusionQueryHandler;
        private ICommandHandler<RemoveEwicExclusionCommand> removeEwicExclusionCommandHandler;
        private ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler;

        public RemoveEwicExclusionManagerHandler(
            ISerializer<EwicExclusionMessageModel> exclusionSerializer,
            IQueryHandler<GetEwicAgenciesWithExclusionParameters, List<Agency>> getEwicAgenciesWithExclusionQueryHandler,
            ICommandHandler<RemoveEwicExclusionCommand> removeEwicExclusionCommandHandler,
            ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler)
        {
            this.exclusionSerializer = exclusionSerializer;
            this.getEwicAgenciesWithExclusionQueryHandler = getEwicAgenciesWithExclusionQueryHandler;
            this.removeEwicExclusionCommandHandler = removeEwicExclusionCommandHandler;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.updateMessageHistoryMessageCommandHandler = updateMessageHistoryMessageCommandHandler;
        }

        public void Execute(RemoveEwicExclusionManager data)
        {
            var excludedAgencies = GetAgenciesForExclusionRemoval(data.ScanCode);

            RemoveExclusions(data.ScanCode);

            List<MessageHistory> messages = GenerateMessages(excludedAgencies.Count);

            List<EwicExclusionMessageModel> serializedMessages = SerializeMessages(
                data.ScanCode, 
                data.ProductDescription, 
                excludedAgencies, 
                messages.Select(m => m.MessageHistoryId).ToList());

            PopulateMessageXml(serializedMessages, messages);

            TransmitExclusionRemovals(messages);
        }

        private void TransmitExclusionRemovals(List<MessageHistory> messagesToTransmit)
        {
            try
            {
                // Just commenting out as there is no replacement for this.
                // It is to be decided if this flow is needed.
                // This flow doesn't even work as of now.
                
                // esbMessageProducer.SendMessages(messagesToTransmit);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while sending the exclusion to R10.", ex);
            }
        }

        private List<MessageHistory> GenerateMessages(int messageCount)
        {
            var messagesToGenerate = new List<MessageHistory>();

            for (int count = 0; count < messageCount; count++)
            {
                var newMessage = new MessageHistory
                {
                    MessageTypeId = MessageTypes.Ewic,
                    MessageStatusId = MessageStatusTypes.Sent,
                    Message = String.Empty,
                    InsertDate = DateTime.Now,
                    InProcessBy = null,
                    ProcessedDate = DateTime.Now
                };

                messagesToGenerate.Add(newMessage);
            }

            var command = new SaveToMessageHistoryCommand { Messages = messagesToGenerate };

            saveToMessageHistoryCommandHandler.Execute(command);

            return messagesToGenerate;
        }

        private void PopulateMessageXml(List<EwicExclusionMessageModel> serializedMessages, List<MessageHistory> messages)
        {
            var command = new UpdateMessageHistoryMessageCommand();

            foreach (var message in messages)
            {
                message.Message = serializedMessages.Where(m => m.MessageHistoryId == message.MessageHistoryId).Single().SerializedMessage;

                command.Message = XDocument.Parse(message.Message).ToString();
                command.MessageHistoryId = message.MessageHistoryId;

                updateMessageHistoryMessageCommandHandler.Execute(command);
            }
        }

        private void RemoveExclusions(string scanCode)
        {
            var removeEwicExclusionCommand = new RemoveEwicExclusionCommand { ScanCode = scanCode };

            try
            {
                removeEwicExclusionCommandHandler.Execute(removeEwicExclusionCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while saving the exclusion.", ex);
            }
        }

        private List<EwicExclusionMessageModel> SerializeMessages(string scanCode, string productDescription, List<Agency> excludedAgencies, List<int> messageHistoryId)
        {
            var messageModels = new List<EwicExclusionMessageModel>();

            EwicExclusionMessageModel messageModel;
            int messageIdCount = 0;

            foreach (var agency in excludedAgencies)
            {
                messageModel = new EwicExclusionMessageModel
                {
                    ActionTypeId = MessageActionTypes.Delete,
                    ScanCode = scanCode,
                    AgencyId = agency.AgencyId,
                    ProductDescription = productDescription,
                    MessageHistoryId = messageHistoryId[messageIdCount]
                };

                string serializedMessage = exclusionSerializer.Serialize(messageModel);

                messageModel.SerializedMessage = serializedMessage;
                messageModels.Add(messageModel);

                messageIdCount++;
            }

            return messageModels;
        }

        private List<Agency> GetAgenciesForExclusionRemoval(string scanCode)
        {
            var parameters = new GetEwicAgenciesWithExclusionParameters { ScanCode = scanCode };

            var excludedAgencies = new List<Agency>();

            try
            {
                excludedAgencies = getEwicAgenciesWithExclusionQueryHandler.Search(parameters);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while querying for existing exclusions.", ex);
            }

            if (excludedAgencies.Count == 0)
            {
                throw new ArgumentException(String.Format("Scan code {0} has already been excluded for all agencies.", scanCode));
            }
            else
            {
                return excludedAgencies;
            }
        }
    }
}
