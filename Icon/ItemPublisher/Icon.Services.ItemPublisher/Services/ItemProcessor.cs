using Icon.Logging;
using Icon.Services.ItemPublisher.Infrastructure;
using Icon.Services.ItemPublisher.Infrastructure.Esb;
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
        private readonly IEsbService esbService;
        private readonly ILogger<ItemProcessor> logger;
        private readonly ServiceSettings serviceSettings;
        private readonly ISystemListBuilder systemListBuilder;

        public ItemProcessor(
            IEsbService esbService,
            ILogger<ItemProcessor> logger,
            ServiceSettings serviceSettings,
            ISystemListBuilder systemListBuilder)
        {
            this.esbService = esbService;
            this.logger = logger;
            this.serviceSettings = serviceSettings;
            this.systemListBuilder = systemListBuilder;
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
        public async Task<List<EsbSendResult>> ProcessRetailRecords(List<MessageQueueItemModel> records)
        {
            List<EsbSendResult> response = new List<EsbSendResult>();
            List<MessageQueueItemModel> retailItems = records.Where(x => x.Item.ItemTypeCode != ItemPublisherConstants.NonRetailSaleTypeCode
            && (!x.Item.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.DepartmentSale) ||
                x.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] == "No")
            ).ToList();

            if (retailItems.Count > 0)
            {
                response.Add(await this.EsbProcess(retailItems, this.systemListBuilder.BuildRetailNonReceivingSystemsList(), false));
            }

            return response;
        }

        /// <summary>
        /// Takes a list of MessageQueueItemModels and filters them into non-retail only and calls the ESB
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<List<EsbSendResult>> ProcessNonRetailRecords(List<MessageQueueItemModel> records)
        {
            List<EsbSendResult> response = new List<EsbSendResult>();
            List<MessageQueueItemModel> nonRetailItems = records.Where(x => x.Item.ItemTypeCode == ItemPublisherConstants.NonRetailSaleTypeCode
              && (!x.Item.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.DepartmentSale) ||
                x.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] == "No")
            ).ToList();

            if (nonRetailItems.Count > 0)
            {
                response.Add(await this.EsbProcess(nonRetailItems, this.systemListBuilder.BuildNonRetailReceivingSystemsList(), false));
            }

            return response;
        }

        /// <summary>
        /// Takes a list of MessageQueueItemModels and filters them into department sale only and calls the ESB
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        public async Task<List<EsbSendResult>> ProcessDepartmentSaleRecords(List<MessageQueueItemModel> records)
        {
            List<EsbSendResult> response = new List<EsbSendResult>();
            List<MessageQueueItemModel> departmentSaleItems = records.Where(x =>
                x.Item.ItemAttributes.ContainsKey(ItemPublisherConstants.Attributes.DepartmentSale) &&
                x.Item.ItemAttributes[ItemPublisherConstants.Attributes.DepartmentSale] == "Yes").ToList();

            if (departmentSaleItems.Count > 0)
            {
                response.Add(await this.EsbProcess(departmentSaleItems, this.systemListBuilder.BuildDepartmentSaleNonReceivingSystemsList(), true));
            }

            return response;
        }

        /// <summary>
        /// Calls the ESB to process our list of queue records
        /// </summary>
        /// <param name="records"></param>
        /// <returns></returns>
        private async Task<EsbSendResult> EsbProcess(List<MessageQueueItemModel> records, List<string> nonReceivingSystems, bool isDepartmentSale)
        {
            return await this.esbService.Process(records, nonReceivingSystems, isDepartmentSale);
        }
    }
}