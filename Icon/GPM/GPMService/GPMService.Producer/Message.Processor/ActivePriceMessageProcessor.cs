﻿using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
using GPMService.Producer.Helpers;
using GPMService.Producer.Message.Parser;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Publish;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.DbContextFactory;
using Icon.Esb;
using Icon.Logging;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using TIBCO.EMS;

namespace GPMService.Producer.Message.Processor
{
    internal class ActivePriceMessageProcessor : IMessageProcessor
    {
        private readonly IMessageParser<JobSchedule> messageParser;
        private readonly IActivePriceProcessorDAL activePriceProcessorDAL;
        private readonly ICommonDAL commonDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly EsbConnectionSettings esbConnectionSettings;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly ISerializer<MammothPricesType> serializer;
        private readonly IMessagePublisher messagePublisher;
        private readonly ErrorEventPublisher errorEventPublisher;
        private readonly ILogger<ActivePriceMessageProcessor> logger;
        public ActivePriceMessageProcessor(
            IMessageParser<JobSchedule> messageParser,
            IActivePriceProcessorDAL activePriceProcessorDAL,
            ICommonDAL commonDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            EsbConnectionSettings esbConnectionSettings,
            IDbContextFactory<MammothContext> mammothContextFactory,
            ISerializer<MammothPricesType> serializer,
            IMessagePublisher messagePublisher,
            ErrorEventPublisher errorEventPublisher,
            ILogger<ActivePriceMessageProcessor> logger
            )
        {
            this.messageParser = messageParser;
            this.activePriceProcessorDAL = activePriceProcessorDAL;
            this.commonDAL = commonDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.esbConnectionSettings = esbConnectionSettings;
            this.mammothContextFactory = mammothContextFactory;
            this.serializer = serializer;
            this.messagePublisher = messagePublisher;
            this.errorEventPublisher = errorEventPublisher;
            this.logger = logger;
        }

