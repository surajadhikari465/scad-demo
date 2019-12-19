using Esb.Core.Serializer;
using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure.Esb;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Contracts = Icon.Esb.Schemas.Wfm.Contracts;

namespace Icon.Services.ItemPublisher.Services
{
    public class EsbService : IEsbService
    {
        private readonly IEsbClient esbClient;
        private readonly IEsbMessageBuilder esbMessageBuilder;
        private readonly ILogger<ItemPublisherService> logger;
        private readonly ISerializer<Contracts.items> serializer;

        public EsbService(ISerializer<Icon.Esb.Schemas.Wfm.Contracts.items> serializer,
            IEsbClient esbClient,
            IEsbMessageBuilder esbMessageBuilder,
            ILogger<ItemPublisherService> logger)
        {
            this.serializer = serializer;
            this.esbClient = esbClient;
            this.logger = logger;
            this.esbMessageBuilder = esbMessageBuilder;
        }

        /// <summary>
        /// Determines if we are ready to construct and send esb messages.
        /// Is the cache loaded?
        /// Is the ESB client connection working?
        /// </summary>
        public Task<bool> ReadyForProcessing
        {
            get
            {
                return Task.FromResult<bool>(this.esbMessageBuilder.CacheLoaded);
            }
        }

        /// <summary>
        /// Creates ESB messages and sends them to the ESB
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<EsbSendResult> Process(List<MessageQueueItemModel> records, List<string> nonReceivingSystems, bool isDepartmentSale)
        {
            BuildMessageResult result = await this.esbMessageBuilder.BuildItem(records, isDepartmentSale);

            if (result.Contract != null && result.Contract.item.Length > 0)
            {
                string esbMessage = this.serializer.Serialize(result.Contract);
                this.logger.Debug($"ESB message to be sent. Message={esbMessage}");
                EsbSendResult sendResult = await this.esbClient.SendMessage(esbMessage, nonReceivingSystems);
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