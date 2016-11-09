using Icon.Framework;

namespace Icon.ApiController.DataAccess.Commands
{
    /// <summary>
    /// Class representing a command definition used to persist a log entry
    ///  with information about a controller message processing job to data store
    /// </summary>
    /// <typeparam name="TJobSummary">Type of data to be written</typeparam>
    public class SaveMessageProcessorJobSummaryCommand<TJobSummary>
    {
        public TJobSummary JobSummary { get; set; }
    }
}
