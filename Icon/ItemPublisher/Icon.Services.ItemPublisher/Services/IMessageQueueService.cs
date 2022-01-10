using Icon.Services.ItemPublisher.Infrastructure.MessageQueue;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services
{
    public interface IMessageQueueService
    {
        Task<MessageSendResult> Process(List<MessageQueueItemModel> records, List<string> nonReceivingSystems);

        Task<bool> ReadyForProcessing { get; }
    }
}