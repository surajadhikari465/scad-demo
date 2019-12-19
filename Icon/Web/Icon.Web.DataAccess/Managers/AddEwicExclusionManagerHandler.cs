using Icon.Common.DataAccess;
using Icon.Common.Validators;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
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
    public class AddEwicExclusionManagerHandler : IManagerHandler<AddEwicExclusionManager>
    {
        private IObjectValidator<AddEwicExclusionManager> addEwicExclusionValidator;
        private ISerializer<EwicExclusionMessageModel> exclusionSerializer;
        private IQueryHandler<GetEwicAgenciesWithoutExclusionParameters, List<Agency>> getEwicAgenciesWithoutExclusionQueryHandler;
        private ICommandHandler<AddEwicExclusionCommand> addEwicExclusionCommandHandler;
        private ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler;
        private IMessageProducer esbMessageProducer;

        public AddEwicExclusionManagerHandler(
            IObjectValidator<AddEwicExclusionManager> addEwicExclusionValidator,
            ISerializer<EwicExclusionMessageModel> exclusionSerializer,
            IQueryHandler<GetEwicAgenciesWithoutExclusionParameters, List<Agency>> getEwicAgenciesWithoutExclusionQueryHandler,
            ICommandHandler<AddEwicExclusionCommand> addEwicExclusionCommandHandler,
            ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler,
            IMessageProducer esbMessageProducer)
        {
            this.addEwicExclusionValidator = addEwicExclusionValidator;
            this.exclusionSerializer = exclusionSerializer;
            this.getEwicAgenciesWithoutExclusionQueryHandler = getEwicAgenciesWithoutExclusionQueryHandler;
            this.addEwicExclusionCommandHandler = addEwicExclusionCommandHandler;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.updateMessageHistoryMessageCommandHandler = updateMessageHistoryMessageCommandHandler;
            this.esbMessageProducer = esbMessageProducer;
        }

        public void Execute(AddEwicExclusionManager data)
        {
            var validationResult = addEwicExclusionValidator.Validate(data);

            if (!validationResult.IsValid)
            {
                throw new ArgumentException(String.Format("Error: {0}", validationResult.Error));
            }

            var agenciesForExclusion = GetAgenciesForExclusion(data.ScanCode);

            SaveExclusions(data.ScanCode, agenciesForExclusion);

            List<MessageHistory> messages = GenerateMessages(agenciesForExclusion.Count);

            List<EwicExclusionMessageModel> serializedMessages = SerializeMessages(
                data.ScanCode, 
                data.ProductDescription, 
                agenciesForExclusion, 
                messages.Select(m => m.MessageHistoryId).ToList());

            PopulateMessageXml(serializedMessages, messages);

            TransmitExclusions(messages);
        }

        private void TransmitExclusions(List<MessageHistory> messagesToTransmit)
        {
            try
            {
                esbMessageProducer.SendMessages(messagesToTransmit);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while sending the exclusion to R10.", ex);
            }
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

        private List<EwicExclusionMessageModel> SerializeMessages(string scanCode, string productDescription, List<Agency> agenciesForExclusion, List<int> messageHistoryId)
        {
            var messageModels = new List<EwicExclusionMessageModel>();

            EwicExclusionMessageModel messageModel;
            int messageIdCount = 0;

            foreach (var agency in agenciesForExclusion)
            {
                messageModel = new EwicExclusionMessageModel
                {
                    ActionTypeId = MessageActionTypes.AddOrUpdate,
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

        private void SaveExclusions(string scanCode, List<Agency> agenciesForExclusion)
        {
            var addEwicExclusionCommand = new AddEwicExclusionCommand
            {
                ScanCode = scanCode,
                Agencies = agenciesForExclusion
            };

            try
            {
                addEwicExclusionCommandHandler.Execute(addEwicExclusionCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while saving the exclusion.", ex);
            }
        }

        private List<Agency> GetAgenciesForExclusion(string scanCode)
        {
            var getEwicAgenciesWithoutExclusionParameters = new GetEwicAgenciesWithoutExclusionParameters { ScanCode = scanCode };

            List<Agency> agenciesForExclusion = new List<Agency>();

            try
            {
                agenciesForExclusion = getEwicAgenciesWithoutExclusionQueryHandler.Search(getEwicAgenciesWithoutExclusionParameters);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while querying for existing exclusions.", ex);
            }

            if (agenciesForExclusion.Count == 0)
            {
                throw new ArgumentException(String.Format("Scan Code {0} has already been excluded for all agencies.", scanCode));
            }
            else
            {
                return agenciesForExclusion;
            }
        }
    }
}
