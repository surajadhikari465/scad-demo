using Icon.Logging;
using MoreLinq;
using PushController.DataAccess.Interfaces;
using System;
using System.Data;
using System.Data.SqlClient;

namespace PushController.DataAccess.Commands
{
    public class UpdatePublishTableDatesCommandHandler : ICommandHandler<UpdatePublishTableDatesCommand>
    {
        private ILogger<UpdatePublishTableDatesCommandHandler> logger;
        
        public UpdatePublishTableDatesCommandHandler(ILogger<UpdatePublishTableDatesCommandHandler> logger)
        {
            this.logger = logger;
        }

        public void Execute(UpdatePublishTableDatesCommand command)
        {
            if (command.PublishedPosData == null || command.PublishedPosData.Count == 0)
            {
                logger.Warn("UpdatePublishTableDatesCommandHandler was called with an empty or null list.  Check the execution path in the calling method for potential errors.");
                return;
            }

            SqlParameter statusParameter = new SqlParameter("ProcessedSuccessfully", SqlDbType.Bit);
            statusParameter.Value = command.ProcessedSuccessfully;
            
            SqlParameter recordsParameter = new SqlParameter("RecordsToUpdate", SqlDbType.Structured);
            recordsParameter.TypeName = "dbo.IconPosPublishEventIdType";

            SqlParameter dateParameter = new SqlParameter("Date", SqlDbType.DateTime2);
            dateParameter.Value = command.Date;

            recordsParameter.Value = command.PublishedPosData.ConvertAll(p => new
            {
                IconPosPushPublishId = p.IConPOSPushPublishID
            }).ToDataTable();

            string sql = "EXEC dbo.UpdatePublishTableDates @ProcessedSuccessfully, @RecordsToUpdate, @Date";

            command.Context.Database.ExecuteSqlCommand(sql, statusParameter, recordsParameter, dateParameter);

            logger.Info(String.Format("Successfully updated {0} IConPOSPushPublish record(s) with a {1} of {2} beginning with IConPOSPushPublishID {3}.",
                command.PublishedPosData.Count, command.ProcessedSuccessfully ? "ProcessedDate" : "ProcessingFailedDate", command.Date, command.PublishedPosData[0].IConPOSPushPublishID));
        }
    }
}
