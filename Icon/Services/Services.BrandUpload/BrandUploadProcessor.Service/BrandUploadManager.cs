using System;
using System.Collections.Generic;
using System.Linq;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.DataAccess.Commands;
using BrandUploadProcessor.DataAccess.Queries;
using BrandUploadProcessor.Service.ExcelParsing;
using BrandUploadProcessor.Service.ExcelParsing.Interfaces;
using BrandUploadProcessor.Service.Interfaces;
using BrandUploadProcessor.Service.Mappers.Interfaces;
using BrandUploadProcessor.Service.Validation;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.DataAccess;
using Icon.Logging;
using OfficeOpenXml;

namespace BrandUploadProcessor.Service
{
    public class BrandUploadManager : IBrandUploadManager
    {
        internal BrandUploadInformation activeUpload;
        internal ExcelPackage activeExcelData;
        internal List<BrandAttributeModel> brandAttributeModels;


        private readonly ILogger<BrandUploadManager> logger;

        private readonly ICommandHandler<SetStatusCommand> setStatusCommandHandler;
        private readonly ICommandHandler<ClearErrorsCommand> clearErrorsCommandHandler;
        private readonly ICommandHandler<SaveErrorsCommand> saveErrorsCommandHandler;
        private readonly ICommandHandler<PublishBrandUpdatesCommand> publishBrandUpdatesCommandHandler;

        private readonly IQueryHandler<GetFileContentParameters, GetFileContentResults> getFileContentQueryHandler;
        private readonly IQueryHandler<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>, IEnumerable<BrandAttributeModel>> getBrandAttributeModelQueryHandler;

        private readonly IExcelWorksheetParser excelWorksheetParser;
        private readonly IExcelWorksheetHeadersParser excelWorksheetHeadersParser;
        private readonly IExcelRowParser excelRowParser;

        private readonly IExcelWorkbookValidator excelWorkbookValidator;
        private readonly IColumnHeadersValidator columnHeadersValidator;
        private readonly IRowObjectsValidator rowObjectsValidator;
        private  RowObjectValidatorResponse validationResponse;

        private readonly IRowObjectToAddBrandModelMapper addBrandModelMapper;
        private readonly IRowObjectToUpdateBrandModelMapper updateBrandModelMapper;
        private readonly IAddUpdateBrandManager addUpdateBrandManager;

        private readonly IServiceConfiguration sercieConfiguration;
        
        private List<ColumnHeader> headers;
        private List<RowObject> rowData;

        public BrandUploadManager(ILogger<BrandUploadManager> logger,
            ICommandHandler<SetStatusCommand> setStatusCommandHandler,
            ICommandHandler<ClearErrorsCommand> clearErrorsCommandHandler,
            ICommandHandler<SaveErrorsCommand> saveErrorsCommandHandler,
            IQueryHandler<GetFileContentParameters, GetFileContentResults> getFileContentQueryHandler,
            IQueryHandler<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>, 
            IEnumerable<BrandAttributeModel>> getBrandAttributeModelQueryHandler,
            IExcelWorksheetParser excelWorksheetParser,
            IExcelWorksheetHeadersParser excelWorksheetHeadersParser,
            IExcelRowParser excelRowParser,
            IExcelWorkbookValidator excelWorkbookValidator,
            IColumnHeadersValidator columnHeadersValidator,
            IRowObjectsValidator rowObjectsValidator,
            IRowObjectToAddBrandModelMapper addBrandModelMapper,
            IRowObjectToUpdateBrandModelMapper updateBrandModelMapper,
            IAddUpdateBrandManager addUpdateBrandManager,
            ICommandHandler<PublishBrandUpdatesCommand> publishBrandUpdatesCommandHandler,
            IServiceConfiguration sercieConfiguration)
        {
            this.logger = logger;
            this.setStatusCommandHandler = setStatusCommandHandler;
            this.clearErrorsCommandHandler = clearErrorsCommandHandler;
            this.saveErrorsCommandHandler = saveErrorsCommandHandler;
            this.getFileContentQueryHandler = getFileContentQueryHandler;
            this.getBrandAttributeModelQueryHandler = getBrandAttributeModelQueryHandler;
            this.excelWorksheetParser = excelWorksheetParser;
            this.excelWorksheetHeadersParser = excelWorksheetHeadersParser;
            this.excelRowParser = excelRowParser;
            this.excelWorkbookValidator = excelWorkbookValidator;
            this.columnHeadersValidator = columnHeadersValidator;
            this.rowObjectsValidator = rowObjectsValidator;
            this.addBrandModelMapper = addBrandModelMapper;
            this.updateBrandModelMapper = updateBrandModelMapper;
            this.addUpdateBrandManager = addUpdateBrandManager;
            this.publishBrandUpdatesCommandHandler = publishBrandUpdatesCommandHandler;
            this.sercieConfiguration = sercieConfiguration;
        } 

