using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
using GPMService.Producer.GPMException;
using GPMService.Producer.Helpers;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Publish;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.Esb;
using Icon.Esb.Schemas.Wfm.Contracts;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using TIBCO.EMS;

namespace GPMService.Producer.Message.Processor
{
    internal class NearRealTimeMessageProcessor : IMessageProcessor
    {
        private readonly IMessageParser<items> messageParser;
        private readonly INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ProcessBODErrorHandler processBODHandler;
        private readonly ConfirmBODErrorHandler confirmBODHandler;
        private readonly ErrorEventPublisher errorEventPublisher;
        private readonly EsbConnectionSettings nearRealTimeListenerEsbConnectionSettings;
        private readonly IMessagePublisher messagePublisher;
        private readonly ILogger<NearRealTimeMessageProcessor> logger;
        public NearRealTimeMessageProcessor(
            IMessageParser<items> messageParser,
            INearRealTimeProcessorDAL nearRealTimeProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ProcessBODErrorHandler processBODHandler,
            ConfirmBODErrorHandler confirmBODHandler,
            ErrorEventPublisher errorEventPublisher,
            // Using named injection.
            // Changing the variable name would require change in SimpleInjectiorInitializer.cs file as well.
            EsbConnectionSettings nearRealTimeListenerEsbConnectionSettings,
            IMessagePublisher messagePublisher,
            ILogger<NearRealTimeMessageProcessor> logger
            )
        {
            this.messageParser = messageParser;
            this.nearRealTimeProcessorDAL = nearRealTimeProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.processBODHandler = processBODHandler;
            this.confirmBODHandler = confirmBODHandler;
            this.nearRealTimeListenerEsbConnectionSettings = nearRealTimeListenerEsbConnectionSettings;
            this.messagePublisher = messagePublisher;
            this.logger = logger;
        }

        public void ProcessReceivedMessage(ReceivedMessage receivedMessage)
        {
            string transactionID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID);
            string correlationID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID);
            string sequenceID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID);
            Dictionary<string, string> messageProperties = new Dictionary<string, string>()
            {
                { Constants.MessageHeaders.TransactionID, transactionID},
                { Constants.MessageHeaders.CorrelationID, correlationID},
                { Constants.MessageHeaders.SequenceID, sequenceID},
                { Constants.MessageHeaders.ResetFlag,
                    receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.ResetFlag)},
                { Constants.MessageHeaders.TransactionType,
                    receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionType)},
                { Constants.MessageHeaders.Source,
                    receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.Source)},
                { Constants.MessageHeaders.nonReceivingSysName,
                    receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.nonReceivingSysName)},
            };
            MessageSequenceOutput messageSequenceOutput = null;
            try
            {
                messageSequenceOutput = ValidateMessageSequence(receivedMessage);
                if (messageSequenceOutput.IsInSequence || messageSequenceOutput.IsAlreadyProcessed)
                {
                    items gpmReceivedItems = messageParser.ParseMessage(receivedMessage);
                    MammothPricesType mammothPrices = MapToMammothPrices(gpmReceivedItems);
                    if (messageSequenceOutput.IsInSequence)
                    {
                        nearRealTimeProcessorDAL.ProcessPriceMessage(receivedMessage, mammothPrices);
                    }
                    AcknowledgeEsbMessage(receivedMessage);
                    if (!(messageSequenceOutput.IsAlreadyProcessed && !messageSequenceOutput.IsInSequence))
                    {
                        messagePublisher.PublishMessage(receivedMessage.esbMessage.MessageText, messageProperties);
                        HandleEmergencyPrices(mammothPrices);
                    }
                    nearRealTimeProcessorDAL.ArchiveMessage(receivedMessage, null, null);
                    logger.Info($@"Successfully processed 
MessageID: {transactionID}, 
PatchFamilyID: {correlationID}, 
SequenceID: {sequenceID}.");
                }
                else if (
                    !messageSequenceOutput.IsInSequence
                    && !messageSequenceOutput.IsAlreadyProcessed
                    && (receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount) == null
                    || int.Parse(receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount)) < gpmProducerServiceSettings.MaxRedeliveryCount)
                    )
                {
                    logger.Warn($@"Requesting redelivery for 
MessageID: {transactionID}, 
and PatchFamilyID: {correlationID}. 
Current JMSXDeliveryCount is {receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount ?? "1")}."
);
                    string errorDetails = $@"MessageID [{receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID)}] is out of sequence. 
