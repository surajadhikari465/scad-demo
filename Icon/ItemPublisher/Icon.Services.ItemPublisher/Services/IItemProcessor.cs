﻿using Icon.Services.ItemPublisher.Infrastructure.MessageQueue;
using Icon.Services.ItemPublisher.Infrastructure.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Services
{
    public interface IItemProcessor
    {
        Task<List<MessageSendResult>> ProcessNonRetailRecords(List<MessageQueueItemModel> records);

        Task<List<MessageSendResult>> ProcessRetailRecords(List<MessageQueueItemModel> records);

        Task<bool> ReadyForProcessing { get; }
    }
}