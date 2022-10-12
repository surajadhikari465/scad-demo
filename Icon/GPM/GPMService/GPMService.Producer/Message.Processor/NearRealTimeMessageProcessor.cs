using GPMService.Producer.DataAccess;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Settings;
using System;
using System.Collections.Generic;

namespace GPMService.Producer.Message.Processor
{
    internal class NearRealTimeMessageProcessor: IMessageProcessor
    {
        IMessageParser messageParser;
        INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        GPMProducerServiceSettings gpmProducerServiceSettings;
        public NearRealTimeMessageProcessor(
            IMessageParser messageParser,
            INearRealTimeProcessorDAL nearRealTimeProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings
            )
        {
            this.messageParser = messageParser;
            this.nearRealTimeProcessorDAL = nearRealTimeProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
        }

        public void ProcessMessage(ReceivedMessage receivedMessage)
        {
            ValidateMessageSequence(receivedMessage);
        }

        private MessageSequenceOutput ValidateMessageSequence(ReceivedMessage receivedMessage)
        {
            string sequenceID = receivedMessage.esbMessage.GetProperty("SequenceID");
            string correlationID = receivedMessage.esbMessage.GetProperty("CorrelationID");
            if (string.IsNullOrEmpty(sequenceID) || int.Parse(sequenceID) < 1 || string.IsNullOrEmpty(correlationID))
            {
                throw new Exception($@"Message Sequence ID or CorrelationID is invalid.SequenceID must be greater than 0 or CorrelationID must not be empty. MessageID: ${receivedMessage.esbMessage.GetProperty("TransactionID")}, CorrelationID(PatchFamilyID): ${correlationID}, SequenceID: ${sequenceID}");
            }
            IList<MessageSequenceModel> messageSequenceData = nearRealTimeProcessorDAL.GetLastSequence(correlationID);
            bool messageSequenceDataAvailable = messageSequenceData.Count > 0;
            return new MessageSequenceOutput
            {
                PatchFamilyId = correlationID,
                SequenceID = int.Parse(sequenceID),
                LastProcessedGpmSequenceID = messageSequenceDataAvailable ? int.Parse(messageSequenceData[0].LastProcessedGpmSequenceID) : 0,
                IsInSequence = (messageSequenceDataAvailable && int.Parse(sequenceID) == int.Parse(messageSequenceData[0].LastProcessedGpmSequenceID) + 1)
                || (int.Parse(sequenceID) == 1 && !messageSequenceDataAvailable)
                || (int.Parse(receivedMessage.esbMessage.GetProperty("ResetFlag")) == gpmProducerServiceSettings.ResetFlagTrueValue),
                IsAlreadyProcessed = int.Parse(sequenceID) <= int.Parse(messageSequenceData[0].LastProcessedGpmSequenceID)
            };
        }
    }
}
