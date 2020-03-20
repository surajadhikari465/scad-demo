using System.Collections.Generic;
using System.Linq;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Dapper;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadStatusQuery : IQueryHandler<GetBulkContactUploadStatusParameters, List<BulkContactUploadStatusModel>>
	{
		private readonly IDbConnection connection;


		public GetBulkContactUploadStatusQuery(IDbConnection connection)
		{
			this.connection = connection;
		}

		public List<BulkContactUploadStatusModel> Search(GetBulkContactUploadStatusParameters parameters)
		{
			var query = $@"
				SELECT TOP {parameters.RowCount} 
					BulkContactUploadId,
					FileName,
					FileUploadTime,
					UploadedBy
				FROM BulkContactUpload
				ORDER BY BulkContactUploadId DESC";

			return this.connection.Query<BulkContactUploadStatusModel>(query).ToList();
		}
	}
}