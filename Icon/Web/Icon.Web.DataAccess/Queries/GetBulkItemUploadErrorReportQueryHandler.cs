using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkItemUploadErrorReportQueryHandler : IQueryHandler<GetBulkItemUploadErrorReportParameters, BulkItemUploadErrorExportModel>
    {       
        private readonly IDbConnection db;

        public GetBulkItemUploadErrorReportQueryHandler(IDbConnection context)
        {
            this.db = context;
        }
        public BulkItemUploadErrorExportModel Search(GetBulkItemUploadErrorReportParameters parameters)
        {
            BulkItemUploadErrorExportModel bulkExp = new BulkItemUploadErrorExportModel();

            var resultSet = db.QueryMultiple("Exec app.GetBulkItemUploadData @bulkItemUploadId", new { bulkItemUploadId = parameters.BulkItemUploadId });
            bulkExp.bulkItemUploadModel = resultSet.Read<BulkItemUploadModel>().FirstOrDefault();
            bulkExp.bulkUploadErrorModels = resultSet.Read<BulkUploadErrorModel>().ToList();
            return bulkExp;
        }
    }
}