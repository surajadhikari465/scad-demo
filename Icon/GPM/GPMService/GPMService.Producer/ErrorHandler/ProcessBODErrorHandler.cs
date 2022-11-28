using GPMService.Producer.DataAccess;
using GPMService.Producer.Helpers;
using GPMService.Producer.Model;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.Esb.Producer;
using Icon.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;

namespace GPMService.Producer.ErrorHandler
{
    internal class ProcessBODErrorHandler
    {
        private readonly INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly IEsbProducer processBODEsbProducer;
        private readonly ISerializer<PriceChangeMaster> serializer;
        private readonly ILogger<ProcessBODErrorHandler> logger;
        private readonly RetryPolicy retrypolicy;
        public ProcessBODErrorHandler(
            INearRealTimeProcessorDAL nearRealTimeProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEsbProducer processBODEsbProducer,
            ISerializer<PriceChangeMaster> serializer,
            ILogger<ProcessBODErrorHandler> logger
            )
        {
            this.nearRealTimeProcessorDAL = nearRealTimeProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.processBODEsbProducer = processBODEsbProducer;
            this.serializer = serializer;
            this.logger = logger;
            this.retrypolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
            string serviceType = gpmProducerServiceSettings.ServiceType;
            var computedClientId = $"GPMService.Type-{serviceType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
            logger.Info("Opening ProcessBOD publisher ESB Connection");
            processBODEsbProducer.OpenConnection(clientId);
            logger.Info("ProcessBOD publisher ESB Connection Opened");
        }
        public void HandleError(ReceivedMessage receivedMessage, MessageSequenceOutput messageSequenceOutput)
        {
            string patchFamilyID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID);
            string messageID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID);
            string transactionType = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionType);
            string resetFlag = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.ResetFlag);
            string source = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.Source);
            int sequenceID = int.Parse(receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID));
            int lastProcessedGpmSequenceID = messageSequenceOutput.LastProcessedGpmSequenceID;
            for (int i = 0; i <= sequenceID - lastProcessedGpmSequenceID; i++)
            {
                PriceChangeMaster priceChangeMaster = new PriceChangeMaster()
                {
                    isCheckPoint = false,
                    BusinessKey = new PriceChangeMasterTypeBusinessKey()
                    {
                        variationID = "0",
                        Value = "00000000-0000-4000-8000-000000000000"
                    },
                    PriceChangeHeader = new[]
                    {
                        new PriceChangeType
                        {
                            PatchFamilyID = patchFamilyID,
                            PatchNum = (lastProcessedGpmSequenceID + i).ToString(),
                            TimeStamp = DateTimeOffset.Now.ToString("O")
                        }
                    }
                };
                string processBODXMLMessage = serializer.Serialize(priceChangeMaster, new Utf8StringWriter());
                Dictionary<string, string> processBODXMLMessageProperties = new Dictionary<string, string>()
                {
                    { "TransactionID", messageID },
                    { "CorrelationID", patchFamilyID },
                    { "SequenceID", receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID) },
                    { "ResetFlag", Constants.ResetFlagValues.ResetFlagTrueValue.Equals(resetFlag) ? "true" : "false" },
                    { "TransactionType", transactionType },
                    { "Source", source },
                };
                retrypolicy.Execute(() =>
                {
                    processBODEsbProducer.Send(processBODXMLMessage, processBODXMLMessageProperties);
                });
                nearRealTimeProcessorDAL.ArchiveErrorResponseMessage(messageID, Constants.MessageTypeNames.ProcessBOD, processBODXMLMessage, processBODXMLMessageProperties);
            }
        }
    }
}
