using Icon.Web.Common.BulkUpload;
using Icon.Web.DataAccess.Models;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Domain.BulkImport
{
    public interface IBulkUploadService
    {
        void BulkUpload(BulkUploadDataType bulkUploadType, BulkUploadActionType bulkUploadFileType, string fileName, byte[] fileContent, string uploadedBy);
        BulkUploadStatusModel GetBulkUpload(BulkUploadDataType bulkUploadType, int bulkUploadId);
        List<BulkUploadStatusModel> GetBulkUploads(BulkUploadDataType bulkUploadType, int rowCount);
        List<BulkUploadErrorModel> GetBulkUploadErrors(BulkUploadDataType bulkUploadType, int bulkUploadId);
        BulkUploadErrorExportModel GetBulkUploadErrorExport(BulkUploadDataType bulkUploadType, int bulkUploadId);
    }
}