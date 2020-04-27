using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;

namespace BrandUploadProcessor.Service.Interfaces
{
    public interface IBrandUploadManager
    {
        void SetActiveUpload(BrandUploadInformation uploadInformation);
        void SetStatus(Enums.FileStatusEnum status, string message = "", int percentageProcessed = 0);
        void GetExcelData();
        void GetAttributeData();
        void Validate();
        void Process();

        
    }

    public interface IBrandsCache
    {
        List<BrandModel> Brands { get; set; }
        void Refresh();
    }
}