using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadStatusQuery : IQueryHandler<GetBulkUploadStatusParameters, List<BulkUploadStatusModel>>
    {
        private readonly IDbConnection connection;

        public GetBulkUploadStatusQuery(IDbConnection connection)
        {
            this.connection = connection;
        }

        public List<BulkUploadStatusModel> Search(GetBulkUploadStatusParameters parameters)
        {
            var query = $@"
                SELECT TOP {parameters.RowCount} Count(*) as ErrorCount, b.BulkUploadId
                into #tmp
                FROM BulkUpload b
                INNER JOIN BulkUploadErrors be ON b.BulkUploadId = be.BulkUploadId
                WHERE b.BulkUploadDataTypeId = @BulkUploadDataTypeId
                GROUP BY  b.BulkUploadId
                ORDER BY BulkUploadId DESC

            SELECT TOP {parameters.RowCount} 
                    b.BulkUploadId,
	                b.FileName,
	                b.FileModeTypeId AS FileModeType,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message,
                    CASE WHEN IsNull(PercentageProcessed, 0) > 100 THEN 100 ELSE IsNull(PercentageProcessed, 0) END AS PercentageProcessed,
                    IsNull(ErrorCount,0) as NumberOfRowsWithError
                FROM BulkUpload b
                INNER JOIN BulkUploadStatus s ON b.StatusId = s.Id
                INNER JOIN BulkUploadDataTypes dt ON b.BulkUploadDataTypeId = dt.BulkUploadDataTypeId
                LEFT JOIN #tmp on b.BulkUploadId = #tmp.BulkUploadId
                WHERE b.BulkUploadDataTypeId = @BulkUploadDataTypeId
                ORDER BY BulkUploadId DESC";

            var data = connection.Query<BulkUploadStatusModel>(query, new { BulkUploadDataTypeId = parameters.BulkUploadDataType }).ToList();
            return data;
        }
    }
}