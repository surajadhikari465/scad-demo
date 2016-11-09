using Dapper;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Common.DataAccess.DbProviders;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.DataAccess.Queries
{
    public class UpdateAndGetEventQueueInProcessQueryHandler : IQueryHandler<UpdateAndGetEventQueueInProcessQuery, List<EventQueueModel>>
    {
        private IDbProvider dbProvider;

        public UpdateAndGetEventQueueInProcessQueryHandler(IDbProvider dbProvider)
        {
            this.dbProvider = dbProvider;
        }

        public List<EventQueueModel> Search(UpdateAndGetEventQueueInProcessQuery data)
        {
            string sql = @"UPDATE [mammoth].[ItemLocaleChangeQueue]
	                            SET InProcessBy = NULL
	                            WHERE InProcessBy = @JobInstance

                            ;WITH Publish
                            AS
                            (
	                            SELECT TOP(@NumberOfRows) 
		                            QueueID,
		                            Item_Key AS ItemKey,
		                            Store_No AS StoreNo,
		                            Identifier,
		                            EventTypeID,
		                            EventReferenceID,
		                            InsertDate,
		                            ProcessFailedDate,
		                            InProcessBy
	                            FROM
		                            [mammoth].[ItemLocaleChangeQueue] WITH (ROWLOCK, READPAST, UPDLOCK)
	                            WHERE
		                            InProcessBy IS NULL
		                            AND ProcessFailedDate IS NULL
	                            ORDER BY
		                            InsertDate
                            )

                            UPDATE Publish SET InProcessBy = @JobInstance OUTPUT inserted.*";

            List<EventQueueModel> eventQueueModels = this.dbProvider.Connection
                .Query<EventQueueModel>(
                    sql,
                    new
                    {
                        JobInstance = data.Instance,
                        NumberOfRows = data.MaxNumberOfRowsToMark
                    },
                    transaction: dbProvider.Transaction)
                .ToList();

            return eventQueueModels;
        }
    }
}
