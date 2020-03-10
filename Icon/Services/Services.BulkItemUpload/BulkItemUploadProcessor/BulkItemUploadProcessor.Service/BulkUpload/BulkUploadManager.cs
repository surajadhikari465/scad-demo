using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Commands;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using BulkItemUploadProcessor.Service.ExcelParsing.Interfaces;
using BulkItemUploadProcessor.Service.Interfaces;
using BulkItemUploadProcessor.Service.Mappers.Interfaces;
using BulkItemUploadProcessor.Service.Validation;
using BulkItemUploadProcessor.Service.Validation.Interfaces;
using BulkItemUploadProcessor.Service.Validation.Validation.Interfaces;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Logging;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BulkItemUploadProcessor.Service.BulkUpload
{
    public class BulkUploadManager : IBulkUploadManager
    {
        private readonly ILogger<BulkUploadManager> logger;
        private readonly ICommandHandler<SetStatusCommand> setStatusCommandHandler;
        private readonly ICommandHandler<ClearErrorsCommand> clearErrorsCommandHandler;
        private readonly ICommandHandler<SaveErrorsCommand> saveErrorsCommandHandler;
        private readonly IQueryHandler<GetFileContentParameters, GetFileContentResults> getFileContentQueryHandler;
        private readonly IValidationManager validationManager;
        private readonly IQueryHandler<GetItemIdFromScanCodeParameters, int?> getItemIdFromScanCodeHandler;
        private readonly IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel> getItemPropertiesFromMerchQueryHandler;

        private readonly IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributeModelQueryHandler;
        private readonly IRowObjectToUpdateItemModelMapper updateItemMapper;
        internal BulkItemUploadInformation activeUpload;
        internal ExcelPackage activeExcelData;
        internal List<AttributeModel> attributeModels;
        internal List<NewItemViewModel> newItems;
        internal List<EditItemViewModel> editedItems;

        private readonly IExcelWorksheetParser excelWorksheetParser;
        private readonly IExcelWorksheetHeadersParser excelWorksheetHeadersParser;
        private readonly IExcelRowParser excelRowParser;
        private readonly IRowObjectsValidator rowObjectsValidator;
        private readonly ICommandHandler<UpdateItemsCommand> updateItemsCommandHandler;
        private readonly IHierarchyCache hierarchyCache;
        private readonly IMerchItemPropertiesCache merchItemPropertiesCache;

        private List<RowObject> rowData;
        public IRowObjectToAddItemModelMapper addItemModelMapper;
        private readonly ICommandHandler<AddItemsCommand> addItemsCommandHandler;
        private readonly IExcelWorkbookValidator excelWorkbookValidator;
        private readonly ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler;
        private List<ColumnHeader> headers;
        private RowObjectValidatorResponse validationResponse;

        public BulkUploadManager(
            ILogger<BulkUploadManager> logger,
            ICommandHandler<SetStatusCommand> statusCommandHandler,
            ICommandHandler<ClearErrorsCommand> clearErrrorsCommandHandler,
            ICommandHandler<SaveErrorsCommand> saveErrorsCommandHandler,
            IQueryHandler<GetFileContentParameters, GetFileContentResults> fileContentQueryHandler,
            IValidationManager validationManagerHandler,
            IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel> getItemPropertiesFromMerchQueryHandler,
            IQueryHandler<GetItemIdFromScanCodeParameters, int?> getItemIdFromScanCodeHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>> getAttributeModelQueryHandler,
            IRowObjectToUpdateItemModelMapper updateItemMapper,
            IExcelWorksheetParser excelWorksheetParser,
            IExcelWorksheetHeadersParser excelWorksheetHeadersParser,
            IExcelRowParser excelRowParser,
            IRowObjectsValidator rowObjectsValidator,
            ICommandHandler<UpdateItemsCommand> updateItemsCommandHandler,
            IHierarchyCache hierarchyCache,
            IMerchItemPropertiesCache merchItemPropertiesCache,
            IRowObjectToAddItemModelMapper addItemModelMapper,
            ICommandHandler<AddItemsCommand> addItemsCommandHandler,
            IExcelWorkbookValidator excelWorkbookValidator,
            ICommandHandler<PublishItemUpdatesCommand> publishItemUpdatesCommandHandler)
        {
            this.logger = logger;
            setStatusCommandHandler = statusCommandHandler;
            getFileContentQueryHandler = fileContentQueryHandler;
            validationManager = validationManagerHandler;
            this.getItemPropertiesFromMerchQueryHandler = getItemPropertiesFromMerchQueryHandler;
            clearErrorsCommandHandler = clearErrrorsCommandHandler;
            this.saveErrorsCommandHandler = saveErrorsCommandHandler;
            this.getItemIdFromScanCodeHandler = getItemIdFromScanCodeHandler;
            this.getAttributeModelQueryHandler = getAttributeModelQueryHandler;
            this.updateItemMapper = updateItemMapper;
            this.excelWorksheetParser = excelWorksheetParser;
            this.excelWorksheetHeadersParser = excelWorksheetHeadersParser;
            this.excelRowParser = excelRowParser;
            this.rowObjectsValidator = rowObjectsValidator;
            this.updateItemsCommandHandler = updateItemsCommandHandler;
            this.hierarchyCache = hierarchyCache;
            this.merchItemPropertiesCache = merchItemPropertiesCache;
            this.addItemModelMapper = addItemModelMapper;
            this.addItemsCommandHandler = addItemsCommandHandler;
            this.excelWorkbookValidator = excelWorkbookValidator;
            this.publishItemUpdatesCommandHandler = publishItemUpdatesCommandHandler;
        }

        public void SetActiveUpload(BulkItemUploadInformation uploadInformation)
        {
            this.activeUpload = uploadInformation;
            this.GetExcelData();
        }

        public virtual void GetExcelData()
        {
            VerifyActiveUpload();
            activeExcelData?.Dispose();

            var parameters = new GetFileContentParameters { BulkItemUploadId = activeUpload.BulkItemUploadId };
            var fileContent = getFileContentQueryHandler.Search(parameters).Data;

            if (fileContent == null) throw new Exception("No File data found");

            activeExcelData = excelWorksheetParser.Parse(fileContent);
        }

        public void SetStatus(Enums.FileStatusEnum status, string message = "", int PercentageProcessed = 0)
        {
            VerifyActiveUpload();

            var command = new SetStatusCommand { FileStatus = status, BulkItemUploadId = activeUpload.BulkItemUploadId, Message = message, PercentageProcessed = PercentageProcessed };
            setStatusCommandHandler.Execute(command);

            logger.Info($"Setting Status: {message}");
        }

        public void GetAttributeData()
        {
            attributeModels = getAttributeModelQueryHandler.Search(new EmptyQueryParameters<IEnumerable<AttributeModel>>()).ToList();
        }

        public void Validate()
        {
            hierarchyCache.Refresh();
            merchItemPropertiesCache.Refresh();
            ClearErrors(activeUpload.BulkItemUploadId);
            SetStatus(Enums.FileStatusEnum.Processing, "Validating Excel File", 10);
            var result = excelWorkbookValidator.Validate(activeUpload, activeExcelData);
            if (!result.IsValid)
                throw new InvalidOperationException(result.Error);

            var sheet = activeExcelData.Workbook.Worksheets[Constants.ItemWorksheetName];

            SetStatus(Enums.FileStatusEnum.Processing, "Parsing Headers", 20);
            headers = excelWorksheetHeadersParser.Parse(sheet);

            SetStatus(Enums.FileStatusEnum.Processing, "Parsing Rows", 30);
            rowData = excelRowParser.Parse(sheet, headers);

            SetStatus(Enums.FileStatusEnum.Processing, "Validating Rows", 40);
            validationResponse = rowObjectsValidator.Validate(activeUpload.FileModeType, rowData, headers, attributeModels);
        }

        public void ClearErrors(int bulkItemUploadId)
        {
            clearErrorsCommandHandler.Execute(new ClearErrorsCommand() { BulkItemUploadId = bulkItemUploadId });
        }

        public void Process()
        {
            int failedRowsCount = 0;
            int validRows = 0;
            int totalRows = 0;

            SetStatus(Enums.FileStatusEnum.Processing, "Processing Validated Data", 60);
            if (validationResponse.ValidRows.Any())
            {
                validRows = validationResponse.ValidRows.Count;
                if (activeUpload.FileModeType == Enums.FileModeTypeEnum.CreateNew)
                {
                    ProcessCreateNewUpload();
                }
                else
                {
                    ProcessUpdateUpload();
                }
            }

            if (validationResponse.InvalidRows.Any())
            {
                failedRowsCount = validationResponse.InvalidRows.Select(x => x.RowId).Distinct().Count();
                ProcessErrors(activeUpload.BulkItemUploadId, validationResponse.InvalidRows);
            }

            totalRows = failedRowsCount + validRows;
            SetStatus(validationResponse.InvalidRows.Any() ? Enums.FileStatusEnum.Error : Enums.FileStatusEnum.Complete, $"{validRows} of {totalRows} Rows Successful.", 100);
        }

        private void ProcessCreateNewUpload()
        {
            SetStatus(Enums.FileStatusEnum.Processing, "Creating Items", 80);
            var mapperResponse = addItemModelMapper.Map(validationResponse.ValidRows, headers, attributeModels, activeUpload.UploadedBy);
            var command = new AddItemsCommand
            {
                Items = mapperResponse.Items
            };
            addItemsCommandHandler.Execute(command);

            if (command.InvalidItems != null && command.InvalidItems.Any())
            {
                foreach (var invalidItem in command.InvalidItems)
                {
                    if (mapperResponse.ItemToRowDictionary.ContainsKey(invalidItem.Item))
                    {
                        var rowObject = mapperResponse.ItemToRowDictionary[invalidItem.Item];
                        validationResponse.InvalidRows.Add(new InvalidRowError
                        {
                            RowId = rowObject.Row,
                            Error = invalidItem.Error
                        });
                    }
                }
            }
            if (command.AddedItems != null && command.AddedItems.Any())
            {
                publishItemUpdatesCommandHandler.Execute(new PublishItemUpdatesCommand
                {
                    ScanCodes = command.AddedItems.Select(i => i.ScanCode).ToList()
                });
            }
        }

        private void ProcessUpdateUpload()
        {
            SetStatus(Enums.FileStatusEnum.Processing, "Updating Items", 80);
            var items = updateItemMapper.Map(validationResponse.ValidRows, headers, attributeModels, activeUpload.UploadedBy);
            updateItemsCommandHandler.Execute(new UpdateItemsCommand
            {
                Items = items
            });
            publishItemUpdatesCommandHandler.Execute(new PublishItemUpdatesCommand
            {
                ScanCodes = items.Select(i => i.ScanCode).ToList()
            });
        }

        private void ProcessErrors(int bulkItemUploadId, List<InvalidRowError> invalidRows)
        {
            if (invalidRows.Count == 0) return;
            logger.Info($"Items with Errors: {invalidRows.Count}");

            SetStatus(Enums.FileStatusEnum.Processing, "Processing Errors", 90);
            var errors = from i in invalidRows
                         group i by i.RowId
                         into grouping
                         let errorsByRow = grouping.Select(s => s.Error).ToList()
                         let rowId = grouping.Key
                         select new SaveErrorsCommand()
                         {
                             BulkItemUploadId = bulkItemUploadId,
                             RowId = rowId,
                             ErrorList = errorsByRow
                         };

            foreach (var saveErrorCommand in errors)
            {
                saveErrorsCommandHandler.Execute(saveErrorCommand);
            }
        }

        internal void VerifyActiveUpload()
        {
            if (this.activeUpload == null) throw new Exception("SetActiveUpload must be called before performing this action.");
        }

        public void Dispose()
        {
            activeExcelData?.Dispose();
        }
    }
}