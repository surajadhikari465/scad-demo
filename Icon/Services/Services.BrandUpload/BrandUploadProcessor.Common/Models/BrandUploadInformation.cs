using System;
using static BrandUploadProcessor.Common.Enums;

namespace BrandUploadProcessor.Common.Models
{
    public class BrandUploadInformation
    {
        public BrandUploadInformation() { }


        public BrandUploadInformation(int bulkUploadId, string fileName, FileModeTypeEnum fileModeType, DateTime fileUploadTime, string uploadedBy, int statusId)
        {
            BulkUploadId = bulkUploadId;
            FileName = fileName;
            FileModeTypeId = fileModeType;
            FileUploadTime = fileUploadTime;
            UploadedBy = uploadedBy;
            StatusId = statusId;
        }

        public int BulkUploadId { get; set; }
        public string FileName { get; set; }
         public Enums.FileModeTypeEnum FileModeTypeId { get; set; }
        public DateTime FileUploadTime { get; set; }
        public string UploadedBy { get; set; }
        public int StatusId { get; set; }
    }
}
