using System.Collections.Generic;
using System.Data;
using BrandUploadProcessor.Common.Models;
using Dapper;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.DataAccess.Queries
{
    public class GetBrandUploadsQueryHandler : IQueryHandler<EmptyQueryParameters<IEnumerable<BrandUploadInformation>>, IEnumerable<BrandUploadInformation>>
    {
        private readonly IDbConnection connection;
        public GetBrandUploadsQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public IEnumerable<BrandUploadInformation> Search(EmptyQueryParameters<IEnumerable<BrandUploadInformation>> parameter)
        {
            var query = @"
                DECLARE @processingId INT = (SELECT TOP 1 Id FROM dbo.BulkUploadStatus WHERE Status = 'Processing')

                UPDATE BulkUpload
                SET StatusId = @processingId
                    OUTPUT inserted.*
                WHERE BulkUploadId IN (
                    SELECT TOP (1) bi.BulkUploadId 
                    FROM BulkUpload bi
                    INNER JOIN BulkUploadStatus bis ON bi.StatusId = bis.Id 
                    INNER JOIN BulkUploadDataTypes budt on bi.BulkUploadDataTypeId = budt.BulkUploadDataTypeId
                    WHERE bis.STATUS = 'New'
                    AND budt.datatype = 'Brand'
                    ORDER BY bi.BulkUploadId ASC
                )";
            var results = connection.Query<BrandUploadInformation>(query);
            return results;
        }
    }
}
