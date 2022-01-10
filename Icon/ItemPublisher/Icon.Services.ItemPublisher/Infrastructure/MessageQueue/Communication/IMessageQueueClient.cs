using System.Collections.Generic;
using System.Threading.Tasks;

namespace Icon.Services.ItemPublisher.Infrastructure.MessageQueue
{
    public interface IMessageQueueClient
    {
        Task<MessageSendResult> SendMessage(string message, List<string> nonReceivingSystems);
    }
}