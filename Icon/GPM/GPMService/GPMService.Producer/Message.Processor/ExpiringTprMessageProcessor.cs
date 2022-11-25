using GPMService.Producer.DataAccess;
using GPMService.Producer.ErrorHandler;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Model;
using GPMService.Producer.Publish;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.Esb;
using Icon.Logging;
using Mammoth.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using GPMService.Producer.Message.Parser;
using Icon.DbContextFactory;
using GPMService.Producer.Helpers;
using TIBCO.EMS;

namespace GPMService.Producer.Message.Processor
{
    internal class ExpiringTprMessageProcessor : IMessageProcessor
    {
        private readonly IMessageParser<JobSchedule> messageParser;
        private readonly IExpiringTprProcessorDAL expiringTprProcessorDAL;
        private readonly ICommonDAL commonDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly EsbConnectionSettings esbConnectionSettings;
        private readonly IDbContextFactory<MammothContext> mammothContextFactory;
        private readonly ISerializer<MammothPricesType> serializer;
        private readonly IMessagePublisher messagePublisher;
        private readonly ErrorEventPublisher errorEventPublisher;
        private readonly ILogger<ExpiringTprMessageProcessor> logger;
        public ExpiringTprMessageProcessor(
            IMessageParser<JobSchedule> messageParser,
            IExpiringTprProcessorDAL expiringTprProcessorDAL,
            ICommonDAL commonDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            EsbConnectionSettings esbConnectionSettings,
            IDbContextFactory<MammothContext> mammothContextFactory,
            ISerializer<MammothPricesType> serializer,
            IMessagePublisher messagePublisher,
            ErrorEventPublisher errorEventPublisher,
            ILogger<ExpiringTprMessageProcessor> logger
            )
        {
            this.messageParser = messageParser;
            this.expiringTprProcessorDAL = expiringTprProcessorDAL;
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
                if ("running".Equals(jobScheduleMessage.Status))
                {
                    logger.Info($@"Region: ${jobScheduleMessage.Region}. Tpr Service Already running So Service will not run.");
                }
                else
                {
                    commonDAL.UpdateStatusToRunning(jobScheduleMessage.JobScheduleId);
                    logger.Info($@"Region: ${jobScheduleMessage.Region}. Starting Expiring TPR service.");
                    ProcessExpiringTprs(jobScheduleMessage);
                    commonDAL.UpdateStatusToReady(jobScheduleMessage.JobScheduleId);
                    logger.Info($@"Region: ${jobScheduleMessage.Region}. Ending Expiring TPR service successfully.");
                }
            }
            catch (Exception e)
            {
                logger.Error($@"Region: ${jobScheduleMessage?.Region}, Expiring TPR Service error occurred. ${e}");
                errorEventPublisher.PublishErrorEvent(
                    "ExpiringTpr",
                    receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.TransactionID),
                    new Dictionary<string, string>()
                    {
                        { Constants.MessageHeaders.Region, receivedMessage.esbMessage.GetProperty(Constants.MessageHeaders.RegionCode) }
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

        private void ProcessExpiringTprs(JobSchedule jobScheduleMessage)
        {
            using (var mammothContext = mammothContextFactory.CreateContext())
            {
                IEnumerable<GetExpiringTprsQueryModel> expiringTprsEnumerable = expiringTprProcessorDAL.GetExpiringTprs(mammothContext, jobScheduleMessage.Region);
                IList<GetExpiringTprsQueryModel> expiringTprsSubset = new List<GetExpiringTprsQueryModel>();
                foreach (GetExpiringTprsQueryModel expiringTpr in expiringTprsEnumerable)
                {
                    expiringTprsSubset.Add(expiringTpr);
                    if (expiringTprsSubset.Count == gpmProducerServiceSettings.ExpiringTprSubsetSize)
                    {
                        ProcessExpiringTprsSubset(jobScheduleMessage, expiringTprsSubset);
                        expiringTprsSubset.Clear();
                    }
                }
            }
        }

        private void ProcessExpiringTprsSubset(JobSchedule jobScheduleMessage, IList<GetExpiringTprsQueryModel> expiringTprsSubset)
        {
            IEnumerable<int> uniqueBusinessUnits = expiringTprsSubset.Select(x => x.BusinessUnitID).Distinct();
            IList<MammothPriceType> mammothPriceList = new List<MammothPriceType>();
            foreach (int businessUnit in uniqueBusinessUnits)
            {
                IEnumerable<GetExpiringTprsQueryModel> currentBusinessUnitExpiringTprs = expiringTprsSubset.Where(x => x.BusinessUnitID == businessUnit);
                MammothPricesType mammothPrices = new MammothPricesType();
                foreach (GetExpiringTprsQueryModel currentBusinessUnitExpiringTpr in currentBusinessUnitExpiringTprs)
                {
                    MammothPriceType mammothPrice = new MammothPriceType()
                    {
                        Region = currentBusinessUnitExpiringTpr.Region,
                        PriceID = currentBusinessUnitExpiringTpr.PriceID,
                        PriceIDSpecified = true,
                        BusinessUnit = currentBusinessUnitExpiringTpr.BusinessUnitID,
                        ItemId = currentBusinessUnitExpiringTpr.ItemID,
                        GpmId = currentBusinessUnitExpiringTpr.GpmID,
                        Multiple = currentBusinessUnitExpiringTpr.Multiple,
                        Price = currentBusinessUnitExpiringTpr.Price,
                        StartDate = currentBusinessUnitExpiringTpr.StartDate,
                        EndDate = currentBusinessUnitExpiringTpr.EndDate,
                        EndDateSpecified = currentBusinessUnitExpiringTpr.EndDate.HasValue,
                        PriceType = currentBusinessUnitExpiringTpr.PriceType,
                        PriceTypeAttribute = currentBusinessUnitExpiringTpr.PriceTypeAttribute,
                        SellableUom = currentBusinessUnitExpiringTpr.SellableUOM,
                        CurrencyCode = currentBusinessUnitExpiringTpr.CurrencyCode,
                        TagExpirationDate = currentBusinessUnitExpiringTpr.TagExpirationDate,
                        TagExpirationDateSpecified = currentBusinessUnitExpiringTpr.TagExpirationDate.HasValue,
                        Action = Constants.PriceActions.Delete,
                        ItemTypeCode = currentBusinessUnitExpiringTpr.ItemTypeCode,
                        StoreName = currentBusinessUnitExpiringTpr.StoreName,
                        ScanCode = currentBusinessUnitExpiringTpr.ScanCode,
                        SubTeamNumber = currentBusinessUnitExpiringTpr.SubTeamNumber,
                        SubTeamNumberSpecified = currentBusinessUnitExpiringTpr.SubTeamNumber.HasValue,
                        PercentOff = currentBusinessUnitExpiringTpr.PercentOff,
                        PercentOffSpecified = currentBusinessUnitExpiringTpr.PercentOff.HasValue
                    };
                }
                mammothPrices.MammothPrice = mammothPriceList.ToArray();
                int counterLimit = (int)Math.Ceiling((double)mammothPrices.MammothPrice.Length / gpmProducerServiceSettings.ExpiringTprBatchSize);
                for (int counter = 0; counter < counterLimit; counter++)
                {
                    int startPosition = counter * gpmProducerServiceSettings.ExpiringTprBatchSize;
                    int endPosition = (counter + 1) * gpmProducerServiceSettings.ExpiringTprBatchSize;
                    MammothPricesType mammothPricesToBeSent = new MammothPricesType()
                    {
                        MammothPrice = mammothPrices.MammothPrice.Where((x, i) => i >= startPosition && i < endPosition).ToArray()
                    };
                    Dictionary<string, string> messageProperties = new Dictionary<string, string>()
                        {
                            { Constants.MessageHeaders.TransactionID, Guid.NewGuid().ToString() },
                            { Constants.MessageHeaders.nonReceivingSysName, "ExpiredPriceReductionToPOS" },
                            { Constants.MessageHeaders.ResetFlag, "false" },
                            { Constants.MessageHeaders.TransactionType, Constants.TransactionTypes.ExpiringTprs },
                            { Constants.MessageHeaders.Source, Constants.Sources.JustInTimeSource },
                            { Constants.MessageHeaders.RegionCode, jobScheduleMessage.Region },
                        };
                    try
                    {
                        messagePublisher.PublishMessage(serializer.Serialize(mammothPricesToBeSent, new Utf8StringWriter()), messageProperties);
                        logger.Info($@"Region: ${jobScheduleMessage.Region} | BusinessUnitID ${businessUnit} | Number of expiring TPR Records sent to EMS: ${mammothPricesToBeSent.MammothPrice.Length}");
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
            logger.Info($@"Region: ${messageProperties[Constants.MessageHeaders.RegionCode]}. Started sending Row By Row Expiring Tprs.");
            messageProperties[Constants.MessageHeaders.nonReceivingSysName] = "MammothR10";
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
                    logger.Error($@"Error while sending expiring Tpr for Item ID: ${currentMammothPrices.MammothPrice[0].ItemId}, Business Unit ID: ${currentMammothPrices.MammothPrice[0].BusinessUnit}, Error is: ${ex}");
                }
            }
        }

        public void Process()
        {
            throw new NotImplementedException();
        }
    }
}
