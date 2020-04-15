using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadByIdQueryHandler : IQueryHandler<GetBulkUploadByIdParameters, BulkUploadStatusModel>
    {
        private readonly IDbConnection connection;


        public GetBulkUploadByIdQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public BulkUploadStatusModel Search(GetBulkUploadByIdParameters parameters)
        {
            var query = @"       
                SELECT b.BulkUploadId,
	                b.FileName,
	                b.FileModeTypeId AS FileModeType,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message,
                    CASE WHEN IsNull(PercentageProcessed, 0) > 100 THEN 100 ELSE IsNull(PercentageProcessed, 0) END AS PercentageProcessed
                FROM BulkUpload b
                INNER JOIN BulkUploadStatus s ON b.StatusId = s.Id
                WHERE b.BulkUploadId = @BulkUploadId
                    AND b.BulkUploadDataTypeId = @BulkUploadDataTypeId";

            return connection.QueryFirstOrDefault<BulkUploadStatusModel>(
                query,
                new
                {
                    parameters.BulkUploadId,
                    BulkUploadDataTypeId = parameters.BulkUploadDataType
                });
        }
    }
}