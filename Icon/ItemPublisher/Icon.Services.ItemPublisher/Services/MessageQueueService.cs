using Esb.Core.Serializer;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Services.ItemPublisher.Services
{
    public class MessageQueueService : IMessageQueueService
    {
        private readonly IMessageQueueClient messageQueueClient;
        private readonly IMessageBuilder MessageBuilder;
        private readonly ILogger<ItemPublisherService> logger;
        private readonly ISerializer<Contracts.items> serializer;

        public MessageQueueService(ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items> serializer,
            IMessageQueueClient messageQueueClient,
            IMessageBuilder MessageBuilder,
            ILogger<ItemPublisherService> logger)
        {
            this.serializer = serializer;
            this.messageQueueClient = messageQueueClient;
            this.logger = logger;
            this.MessageBuilder = MessageBuilder;
        }

        /// <summary>
        /// Determines if we are ready to construct and send dvs messages.
        /// Is the cache loaded?
        /// Is the DVS client connection working?
        /// </summary>
        public Task<bool> ReadyForProcessing
        {
            get
            {
                return Task.FromResult<bool>(this.MessageBuilder.CacheLoaded);
            }
        }

        /// <summary>
        /// Creates DVS messages and sends them to the DVS
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<MessageSendResult> Process(List<MessageQueueItemModel> records, List<string> nonReceivingSystems)
        {
            BuildMessageResult result = await this.MessageBuilder.BuildItem(records);

            if (result.Contract != null && result.Contract.item.Length > 0)
            {
                string message = this.serializer.Serialize(result.Contract);
                this.logger.Debug($"Message to be sent. Message={message}");
                MessageSendResult sendResult = await this.messageQueueClient.SendMessage(message, nonReceivingSystems);
                sendResult.SetWarnings(result.Errors);
                return sendResult;
            }
            else
            {
                this.logger.Info("Contract has no data, possibly because all records have been skipped");
                return null;
            }
        }
    }
}