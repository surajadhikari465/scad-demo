using System.Data.SqlClient;
using System.Linq;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadByIdQueryHandler : IQueryHandler<GetBulkUploadByIdParameters, BulkUploadStatusModel>
    {
        private readonly IconContext context;


        public GetBulkUploadByIdQueryHandler(IconContext context)
        {
            this.context = context;
        }

        public BulkUploadStatusModel Search(GetBulkUploadByIdParameters parameters)
        {
            var query = @"       
                SELECT b.BulkItemUploadId,
	                b.FileName,
	                b.FileModeType,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message
                FROM BulkItemUpload b
                INNER JOIN BulkItemUploadStatus s ON b.StatusId = s.Id
                WHERE b.BulkItemUploadId = @Id";

            var idParam = new SqlParameter("Id", parameters.BulkItemUploadId);

            var data = this.context.Database.SqlQuery<BulkUploadStatusModel>(query, idParam).ToList();
            return data.FirstOrDefault();
        }
    }
}