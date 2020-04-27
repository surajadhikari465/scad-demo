using BrandUploadProcessor.Common.Models;
using Icon.Common.DataAccess;

namespace BrandUploadProcessor.DataAccess.Queries
{
    public class GetFileContentParameters : IQuery<GetFileContentResults>
    {
        public int BulkUploadId { get; set; }
    }
}