using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Esb;
using Icon.Esb.Subscriber;
using Icon.Esb.MessageParsers;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using Polly;
using Polly.Retry;
using MammothR10Price.Mapper;
using MammothR10Price.Publish;
using TIBCO.EMS;

using Items = Icon.Esb.Schemas.Wfm.Contracts.items;

namespace MammothR10Price.Message.Processor
{
    public class MammothR10PriceProcessor: IMessageProcessor
    {
        private readonly MammothR10PriceServiceSettings serviceSettings;
        private readonly IMessageParser<MammothPricesType> messageParser;
        private readonly IErrorEventPublisher errorEventPublisher;
        private readonly IMessagePublisher messagePublisher;
        private readonly ILogger<MammothR10PriceProcessor> logger;
        private readonly EsbConnectionSettings esbConnectionSettings;
        private readonly IMapper<IList<MammothPriceType>, Items> itemPriceCanonicalMapper;
        private readonly RetryPolicy retryPolicy;

        public MammothR10PriceProcessor(
            MammothR10PriceServiceSettings serviceSettings,
            IMessageParser<MammothPricesType> messageParser,
            IErrorEventPublisher errorEventPublisher,
            IMessagePublisher messagePublisher,
            ILogger<MammothR10PriceProcessor> logger,
            EsbConnectionSettings esbConnectionSettings,
            IMapper<IList<MammothPriceType>, Items> itemPriceCanonicalMapper
        )
        {
            this.serviceSettings = serviceSettings;
            this.messageParser = messageParser;
            this.errorEventPublisher = errorEventPublisher;
            this.messagePublisher = messagePublisher;
            this.logger = logger;
            this.esbConnectionSettings = esbConnectionSettings;
            this.itemPriceCanonicalMapper = itemPriceCanonicalMapper;

            retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    serviceSettings.SendMessageRetryCount, 
                    retryAttempt => TimeSpan.FromMilliseconds(serviceSettings.SendMessageRetryDelayInMilliseconds)
                );
        }

        public void ProcessReceivedMessage(IEsbMessage message)
        {
            var messageProperties = new Dictionary<string, string>()
            {
                { Constants.MessageProperty.TransactionId, message.GetProperty(Constants.MessageProperty.TransactionId) },
                { Constants.MessageProperty.TransactionType, message.GetProperty(Constants.MessageProperty.TransactionType) },
                { Constants.MessageProperty.CorrelationId, message.GetProperty(Constants.MessageProperty.CorrelationId) },
                { Constants.MessageProperty.Source, message.GetProperty(Constants.MessageProperty.Source) },
                { Constants.MessageProperty.SequenceId, message.GetProperty(Constants.MessageProperty.SequenceId) },
                { Constants.MessageProperty.ResetFlag, message.GetProperty(Constants.MessageProperty.ResetFlag) },
                { Constants.MessageProperty.NonReceivingSystems, message.GetProperty(Constants.MessageProperty.NonReceivingSystems) }
            };

            try
            {
                var mammothPrices = messageParser.ParseMessage(message);
                IEnumerable<int> distinctBusinessUnits = mammothPrices.MammothPrice.Select(x => x.BusinessUnit).Distinct();

                foreach (int businessUnit in distinctBusinessUnits)
                {
                    this.retryPolicy.Execute(() =>
                    {
                        IList<MammothPriceType> businessUnitPrices = mammothPrices.MammothPrice.Where(x => x.BusinessUnit == businessUnit).ToList();
                        var itemPriceCanonical = itemPriceCanonicalMapper.Transform(businessUnitPrices);
                        string xmlMessage = itemPriceCanonicalMapper.ToXml(itemPriceCanonical);

                        if (Constants.Source.Mammoth.Equals(messageProperties[Constants.MessageProperty.Source]))
                        {
                            string messageId = Guid.NewGuid().ToString();
                            messageProperties[Constants.MessageProperty.TransactionId] = messageId;
                            messageProperties[Constants.MessageProperty.MammothMessageId] = messageId;
                        }

                        messagePublisher.Publish(xmlMessage, messageProperties);
                        ArchiveMessage(businessUnitPrices, itemPriceCanonical, messageProperties);
                    });
                }

                logger.Info($"Successfully processed Mammoth Price  MessageID: {message.GetProperty(Constants.MessageProperty.TransactionId)}");
            }
            catch (Exception ex)
            {
                logger.Error(ex.ToString());
                try
                {
                    errorEventPublisher.PublishErrorEvent(
                        serviceSettings.ApplicationName,
                        messageProperties[Constants.MessageProperty.TransactionId],
                        messageProperties,
                        ex.Message,
                        $"Exception : {ex.GetType()}",
                        ex.ToString(),
                        "Fatal"
                    );
                }
                catch(Exception errorPublisherException)
                {
                    logger.Error($"Publishing Error Event failed with following Exception: {errorPublisherException}");    
                }
            }
            finally
            {
                AcknowledgeMessage(message);
            }
        }

        // TODO: Complete the Archiving Part
        private void ArchiveMessage(IList<MammothPriceType> mammothPrices, Items itemPriceCanonical, IDictionary<string, string> messageProperties)
        {

        }

        private void AcknowledgeMessage(IEsbMessage message)
        {
            if (
                esbConnectionSettings.SessionMode == SessionMode.ClientAcknowledge
                || esbConnectionSettings.SessionMode == SessionMode.ExplicitClientAcknowledge
                || esbConnectionSettings.SessionMode == SessionMode.ExplicitClientDupsOkAcknowledge
               )
            {
                message.Acknowledge();
            }
        }
    }
}
