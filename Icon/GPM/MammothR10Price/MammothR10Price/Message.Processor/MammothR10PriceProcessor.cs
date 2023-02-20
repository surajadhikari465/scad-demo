using System;
using System.Collections.Generic;
using System.Linq;
using Icon.Esb.Schemas.Mammoth;
using Icon.Logging;
using Polly;
using Polly.Retry;
using MammothR10Price.Mapper;
using MammothR10Price.Message.Archive;
using MammothR10Price.Publish;

using Items = Icon.Esb.Schemas.Wfm.Contracts.items;
using Wfm.Aws.ExtendedClient.SQS.Model;
using MammothR10Price.Message.Parser;

namespace MammothR10Price.Message.Processor
{
    public class MammothR10PriceProcessor: IMessageProcessor
    {
        private readonly MammothR10PriceServiceSettings serviceSettings;
        private readonly IMessageParser<MammothPricesType> messageParser;
        private readonly IErrorEventPublisher errorEventPublisher;
        private readonly IMessagePublisher messagePublisher;
        private readonly IMessageArchiver messageArchiver;
        private readonly ILogger<MammothR10PriceProcessor> logger;
        private readonly IMapper<IList<MammothPriceType>, Items> itemPriceCanonicalMapper;
        private readonly RetryPolicy retryPolicy;

        public MammothR10PriceProcessor(
            MammothR10PriceServiceSettings serviceSettings,
            IMessageParser<MammothPricesType> messageParser,
            IErrorEventPublisher errorEventPublisher,
            IMessagePublisher messagePublisher,
            IMessageArchiver messageArchiver,
            ILogger<MammothR10PriceProcessor> logger,
            IMapper<IList<MammothPriceType>, Items> itemPriceCanonicalMapper
        )
        {
            this.serviceSettings = serviceSettings;
            this.messageParser = messageParser;
            this.errorEventPublisher = errorEventPublisher;
            this.messagePublisher = messagePublisher;
            this.messageArchiver = messageArchiver;
            this.logger = logger;
            this.itemPriceCanonicalMapper = itemPriceCanonicalMapper;

            retryPolicy = Policy
                .Handle<Exception>()
                .WaitAndRetry(
                    serviceSettings.SendMessageRetryCount, 
                    retryAttempt => TimeSpan.FromMilliseconds(serviceSettings.SendMessageRetryDelayInMilliseconds)
                );
        }

        public void ProcessReceivedMessage(SQSExtendedClientReceiveModel message)
        {
            IDictionary<string, string> messageProperties = new Dictionary<string, string>()
            {
                { Constants.MessageProperty.TransactionId, message.MessageAttributes[Constants.MessageProperty.TransactionId] },
                { Constants.MessageProperty.TransactionType, message.MessageAttributes[Constants.MessageProperty.TransactionType] },
                { Constants.MessageProperty.CorrelationId, message.MessageAttributes[Constants.MessageProperty.CorrelationId] },
                { Constants.MessageProperty.Source, message.MessageAttributes[Constants.MessageProperty.Source] },
                { Constants.MessageProperty.SequenceId, message.MessageAttributes[Constants.MessageProperty.SequenceId] },
                { Constants.MessageProperty.ResetFlag, message.MessageAttributes[Constants.MessageProperty.ResetFlag] }
            };
            try
            {
                var mammothPrices = messageParser.ParseMessage(message);
                IEnumerable<int> distinctBusinessUnits = mammothPrices.MammothPrice.Select(x => x.BusinessUnit).Distinct();
                foreach (int businessUnit in distinctBusinessUnits)
                {
                    this.retryPolicy.Execute(() =>
                    {
                        string messageId = Guid.NewGuid().ToString();
                        IList<MammothPriceType> businessUnitPrices = mammothPrices.MammothPrice.Where(x => x.BusinessUnit == businessUnit).ToList();
                        var itemPriceCanonical = itemPriceCanonicalMapper.Transform(businessUnitPrices);
                        string xmlMessage = itemPriceCanonicalMapper.ToXml(itemPriceCanonical);
                        messageProperties[Constants.MessageProperty.nonReceivingSysName] = serviceSettings.NonReceivingSystems;
                        Dictionary<string, string> esbMessageProperties = new Dictionary<string, string>(messageProperties);
                        Dictionary<string, string> dbArchiveMessageProperties = new Dictionary<string, string>();
                        dbArchiveMessageProperties[Constants.MessageProperty.MessageId] = messageProperties[Constants.MessageProperty.TransactionId];
                        if (Constants.Source.Mammoth.Equals(messageProperties[Constants.MessageProperty.Source]))
                        {
                            esbMessageProperties[Constants.MessageProperty.TransactionId] = messageId;
                            esbMessageProperties[Constants.MessageProperty.MammothMessageId] =
                            messageProperties[Constants.MessageProperty.TransactionId];
                            dbArchiveMessageProperties[Constants.MessageProperty.MessageId] = messageId;
                            dbArchiveMessageProperties[Constants.MessageProperty.MammothMessageId] =
                            messageProperties[Constants.MessageProperty.TransactionId];
                        }
                        messagePublisher.Publish(xmlMessage, esbMessageProperties);
                        messageArchiver.ArchiveMessage(
                            businessUnitPrices,
                            xmlMessage,
                            messageProperties,
                            dbArchiveMessageProperties
                            );
                    });
                }
                logger.Info($"Successfully processed Mammoth Price MessageID: {message.MessageAttributes[Constants.MessageProperty.TransactionId]}, mapped it to the price canonical, and sent a message to queue.");
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
                        message.S3Details[0].Data,
                        ex.GetType().ToString(),
                        ex.Message
                    );
                }
                catch (Exception errorPublisherException)
                {
                    logger.Error($"Publishing Error Event failed with following Exception: {errorPublisherException}");
                }
            }
        }
    }
}
