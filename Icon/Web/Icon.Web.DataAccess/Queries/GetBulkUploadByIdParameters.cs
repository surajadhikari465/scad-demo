using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkUploadByIdParameters : IQuery<BulkUploadStatusModel>
    {
        public int BulkItemUploadId { get; set; }
    }
}