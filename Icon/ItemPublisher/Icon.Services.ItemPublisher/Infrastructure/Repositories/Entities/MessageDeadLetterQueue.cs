using System;
using System.Collections.Generic;

namespace Icon.Services.ItemPublisher.Repositories.Entities
{
    public class MessageDeadLetterQueue
    {
        public MessageDeadLetterQueue()
        {

        }

        public MessageDeadLetterQueue(string exception, List<int> itemIds)
        {
            this.Exception = exception;
            this.ItemIds = itemIds;
            this.MessageType = "GlobalItem";
            this.DateCreated = DateTime.UtcNow;
        }

        public string Exception { get; set; }
        public List<int> ItemIds { get; set; } = new List<int>();
        public string MessageType { get; set; }
        public DateTime DateCreated { get; set; }
    }
}