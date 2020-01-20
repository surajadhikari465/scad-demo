using System.Collections.Generic;

namespace Icon.Esb.Producer
{
    public interface IEsbProducer : IEsbConnection
    {
        void Send(string message, Dictionary<string, string> messageProperties = null);
        void Send(string message, string messageId, Dictionary<string, string> messageProperties = null);
        void SetTibcoClientId(string clientId);
    }
}
