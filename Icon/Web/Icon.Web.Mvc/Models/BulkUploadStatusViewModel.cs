using System;

namespace Icon.Web.Mvc.Models
{
    public class BulkUploadStatusViewModel
    {
        public int BulkItemUploadId { get; set; }
        public string FileName { get; set; }
        public int FileModeType { get; set; }
        public DateTime FileUploadTime { get; set; }
        public string UploadedBy { get; set; }
        public string Status { get; set; }
    }
}