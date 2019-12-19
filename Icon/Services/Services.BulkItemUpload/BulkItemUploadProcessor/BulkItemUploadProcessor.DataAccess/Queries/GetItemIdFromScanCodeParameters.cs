using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class GetItemIdFromScanCodeParameters : IQuery<int?>
    {
        public string ScanCode { get; set; }
    }
}