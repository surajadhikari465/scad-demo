using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Framework
{
    /// <summary>
    /// Partial class for APIMessageProcessorLogEntry entity object
    /// </summary>
    public partial class APIMessageProcessorLogEntry
    {
        /// <summary>
        /// Custom ToString() implementation for APIMessageProcessorLogEntry class
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return $"ID:{APIMessageMonitorLogID} MessageType:{MessageTypeID} Processed:{CountProcessedMessages} Failed:{CountFailedMessages} (Total:{CountTotalMessages})"
                + $" Start:{StartTime:yyyy-MM-ddTHH:mm:ss.fff} End:{EndTime:yyyy-MM-ddTHH:mm:ss.fff}";
        }

        /// <summary>
        /// Calculated property adding the processed & failed message properties (or default values of 0) together
        /// </summary>
        public int CountTotalMessages
        {
            get
            {
                return this.CountProcessedMessages.GetValueOrDefault(0) + this.CountFailedMessages.GetValueOrDefault(0);
            }
        }
    }
}
