using Icon.Services.ItemPublisher.Infrastructure.Models;
using Icon.Services.ItemPublisher.Repositories.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.Repositories
{
    public interface IItemPublisherRepository : ITransactionRepository
    {
        Task<List<MessageQueueItemModel>> DequeueMessageQueueRecords(int batchSize = 1);

        Task AddMessageQueueHistoryRecords(List<MessageQueueItemArchive> history);

        Task AddDeadLetterMessageQueueRecord(MessageDeadLetterQueue messageDeadLetterQueue);
    }
}