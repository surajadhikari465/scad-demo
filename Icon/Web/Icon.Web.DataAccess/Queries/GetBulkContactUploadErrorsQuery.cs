using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using Dapper;
using Icon.Common.DataAccess;
using Icon.Framework;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
	public class GetBulkContactUploadErrorsQuery : IQueryHandler<GetBulkContactUploadErrorsPrameters, List<BulkUploadErrorModel>>
	{
		private readonly IconContext context;


		public GetBulkContactUploadErrorsQuery(IconContext context)
		{
			this.context = context;
		}

		public List<BulkUploadErrorModel> Search(GetBulkContactUploadErrorsPrameters parameters)
		{

			SqlParameter bulkContactUploadId = new SqlParameter("@BulkContactUploadId", SqlDbType.Int);
			bulkContactUploadId.Value = parameters.BulkContactUploadId;

			var sql =
				"select * from BulkContactUploadErrors where BulkContactUploadId = @BulkContactUploadId order by BulkContactUploadErrorId asc";
			return context.Database.SqlQuery<BulkUploadErrorModel>(sql, bulkContactUploadId).ToList();
		}
	}
}