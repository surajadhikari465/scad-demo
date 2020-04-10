using Icon.Services.ItemPublisher.Infrastructure.Esb;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services
{
    public interface IEsbService
    {
        Task<EsbSendResult> Process(List<MessageQueueItemModel> records, List<string> nonReceivingSystems);

        Task<bool> ReadyForProcessing { get; }
    }
}