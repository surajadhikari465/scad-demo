using GPMService.Producer.DataAccess;
using GPMService.Producer.Helpers;
using GPMService.Producer.Model;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.Esb.Producer;
using Icon.Esb.Schemas.Infor;
using Icon.Logging;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.Globalization;
using Wfm.Aws.Helpers;
using Wfm.Aws.S3;

namespace GPMService.Producer.ErrorHandler
{
    internal class ProcessBODErrorHandler
    {
        private readonly INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly IEsbProducer processBODEsbProducer;
        private readonly IS3Facade s3Facade;
        private readonly ISerializer<PriceChangeMaster> serializer;
        private readonly ILogger<ProcessBODErrorHandler> logger;
        private readonly RetryPolicy retrypolicy;
        public ProcessBODErrorHandler(
            INearRealTimeProcessorDAL nearRealTimeProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEsbProducer processBODEsbProducer,
            IS3Facade s3Facade,
            ISerializer<PriceChangeMaster> serializer,
            ILogger<ProcessBODErrorHandler> logger
            )
        {
            this.nearRealTimeProcessorDAL = nearRealTimeProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.processBODEsbProducer = processBODEsbProducer;
            this.s3Facade = s3Facade;
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
            string patchFamilyID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.CorrelationID.ToLower());
            string messageID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionID.ToLower());
            string transactionType = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionType.ToLower());
            string resetFlag = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.ResetFlag.ToLower());
            string source = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.Source.ToLower());
            int sequenceID = int.Parse(receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.SequenceID.ToLower()));
            int lastProcessedGpmSequenceID = messageSequenceOutput.LastProcessedGpmSequenceID;
            for (int i = 0; i <= sequenceID - lastProcessedGpmSequenceID; i++)
            {
                PriceChangeMaster priceChangeMaster = new PriceChangeMaster()
                {
                    isCheckPoint = false,
                    isCheckPointSpecified = true,
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
                            TimeStamp = DateTimeOffset.Now.ToString("O"),
                            TimeStampSpecified = true
                        }
                    }
                };
                string processBODXMLMessage = serializer.Serialize(priceChangeMaster, new Utf8StringWriter());
                Dictionary<string, string> processBODXMLMessageProperties = new Dictionary<string, string>()
                {
                    { Constants.MessageHeaders.TransactionID, messageID },
                    { Constants.MessageHeaders.CorrelationID, patchFamilyID },
                    { Constants.MessageHeaders.SequenceID, receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.SequenceID.ToLower()) },
                    { Constants.MessageHeaders.ResetFlag, Constants.ResetFlagValues.ResetFlagTrueValue.Equals(resetFlag) ? "true" : "false" },
                    { Constants.MessageHeaders.TransactionType, transactionType },
                    { Constants.MessageHeaders.Source, source },
                };
                retrypolicy.Execute(() =>
                {
                    s3Facade.PutObject(
                        gpmProducerServiceSettings.GpmProcessBODBucket,
                        $"{System.DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{Guid.NewGuid()}",
                        processBODXMLMessage,
                        processBODXMLMessageProperties
);
                });
                retrypolicy.Execute(() =>
                {
                    processBODEsbProducer.Send(processBODXMLMessage, processBODXMLMessageProperties);
                });
                nearRealTimeProcessorDAL.ArchiveErrorResponseMessage(messageID, Constants.MessageTypeNames.ProcessBOD, processBODXMLMessage, processBODXMLMessageProperties);
            }
        }
    }
}
