using Dapper;
using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;
using System.Data;
using System.Linq;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadErrorReportQueryHandler : IQueryHandler<GetBulkUploadErrorReportParameters, BulkUploadErrorExportModel>
    {       
        private readonly IDbConnection connection;

        public GetBulkUploadErrorReportQueryHandler(IDbConnection connection)
        {
            this.connection = connection;
        }

        public BulkUploadErrorExportModel Search(GetBulkUploadErrorReportParameters parameters)
        {
            BulkUploadErrorExportModel bulkExp = new BulkUploadErrorExportModel();

            var resultSet = connection.QueryMultiple(
                "EXEC app.GetBulkUploadData @bulkUploadId, @bulkUploadDataTypeId", 
                new 
                { 
                    bulkUploadId = parameters.BulkUploadId,
                    bulkUploadDataTypeId = parameters.BulkUploadDataType
                });
            bulkExp.BulkUploadModel = resultSet.Read<BulkUploadModel>().FirstOrDefault();
            bulkExp.bulkUploadErrorModels = resultSet.Read<BulkUploadErrorModel>().ToList();
            return bulkExp;
        }
    }
}