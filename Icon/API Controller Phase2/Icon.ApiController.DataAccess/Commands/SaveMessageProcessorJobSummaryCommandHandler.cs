using Icon.RenewableContext;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Logging;
using System;
using Icon.DbContextFactory;

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
        private IDbContextFactory<IconContext> iconContextFactory;

        /// <summary>
        /// Parametrized constructor
        /// </summary>
        /// <param name="logger">ILogger implementation the command handler will use when logging</param>
        /// <param name="globalContext">The data context the command handler will use to interact with the database</param>
        public SaveMessageProcessorJobSummaryCommandHandler(
            ILogger<SaveMessageProcessorJobSummaryCommandHandler> logger,
            IDbContextFactory<IconContext> iconContextFactory)
        {
            this.logger = logger;
            this.iconContextFactory = iconContextFactory;
        }

        public void Execute(SaveMessageProcessorJobSummaryCommand<APIMessageProcessorLogEntry> command)
        {
            using (var context = iconContextFactory.CreateContext())
            {
                context.APIMessageMonitorLog.Add(command.JobSummary);
                context.SaveChanges();

                logger.Info($"Saved message processor job summary [{command?.JobSummary}] to the {nameof(context.APIMessageMonitorLog)} table.");
            }
        }
    }
}
