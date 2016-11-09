using Icon.Framework;
using System.Collections.Generic;

namespace Icon.ApiController.Controller.QueueReaders
{
    public interface IQueueReader<TMessageQueue, TMiniBulk>
    {
        List<TMessageQueue> GetQueuedMessages();
        List<TMessageQueue> GroupMessagesForMiniBulk(List<TMessageQueue> messagesReadyForMiniBulk);
        TMiniBulk BuildMiniBulk(List<TMessageQueue> messages);
    }
}
