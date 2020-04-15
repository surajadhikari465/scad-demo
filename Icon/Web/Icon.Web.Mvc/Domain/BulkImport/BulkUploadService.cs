using Icon.Common.DataAccess;
using Icon.Web.Common.BulkUpload;
using Icon.Web.DataAccess.Commands;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using System.Collections.Generic;

namespace Icon.Web.Mvc.Domain.BulkImport
{
    public class BulkUploadService : IBulkUploadService
    {
        private IQueryHandler<GetBulkUploadErrorsPrameters, List<BulkUploadErrorModel>> getBulkUploadErrorsQueryHandler;
        private ICommandHandler<BulkUploadCommand> bulkUploadCommandHandler;
        private IQueryHandler<GetBulkUploadStatusParameters, List<BulkUploadStatusModel>> getBulkUploadStatusQueryHandler;
        private IQueryHandler<GetBulkUploadByIdParameters, BulkUploadStatusModel> getBulkUploadByIdQueryHandler;
        private IQueryHandler<GetBulkUploadErrorReportParameters, BulkUploadErrorExportModel> getBulkUploadErrorReportQueryHandler;

        public BulkUploadService(
            IQueryHandler<GetBulkUploadErrorsPrameters, List<BulkUploadErrorModel>> getBulkUploadErrorsQueryHandler,
            ICommandHandler<BulkUploadCommand> bulkUploadCommandHandler,
            IQueryHandler<GetBulkUploadStatusParameters, List<BulkUploadStatusModel>> getBulkUploadStatusQueryHandler,
            IQueryHandler<GetBulkUploadByIdParameters, BulkUploadStatusModel> getBulkUploadByIdQueryHandler,
            IQueryHandler<GetBulkUploadErrorReportParameters, BulkUploadErrorExportModel> getBulkUploadErrorReportQueryHandler)
        {
            this.getBulkUploadErrorsQueryHandler = getBulkUploadErrorsQueryHandler;
            this.bulkUploadCommandHandler = bulkUploadCommandHandler;
            this.getBulkUploadStatusQueryHandler = getBulkUploadStatusQueryHandler;
            this.getBulkUploadByIdQueryHandler = getBulkUploadByIdQueryHandler;
            this.getBulkUploadErrorReportQueryHandler = getBulkUploadErrorReportQueryHandler;
        }

        public void BulkUpload(BulkUploadDataType bulkUploadDataType, BulkUploadActionType bulkUploadFileType, string fileName, byte[] fileContent, string uploadedBy)
        {
            bulkUploadCommandHandler.Execute(new BulkUploadCommand
            {
                BulkUploadModel = new BulkUploadModel
                {
                    FileContent = fileContent,
                    BulkUploadDataType = bulkUploadDataType,
                    FileModeType = bulkUploadFileType,
                    FileName = fileName,
                    UploadedBy = uploadedBy
                }
            });
        }

        public List<BulkUploadErrorModel> GetBulkUploadErrors(BulkUploadDataType bulkUploadType, int bulkUploadId)
        {
            return getBulkUploadErrorsQueryHandler.Search(new GetBulkUploadErrorsPrameters
            {
                BulkUploadId = bulkUploadId,
                BulkUploadDataType = bulkUploadType
            });
        }

        public BulkUploadStatusModel GetBulkUpload(BulkUploadDataType bulkUploadType, int bulkUploadId)
        {
            return getBulkUploadByIdQueryHandler.Search(new GetBulkUploadByIdParameters
            {
                BulkUploadId = bulkUploadId,
                BulkUploadDataType = bulkUploadType
            });
        }

        public List<BulkUploadStatusModel> GetBulkUploads(BulkUploadDataType bulkUploadType, int rowCount)
        {
            return getBulkUploadStatusQueryHandler.Search(new GetBulkUploadStatusParameters
            {
                RowCount = rowCount,
                BulkUploadDataType = bulkUploadType
            });
        }

        public BulkUploadErrorExportModel GetBulkUploadErrorExport(BulkUploadDataType bulkUploadType, int bulkUploadId)
        {
            return getBulkUploadErrorReportQueryHandler.Search(new GetBulkUploadErrorReportParameters
            {
                BulkUploadId = bulkUploadId,
                BulkUploadDataType = bulkUploadType
            });
        }
    }
}