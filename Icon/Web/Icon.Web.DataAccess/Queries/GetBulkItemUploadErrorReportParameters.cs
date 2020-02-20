using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkItemUploadErrorReportParameters : IQuery<BulkItemUploadErrorExportModel>
    {
        public int BulkItemUploadId { get; set; }
    }
}