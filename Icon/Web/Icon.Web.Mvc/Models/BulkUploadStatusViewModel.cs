using System;

namespace Icon.Web.Mvc.Models
{
    public class BulkUploadStatusViewModel
    {
        public int BulkUploadId { get; set; }
        public string FileName { get; set; }
        public string BulkUploadDataType { get; set; }
        public string FileModeType { get; set; }
        public DateTime FileUploadTime { get; set; }
        public string UploadedBy { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public int? PercentageProcessed { get; set; }
        public int NumberOfRowsWithError { get; set; }
    }
}