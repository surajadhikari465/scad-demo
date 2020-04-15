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

                UPDATE BulkUpload
                SET StatusId = @processingId
                    OUTPUT 
                        inserted.BulkUploadId,
                        inserted.BulkUploadId,
                        inserted.FileName,
                        inserted.FileModeTypeId AS FileModeType,
                        inserted.FileUploadTime, 
                        inserted.UploadedBy,
                        inserted.StatusId 
                WHERE BulkUploadId IN (
                    SELECT TOP (1) bi.BulkUploadId 
                    FROM BulkUpload bi
                    INNER JOIN BulkUploadStatus bis ON bi.StatusId = bis.Id 
                    WHERE bis.STATUS = 'New'
                    ORDER BY bi.BulkUploadId ASC
                )";
            var results = Connection.Query<BulkItemUploadInformation>(query);
            return results;
        }
    }
}