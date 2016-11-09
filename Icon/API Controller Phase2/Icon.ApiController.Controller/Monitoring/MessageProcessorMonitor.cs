using Icon.ApiController.Common;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.ApiController.Controller.Monitoring
{
    /// <summary>
    /// Class for performing job monitoring: recording the results of each 
    /// controller job (mini-batch process) in the Job Monitoring data store
    /// </summary>
    public class MessageProcessorMonitor : IMessageProcessorMonitor
    {
        private ICommandHandler<SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>> writeLogEntryCommandHandler;

        public MessageProcessorMonitor(
            ICommandHandler<SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>> writeLogEntryCommandHandler)
        {
            this.writeLogEntryCommandHandler = writeLogEntryCommandHandler;
        }


        /// <summary>
        /// Writes a log entry to the APIMessageMonitorLog table in the database,
        ///  representing a completed API Controller message processing job
        /// </summary>
        /// <param name="logEntryData">object representing the job data to be written to the log</param>
        /// <returns>Positive value when successful (ID of inserted record)</returns>
        public int RecordResults(APIMessageProcessorLogEntry logEntryData)
        {
            var command = new SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>
            {
                JobSummary = logEntryData
            };
            writeLogEntryCommandHandler.Execute(command);
            //return inserted id or value representing error
            return logEntryData?.APIMessageMonitorLogID ?? -1;
        }

        /// <summary>
        /// Writes a log entry to the APIMessageMonitorLog table in the database,
        ///  representing a completed API Controller message processing job
        /// </summary>
        /// <param name="messageTypeID">type of message being processed (ItemLocale/Price/Product etc.)</param>
        /// <param name="start">when the job began</param>
        /// <param name="end">when the job ended</param>
        /// <param name="successfulMessageCount">count of messages successfully processed during the job</param>
        /// <param name="failedMessageCount">count of messages which encountered errors during job processing</param>
        /// <returns>Positive value when successful (ID of inserted record)</returns>
        public int RecordResults(int messageTypeID, DateTime start, DateTime end, int successfulMessageCount, int failedMessageCount)
        {
            var logEntryData = new APIMessageProcessorLogEntry()
            {
                MessageTypeID = messageTypeID,
                StartTime = start,
                EndTime = end,
                CountProcessedMessages = successfulMessageCount,
                CountFailedMessages = failedMessageCount
            };
            return RecordResults(logEntryData);
        }
    }
}
