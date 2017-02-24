using Dapper;
using Icon.Common.DataAccess;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
   public class GetItemMovementTableRowCount : IQueryHandler<GetItemMovementTableRowCountParameters, int>
    {
        private IDbProvider db;

        public GetItemMovementTableRowCount(IDbProvider db)
        {
            this.db = db;
        }
        public int Search(GetItemMovementTableRowCountParameters parameters)
        {     // get total rows in app movement
             string sql = @"SELECT count(*) FROM app.ItemMovement WITH (NOLOCK) WHERE ProcessFailedDate IS NULL";
            int result = (int)this.db.Connection.ExecuteScalar(sql);
            return result;
        }
    }
}
