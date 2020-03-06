using System.Data.SqlClient;
using System.Linq;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadByIdQueryHandler : IQueryHandler<GetBulkContactUploadByIdParameters, BulkItemUploadStatusModel>
	{
		private readonly IconContext context;


		public GetBulkContactUploadByIdQueryHandler(IconContext context)
		{
			this.context = context;
		}

		public BulkItemUploadStatusModel Search(GetBulkContactUploadByIdParameters parameters)
		{
			var query = @"       
                SELECT b.BulkContactUploadId,
	                b.FileName,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message,
                    CASE WHEN IsNull(PercentageProcessed, 0) > 100 THEN 100 ELSE IsNull(PercentageProcessed, 0) END 0 AS PercentageProcessed
                FROM BulkContactUpload b
                INNER JOIN BulkUploadStatus s ON b.StatusId = s.Id
                WHERE b.BulkContactUploadId = @Id";

			var idParam = new SqlParameter("Id", parameters.BulkContactUploadId);

			var data = this.context.Database.SqlQuery<BulkItemUploadStatusModel>(query, idParam).ToList();
			return data.FirstOrDefault();
		}
	}
}
