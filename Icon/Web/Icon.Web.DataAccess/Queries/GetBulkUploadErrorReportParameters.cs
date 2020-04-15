using Icon.Common.DataAccess;
using Icon.Web.Common.BulkUpload;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadErrorReportParameters : IQuery<BulkUploadErrorExportModel>
    {
        public int BulkUploadId { get; set; }
        public BulkUploadDataType BulkUploadDataType { get; set; }
    }
}