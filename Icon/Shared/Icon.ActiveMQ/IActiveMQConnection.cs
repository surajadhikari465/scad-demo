using System;

namespace Icon.ActiveMQ
{
    public interface IActiveMQConnection: IDisposable
    {
        string ClientId { get; set; }
        ActiveMQConnectionSettings Settings { get; }
        bool IsConnected { get; }
        void OpenConnection(string clientId);
    }
}
