using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadErrorsQuery : IQueryHandler<GetBulkUploadErrorsPrameters, List<BulkUploadErrorModel>>
    {
        private readonly IDbConnection connection;

        public GetBulkUploadErrorsQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<BulkUploadErrorModel> Search(GetBulkUploadErrorsPrameters parameters)
        {
            var sql = "SELECT * FROM BulkUploadErrors WHERE BulkUploadId = @BulkUploadId ORDER BY RowId ASC";
            return connection.Query<BulkUploadErrorModel>(sql, parameters).ToList();
        }
    }
}