        public void SetActiveUpload(BrandUploadInformation uploadInformation)
        {
            this.activeUpload = uploadInformation;
            this.GetExcelData();
        }
        internal void VerifyActiveUpload()
        {
            if (this.activeUpload == null) throw new Exception("SetActiveUpload must be called before performing this action.");
        }

        public void SetStatus(Enums.FileStatusEnum status, string message = "", int percentageProcessed = 0)
        {
            VerifyActiveUpload();

            var command = new SetStatusCommand { FileStatus = status, BulkUploadId = activeUpload.BulkUploadId, Message = message, PercentageProcessed = percentageProcessed };
            setStatusCommandHandler.Execute(command);

            logger.Info($"Setting Status: {message}");
        }
        public virtual void GetExcelData()
        {
            VerifyActiveUpload();
            activeExcelData?.Dispose();

            var parameters = new GetFileContentParameters { BulkUploadId = activeUpload.BulkUploadId };
            var fileContent = getFileContentQueryHandler.Search(parameters).Data;

            if (fileContent == null) throw new Exception("No File data found");

            activeExcelData = excelWorksheetParser.Parse(fileContent);
        }

        public void Validate()
        {
            ClearErrors(activeUpload.BulkUploadId);
            SetStatus(Enums.FileStatusEnum.Processing, "Validating Excel File", 10);
            var result = excelWorkbookValidator.Validate(activeUpload, activeExcelData);
            if (!result.IsValid)
                throw new InvalidOperationException(result.Error);

            var columHeaderValidationResult = columnHeadersValidator.Validate(activeUpload, activeExcelData);
            var sheet = activeExcelData.Workbook.Worksheets[Constants.BrandWorksheetName];
            
            SetStatus(Enums.FileStatusEnum.Processing, "Parsing Headers", 20);
            headers = excelWorksheetHeadersParser.Parse(sheet);

            SetStatus(Enums.FileStatusEnum.Processing, "Parsing Rows", 30);
            rowData = excelRowParser.Parse(sheet, headers);

            if (!columHeaderValidationResult.IsValid)
            {
                SetErrorForAllRows(columHeaderValidationResult);
            }
            else
            {
                SetStatus(Enums.FileStatusEnum.Processing, "Validating Rows", 40);
                validationResponse = rowObjectsValidator.Validate(activeUpload.FileModeTypeId, rowData, headers, brandAttributeModels);
            }
        }

        public void GetAttributeData()
        {
            brandAttributeModels = getBrandAttributeModelQueryHandler.Search(new EmptyQueryParameters<IEnumerable<BrandAttributeModel>>()).ToList();
        }

        private void SetErrorForAllRows(ValidationResponse columHeaderValidationResult)
        {
            List<InvalidRowError> errors = new List<InvalidRowError>();
            validationResponse = new RowObjectValidatorResponse();

            foreach (RowObject rowObject in rowData)
            {
                errors.Add(new InvalidRowError
                {
                    RowId = rowObject.Row,
                    Error = columHeaderValidationResult.Error
                });
            }
            validationResponse.InvalidRows.AddRange(errors);
            SetStatus(Enums.FileStatusEnum.Processing, "Processing Complete", 100);
        }
        public void ClearErrors(int brandUploadId)
        {
            clearErrorsCommandHandler.Execute(new ClearErrorsCommand() { BulkUploadId = brandUploadId });
        }


