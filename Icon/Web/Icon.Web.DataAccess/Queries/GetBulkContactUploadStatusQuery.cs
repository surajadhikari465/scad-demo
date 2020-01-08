using System.Collections.Generic;
using System.Linq;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadStatusQuery : IQueryHandler<GetBulkContactUploadStatusParameters, List<BulkContactUploadStatusModel>>
	{
		private readonly IconContext context;


		public GetBulkContactUploadStatusQuery(IconContext context)
		{
			this.context = context;
		}

		public List<BulkContactUploadStatusModel> Search(GetBulkContactUploadStatusParameters parameters)
		{
			var query = $@"
                SELECT TOP {parameters.RowCount} 
                    b.BulkContactUploadId,
	                b.FileName,
	                b.FileUploadTime,
	                b.UploadedBy,
	                s.STATUS,
                    b.Message
                FROM BulkContactUpload b
                INNER JOIN BulkUploadStatus s ON b.StatusId = s.Id
                ORDER BY BulkContactUploadId DESC";

			var data = this.context.Database.SqlQuery<BulkContactUploadStatusModel>(query).ToList();
			return data;
		}
	}
}