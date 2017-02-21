using Dapper;
using Icon.Common.DataAccess;
using System;

namespace Icon.Monitoring.DataAccess.Queries
{
   public class GetItemMovementTableRowCount : IQueryHandler<GetItemMovementTableRowCountParameters, string>
    {
        private IDbProvider db;

        public GetItemMovementTableRowCount(IDbProvider db)
        {
            this.db = db;
        }
        public string Search(GetItemMovementTableRowCountParameters parameters)
        {     // get total rows in app movement
             string sql = @"SELECT count(*) FROM app.ItemMovement WITH (NOLOCK)";
            string result = (string)this.db.Connection.ExecuteScalar(sql);
            return result;
        }
    }
}