Putting back into the queue for redelivery. 
The current JMSXDelivery count is {receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount) ?? "1"}.";
                    nearRealTimeProcessorDAL.ArchiveMessage(receivedMessage, Constants.ErrorCodes.OutOfSequenceRedelivery, errorDetails);
                }
                else if (
                    !messageSequenceOutput.IsInSequence
                    && !messageSequenceOutput.IsAlreadyProcessed
                    && receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount) != null
                    && int.Parse(receivedMessage.esbMessage.GetProperty(Constants.JMSMessageHeaders.JMSXDeliveryCount)) == gpmProducerServiceSettings.MaxRedeliveryCount
                    )
                {
                    string exceptionMessage = $@"Message is out of sequence and has exceeded the redelivery count of {gpmProducerServiceSettings.MaxRedeliveryCount}. 
For MessageID: {transactionID}, 
PatchFamilyID: {correlationID}, 
SequenceID: {sequenceID}";
                    throw new MessageOutOfSequenceException(exceptionMessage);
                }
            }
            catch (Exception exception)
            {
                logger.Error($@"There was an error processing MessageID: {transactionID}. ErrorMessage: {exception.Message}");
                if (exception is MessageOutOfSequenceException)
                {
                    processBODHandler.HandleError(receivedMessage, messageSequenceOutput);
                }
                else if (
                    exception is MappingException
                    || exception is ZeroRowsImpactedException
                    || exception is DataErrorException
                    || exception is DatabaseErrorException
                    || exception is ActionNotSuppliedException
                    || exception is InvalidMessageHeaderException
                    )
                {
                    confirmBODHandler.HandleError(receivedMessage, exception);
                }
                AcknowledgeEsbMessage(receivedMessage);
                errorEventPublisher.PublishErrorEvent(
                    "GPMNearRealTime",
                    transactionID,
                    messageProperties,
                    receivedMessage.esbMessage.MessageText,
                    exception.GetType().ToString(),
                    exception.Message,
                    "Fatal"
                    );
                nearRealTimeProcessorDAL.ArchiveMessage(receivedMessage, exception.GetType().ToString(), exception.Message);
            }
        }

        private void HandleEmergencyPrices(MammothPricesType mammothPrices)
        {
            MammothPriceType[] emergencyPrices = mammothPrices
                .MammothPrice
                .Where((mammothPrice) => DateTime.Compare(mammothPrice.StartDate.Date, DateTime.Today.Date) <= 0)
                .ToArray();
            if (emergencyPrices.Length > 0)
            {
                MammothPricesType emergencyMammothPrices = new MammothPricesType()
                {
                    MammothPrice = emergencyPrices
                };
                nearRealTimeProcessorDAL.InsertEmergencyPrices(emergencyMammothPrices);
            }
        }

        private void AcknowledgeEsbMessage(ReceivedMessage receivedMessage)
        {
            if (
                nearRealTimeListenerEsbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge
                || nearRealTimeListenerEsbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge
                || nearRealTimeListenerEsbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge
               )
            {
                receivedMessage.esbMessage.Acknowledge();
            }
        }

        private MammothPricesType MapToMammothPrices(items gpmReceivedItems)
        {
            MammothPricesType mammothPrices = new MammothPricesType();
            IList<MammothPriceType> mammothPriceList = new List<MammothPriceType>();
            try
            {
                foreach (ItemType gpmReceivedItem in gpmReceivedItems.item)
                {
                    foreach (LocaleType gpmReceivedLocale in gpmReceivedItem.locale)
                    {
                        GetRegionCodeQueryModel getRegionCodeQueryModel = nearRealTimeProcessorDAL.GetRegionCodeQuery(gpmReceivedLocale.id)[0];
                        StoreItemAttributesType storeItemAttributes = gpmReceivedLocale.Item as StoreItemAttributesType;
                        foreach (PriceType price in storeItemAttributes.prices)
                        {
                            MammothPriceType mammothPrice = new MammothPriceType()
                            {
                                Region = getRegionCodeQueryModel.Region,
                                BusinessUnit = getRegionCodeQueryModel.BusinessUnitID,
                                ItemId = gpmReceivedItem.id,
                                GpmId = price.Id, // TODO: Check if correct during testing
                                Multiple = price.priceMultiple,
                                Price = price.priceAmount.amount,
                                StartDate = price.priceStartDate,
                                EndDate = price.priceEndDate,
                                PriceType = nameof(price.type.id), // TODO: Check if correct during testing
                                PriceTypeAttribute = nameof(price.type.type.id), // TODO: Check if correct during testing
                                SellableUom = nameof(price.uom.code), // TODO: Check if correct during testing
                                CurrencyCode = nameof(price.currencyTypeCode), // TODO: Check if correct during testing
                                Action = nameof(price.Action), // TODO: Check if correct during testing
                                ItemTypeCode = gpmReceivedItem.@base.type.code, // TODO: Check if correct during testing
                                StoreName = gpmReceivedLocale.name,
                                ScanCode = storeItemAttributes.scanCode[0].code,
                            };
                            if (price.traits != null && price.traits.Length > 0)
                            {
                                TraitType nteTrait = Array.Find(price.traits, trait => "NTE".Equals(trait.code));
                                if (nteTrait != null && nteTrait.type.value.Length > 0 && String.IsNullOrEmpty(nteTrait.type.value[0].value))
                                {
                                    mammothPrice.TagExpirationDate = DateTime.Parse(nteTrait.type.value[0].value); // TODO: Check if correct parsing and timezone during testing
                                    mammothPrice.TagExpirationDateSpecified = true;
                                }
                            }
                            mammothPrice.EndDateSpecified = mammothPrice.EndDate.HasValue;
                            mammothPriceList.Add(mammothPrice);
                        }
                        foreach (RewardType reward in storeItemAttributes.rewards)
                        {
                            if (reward.rewardPercentage > 0)
                            {
                                MammothPriceType mammothPrice = new MammothPriceType()
                                {
                                    Region = getRegionCodeQueryModel.Region,
                                    BusinessUnit = getRegionCodeQueryModel.BusinessUnitID,
                                    ItemId = gpmReceivedItem.id,
                                    GpmId = reward.Id, // TODO: Check if correct during testing
                                    Multiple = reward.rewardMultiple,
                                    Price = 0,
                                    StartDate = reward.rewardStartDate,
                                    EndDate = reward.rewardEndDate,
                                    PriceType = nameof(reward.type.id), // TODO: Check if correct during testing
                                    PriceTypeAttribute = nameof(reward.type.type.id), // TODO: Check if correct during testing
                                    SellableUom = nameof(reward.uom.code), // TODO: Check if correct during testing
                                    Action = nameof(reward.Action), // TODO: Check if correct during testing
                                    ItemTypeCode = gpmReceivedItem.@base.type.code, // TODO: Check if correct during testing
                                    StoreName = gpmReceivedLocale.name,
                                    ScanCode = storeItemAttributes.scanCode[0].code,
                                    PercentOff = reward.rewardPercentage,
                                    PercentOffSpecified = true // TODO: Test for PercentOff null scenarios
                                };
                                mammothPrice.EndDateSpecified = mammothPrice.EndDate.HasValue;
                                mammothPriceList.Add(mammothPrice);
                            }
                        }
                    }
                }
            }
            catch (Exception e)
            {
                throw new MappingException(e.Message, e);
            }
            mammothPrices.MammothPrice = mammothPriceList.ToArray();
            return mammothPrices;
        }

        private MessageSequenceOutput ValidateMessageSequence(ReceivedMessage receivedMessage)
        {
            string messageID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID) ?? "";
            string sequenceIDString = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID);
            string patchFamilyID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID);
            string resetFlag = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.ResetFlag) ?? Constants.ResetFlagValues.ResetFlagFalseValue;
            if (string.IsNullOrEmpty(sequenceIDString) || int.Parse(sequenceIDString) < 1 || string.IsNullOrEmpty(patchFamilyID))
            {
                throw new InvalidMessageHeaderException($@"Message Sequence ID or CorrelationID is invalid.SequenceID must be greater than 0 or CorrelationID must not be empty. MessageID: {messageID}, CorrelationID(PatchFamilyID): {patchFamilyID}, SequenceID: {sequenceIDString}");
            }
            int sequenceID = int.Parse(sequenceIDString);
            IList<MessageSequenceModel> messageSequenceData = nearRealTimeProcessorDAL.GetLastSequence(patchFamilyID);
            bool messageSequenceDataAvailable = messageSequenceData.Count > 0;
            int lastProcessedGpmSequenceID = messageSequenceDataAvailable ? int.Parse(messageSequenceData[0].LastProcessedGpmSequenceID) : 0;
            return new MessageSequenceOutput
            {
                PatchFamilyId = patchFamilyID,
                SequenceID = sequenceID,
                LastProcessedGpmSequenceID = lastProcessedGpmSequenceID,
                IsInSequence = (messageSequenceDataAvailable && sequenceID == int.Parse(messageSequenceData[0].LastProcessedGpmSequenceID) + 1)
                || (sequenceID == 1 && !messageSequenceDataAvailable)
                || Constants.ResetFlagValues.ResetFlagTrueValue.Equals(resetFlag),
                IsAlreadyProcessed = sequenceID <= lastProcessedGpmSequenceID
            };
        }

        public void Process()
        {
            throw new NotImplementedException();
        }
    }
}
