using GPMService.Producer.Archive;
using GPMService.Producer.DataAccess;
using GPMService.Producer.Helpers;
using GPMService.Producer.Model;
using GPMService.Producer.Model.DBModel;
using GPMService.Producer.Publish;
using GPMService.Producer.Serializer;
using GPMService.Producer.Settings;
using Icon.Common.Xml;
using Icon.Esb;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.IO;

namespace GPMService.Producer.Message.Processor
{
    internal class EmergencyPriceMessageProcessor : IMessageProcessor
    {
        private readonly IEmergencyPriceProcessorDAL emergencyPriceProcessorDAL;
        private readonly GPMProducerServiceSettings gpmProducerServiceSettings;
        private readonly ISerializer<MammothPricesType> mammothPricesSerializer;
        private readonly ISerializer<MammothPriceType> mammothPriceSerializer;
        private readonly IMessagePublisher messagePublisher;
        private readonly JustInTimePriceArchiver justInTimePriceArchiver;
        private readonly ILogger<EmergencyPriceMessageProcessor> logger;

        public EmergencyPriceMessageProcessor
            (
            IEmergencyPriceProcessorDAL emergencyPriceProcessorDAL,
            GPMProducerServiceSettings gpmProducerServiceSettings,
            ISerializer<MammothPricesType> mammothPricesSerializer,
            ISerializer<MammothPriceType> mammothPriceSerializer,
            IMessagePublisher messagePublisher,
            JustInTimePriceArchiver justInTimePriceArchiver,
            ILogger<EmergencyPriceMessageProcessor> logger
            )
        {
            this.emergencyPriceProcessorDAL = emergencyPriceProcessorDAL;
            this.gpmProducerServiceSettings = gpmProducerServiceSettings;
            this.mammothPricesSerializer = mammothPricesSerializer;
            this.mammothPriceSerializer = mammothPriceSerializer;
            this.messagePublisher = messagePublisher;
            this.justInTimePriceArchiver = justInTimePriceArchiver;
            this.logger = logger;
        }

        public void Process()
        {
            try
            {
                logger.Info("Starting Mammoth Emergency Price Service");
                bool emergencyPricesExist = emergencyPriceProcessorDAL.EmergencyPricesExist();
                int messageSendErrorCount = 0;
                while (emergencyPricesExist)
                {
                    MammothPricesType emergencyMammothPrices = GetEmergencyPrices();
                    if (emergencyMammothPrices.MammothPrice != null && emergencyMammothPrices.MammothPrice.Length > 0)
                    {
                        Dictionary<string, string> messageProperties = new Dictionary<string, string>()
                        {
                            { Constants.MessageHeaders.TransactionID, Guid.NewGuid().ToString() },
                            { Constants.MessageHeaders.ResetFlag, "false" },
                            { Constants.MessageHeaders.TransactionType, Constants.TransactionTypes.Price },
                            { Constants.MessageHeaders.Source, Constants.Sources.JustInTimeSource },
                            { Constants.MessageHeaders.RegionCode, emergencyMammothPrices.MammothPrice[0].Region }
                        };
                        try
                        {
                            SendEmergencyPrices(emergencyMammothPrices, messageProperties);
                            justInTimePriceArchiver.ArchivePrice(emergencyMammothPrices, messageProperties);
                            messageSendErrorCount = 0;
                        }
                        catch (Exception ex)
                        {
                            messageSendErrorCount++;
                            logger.Error($"Error occurred when attempting to send prices to ESB. Will attempt to requeue emergency prices. Error: ${ex}");
                            emergencyPriceProcessorDAL.InsertPricesIntoEmergencyQueue(emergencyMammothPrices);
                        }
                        if (messageSendErrorCount == 0 || messageSendErrorCount < gpmProducerServiceSettings.SendMessageRetryCount)
                        {
                            emergencyPricesExist = emergencyPriceProcessorDAL.EmergencyPricesExist();
                        }
                        else
                        {
                            logger.Error($"Exceeded max number of send errors - ${gpmProducerServiceSettings.SendMessageRetryCount}. Will retry sending emergency prices on next run.");
                            emergencyPricesExist = false;
                        }
                    }
                    else
                    {
                        messageSendErrorCount = 0;
                        emergencyPricesExist = emergencyPriceProcessorDAL.EmergencyPricesExist();
                    }
                }
                logger.Info("Ending Mammoth Emergency Price Service successfully.");
            }
            catch (Exception ex)
            {
                logger.Error($"Error occurred in the Mammoth Emergency Price Service. Error: ${ex}");
            }
        }

        private void SendEmergencyPrices(MammothPricesType emergencyMammothPrices, Dictionary<string, string> messageProperties)
        {
            messagePublisher.PublishMessage(mammothPricesSerializer.Serialize(emergencyMammothPrices, new Utf8StringWriter()), messageProperties);
        }

        private MammothPricesType GetEmergencyPrices()
        {
            List<GetEmergencyPricesQueryModel> emergencyPricesDB = emergencyPriceProcessorDAL.GetEmergencyPrices();
            List<MammothPriceType> mammothPriceList = new List<MammothPriceType>();
            MammothPricesType emergencyMammothPrices = new MammothPricesType();
            foreach (GetEmergencyPricesQueryModel emergencyPriceDB in emergencyPricesDB)
            {
                using (TextReader textReader = new StringReader(Utility.RemoveUnusableCharactersFromXml(emergencyPriceDB.MammothPriceXml)))
                {
                    mammothPriceList.Add(mammothPriceSerializer.Deserialize(textReader));
                }
            }
            emergencyMammothPrices.MammothPrice = mammothPriceList.ToArray();
            return emergencyMammothPrices;
        }

        public void ProcessReceivedMessage(ReceivedMessage receivedMessage)
        {
            throw new NotImplementedException();
        }
    }
}
