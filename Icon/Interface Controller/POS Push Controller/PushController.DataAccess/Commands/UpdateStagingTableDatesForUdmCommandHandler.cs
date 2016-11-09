using Icon.RenewableContext;
using Icon.Framework;
using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Commands
{
    public class UpdateStagingTableDatesForUdmCommandHandler : ICommandHandler<UpdateStagingTableDatesForUdmCommand>
    {
        private ILogger<UpdateStagingTableDatesForUdmCommandHandler> logger;
        private IRenewableContext<IconContext> context;

        public UpdateStagingTableDatesForUdmCommandHandler(ILogger<UpdateStagingTableDatesForUdmCommandHandler> logger, IRenewableContext<IconContext> context)
        {
            this.logger = logger;
            this.context = context;
        }

        public void Execute(UpdateStagingTableDatesForUdmCommand command)
        {
            if (command.StagedPosData == null || command.StagedPosData.Count == 0)
            {
                logger.Warn("UpdateStagingTableDatesForUdmCommandHandler was called with an empty or null list.  Check the execution path in the calling method for potential errors.");
                return;
            }

            SqlParameter statusParameter = new SqlParameter("ProcessedSuccessfully", SqlDbType.Bit);
            statusParameter.Value = command.ProcessedSuccessfully;
            
            SqlParameter recordsParameter = new SqlParameter("RecordsToUpdate", SqlDbType.Structured);
            recordsParameter.TypeName = "app.IrmaPushIdType";

            SqlParameter dateParameter = new SqlParameter("Date", SqlDbType.DateTime2);
            dateParameter.Value = command.Date;

            recordsParameter.Value = command.StagedPosData.ConvertAll(p => new
            {
                IrmaPushId = p.IRMAPushID
            }).ToDataTable();

            string sql = "EXEC app.UpdateStagingTableDatesForUdm @ProcessedSuccessfully, @RecordsToUpdate, @Date";

            context.Context.Database.ExecuteSqlCommand(sql, statusParameter, recordsParameter, dateParameter);

            logger.Info(String.Format("Successfully updated {0} IRMAPush record(s) with a {1} of {2} beginning with IRMAPushID {3}.",
                command.StagedPosData.Count, command.ProcessedSuccessfully ? "InUdmDate" : "UdmFailedDate", command.Date, command.StagedPosData[0].IRMAPushID));
        }
    }
}
