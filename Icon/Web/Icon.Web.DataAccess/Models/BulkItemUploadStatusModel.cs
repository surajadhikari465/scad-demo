using System;

namespace Icon.Web.DataAccess.Models
{
    public class BulkItemUploadStatusModel
    {
        public int BulkItemUploadId { get; set; }
        public string FileName { get; set; }
        public bool FileModeType { get; set; }
        public DateTime FileUploadTime { get; set; }
        public string UploadedBy { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public int? PercentageProcessed { get; set; }
        public int NumberOfRowsWithError { get; set; }
    }
}