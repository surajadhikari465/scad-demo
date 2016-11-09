using Icon.Framework;
using System.Collections.Generic;

namespace Icon.Ewic.Transmission.Producers
{
    public interface IMessageProducer
    {
        void SendMessages(List<MessageHistory> messages);
    }
}
