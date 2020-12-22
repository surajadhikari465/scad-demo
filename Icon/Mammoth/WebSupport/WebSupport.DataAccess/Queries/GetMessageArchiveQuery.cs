using Dapper;
using Icon.Common.DataAccess;
using System.Collections.Generic;
using System.Linq;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    public class GetMessageArchiveQuery : IQueryHandler<GetMessageArchiveQueryParameters, IList<ArchivedMessage>>
    {

        public IList<ArchivedMessage> Search(GetMessageArchiveQueryParameters parameters)
        {
			using (var db = new DBAdapter(parameters.Region))
			{
				// Query Archive Events
				return db.Connection.Query<ArchivedMessage>(@"
							SELECT TOP(@MaxRecords) 
									MessageArchiveID AS ArchiveID, 
									BusinessUnitID AS StoreBU, 
									EventType AS [Event], 
									KeyID, 
									SecondaryKeyID,
									CASE Status 
										WHEN 'F' THEN 'Failed' 
										WHEN 'P' THEN 'Processed' 
										ELSE 'Unprocessed' 
									END AS Status, 
									CONVERT(VARCHAR, InsertDate, 120) AS InsertDate, 
									ResetBy AS ResetBy
							FROM amz.MessageArchive
							WHERE (@KeyID IS NULL OR KeyID = @KeyID) 
								AND (BusinessUnitID IN @StoresBU) 
								AND EventType = ISNULL(@EventType, EventType)
								AND EventType LIKE (CASE @MessageType
														WHEN 'Inventory' THEN 'INV_%'
														WHEN 'PurchaseOrder' THEN 'PO_%'
														WHEN 'Receipt' THEN 'RCPT_%'
														WHEN 'TransferOrder' THEN 'TSF_%'
														ELSE EventType
													END)
								AND (@StartDatetime IS NULL OR InsertDate >= @StartDatetime)
								AND (@EndDatetime IS NULL OR InsertDate <= @EndDatetime)
								AND Status = IsNull(@Status, Status)",
							parameters)
					.ToList();			
			}
		}
    }
}
