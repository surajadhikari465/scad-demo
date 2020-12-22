using Dapper;
using Icon.Common.DataAccess;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WebSupport.DataAccess.Models;

namespace WebSupport.DataAccess.Queries
{
    /// <summary>
    /// GetPurchaseOrdersMessagesToReset Query
    /// </summary>
    public class GetPurchaseOrdersMessagesToResetQuery : IQueryHandler<GetPurchaseOrdersMessagesToResetQueryParameters, IList<PurchaseOrderArchivedMessage>>
    {
        public IList<PurchaseOrderArchivedMessage> Search(GetPurchaseOrdersMessagesToResetQueryParameters parameters)
        {
			using (var db = new DBAdapter(parameters.Region))
			{
				return db.Connection.Query<PurchaseOrderArchivedMessage>(@"
							WITH POMessagesToReset as (
								SELECT MessageArchiveID AS ArchiveID, 
								KeyID AS PurchaseOrderId,
								ROW_NUMBER() OVER (PARTITION BY [KeyID] ORDER BY [InsertDate] DESC) AS rn
							  FROM [ItemCatalog].[amz].[MessageArchive]
							  WHERE [EventType] like 'PO_%'
								and [KeyId] in @PurchaseOrderIdList
							)
							SELECT *
							  FROM POMessagesToReset
							  WHERE rn = 1", parameters)
				.ToList();
			}
		}
    }
}
