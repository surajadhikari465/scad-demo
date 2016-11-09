using Icon.Framework;
using System;

namespace Icon.ApiController.Controller.Monitoring
{
    public interface IMessageProcessorMonitor
    {
        int RecordResults(int messageTypeID, DateTime start, DateTime end, int successfulMessageCount, int failedMessageCount);
        int RecordResults(APIMessageProcessorLogEntry dataToLog);
    }
}