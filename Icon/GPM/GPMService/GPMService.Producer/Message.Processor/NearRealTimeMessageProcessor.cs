using GPMService.Producer.DataAccess;
using GPMService.Producer.Helpers;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Settings;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Esb.Subscriber;
using System;
using System.Collections.Generic;
using static System.Net.Mime.MediaTypeNames;

namespace GPMService.Producer.Message.Processor
{
    internal class NearRealTimeMessageProcessor: IMessageProcessor
    {
        private readonly IMessageParser<items> messageParser;
        private readonly INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        public NearRealTimeMessageProcessor(
            IMessageParser<items> messageParser,
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
            MessageSequenceOutput messageSequenceOutput = ValidateMessageSequence(receivedMessage);
            if (messageSequenceOutput.IsInSequence || messageSequenceOutput.IsAlreadyProcessed)
            {
                items gpmReceived = messageParser.ParseMessage(receivedMessage);
                // TODO: Add further logic
            } else if (
                !messageSequenceOutput.IsInSequence
                && !messageSequenceOutput.IsAlreadyProcessed
                && (receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount) == null
                || int.Parse(receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount)) < gpmProducerServiceSettings.MaxRedeliveryCount)
                )
            {
                string errorDetails = $@"MessageID [${receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID)}] is out of sequence. 
Putting back into the queue for redelivery. 
The current JMSXDelivery count is ${receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount) ?? "1"}.";
                nearRealTimeProcessorDAL.ArchiveMessage(receivedMessage, Constants.ErrorCodes.OutOfSequenceRedelivery, errorDetails);
            } else if (
                !messageSequenceOutput.IsInSequence
                && !messageSequenceOutput.IsAlreadyProcessed
                && receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount) != null
                && int.Parse(receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount)) == gpmProducerServiceSettings.MaxRedeliveryCount
                )
            {
                string exceptionMessage = $@"Message is out of sequence and has exceeded the redelivery count of ${gpmProducerServiceSettings.MaxRedeliveryCount}. 
For MessageID: ${receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID)}, 
PatchFamilyID: ${receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID)}, 
SequenceID: ${receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID)}";
                throw new Exception(exceptionMessage);
            }
        }

        private MessageSequenceOutput ValidateMessageSequence(ReceivedMessage receivedMessage)
        {
            string sequenceID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID);
            string correlationID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID);
            if (string.IsNullOrEmpty(sequenceID) || int.Parse(sequenceID) < 1 || string.IsNullOrEmpty(correlationID))
            {
                throw new Exception($@"Message Sequence ID or CorrelationID is invalid.SequenceID must be greater than 0 or CorrelationID must not be empty. MessageID: ${receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID)}, CorrelationID(PatchFamilyID): ${correlationID}, SequenceID: ${sequenceID}");
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
                || (int.Parse(receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.ResetFlag)) == gpmProducerServiceSettings.ResetFlagTrueValue),
                IsAlreadyProcessed = int.Parse(sequenceID) <= int.Parse(messageSequenceData[0].LastProcessedGpmSequenceID)
            };
        }
    }
}
