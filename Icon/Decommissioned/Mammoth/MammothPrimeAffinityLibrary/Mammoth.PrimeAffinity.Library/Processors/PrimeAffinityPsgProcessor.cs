using Esb.Core.MessageBuilders;
using Icon.Logging;
using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.PrimeAffinity.Library.Commands;
using Mammoth.PrimeAffinity.Library.Constants;
using Mammoth.PrimeAffinity.Library.Esb;
using Mammoth.PrimeAffinity.Library.MessageBuilders;
using MoreLinq;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.PrimeAffinity.Library.Processors
{
    public class PrimeAffinityPsgProcessor : IPrimeAffinityPsgProcessor<PrimeAffinityPsgProcessorParameters>
    {
        private PrimeAffinityPsgProcessorSettings settings;
        private IMessageBuilder<PrimeAffinityMessageBuilderParameters> messageBuilder;
        private ICommandHandler<ArchivePrimeAffinityMessageCommand> archivePrimeAffinityMessagesCommandHandler;
        private IEsbConnectionCacheFactory connectionCacheFactory;
        private ILogger<PrimeAffinityPsgProcessor> logger;

        public PrimeAffinityPsgProcessor(
            PrimeAffinityPsgProcessorSettings settings,
            IMessageBuilder<PrimeAffinityMessageBuilderParameters> messageBuilder,
            ICommandHandler<ArchivePrimeAffinityMessageCommand> archivePrimeAffinityMessagesCommandHandler,
            IEsbConnectionCacheFactory connectionCacheFactory,
            ILogger<PrimeAffinityPsgProcessor> logger)
        {
            this.settings = settings;
            this.messageBuilder = messageBuilder;
            this.archivePrimeAffinityMessagesCommandHandler = archivePrimeAffinityMessagesCommandHandler;
            this.connectionCacheFactory = connectionCacheFactory;
            this.logger = logger;
        }

        public void SendPsgs(PrimeAffinityPsgProcessorParameters parameters)
        {
            logger.Info(new { Message = "Sending Prime Affinity PSGs.", Region = parameters.Region, MessageAction = parameters.MessageAction.ToString() }.ToJson());
            using (var producer = connectionCacheFactory.CreateProducer())
            {
                foreach (var psgBatch in parameters.PrimeAffinityMessageModels.DistinctBy(p => new { p.BusinessUnitID, p.ItemID, p.MessageAction }).Batch(100))
                {
                    foreach (var psgBatchGroup in psgBatch.GroupBy(b => b.BusinessUnitID))
                    {
                        var psgList = psgBatchGroup.ToList();
                        var message = messageBuilder.BuildMessage(new PrimeAffinityMessageBuilderParameters
                        {
                            PrimeAffinityMessageModels = psgList,
                        });
                        try
                        {
                            var messageId = Guid.NewGuid().ToString();
                            var messageProperties = new Dictionary<string, string>
                            {
                                { PrimeAffinityConstants.Esb.IconMessageIdMessageHeaderKey, messageId },
                                { PrimeAffinityConstants.Esb.SourceMessageHeaderKey, PrimeAffinityConstants.Esb.SourceMessageHeaderValue },
                                { PrimeAffinityConstants.Esb.NonReceivingSystemsMessageHeaderKey, settings.NonReceivingSystems },
								{ PrimeAffinityConstants.Esb.TransactionTypeKey, "Item/Locale"}
							};
                            producer.Send(message, messageId, messageProperties);
                            archivePrimeAffinityMessagesCommandHandler.Execute(new ArchivePrimeAffinityMessageCommand
                            {
                                Message = message,
                                MessageStatusId = MessageStatusTypes.Sent,
                                PrimeAffinityMessageModels = psgList,
                                MessageHeadersJson = messageProperties.ToJson(),
                                MessageId = messageId
                            });

                            logger.Info(
                                new
                                {
                                    Message = "Sent Prime Affinity PSGs.",
                                    Region = parameters.Region,
                                    MessageAction = parameters.MessageAction.ToString(),
                                    BusinessUnitID = psgBatchGroup.Key,
                                    NumberOfPSGsSent = psgList.Count
                                }.ToJson());
                        }
                        catch (Exception ex)
                        {
                            logger.Error(
                                new
                                {
                                    Message = "Unexpected Error occurred when sending Prime Affinity PSGs.",
                                    Region = parameters.Region,
                                    MessageAction = parameters.MessageAction.ToString(),
                                    BusinessUnitID = psgBatchGroup.Key,
                                    NumberOfPSGsSent = psgList.Count,
                                    Error = ex.ToString()
                                }.ToJson());

                            foreach (var price in psgList)
                            {
                                price.ErrorCode = PrimeAffinityConstants.Errors.Codes.FailedToSendToEsb;
                                price.ErrorDetails = ex.ToString();
                            }
                            archivePrimeAffinityMessagesCommandHandler.Execute(new ArchivePrimeAffinityMessageCommand
                            {
                                Message = message,
                                MessageStatusId = MessageStatusTypes.Failed,
                                PrimeAffinityMessageModels = psgList
                            });
                        }
                    }
                }
            }
        }
    }
}