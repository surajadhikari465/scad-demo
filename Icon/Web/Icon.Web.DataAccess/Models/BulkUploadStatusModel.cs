using Icon.Web.Common.BulkUpload;
using System;

namespace Icon.Web.DataAccess.Models
{
    public class BulkUploadStatusModel
    {
        public int BulkUploadId { get; set; }
        public string FileName { get; set; }
        public BulkUploadActionType FileModeType { get; set; }
        public DateTime FileUploadTime { get; set; }
        public string UploadedBy { get; set; }
        public string Status { get; set; }
        public string Message { get; set; }
        public int? PercentageProcessed { get; set; }
        public int NumberOfRowsWithError { get; set; }
    }
}