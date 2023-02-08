using System.Collections.Generic;

namespace WebSupport.Clients
{
    public interface IMammothGpmBridgeClient
    {
        void SendToJustInTimeConsumers(string message, Dictionary<string, string> messageProperties, string irmaRegion, string system);
        void SendToGpmProcessBod(string message, string messageId, Dictionary<string, string> messageProperties);
    }
}
