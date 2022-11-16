using GPMService.Producer.DataAccess;
using GPMService.Producer.GPMException;
using GPMService.Producer.Helpers;
using GPMService.Producer.Model;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.Esb.Producer;
using Polly;
using Polly.Retry;
using System;
using System.Collections.Generic;
using System.IO;

namespace GPMService.Producer.ErrorHandler
{
    internal class ConfirmBODErrorHandler
    {

        private readonly INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly IEsbProducer esbProducer;
        private readonly ISerializer<ConfirmBODType> serializer;
        private readonly StringWriter utf8StringWriter = new Utf8StringWriter();
        private readonly RetryPolicy retrypolicy;
        public ConfirmBODErrorHandler(
            INearRealTimeProcessorDAL nearRealTimeProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            IEsbProducer esbProducer
            )
        {
            this.nearRealTimeProcessorDAL = nearRealTimeProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.esbProducer = esbProducer;
            serializer = new Serializer<ConfirmBODType>();
            this.retrypolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                gpmProducerServiceSettings.DbErrorRetryCount,
                retryAttempt => TimeSpan.FromMilliseconds(gpmProducerServiceSettings.SendMessageRetryDelayInMilliseconds)
                );
        }
        public void HandleError(ReceivedMessage receivedMessage, Exception exception)
        {
            ConfirmBODType confirmBODType = new ConfirmBODType()
            {
                releaseID = "9.2",
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
                    BOD = new []
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
                                MessageContent = $@"<![CDATA[""${Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(receivedMessage.esbMessage.MessageText))}""]]>"
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
            } else if ( exception is ZeroRowsImpactedException)
            {
                errorType =  Constants.ErrorTypes.Data;
                errorReasonCode =  Constants.ErrorCodes.ZeroRowsImpacted;
            } else if ( exception is ActionNotSuppliedException)
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
            string confirmBODXMLMessage = serializer.Serialize(confirmBODType, utf8StringWriter);
            retrypolicy.Execute(() =>
            {
                esbProducer.Send(confirmBODXMLMessage, new Dictionary<string, string>());
            });
            string patchFamilyID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID);
            string messageID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID);
            string transactionType = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionType);
            string resetFlag = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.ResetFlag);
            string source = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.Source);
            string sequenceID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID);
            string nonReceivingSysName = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.nonReceivingSysName);
            Dictionary<string, string> receivedMessageProperties = new Dictionary<string, string>()
                {
                    { "TransactionID", messageID },
                    { "CorrelationID", patchFamilyID },
                    { "SequenceID", sequenceID },
                    { "ResetFlag", resetFlag },
                    { "TransactionType", transactionType },
                    { "Source", source },
                    { "nonReceivingSysName", source },
                };
            nearRealTimeProcessorDAL.ArchiveErrorResponseMessage(messageID, Constants.MessageTypeNames.ConfirmBOD, confirmBODXMLMessage, receivedMessageProperties);
        }
    }
}
