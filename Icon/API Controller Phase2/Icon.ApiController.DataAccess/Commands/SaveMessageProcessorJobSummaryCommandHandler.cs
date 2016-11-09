using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;

namespace Icon.ApiController.DataAccess.Commands
{
    /// <summary>
    /// Command handler class for executing the SaveMessageProcessorJobSummaryCommand
    ///  to record the information encapsulated in an APIMessageProcessorLogEntry object
    /// </summary>
    public class SaveMessageProcessorJobSummaryCommandHandler 
        : ICommandHandler<SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry>>
    {
        private ILogger<SaveMessageProcessorJobSummaryCommandHandler> logger;
        private IRenewableContext<IconContext> globalContext;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="logger">ILogger implementation the command handler will use when logging</param>
        /// <param name="globalContext">The data context the command handler will use to interact with the database</param>
        public SaveMessageProcessorJobSummaryCommandHandler(
            ILogger<SaveMessageProcessorJobSummaryCommandHandler> logger,
            IRenewableContext<IconContext> globalContext)
        {
            this.logger = logger;
            this.globalContext = globalContext;
        }

        public void Execute(SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry> command)
        {
            globalContext.Context.APIMessageMonitorLog.Add(command.JobSummary);
            globalContext.Context.SaveChanges();
            logger.Info($"Saved message processor job summary [{command?.JobSummary}] to the {nameof(globalContext.Context.APIMessageMonitorLog)} table.");
        }
    }
}
