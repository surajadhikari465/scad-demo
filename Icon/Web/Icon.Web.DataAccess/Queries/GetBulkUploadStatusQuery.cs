using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadStatusQuery : IQueryHandler<GetBulkUploadStatusParameters, List<BulkUploadStatusModel>>
    {
        private readonly IconContext context;


        public GetBulkUploadStatusQuery(IconContext context)
        {
            this.context = context;
        }

        public List<BulkUploadStatusModel> Search(GetBulkUploadStatusParameters parameters)
        {
            var query = $@"
                SELECT TOP {parameters.RowCount} 
                    b.BulkItemUploadId,
	                b.FileName,
	                b.FileModeType,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message
                FROM BulkItemUpload b
                INNER JOIN BulkItemUploadStatus s ON b.StatusId = s.Id
                ORDER BY BulkItemUploadId DESC";

            var data = this.context.Database.SqlQuery<BulkUploadStatusModel>(query).ToList();
            return data;
        }
    }
}