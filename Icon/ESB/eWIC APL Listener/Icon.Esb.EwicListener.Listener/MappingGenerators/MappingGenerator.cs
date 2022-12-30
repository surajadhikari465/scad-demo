using System.Linq;
using Icon.Common.DataAccess;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Esb.EwicAplListener.DataAccess.Queries;
using Icon.Ewic.Models;
using Icon.Ewic.Serialization.Serializers;
using Icon.Ewic.Transmission.Producers;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Xml.Linq;

namespace Icon.Esb.EwicAplListener.MappingGenerators
{
    public class MappingGenerator : IMappingGenerator
    {
        private ILogger<MappingGenerator> logger;
        private ISerializer<EwicMappingMessageModel> mappingSerializer;
        private IQueryHandler<GetExistingMappingsParameters, List<ScanCodeModel>> getExistingMappingsQuery;
        private ICommandHandler<AddMappingsParameters> addMappingsCommand;
        private ICommandHandler<SaveToMessageHistoryParameters> saveToMessageHistoryCommandHandler;
        private ICommandHandler<UpdateMessageHistoryMessageParameters> updateMessageHistoryMessageCommandHandler;
        public MappingGenerator(
            ILogger<MappingGenerator> logger,
            ISerializer<EwicMappingMessageModel> mappingSerializer,
            IQueryHandler<GetExistingMappingsParameters, List<ScanCodeModel>> getExistingMappingsQuery,
            ICommandHandler<AddMappingsParameters> addMappingsCommand,
            ICommandHandler<SaveToMessageHistoryParameters> saveToMessageHistoryCommandHandler,
            ICommandHandler<UpdateMessageHistoryMessageParameters> updateMessageHistoryMessageCommandHandler
           )
        {
            this.logger = logger;
            this.mappingSerializer = mappingSerializer;
            this.getExistingMappingsQuery = getExistingMappingsQuery;
            this.addMappingsCommand = addMappingsCommand;
            this.saveToMessageHistoryCommandHandler = saveToMessageHistoryCommandHandler;
            this.updateMessageHistoryMessageCommandHandler = updateMessageHistoryMessageCommandHandler;
        }

        public void GenerateMappings(EwicItemModel item)
        {
            var queryParameters = new GetExistingMappingsParameters { AplScanCode = item.ScanCode, AgencyId = item.AgencyId };

            List<ScanCodeModel> existingMappings = getExistingMappingsQuery.Search(queryParameters);

            if (existingMappings.Count > 0)
            {
                SaveMappings(item.AgencyId, item.ScanCode, existingMappings);

                List<MessageHistory> messages = GenerateMessages(existingMappings.Count);

                List<EwicMappingMessageModel> serializedMessages = SerializeMessages(item.ScanCode, item.AgencyId, existingMappings, messages.Select(m => m.MessageHistoryId).ToList());

                PopulateMessageXml(serializedMessages, messages);


                logger.Info(String.Format("Successfully saved and transmitted the mapping messages for APL scan code {0} and agency {1} to R10.",
                    item.ScanCode, item.AgencyId));
            }
            else
            {
                logger.Info(String.Format("No existing mappings found for APL scan code {0}, or all mappings already exist.", item.ScanCode));
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

            var command = new SaveToMessageHistoryParameters { Messages = messagesToGenerate };

            saveToMessageHistoryCommandHandler.Execute(command);

            return messagesToGenerate;
        }

        private void PopulateMessageXml(List<EwicMappingMessageModel> serializedMessages, List<MessageHistory> messages)
        {
            var command = new UpdateMessageHistoryMessageParameters();

            foreach (var message in messages)
            {
                message.Message = serializedMessages.Where(m => m.MessageHistoryId == message.MessageHistoryId).Single().SerializedMessage;

                command.Message = XDocument.Parse(message.Message).ToString();
                command.MessageHistoryId = message.MessageHistoryId;

                updateMessageHistoryMessageCommandHandler.Execute(command);
            }
        }

        private List<EwicMappingMessageModel> SerializeMessages(string aplScanCode, string agencyId, List<ScanCodeModel> existingMappings, List<int> messageHistoryId)
        {
            var messageModels = new List<EwicMappingMessageModel>();

            EwicMappingMessageModel messageModel;
            int messageIdCount = 0;

            foreach (var mapping in existingMappings)
            {
                messageModel = new EwicMappingMessageModel
                {
                    ActionTypeId = MessageActionTypes.AddOrUpdate,
                    AplScanCode = aplScanCode,
                    WfmScanCode = mapping.ScanCode,
                    AgencyId = agencyId,
                    ProductDescription = mapping.ProductDescription,
                    MessageHistoryId = messageHistoryId[messageIdCount]
                };

                string serializedMessage = mappingSerializer.Serialize(messageModel);

                messageModel.SerializedMessage = serializedMessage;
                messageModels.Add(messageModel);

                messageIdCount++;
            }

            return messageModels;
        }

        private void SaveMappings(string agencyId, string aplScanCode, List<ScanCodeModel> existingMappings)
        {
            logger.Info(String.Format("Found the following WFM scan codes mapped to APL scan code {0}: {1}.",
                    aplScanCode, String.Join(", ", existingMappings.Select(m => m.ScanCode))));

            var newMappings = new List<Mapping>();

            foreach (var mapping in existingMappings)
            {
                var newMapping = new Mapping
                {
                    AgencyId = agencyId,
                    AplScanCode = aplScanCode,
                    ScanCodeId = mapping.ScanCodeId
                };

                newMappings.Add(newMapping);
            }

            var commandParameters = new AddMappingsParameters { Mappings = newMappings };

            addMappingsCommand.Execute(commandParameters);

            logger.Info(String.Format("Successfully added all mappings for APL scan code {0}.", aplScanCode));
        }
    }
}
