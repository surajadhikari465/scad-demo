using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WebSupport.DataAccess.Commands
{
    public class ReQueueMessageArchiveCommandHandler : ICommandHandler<ReQueueMessageArchiveCommandParameters>
    {
        public void Execute(ReQueueMessageArchiveCommandParameters parameters)
        {
			using (var db = new DBAdapter(parameters.Region))
			{
				// Update the messages
				parameters.RowsAffected = db.Connection.Execute(@" 
							UPDATE amz.MessageArchive 
							SET ProcessTimes = 0, 
								Status = 'U', 
								LastReprocessID = NULL, 
								LastReprocess = NULL, 
								ResetBy = @UserName
							WHERE MessageArchiveID in @MessageArchiveIDs",
							parameters);
			}
		}
    }
}
