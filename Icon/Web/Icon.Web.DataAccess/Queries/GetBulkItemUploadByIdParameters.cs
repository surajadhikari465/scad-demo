using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Models;

namespace Icon.Web.DataAccess.Queries
{
    public class GetBulkItemUploadByIdParameters : IQuery<BulkItemUploadStatusModel>
    {
        public int BulkItemUploadId { get; set; }
    }
}