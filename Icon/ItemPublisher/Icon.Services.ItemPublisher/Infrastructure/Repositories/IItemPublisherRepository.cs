using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Repositories
{
    public interface IItemPublisherRepository
    {
        Task<List<MessageQueueItemModel>> GetMessageItemModels(List<MessageQueueItem> messageQueueItems);

        Task AddMessageQueueHistoryRecords(List<MessageQueueItemArchive> history);

        Task AddDeadLetterMessageQueueRecord(MessageDeadLetterQueue messageDeadLetterQueue);

        Task<List<MessageQueueItem>> DequeueMessageQueueItems(int batchSize = 1);
    }
}