        public void Process()
        {
            int failedRowsCount = 0;
            int validRows =0;
            int totalRows= 0;

            SetStatus(Enums.FileStatusEnum.Processing, "Processing Validated Data", 60);
            if (validationResponse.ValidRows.Any())
            {
                validRows = validationResponse.ValidRows.Count;
                totalRows = (validationResponse.InvalidRows.Any() ? validationResponse.InvalidRows.Select(s => s.RowId).Distinct().Count() : failedRowsCount) + validRows;
                if (activeUpload.FileModeTypeId == Enums.FileModeTypeEnum.CreateNew)
                {
                    ProcessCreateNewUpload();
                }
                else
                {
                    ProcessUpdateUpload();
                }
            }
            else
            {
                totalRows = (validationResponse.InvalidRows.Any() ? validationResponse.InvalidRows.Select(s => s.RowId).Distinct().Count() : failedRowsCount);
            }

            if (validationResponse.InvalidRows.Any())
            {
                failedRowsCount = validationResponse.InvalidRows.Select(x => x.RowId).Distinct().Count();
                validRows = totalRows - failedRowsCount;
                ProcessErrors(activeUpload.BulkUploadId, validationResponse.InvalidRows);
            }

            SetStatus(validationResponse.InvalidRows.Any() ? Enums.FileStatusEnum.Error : Enums.FileStatusEnum.Complete, $"{validRows} of {totalRows} Rows Successful.", 100);

        }

        private void ProcessCreateNewUpload()
        {
            SetStatus(Enums.FileStatusEnum.Processing, "Creating Brands", 80);


            var mapperResponse = addBrandModelMapper.Map(validationResponse.ValidRows, headers, brandAttributeModels, activeUpload.UploadedBy);
            List<ErrorItem<AddBrandModel>> invalidBrands = new List<ErrorItem<AddBrandModel>>();
            List<int> addedBrands = new List<int>();
            addUpdateBrandManager.CreateBrands(mapperResponse.Brands, invalidBrands, addedBrands);

            if (invalidBrands.Any())
            {
                foreach (var invalidItem in invalidBrands)
                {
                    if (mapperResponse.BrandToRowDictionary.ContainsKey(invalidItem.Item))
                    {
                        var rowObject = mapperResponse.BrandToRowDictionary[invalidItem.Item];
                        validationResponse.InvalidRows.Add(new InvalidRowError
                        {
                            RowId = rowObject.Row,
                            Error = invalidItem.Error
                        });
                    }
                }
            }

            if (addedBrands.Any())
            {
                publishBrandUpdatesCommandHandler.Execute(new PublishBrandUpdatesCommand()
                {
                    BrandIds = addedBrands,
                    Regions =  sercieConfiguration.BrandRefreshEventConfiguredRegions
                });
            }
        }
        private void ProcessUpdateUpload()
        {
            List<ErrorItem<UpdateBrandModel>> invalidBrands = new List<ErrorItem<UpdateBrandModel>>();
            List<int> updatedBrands = new List<int>();

            SetStatus(Enums.FileStatusEnum.Processing, "Updating Items", 80);
            var mapperResponse = updateBrandModelMapper.Map(validationResponse.ValidRows, headers, brandAttributeModels, activeUpload.UploadedBy);
            addUpdateBrandManager.UpdateBrands(mapperResponse.Brands, invalidBrands, updatedBrands);

            if (invalidBrands.Any())
            {
                foreach (var invalidItem in invalidBrands)
                {
                    if (mapperResponse.BrandToRowDictionary.ContainsKey(invalidItem.Item))
                    {
                        var rowObject = mapperResponse.BrandToRowDictionary[invalidItem.Item];
                        validationResponse.InvalidRows.Add(new InvalidRowError
                        {
                            RowId = rowObject.Row,
                            Error = invalidItem.Error
                        });
                    }
                }
            }

            if (updatedBrands.Any())
            {
                publishBrandUpdatesCommandHandler.Execute(new PublishBrandUpdatesCommand()
                {
                    BrandIds = updatedBrands,
                    Regions = sercieConfiguration.BrandRefreshEventConfiguredRegions
                });
            }
        }
        private void ProcessErrors(int brandUploadId, List<InvalidRowError> invalidRows)
        {
            if (invalidRows.Count == 0) return;
            logger.Info($"Errors found: {invalidRows.Count}");

            SetStatus(Enums.FileStatusEnum.Processing, "Processing Errors", 90);
            var errors = from i in invalidRows
                group i by i.RowId
                into grouping
                let errorsByRow = grouping.Select(s => s.Error).ToList()
                let rowId = grouping.Key
                select new SaveErrorsCommand()
                {
                    BulkUploadId = brandUploadId,
                    RowId = rowId,
                    ErrorList = errorsByRow
                };

            foreach (var saveErrorCommand in errors)
            {
                saveErrorsCommandHandler.Execute(saveErrorCommand);
            }
        }
    }
}