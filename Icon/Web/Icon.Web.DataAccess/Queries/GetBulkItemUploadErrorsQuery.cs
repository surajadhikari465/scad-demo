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
    public class GetBulkItemUploadErrorsQuery : IQueryHandler<GetBulkItemUploadErrorsPrameters, List<BulkUploadErrorModel>>
    {
        private readonly IconContext context;


        public GetBulkItemUploadErrorsQuery(IconContext context)
        {
            this.context = context;
        }

        public List<BulkUploadErrorModel> Search(GetBulkItemUploadErrorsPrameters parameters)
        {

            SqlParameter bulkItemUploadId = new SqlParameter("@BulkItemUploadId", SqlDbType.Int);
            bulkItemUploadId.Value = parameters.BulkItemUploadId;

            var sql =
                "select * from BulkItemUploadErrors where BulkItemUploadId = @BulkItemUploadId order by RowId asc";
            return context.Database.SqlQuery<BulkUploadErrorModel>(sql,bulkItemUploadId).ToList();
        }
    }
}