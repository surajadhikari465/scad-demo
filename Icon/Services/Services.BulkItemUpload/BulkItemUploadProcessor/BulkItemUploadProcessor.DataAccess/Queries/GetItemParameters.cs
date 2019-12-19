using BulkItemUploadProcessor.Common.Models;
using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemParameters : IQuery<ItemDbModel>
    {
        public string ScanCode { get; set; }
    }
}