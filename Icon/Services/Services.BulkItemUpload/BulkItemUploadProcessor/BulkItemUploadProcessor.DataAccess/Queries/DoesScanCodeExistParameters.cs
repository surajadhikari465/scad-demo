using Icon.Common.DataAccess;

namespace BulkItemUploadProcessor.DataAccess.Queries
{
    public class DoesScanCodeExistParameters : IQuery<bool>
    {
        public string ScanCode { get; set; }
    }
}