﻿using System;
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
using MammothR10Price.Message.Archive;
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
        private readonly IMessageArchiver messageArchiver;
        private readonly ILogger<MammothR10PriceProcessor> logger;
        private readonly EsbConnectionSettings esbConnectionSettings;
        private readonly IMapper<IList<MammothPriceType>, Items> itemPriceCanonicalMapper;
        private readonly RetryPolicy retryPolicy;

        public MammothR10PriceProcessor(
            MammothR10PriceServiceSettings serviceSettings,
            IMessageParser<MammothPricesType> messageParser,
            IErrorEventPublisher errorEventPublisher,
            IMessagePublisher messagePublisher,
            IMessageArchiver messageArchiver,
            ILogger<MammothR10PriceProcessor> logger,
            EsbConnectionSettings esbConnectionSettings,
            IMapper<IList<MammothPriceType>, Items> itemPriceCanonicalMapper
        )
        {
            this.serviceSettings = serviceSettings;
            this.messageParser = messageParser;
            this.errorEventPublisher = errorEventPublisher;
            this.messagePublisher = messagePublisher;
            this.messageArchiver = messageArchiver;
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
            IDictionary<string, string> receivedMessageProperties = new Dictionary<string, string>()
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
                        string messageId = Guid.NewGuid().ToString();
                        IList<MammothPriceType> businessUnitPrices = mammothPrices.MammothPrice.Where(x => x.BusinessUnit == businessUnit).ToList();
                        var itemPriceCanonical = itemPriceCanonicalMapper.Transform(businessUnitPrices);
                        string xmlMessage = itemPriceCanonicalMapper.ToXml(itemPriceCanonical);
                        Dictionary<string, string> esbMessageProperties = new Dictionary<string, string>(receivedMessageProperties);
                        Dictionary<string, string> dbArchiveMessageProperties = new Dictionary<string, string>();
                        dbArchiveMessageProperties[Constants.MessageProperty.MessageId] = receivedMessageProperties[Constants.MessageProperty.TransactionId];
                        if (Constants.Source.Mammoth.Equals(receivedMessageProperties[Constants.MessageProperty.Source]))
                        {
                            esbMessageProperties[Constants.MessageProperty.TransactionId] = messageId;
                            esbMessageProperties[Constants.MessageProperty.MammothMessageId] = 
                            esbMessageProperties[Constants.MessageProperty.TransactionId];
                            dbArchiveMessageProperties[Constants.MessageProperty.MessageId] = messageId;
                            dbArchiveMessageProperties[Constants.MessageProperty.MammothMessageId] =
                            receivedMessageProperties[Constants.MessageProperty.TransactionId];
                        }
                        messagePublisher.Publish(xmlMessage, esbMessageProperties);
                        messageArchiver.ArchiveMessage(
                            businessUnitPrices,
                            xmlMessage,
                            receivedMessageProperties,
                            dbArchiveMessageProperties
                            );
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
                        receivedMessageProperties[Constants.MessageProperty.TransactionId],
                        receivedMessageProperties,
                        message.MessageText,
                        ex.GetType().ToString(),
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
