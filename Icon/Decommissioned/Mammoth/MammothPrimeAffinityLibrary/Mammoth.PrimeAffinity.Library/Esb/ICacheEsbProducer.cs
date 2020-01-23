using System;
using System.Collections.Generic;

namespace Mammoth.PrimeAffinity.Library.Esb
{
    public interface ICacheEsbProducer : IDisposable
    {
        void Send(string message, Dictionary<string, string> messageProperties = null);
        void Send(string message, string messageId, Dictionary<string, string> messageProperties = null);
    }
}