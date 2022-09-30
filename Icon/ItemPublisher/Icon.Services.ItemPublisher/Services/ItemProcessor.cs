using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Filters;
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
    /// Retail, Non-Retail or Department Sale and then calling the ESB.
    /// </summary>
    public class ItemProcessor : IItemProcessor
    {
        private readonly IMessageQueueService esbService;
        private readonly ILogger<ItemProcessor> logger;
        private readonly ServiceSettings serviceSettings;
        private readonly ISystemListBuilder systemListBuilder;
        private readonly IFilter ukItemFilter;

        public ItemProcessor(
            IMessageQueueService esbService,
            ILogger<ItemProcessor> logger,
            ServiceSettings serviceSettings,
            ISystemListBuilder systemListBuilder,
            IFilter ukItemFilter)
        {
            this.esbService = esbService;
            this.logger = logger;
            this.serviceSettings = serviceSettings;
            this.systemListBuilder = systemListBuilder;
            this.ukItemFilter = ukItemFilter;
        }

        /// <summary>
        /// Indicates if this class is ready to process messages. The ESB takes a bit to connect and it may not be ready.
        /// </summary>
        /// <returns></returns>
        public Task<bool> ReadyForProcessing
        {
            get
            {

                return this.esbService.ReadyForProcessing;
            }
        }

        /// <summary>
        /// Takes a list of MessageQueueItemModels and filters them into retail only and calls the ESB
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<List<MessageSendResult>> ProcessRetailRecords(List<MessageQueueItemModel> records)
        {
            List<MessageSendResult> response = new List<MessageSendResult>();
            
            // Added UKItemFilter in addition to RetailItems filter
            // UKItemFilter might needed to be removed in future
            List<MessageQueueItemModel> retailItems = records.Where(
                x => (x.Item.ItemTypeCode != ItemPublisherConstants.NonRetailSaleTypeCode && !ukItemFilter.Filter(x.Item))
            ).ToList();

            if (retailItems.Count > 0)
            {
                response.Add(await this.EsbProcess(retailItems, this.systemListBuilder.BuildRetailNonReceivingSystemsList()));
            }

            return response;
        }

        /// <summary>
        /// Takes a list of MessageQueueItemModels and filters them into non-retail only and calls the ESB
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<List<MessageSendResult>> ProcessNonRetailRecords(List<MessageQueueItemModel> records)
        {
            List<MessageSendResult> response = new List<MessageSendResult>();

            // Added UKItemFilter in addition to NonRetailItems filter
            // UKItemFilter might needed to be removed in future
            List<MessageQueueItemModel> nonRetailItems = records.Where(
                x => (x.Item.ItemTypeCode == ItemPublisherConstants.NonRetailSaleTypeCode && !ukItemFilter.Filter(x.Item))
            ).ToList();

            if (nonRetailItems.Count > 0)
            {
                response.Add(await this.EsbProcess(nonRetailItems, this.systemListBuilder.BuildNonRetailReceivingSystemsList()));
            }

            return response;
        }

        /// <summary>
        /// Calls the ESB to process our list of queue records
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private async Task<MessageSendResult> EsbProcess(List<MessageQueueItemModel> records, List<string> nonReceivingSystems)
        {
            return await this.esbService.Process(records, nonReceivingSystems);
        }
    }
}