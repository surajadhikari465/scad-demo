using Icon.Logging;
using Irma.Framework;
using RegionalEventController.DataAccess.Interfaces;
using System;
using System.Linq;
using System.Data.SqlClient;
using System.Data;

namespace RegionalEventController.DataAccess.Commands
{
    public class MarkIconItemChangeQueueEntriesInProcessByCommandHandler : IBulkCommandHandler<MarkIconItemChangeQueueEntriesInProcessByCommand>
    {
        private ILogger<MarkIconItemChangeQueueEntriesInProcessByCommandHandler> logger;
        private IrmaContext context;

        public MarkIconItemChangeQueueEntriesInProcessByCommandHandler(ILogger<MarkIconItemChangeQueueEntriesInProcessByCommandHandler> logger, IrmaContext context)
        {
            this.logger = logger;
            this.context = context;
        }

        public int Execute(MarkIconItemChangeQueueEntriesInProcessByCommand command)
        {
            int returnVal = 0;
            if (command.MaxQueueEntriesToProcess == 0 || String.IsNullOrEmpty(command.Instance))
            {
                logger.Warn("MarkIconItemChangeQueueEntriesInProcessByCommandHandler was called with null parameters.  Check the execution path in the calling method for potential errors.");
                return returnVal;
            }

            SqlParameter numberOfRowsParameter = new SqlParameter("NumberOfRows", SqlDbType.Int);
            numberOfRowsParameter.Value = command.MaxQueueEntriesToProcess;

            SqlParameter jobInstanceParameter = new SqlParameter("JobInstance", SqlDbType.VarChar);
            jobInstanceParameter.Value = command.Instance;

            SqlParameter numberOfRowsMarkedParameter = new SqlParameter("NumberOfRowsMarked", SqlDbType.Int);
            numberOfRowsMarkedParameter.Direction = ParameterDirection.Output;

            string sql = "EXEC dbo.MarkIconItemChangeQueueEntriesInProcess @NumberOfRows, @JobInstance, @NumberOfRowsMarked OUTPUT";

            context.Database.ExecuteSqlCommand(sql, numberOfRowsParameter, jobInstanceParameter, numberOfRowsMarkedParameter);
            int numberOfRowsMarked = int.Parse(numberOfRowsMarkedParameter.Value.ToString());
            return numberOfRowsMarked;
        }
    }
}