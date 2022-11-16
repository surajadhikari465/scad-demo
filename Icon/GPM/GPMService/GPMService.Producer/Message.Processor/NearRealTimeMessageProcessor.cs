using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
using GPMService.Producer.GPMException;
using GPMService.Producer.Helpers;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Settings;
using Icon.Esb.Schemas.Wfm.Contracts;
using System;
using System.Collections.Generic;

namespace GPMService.Producer.Message.Processor
{
    internal class NearRealTimeMessageProcessor: IMessageProcessor
    {
        private readonly IMessageParser<items> messageParser;
        private readonly INearRealTimeProcessorDAL nearRealTimeProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ProcessBODErrorHandler processBODHandler;
        private readonly ConfirmBODErrorHandler confirmBODHandler;
        public NearRealTimeMessageProcessor(
            IMessageParser<items> messageParser,
            INearRealTimeProcessorDAL nearRealTimeProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ProcessBODErrorHandler processBODHandler,
            ConfirmBODErrorHandler confirmBODHandler
            )
        {
            this.messageParser = messageParser;
            this.nearRealTimeProcessorDAL = nearRealTimeProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.processBODHandler = processBODHandler;
            this.confirmBODHandler = confirmBODHandler;
        }

        public void ProcessMessage(ReceivedMessage receivedMessage)
        {
            MessageSequenceOutput messageSequenceOutput = null;
            try
            {
                messageSequenceOutput = ValidateMessageSequence(receivedMessage);
                if (messageSequenceOutput.IsInSequence || messageSequenceOutput.IsAlreadyProcessed)
                {
                    items gpmReceivedItems = messageParser.ParseMessage(receivedMessage);
                    MammothPricesType mammothPrices = MapToMammothPrices(gpmReceivedItems);
                    // TODO: Add further logic
                }
                else if (
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
                }
                else if (
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
            } catch (Exception exception)
            {
                // logger.Error($@"There was an error processing MessageID: ${receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID)}. ErrorMessage: ${exception.Message}");
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
                nearRealTimeProcessorDAL.ArchiveMessage(receivedMessage, "ERROR CODE", exception.Message); // TODO: Fix error code logic
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
                            mammothPrice.EndDateSpecified = mammothPrice.EndDate != null;
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
                                mammothPrice.EndDateSpecified = mammothPrice.EndDate != null;
                                mammothPriceList.Add(mammothPrice);
                            }
                        }
                    }
                }
            } catch (Exception e)
            {
                throw new MappingException(e.Message, e);
            }
            return mammothPrices;
        }

        private MessageSequenceOutput ValidateMessageSequence(ReceivedMessage receivedMessage)
        {
            string sequenceID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.SequenceID);
            string correlationID = receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.CorrelationID);
            if (string.IsNullOrEmpty(sequenceID) || int.Parse(sequenceID) < 1 || string.IsNullOrEmpty(correlationID))
            {
                throw new InvalidMessageHeaderException($@"Message Sequence ID or CorrelationID is invalid.SequenceID must be greater than 0 or CorrelationID must not be empty. MessageID: ${receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID)}, CorrelationID(PatchFamilyID): ${correlationID}, SequenceID: ${sequenceID}");
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
