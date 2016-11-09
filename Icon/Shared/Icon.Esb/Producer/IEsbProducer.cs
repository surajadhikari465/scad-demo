using System.Collections.Generic;

namespace Icon.Esb.Producer
{
    public interface IEsbProducer : IEsbConnection
    {
        void Send(string message, Dictionary<string, string> messageProperties = null);
    }
}
