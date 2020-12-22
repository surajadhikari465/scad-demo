using Dapper;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetMessageArchiveEventsQuery : IQueryHandler<GetMessageArchiveEventsQueryParameters, IList<ArchivedMessage>>
    {

        public IList<ArchivedMessage> Search(GetMessageArchiveEventsQueryParameters parameters)
        {
			using (var db = new DBAdapter(parameters.Region))
			{
				// Query Archive Events
				return db.Connection.Query<ArchivedMessage>(@"
							SELECT TOP(@MaxRecords) 
								MessageArchiveEventID AS ArchiveID, 
								EventTypeCode AS [Event], 
								KeyID, 
								SecondaryKeyID, 
								CASE 
									WHEN ErrorCode IS NOT NULL THEN 'Failed' 
									ELSE 'Processed' 
								END AS Status, 
								CONVERT(VARCHAR, InsertDate, 120) AS InsertDate, 
								'' AS Reset_By
							FROM amz.MessageArchiveEvent
							WHERE (@KeyID IS NULL OR KeyID = @KeyID)  
								AND (@SecondaryKeyID IS NULL OR SecondaryKeyID = @SecondaryKeyID) 
								AND (@StartDatetime IS NULL OR InsertDate >= @StartDatetime)
								AND (@EndDatetime IS NULL OR  InsertDate <= @EndDatetime)
								AND EventTypeCode = ISNULL(@EventType, EventTypeCode)
								AND MessageType like (LEFT(@Queue, 5) +'%')
								AND (
										(@Status IS null) OR 
										(@Status = 'Failed' AND ErrorCode <> '') OR 
										(@Status = 'Processed' AND ErrorCode IS NULL)
									)",
							parameters)
					.ToList();			
			}
		}
    }
}
