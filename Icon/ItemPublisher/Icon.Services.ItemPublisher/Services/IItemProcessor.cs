using Icon.Services.ItemPublisher.Infrastructure.Esb;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services
{
    public interface IItemProcessor
    {
        Task<List<EsbSendResult>> ProcessNonRetailRecords(List<MessageQueueItemModel> records);

        Task<List<EsbSendResult>> ProcessRetailRecords(List<MessageQueueItemModel> records);

        Task<bool> ReadyForProcessing { get; }
    }
}