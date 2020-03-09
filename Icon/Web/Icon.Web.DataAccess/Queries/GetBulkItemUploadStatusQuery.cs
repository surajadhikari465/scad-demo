using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkItemUploadStatusQuery : IQueryHandler<GetBulkItemUploadStatusParameters, List<BulkItemUploadStatusModel>>
    {
        private readonly IconContext context;


        public GetBulkItemUploadStatusQuery(IconContext context)
        {
            this.context = context;
        }

        public List<BulkItemUploadStatusModel> Search(GetBulkItemUploadStatusParameters parameters)
        {
            var query = $@"
                SELECT TOP {parameters.RowCount} Count(*) as ErrorCount, b.BulkItemUploadId
                into #tmp
                FROM BulkItemUpload b
                INNER JOIN BulkItemUploadErrors be
                ON b.BulkItemUploadId = be.BulkItemUploadId
                GROUP BY  b.BulkItemUploadId
                ORDER BY BulkItemUploadId DESC

            SELECT TOP {parameters.RowCount} 
                    b.BulkItemUploadId,
	                b.FileName,
	                b.FileModeType,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message,
                    CASE WHEN IsNull(PercentageProcessed, 0) > 100 THEN 100 ELSE IsNull(PercentageProcessed, 0) END AS PercentageProcessed,
                    IsNull(ErrorCount,0) as NumberOfRowsWithError
                FROM BulkItemUpload b
                INNER JOIN BulkUploadStatus s ON b.StatusId = s.Id
                LEFT JOIN #tmp on b.BulkItemUploadId = #tmp.BulkItemUploadId
                ORDER BY BulkItemUploadId DESC";

            var data = this.context.Database.SqlQuery<BulkItemUploadStatusModel>(query).ToList();
            return data;
        }
    }
}