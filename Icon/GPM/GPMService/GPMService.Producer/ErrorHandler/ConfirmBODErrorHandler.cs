using GPMService.Producer.DataAccess;
using GPMService.Producer.GPMException;
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
using Wfm.Aws.S3;

namespace GPMService.Producer.ErrorHandler
{
    internal class ConfirmBODErrorHandler
    {
        private readonly INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly IEsbProducer confirmBODEsbProducer;
        private readonly IS3Facade s3Facade;
        private readonly ISerializer<ConfirmBODType> serializer;
        private readonly ILogger<ConfirmBODErrorHandler> logger;
        private readonly RetryPolicy retrypolicy;
        public ConfirmBODErrorHandler(
            INearRealTimeProcessorDAL nearRealTimeProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            IEsbProducer confirmBODEsbProducer,
            IS3Facade s3Facade,
            ISerializer<ConfirmBODType> serializer,
            ILogger<ConfirmBODErrorHandler> logger
            )
        {
            this.nearRealTimeProcessorDAL = nearRealTimeProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.confirmBODEsbProducer = confirmBODEsbProducer;
            this.serializer = serializer;
            this.logger = logger;
            this.s3Facade = s3Facade;
            this.retrypolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
            string serviceType = gpmProducerServiceSettings.ServiceType;
            var computedClientId = $"GPMService.Type-{serviceType}.{Environment.MachineName}.{Guid.NewGuid()}";
            var clientId = computedClientId.Substring(0, Math.Min(computedClientId.Length, 255));
            logger.Info("Opening ConfirmBOD publisher ESB Connection");
            confirmBODEsbProducer.OpenConnection(clientId);
            logger.Info("ConfirmBOD publisher ESB Connection Opened");
        }
        public void HandleError(ReceivedMessage receivedMessage, Exception exception)
        {
            ConfirmBODType confirmBODType = new ConfirmBODType()
            {
                releaseID = "9.2",
                systemEnvironmentCode = "Production",
                languageCode = "en-US",
                ApplicationArea = new ApplicationAreaType()
                {
                    CreationDateTime = DateTimeOffset.Now.ToString("O"),
                },
                DataArea = new ConfirmBODDataAreaType()
                {
                    Confirm = new ConfirmType()
                    {
                        TenantID = new MetaIdentifierType()
                        {
                            Value = "Price Listener"
                        }
                    },
                    BOD = new[]
                    {
                        new BODType
                        {
                            Description = new []
                            {
                                new DescriptionType
                                {
                                    Value = exception.Message
                                }
                            },
                            OriginalBOD = new OriginalBODType
                            {
                                MessageContent = $@"<[CDATA[""{Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(receivedMessage.sqsExtendedClientMessage.S3Details[0].Data))}""]]>"
                            }
                        }
                    }
                }
            };
            string errorType = Constants.ErrorTypes.DatabaseConstraint;
            string errorReasonCode = Constants.ErrorCodes.DataError;
            if (exception is MappingException)
            {
                errorType = Constants.ErrorTypes.Schema;
                errorReasonCode = Constants.ErrorCodes.MappingError;
            }
            else if (exception is ZeroRowsImpactedException)
            {
                errorType = Constants.ErrorTypes.Data;
                errorReasonCode = Constants.ErrorCodes.ZeroRowsImpacted;
            }
            else if (exception is ActionNotSuppliedException)
            {
                errorType = Constants.ErrorTypes.DatabaseConstraint;
                errorReasonCode = Constants.ErrorCodes.DatabaseError;
            }
            confirmBODType.DataArea.BOD[0].BODFailureMessage = new BODFailureMessageType
            {
                ErrorProcessMessage = new[]
                {
                    new ErrorProcessMessageType
                    {
                        Type = new TypeType()
                        {
                            Value = errorType
                        },
                        ReasonCode = new ReasonCodeType()
                        {
                            Value = errorReasonCode
                        }
                    }
                }
            };
            string confirmBODXMLMessage = serializer.Serialize(confirmBODType, new Utf8StringWriter());
            retrypolicy.Execute(() =>
            {
                s3Facade.PutObject(
                    gpmProducerServiceSettings.GpmConfirmBODBucket,
                    $"{System.DateTime.UtcNow.ToString("yyyy/MM/dd", CultureInfo.InvariantCulture)}/{Guid.NewGuid()}",
                    confirmBODXMLMessage,
                    new Dictionary<string, string>()
                );
            });
            retrypolicy.Execute(() =>
            {
                confirmBODEsbProducer.Send(confirmBODXMLMessage, new Dictionary<string, string>());
            });
            string patchFamilyID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.CorrelationID.ToLower());
            string messageID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionID.ToLower());
            string transactionType = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.TransactionType.ToLower());
            string resetFlag = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.ResetFlag.ToLower());
            string source = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.Source.ToLower());
            string sequenceID = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.SequenceID.ToLower());
            string nonReceivingSysName = receivedMessage.sqsExtendedClientMessage.S3Details[0].Metadata.GetValueOrDefault(Constants.MessageHeaders.nonReceivingSysName.ToLower());
            Dictionary<string, string> receivedMessageProperties = new Dictionary<string, string>()
                {
                    { Constants.MessageHeaders.TransactionID, messageID },
                    { Constants.MessageHeaders.CorrelationID, patchFamilyID },
                    { Constants.MessageHeaders.SequenceID, sequenceID },
                    { Constants.MessageHeaders.ResetFlag, resetFlag },
                    { Constants.MessageHeaders.TransactionType, transactionType },
                    { Constants.MessageHeaders.Source, source },
                    { Constants.MessageHeaders.nonReceivingSysName, nonReceivingSysName },
                };
            nearRealTimeProcessorDAL.ArchiveErrorResponseMessage(messageID, Constants.MessageTypeNames.ConfirmBOD, confirmBODXMLMessage, receivedMessageProperties);
        }
    }
}
