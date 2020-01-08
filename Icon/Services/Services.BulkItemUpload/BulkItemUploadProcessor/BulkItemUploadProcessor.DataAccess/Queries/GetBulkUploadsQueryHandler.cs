using System.Collections.Generic;
using System.Data;
using BulkItemUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetBulkUploadsQueryHandler : IQueryHandler<GetBulkUploadsParameters, IEnumerable<BulkItemUploadInformation>>
    {
        private readonly IDbConnection Connection;
        public GetBulkUploadsQueryHandler(IDbConnection connection)
        {
            this.Connection = connection;
        }

        public IEnumerable<BulkItemUploadInformation> Search(GetBulkUploadsParameters parameters)
        {
            var query = @"
                DECLARE @processingId INT = (SELECT TOP 1 Id FROM dbo.BulkUploadStatus WHERE Status = 'Processing')

                UPDATE BulkItemUpload
                SET StatusId = @processingId
                    OUTPUT inserted.*
                WHERE BulkItemUploadId IN (
                    SELECT TOP (1) bi.BulkItemUploadId 
                    FROM BulkItemUpload bi
                    INNER JOIN BulkItemUploadStatus bis ON bi.StatusId = bis.Id 
                    WHERE bis.STATUS = 'New'
                    ORDER BY bi.BulkItemUploadId ASC
                )";
            var results = Connection.Query<BulkItemUploadInformation>(query);
            return results;
        }
    }
}