using System.Data.SqlClient;
using System.Linq;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkItemUploadByIdQueryHandler : IQueryHandler<GetBulkItemUploadByIdParameters, BulkItemUploadStatusModel>
    {
        private readonly IconContext context;


        public GetBulkItemUploadByIdQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public BulkItemUploadStatusModel Search(GetBulkItemUploadByIdParameters parameters)
        {
            var query = @"       
                SELECT b.BulkItemUploadId,
	                b.FileName,
	                b.FileModeType,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message,
                    CASE WHEN IsNull(PercentageProcessed, 0) > 100 THEN 100 ELSE IsNull(PercentageProcessed, 0) END 0 AS PercentageProcessed
                FROM BulkItemUpload b
                INNER JOIN BulkUploadStatus s ON b.StatusId = s.Id
                WHERE b.BulkItemUploadId = @Id";

            var idParam = new SqlParameter("Id", parameters.BulkItemUploadId);

            var data = this.context.Database.SqlQuery<BulkItemUploadStatusModel>(query, idParam).ToList();
            return data.FirstOrDefault();
        }
    }
}