        public void ProcessReceivedMessage(ReceivedMessage receivedMessage)
        {
            JobSchedule jobScheduleMessage = null;
            try
            {
                jobScheduleMessage = messageParser.ParseMessage(receivedMessage);
                commonDAL.UpdateStatusToRunning(jobScheduleMessage.JobScheduleId);
                DateTimeOffset currentDateTime = DateTimeOffset.Now;
                logger.Info($@"Region: ${jobScheduleMessage.Region}. Starting Mammoth Active Price Service.");
                ProcessActivePrices(jobScheduleMessage, currentDateTime);
                commonDAL.UpdateStatusToReady(jobScheduleMessage.JobScheduleId);
                logger.Info($@"Region: ${jobScheduleMessage.Region}. Ending Mammoth Active Price Service successfully.");
            }
            catch (Exception e)
            {
                logger.Error($@"Region: ${jobScheduleMessage?.Region}, Mammoth Active Price Service error occurred. ${e}");
                errorEventPublisher.PublishErrorEvent(
                    "MammothActivePrice",
                    receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID),
                    new Dictionary<string, string>()
                    {
                        { Constants.MessageHeaders.RegionCode, receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.RegionCode) }
                    },
                    receivedMessage.esbMessage.MessageText,
                    e.GetType().ToString(),
                    e.Message,
                    "FATAL"
                    );
            }
            finally
            {
                if (
                    esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge
                    || esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge
                    || esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge
                )
                {
                    receivedMessage.esbMessage.Acknowledge();
                }
            }
        }

        private void ProcessActivePrices(JobSchedule jobScheduleMessage, DateTimeOffset currentDateTime)
        {
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                IEnumerable<GetActivePricesQueryModel> activePricesEnumerable = activePriceProcessorDAL.GetActivePrices(mammothContext, jobScheduleMessage.Region);
                IList<GetActivePricesQueryModel> activePricesSubset = new List<GetActivePricesQueryModel>();
                foreach (GetActivePricesQueryModel activePrice in activePricesEnumerable)
                {
                    activePricesSubset.Add(activePrice);
                    if (activePricesSubset.Count == gpmProducerServiceSettings.ActivePriceSubsetSize)
                    {
                        ProcessActivePricesSubset(jobScheduleMessage, activePricesSubset, currentDateTime);
                        activePricesSubset.Clear();
                    }
                }
            }
        }

        private void ProcessActivePricesSubset(JobSchedule jobScheduleMessage, IList<GetActivePricesQueryModel> activePricesSubset, DateTimeOffset currentDateTime)
        {
            IEnumerable<int> uniqueBusinessUnits = activePricesSubset.Select(x => x.BusinessUnitID).Distinct();
            IList<MammothPriceType> mammothPriceList = new List<MammothPriceType>();
            foreach (int businessUnit in uniqueBusinessUnits)
            {
                IEnumerable<GetActivePricesQueryModel> currentBusinessUnitActivePrices = activePricesSubset.Where(x => x.BusinessUnitID == businessUnit);
                MammothPricesType mammothPrices = new MammothPricesType();
                foreach (GetActivePricesQueryModel currentBusinessUnitActivePrice in currentBusinessUnitActivePrices)
                {
                    MammothPriceType mammothPrice = new MammothPriceType()
                    {
                        Region = currentBusinessUnitActivePrice.Region,
                        PriceID = currentBusinessUnitActivePrice.PriceID,
                        PriceIDSpecified = true,
                        BusinessUnit = currentBusinessUnitActivePrice.BusinessUnitID,
                        ItemId = currentBusinessUnitActivePrice.ItemID,
                        GpmId = currentBusinessUnitActivePrice.GpmID,
                        Multiple = currentBusinessUnitActivePrice.Multiple,
                        Price = currentBusinessUnitActivePrice.Price,
                        StartDate = currentBusinessUnitActivePrice.StartDate,
                        EndDate = currentBusinessUnitActivePrice.EndDate,
                        EndDateSpecified = currentBusinessUnitActivePrice.EndDate.HasValue,
                        PriceType = currentBusinessUnitActivePrice.PriceType,
                        PriceTypeAttribute = currentBusinessUnitActivePrice.PriceTypeAttribute,
                        SellableUom = currentBusinessUnitActivePrice.SellableUOM,
                        CurrencyCode = currentBusinessUnitActivePrice.CurrencyCode,
                        TagExpirationDate = currentBusinessUnitActivePrice.TagExpirationDate,
                        TagExpirationDateSpecified = currentBusinessUnitActivePrice.TagExpirationDate.HasValue,
                        Action = Constants.PriceActions.AddOrUpdate,
                        ItemTypeCode = currentBusinessUnitActivePrice.ItemTypeCode,
                        StoreName = currentBusinessUnitActivePrice.StoreName,
                        ScanCode = currentBusinessUnitActivePrice.ScanCode,
                        SubTeamNumber = currentBusinessUnitActivePrice.SubTeamNumber,
                        SubTeamNumberSpecified = currentBusinessUnitActivePrice.SubTeamNumber.HasValue,
                        PercentOff = currentBusinessUnitActivePrice.PercentOff,
                        PercentOffSpecified = currentBusinessUnitActivePrice.PercentOff.HasValue
                    };
                }
                mammothPrices.MammothPrice = mammothPriceList.ToArray();
                int counterLimit = (int)Math.Ceiling((double)mammothPrices.MammothPrice.Length / gpmProducerServiceSettings.ActivePriceBatchSize);
                for (int counter = 0; counter < counterLimit; counter++)
                {
                    int startPosition = counter * gpmProducerServiceSettings.ActivePriceBatchSize;
                    int endPosition = (counter + 1) * gpmProducerServiceSettings.ActivePriceBatchSize;
                    MammothPricesType mammothPricesToBeSent = new MammothPricesType()
                    {
                        MammothPrice = mammothPrices.MammothPrice.Where((x, i) => i >= startPosition && i < endPosition).ToArray()
                    };
                    Dictionary<string, string> messageProperties = new Dictionary<string, string>()
                        {
                            { Constants.MessageHeaders.TransactionID, Guid.NewGuid().ToString() },
                            { Constants.MessageHeaders.CorrelationID, currentDateTime.ToString("O") },
                            { Constants.MessageHeaders.ResetFlag, "false" },
                            { Constants.MessageHeaders.TransactionType, Constants.TransactionTypes.Price },
                            { Constants.MessageHeaders.Source, Constants.Sources.JustInTimeSource },
                            { Constants.MessageHeaders.RegionCode, jobScheduleMessage.Region },
                        };
                    try
                    {
                        messagePublisher.PublishMessage(serializer.Serialize(mammothPricesToBeSent, new Utf8StringWriter()), messageProperties);
                        logger.Info($@"Region: ${jobScheduleMessage.Region} | BusinessUnitID ${businessUnit} | Number of records sent to EMS: ${mammothPricesToBeSent.MammothPrice.Length}");
                    }
                    catch (Exception ex)
                    {
                        logger.Error($@"Region: ${jobScheduleMessage.Region}. ${ex}");
                        ProcessRowByRow(mammothPricesToBeSent, messageProperties);
                    }
                }
            }
        }

        private void ProcessRowByRow(MammothPricesType mammothPricesToBeSent, Dictionary<string, string> messageProperties)
        {
            logger.Info($@"Region: ${messageProperties[Constants.MessageHeaders.RegionCode]}. Started sending Row By Row.");
            for (int i = 0; i < mammothPricesToBeSent.MammothPrice.Length; i++)
            {
                MammothPricesType currentMammothPrices = new MammothPricesType()
                {
                    MammothPrice = new MammothPriceType[1]
                    {
                        mammothPricesToBeSent.MammothPrice[i]
                    }
                };
                try
                {
                    messagePublisher.PublishMessage(serializer.Serialize(currentMammothPrices, new Utf8StringWriter()), messageProperties);
                }
                catch (Exception ex)
                {
                    logger.Error($@"Error while processing record with Item ID: ${currentMammothPrices.MammothPrice[0].ItemId}, Business Unit ID: ${currentMammothPrices.MammothPrice[0].BusinessUnit}, Error is: ${ex}");
                }
            }
        }

        public void Process()
        {
            throw new NotImplementedException();
        }
    }
}
