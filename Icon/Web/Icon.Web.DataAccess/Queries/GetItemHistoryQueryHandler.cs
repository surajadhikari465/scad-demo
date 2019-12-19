using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemHistoryQueryHandler : IQueryHandler<GetItemHistoryParameters, IEnumerable<ItemHistoryDbModel>>
    {
        private IDbConnection dbConnection;

        public GetItemHistoryQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<ItemHistoryDbModel> Search(GetItemHistoryParameters parameters)
        {
            string sql = @"SELECT ItemId,
	            i.ItemTypeId,
	            it.itemTypeCode as ItemTypeCode,
                i.SysStartTimeUtc,
                i.SysEndTimeUtc,
	            i.ItemAttributesJson
            FROM dbo.Item FOR SYSTEM_TIME ALL i
			JOIN dbo.ItemType it on it.itemTypeID = i.itemTypeId 
            WHERE ItemId = @ItemId
            ORDER BY SysStartTimeUtc ASC";

            return this.dbConnection.Query<ItemHistoryDbModel>(sql, parameters);

        }
    }
}
