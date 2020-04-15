using System;
using static BulkItemUploadProcessor.Common.Enums;

namespace BulkItemUploadProcessor.Common.Models
{
    public class BulkItemUploadInformation
    {
        public BulkItemUploadInformation() { }
        
        public BulkItemUploadInformation(int bulkUploadId, string fileName, FileModeTypeEnum fileModeType, DateTime fileUploadTime, string uploadedBy, int statusId)
        {
            BulkUploadId = bulkUploadId;
            FileName = fileName;
            FileModeType = fileModeType;
            FileUploadTime = fileUploadTime;
            UploadedBy = uploadedBy;
            StatusId = statusId;
        }

        public int BulkUploadId { get; set; }
        public string FileName { get; set; }
        public FileModeTypeEnum FileModeType { get; set; }
        public DateTime FileUploadTime { get; set; }
        public string UploadedBy { get; set; }
        public int StatusId { get; set; }
    }
}