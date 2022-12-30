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

namespace Icon.Esb.EwicAplListener.ExclusionGenerators
{
    public class ExclusionGenerator : IExclusionGenerator
    {
        private ILogger<ExclusionGenerator> logger;
        private ISerializer<EwicExclusionMessageModel> exclusionSerializer;
        private IQueryHandler<GetExclusionParameters, ScanCodeModel> getExclusionQuery;
        private ICommandHandler<AddExclusionParameters> addExclusionCommand;
        private ICommandHandler<SaveToMessageHistoryParameters> saveToMessageHistoryCommand;
        private ICommandHandler<UpdateMessageHistoryMessageParameters> updateMessageHistoryMessageCommandHandler;

        public ExclusionGenerator(
            ILogger<ExclusionGenerator> logger,
            ISerializer<EwicExclusionMessageModel> exclusionSerializer,
            IQueryHandler<GetExclusionParameters, ScanCodeModel> getExclusionQuery,
            ICommandHandler<AddExclusionParameters> addExclusionCommand,
            ICommandHandler<SaveToMessageHistoryParameters> saveToMessageHistoryCommand,
            ICommandHandler<UpdateMessageHistoryMessageParameters> updateMessageHistoryMessageCommandHandler)
        {
            this.logger = logger;
            this.exclusionSerializer = exclusionSerializer;
            this.getExclusionQuery = getExclusionQuery;
            this.addExclusionCommand = addExclusionCommand;
            this.saveToMessageHistoryCommand = saveToMessageHistoryCommand;
            this.updateMessageHistoryMessageCommandHandler = updateMessageHistoryMessageCommandHandler;
        }

        public void GenerateExclusions(EwicItemModel item)
        {
            var queryParameters = new GetExclusionParameters { ExcludedScanCode = item.ScanCode };

            ScanCodeModel exclusion = getExclusionQuery.Search(queryParameters);

            if (!String.IsNullOrEmpty(exclusion.ScanCode))
            {
                SaveExclusion(item.AgencyId, item.ScanCode);

                MessageHistory message = GenerateMessage();

                EwicExclusionMessageModel serializedMessage = SerializeMessage(item.ScanCode, item.AgencyId, exclusion.ProductDescription, message.MessageHistoryId);

                PopulateMessageXml(serializedMessage, message);


                logger.Info(String.Format("Successfully transmitted the exclusion for scan code {0} and agency {1} to R10.",
                    item.ScanCode, item.AgencyId));
            }
            else
            {
                logger.Info(String.Format("No current exclusions found for scan code {0}.", item.ScanCode));
            }
        }

        private MessageHistory GenerateMessage()
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

            var command = new SaveToMessageHistoryParameters { Messages = new List<MessageHistory> { newMessage } };

            saveToMessageHistoryCommand.Execute(command);

            return newMessage;
        }

        private void PopulateMessageXml(EwicExclusionMessageModel serializedMessage, MessageHistory message)
        {
            message.Message = serializedMessage.SerializedMessage;

            var command = new UpdateMessageHistoryMessageParameters
            {
                Message = XDocument.Parse(message.Message).ToString(),
                MessageHistoryId = message.MessageHistoryId
            };

            updateMessageHistoryMessageCommandHandler.Execute(command);
        }

        private EwicExclusionMessageModel SerializeMessage(string scanCode, string agencyId, string productDescription, int messageHistoryId)
        {
            var messageModel = new EwicExclusionMessageModel
            {
                ActionTypeId = MessageActionTypes.AddOrUpdate,
                ScanCode = scanCode,
                AgencyId = agencyId,
                MessageHistoryId = messageHistoryId,
                ProductDescription = productDescription
            };

            string serializedMessage = exclusionSerializer.Serialize(messageModel);

            messageModel.SerializedMessage = serializedMessage;

            return messageModel;
        }

        private void SaveExclusion(string agencyId, string scanCode)
        {
            logger.Info(String.Format("Found an existing exclusion for scan code {0}.  Adding the exclusion for agency {1}...",
                    scanCode, agencyId));

            var commandParameters = new AddExclusionParameters { AgencyId = agencyId, ExclusdedScanCode = scanCode };

            addExclusionCommand.Execute(commandParameters);

            logger.Info(String.Format("Successfully added the exclusion for scan code {0} and agency {1}.",
                scanCode, agencyId));
        }

    }
}
