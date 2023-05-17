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
using Icon.Common.Validators;

namespace Icon.Web.DataAccess.Managers
{
    public class AddEwicMappingManagerHandler : IManagerHandler<AddEwicMappingManager>
    {
        private IObjectValidator<AddEwicMappingManager> addEwicMappingValidator;
        private ISerializer<EwicMappingMessageModel> mappingSerializer;
        private IQueryHandler<GetEwicAgenciesWithoutMappingParameters, List<Agency>> getEwicAgenciesForMappingQuery;
        private ICommandHandler<AddEwicMappingCommand> addEwicMappingCommandHandler;
        private ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler;

        public AddEwicMappingManagerHandler(
            IObjectValidator<AddEwicMappingManager> addEwicMappingValidator,
            ISerializer<EwicMappingMessageModel> mappingSerializer,
            IQueryHandler<GetEwicAgenciesWithoutMappingParameters, List<Agency>> getEwicAgenciesForMappingQuery,
            ICommandHandler<AddEwicMappingCommand> addEwicMappingCommandHandler,
            ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler)
        {
            this.addEwicMappingValidator = addEwicMappingValidator;
            this.mappingSerializer = mappingSerializer;
            this.getEwicAgenciesForMappingQuery = getEwicAgenciesForMappingQuery;
            this.addEwicMappingCommandHandler = addEwicMappingCommandHandler;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.updateMessageHistoryMessageCommandHandler = updateMessageHistoryMessageCommandHandler;
        }

        public void Execute(AddEwicMappingManager data)
        {
            var validationResult = addEwicMappingValidator.Validate(data);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(String.Format("Error: {0}", validationResult.Error));
            }

            var agenciesForMapping = GetAgenciesForMapping(data.AplScanCode, data.WfmScanCode);

            SaveMappings(data.AplScanCode, data.WfmScanCode, agenciesForMapping);

            List<MessageHistory> messages = GenerateMessages(agenciesForMapping.Count);

            List<EwicMappingMessageModel> serializedMessages = SerializeMessages(
                data.AplScanCode, 
                data.WfmScanCode, 
                data.ProductDescription, 
                agenciesForMapping, 
                messages.Select(m => m.MessageHistoryId).ToList());

            PopulateMessageXml(serializedMessages, messages);

            TransmitMappings(messages);
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

        private void TransmitMappings(List<MessageHistory> messagesToTransmit)
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
                throw new CommandException("An error occurred while sending the mappings to R10.", ex);
            }
        }

        private void PopulateMessageXml(List<EwicMappingMessageModel> serializedMessages, List<MessageHistory> messages)
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

        private List<EwicMappingMessageModel> SerializeMessages(string aplScanCode, string wfmScanCode, string productDescription, List<Agency> agenciesForMapping, List<int> messageHistoryId)
        {
            var messageModels = new List<EwicMappingMessageModel>();

            EwicMappingMessageModel messageModel;
            int messageIdCount = 0;

            foreach (var agency in agenciesForMapping)
            {
                messageModel = new EwicMappingMessageModel
                {
                    ActionTypeId = MessageActionTypes.AddOrUpdate,
                    AplScanCode = aplScanCode,
                    WfmScanCode = wfmScanCode,
                    AgencyId = agency.AgencyId,
                    ProductDescription = productDescription,
                    MessageHistoryId = messageHistoryId[messageIdCount]
                };

                string serializedMessage = mappingSerializer.Serialize(messageModel);

                messageModel.SerializedMessage = serializedMessage;
                messageModels.Add(messageModel);

                messageIdCount++;
            }

            return messageModels;
        }

        private void SaveMappings(string aplScanCode, string wfmScanCode, List<Agency> agenciesForMapping)
        {
            var addEwicMappingCommand = new AddEwicMappingCommand
            {
                AplScanCode = aplScanCode,
                WfmScanCode = wfmScanCode,
                Agencies = agenciesForMapping
            };

            try
            {
                addEwicMappingCommandHandler.Execute(addEwicMappingCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while saving the mapping.", ex);
            }
        }

        private List<Agency> GetAgenciesForMapping(string aplScanCode, string wfmScanCode)
        {
            var parameters = new GetEwicAgenciesWithoutMappingParameters
            {
                AplScanCode = aplScanCode,
                WfmScanCode = wfmScanCode
            };

            List<Agency> agenciesForMapping = new List<Agency>();

            try
            {
                agenciesForMapping = getEwicAgenciesForMappingQuery.Search(parameters);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while querying for existing mappings.", ex);
            }

            if (agenciesForMapping.Count == 0)
            {
                throw new ArgumentException(String.Format("Scan Code {0} has already been mapped for all agencies.", wfmScanCode));
            }
            else
            {
                return agenciesForMapping;
            }
        }
    }
}
