using Icon.Common.DataAccess;
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
    public class RemoveEwicMappingManagerHandler : IManagerHandler<RemoveEwicMappingManager>
    {
        private ISerializer<EwicMappingMessageModel> mappingSerializer;
        private IQueryHandler<GetEwicAgenciesWithMappingParameters, List<Agency>> getEwicAgenciesWithMappingQueryHandler;
        private ICommandHandler<RemoveEwicMappingCommand> removeEwicMappingCommandHandler;
        private ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler;
        private IMessageProducer esbMessageProducer;

        public RemoveEwicMappingManagerHandler(
            ISerializer<EwicMappingMessageModel> mappingSerializer,
            IQueryHandler<GetEwicAgenciesWithMappingParameters, List<Agency>> getEwicAgenciesWithMappingQueryHandler,
            ICommandHandler<RemoveEwicMappingCommand> removeEwicMappingCommandHandler,
            ICommandHandler<SaveToMessageHistoryCommand> saveToMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageHistoryMessageCommand> updateMessageHistoryMessageCommandHandler,
            IMessageProducer esbMessageProducer)
        {
            this.mappingSerializer = mappingSerializer;
            this.getEwicAgenciesWithMappingQueryHandler = getEwicAgenciesWithMappingQueryHandler;
            this.removeEwicMappingCommandHandler = removeEwicMappingCommandHandler;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.updateMessageHistoryMessageCommandHandler = updateMessageHistoryMessageCommandHandler;
            this.esbMessageProducer = esbMessageProducer;
        }

        public void Execute(RemoveEwicMappingManager data)
        {
            var agenciesWithMapping = GetAgenciesForMappingRemoval(data.WfmScanCode, data.AplScanCode);

            RemoveMappings(data.WfmScanCode, data.AplScanCode);

            List<MessageHistory> messages = GenerateMessages(agenciesWithMapping.Count);

            List<EwicMappingMessageModel> serializedMessages = SerializeMessages(
                data.AplScanCode, 
                data.WfmScanCode, 
                data.ProductDescription, 
                agenciesWithMapping, 
                messages.Select(m => m.MessageHistoryId).ToList());

            PopulateMessageXml(serializedMessages, messages);

            TransmitMappingRemovals(messages);
        }

        private void TransmitMappingRemovals(List<MessageHistory> messagesToTransmit)
        {
            try
            {
                esbMessageProducer.SendMessages(messagesToTransmit);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while sending the mapping removal to R10.", ex);
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

        private void RemoveMappings(string wfmScanCode, string aplScanCode)
        {
            var removeEwicMappingCommand = new RemoveEwicMappingCommand
            {
                AplScanCode = aplScanCode,
                WfmScanCode = wfmScanCode
            };

            try
            {
                removeEwicMappingCommandHandler.Execute(removeEwicMappingCommand);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while saving the mapping.", ex);
            }
        }

        private List<EwicMappingMessageModel> SerializeMessages(string aplScanCode, string wfmScanCode, string productDescription, List<Agency> agenciesWithMapping, List<int> messageHistoryId)
        {
            var messageModels = new List<EwicMappingMessageModel>();

            EwicMappingMessageModel messageModel;
            int messageIdCount = 0;

            foreach (var agency in agenciesWithMapping)
            {
                messageModel = new EwicMappingMessageModel
                {
                    ActionTypeId = MessageActionTypes.Delete,
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

        private List<Agency> GetAgenciesForMappingRemoval(string wfmScanCode, string aplScanCode)
        {
            var parameters = new GetEwicAgenciesWithMappingParameters
            {
                AplScanCode = aplScanCode,
                WfmScanCode = wfmScanCode
            };

            var mappedAgencies = new List<Agency>();

            try
            {
                mappedAgencies = getEwicAgenciesWithMappingQueryHandler.Search(parameters);
            }
            catch (Exception ex)
            {
                throw new CommandException("An error occurred while querying for existing mappings.", ex);
            }

            if (mappedAgencies.Count == 0)
            {
                throw new ArgumentException(String.Format("WFM scan code {0} is currently not mapped to APL scan code {1} for any agencies.", wfmScanCode, aplScanCode));
            }
            else
            {
                return mappedAgencies;
            }
        }
    }
}
