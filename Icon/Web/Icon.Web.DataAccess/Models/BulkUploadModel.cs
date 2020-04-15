using Icon.Web.Common.BulkUpload;

namespace Icon.Web.DataAccess.Models
{
    public class BulkUploadModel
    {
        public string FileName { get; set; }
        public BulkUploadDataType BulkUploadDataType { get; set; }
        public BulkUploadActionType FileModeType { get; set; }
        public byte[] FileContent { get; set; }
        public string UploadedBy { get; set; }
    }
}
