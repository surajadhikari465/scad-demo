using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.MessageQueue;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services
{
    /// <summary>
    /// ItemProcessor is responsible for taking a list of MessageQueueItemModels and filtering them into
    /// Retail, Non-Retail or Department Sale and then calling the DVS.
    /// </summary>
    public class ItemProcessor : IItemProcessor
    {
        private readonly IMessageQueueService dvsService;
        private readonly ILogger<ItemProcessor> logger;
        private readonly ServiceSettings serviceSettings;
        private readonly ISystemListBuilder systemListBuilder;

        public ItemProcessor(
            IMessageQueueService dvsService,
            ILogger<ItemProcessor> logger,
            ServiceSettings serviceSettings,
            ISystemListBuilder systemListBuilder)
        {
            this.dvsService = dvsService;
            this.logger = logger;
            this.serviceSettings = serviceSettings;
            this.systemListBuilder = systemListBuilder;
        }

        /// <summary>
        /// Indicates if this class is ready to process messages. The DVS service takes a bit to connect and it may not be ready.
        /// </summary>
        /// <returns></returns>
        public Task<bool> ReadyForProcessing
        {
            get
            {

                return this.dvsService.ReadyForProcessing;
            }
        }

        /// <summary>
        /// Takes a list of MessageQueueItemModels and filters them into retail only and calls the DVS
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<List<MessageSendResult>> ProcessRetailRecords(List<MessageQueueItemModel> records)
        {
            List<MessageSendResult> response = new List<MessageSendResult>();
            
            List<MessageQueueItemModel> retailItems = records.Where(
                x => (x.Item.ItemTypeCode != ItemPublisherConstants.NonRetailSaleTypeCode)
            ).ToList();

            if (retailItems.Count > 0)
            {
                response.Add(await this.DvsProcess(retailItems, this.systemListBuilder.BuildRetailNonReceivingSystemsList()));
            }

            return response;
        }

        /// <summary>
        /// Takes a list of MessageQueueItemModels and filters them into non-retail only and calls the DVS
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<List<MessageSendResult>> ProcessNonRetailRecords(List<MessageQueueItemModel> records)
        {
            List<MessageSendResult> response = new List<MessageSendResult>();

            List<MessageQueueItemModel> nonRetailItems = records.Where(
                x => (x.Item.ItemTypeCode == ItemPublisherConstants.NonRetailSaleTypeCode)
            ).ToList();

            if (nonRetailItems.Count > 0)
            {
                response.Add(await this.DvsProcess(nonRetailItems, this.systemListBuilder.BuildNonRetailReceivingSystemsList()));
            }

            return response;
        }

        /// <summary>
        /// Calls the DVS to process our list of queue records
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private async Task<MessageSendResult> DvsProcess(List<MessageQueueItemModel> records, List<string> nonReceivingSystems)
        {
            return await this.dvsService.Process(records, nonReceivingSystems);
        }
    }
}