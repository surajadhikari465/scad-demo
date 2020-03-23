using System.Linq;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using Dapper;
using System.Data;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadByIdQueryHandler : IQueryHandler<GetBulkContactUploadByIdParameters, BulkContactUploadStatusModel>
	{
		private readonly IDbConnection connection;

		public GetBulkContactUploadByIdQueryHandler(IDbConnection connection)
		{
			this.connection = connection;
		}

		public BulkContactUploadStatusModel Search(GetBulkContactUploadByIdParameters parameters)
		{
			var query = $@"SELECT
					BulkContactUploadId,
					FileName,
					FileContent
				FROM BulkContactUpload
				WHERE BulkContactUploadId = {parameters.BulkContactUploadId}";

			return this.connection.Query<BulkContactUploadStatusModel>(query).FirstOrDefault();
		}
	}
}
