using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;

namespace BulkItemUploadProcessor.Service.Interfaces
{
    public interface IBulkUploadManager 
    {
        void SetStatus(Enums.FileStatusEnum status, string message = "");
        void GetExcelData();
        void SetActiveUpload(BulkItemUploadInformation uploadInformation);
        void GetAttributeData();
        void Validate();

        void Process();
    }
}