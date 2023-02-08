using System.Collections.Generic;
using WebSupport.Models;

namespace WebSupport.Clients
{
    public interface IJobSchedulerBridgeClient
    {
        void Send(JobScheduleModel request, string message, string messageId, Dictionary<string, string> messageProperties);
    }
}
