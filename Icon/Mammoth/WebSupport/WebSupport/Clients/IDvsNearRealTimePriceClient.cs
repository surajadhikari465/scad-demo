using System.Collections.Generic;

namespace WebSupport.Clients
{
    public interface IDvsNearRealTimePriceClient
    {
        void Send(string message, string messageId, Dictionary<string, string> messageProperties);
    }
}
