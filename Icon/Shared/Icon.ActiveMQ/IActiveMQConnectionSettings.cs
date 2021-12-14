using System;
using Apache.NMS;

namespace Icon.ActiveMQ
{
    public interface IActiveMQConnectionSettings
    {
        string ServerUrl { get; set; }
        string JmsUsername { get; set; }
        string JmsPassword { get; set; }
        string QueueName { get; set; }
        int ReconnectDelay { get; set; }
        AcknowledgementMode SessionMode { get; set; }
    }
}
