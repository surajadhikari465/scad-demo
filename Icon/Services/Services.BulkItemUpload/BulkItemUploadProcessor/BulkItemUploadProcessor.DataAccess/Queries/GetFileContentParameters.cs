using BulkItemUploadProcessor.Common.Models;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetFileContentParameters : IQuery<GetFileContentResults>
    {
        public int BulkUploadId { get; set; }
    }
}