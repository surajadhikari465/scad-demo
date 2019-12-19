using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
    public class GetItemInforHistoryQueryHandler : IQueryHandler<GetItemInforHistoryParameters, IEnumerable<ItemInforHistoryDbModel>>
    {
        private IDbConnection dbConnection;

        public GetItemInforHistoryQueryHandler(IDbConnection dbConnection)
        {
            this.dbConnection = dbConnection;
        }

        public IEnumerable<ItemInforHistoryDbModel> Search(GetItemInforHistoryParameters parameters)
        {
            string sql = @"SELECT ItemId,
	            JsonObject
            from 
            infor.ItemHistory
            WHERE ItemId = @ItemId";

            return this.dbConnection.Query<ItemInforHistoryDbModel>(sql, parameters);
        }
    }
}
