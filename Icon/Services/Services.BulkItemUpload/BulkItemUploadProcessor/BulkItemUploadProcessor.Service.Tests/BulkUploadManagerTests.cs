using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Commands;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.BulkUpload;
using BulkItemUploadProcessor.Service.Cache.Interfaces;
using BulkItemUploadProcessor.Service.ExcelParsing.Interfaces;
using BulkItemUploadProcessor.Service.Interfaces;
using BulkItemUploadProcessor.Service.Mappers.Interfaces;
using BulkItemUploadProcessor.Service.Validation.Interfaces;
using BulkItemUploadProcessor.Service.Validation.Validation.Interfaces;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using OfficeOpenXml;
using System;
using System.Collections.Generic;
using System.IO;

namespace BulkItemUploadProcessor.Service.Tests
{
    [TestClass]
    public class BulkUploadManagerTests
    {
        private Mock<BulkUploadManager> mockBulUploadManager;
        private Mock<ILogger<BulkUploadManager>> mockLogger;
        private Mock<ICommandHandler<SetStatusCommand>> mockSetStatusCommandHandler;
        private Mock<IQueryHandler<GetFileContentParameters, GetFileContentResults>> mockGetFileContentQueryHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> mockGetAttributeQueryHandler;
        private Mock<IValidationManager> mockValidationManager;
        private Mock<ICommandHandler<ClearErrorsCommand>> mockClearErrorsCommandHandler;
        private Mock<ICommandHandler<SaveErrorsCommand>> mockSaveErrorsCommandHandler;
        private Mock<ICommandHandler<PublishItemUpdatesCommand>> mockPublishCommandHandler;
        private Mock<IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel>> mockGetItemPropertiesFromMerchQueryHandler;
        private Mock<IQueryHandler<GetItemIdFromScanCodeParameters, int?>> mockGetItemIdFromScanCodeHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>mockGetAttributeModelQueryHandler;
        private Mock<IRowObjectToUpdateItemModelMapper> mockUpdateItemMapper;
        private Mock<IExcelWorksheetParser>mockExcelWorksheetParse;
        private Mock<IExcelWorksheetHeadersParser> mockExcelWorksheetHeadersParser;
        private Mock<IExcelRowParser> mockExcelRowParser;
        private Mock<IRowObjectsValidator> mockRowObjectsValidator;
        private Mock<IColumnHeadersValidator> mockColumnHeadersValidator;
        private Mock<IHierarchyCache> mockHierarchyCache;
        private Mock<IMerchItemPropertiesCache> mockMerchItemPropertiesCache;
        private Mock<IRowObjectToAddItemModelMapper> mockAddItemModelMapper;
        private Mock<IExcelWorkbookValidator> mockExcelWorkbookValidator;
        private Mock<ICommandHandler<PublishItemUpdatesCommand>> mockPublishItemUpdatesCommandHandler;
        private byte[] testExcelData;
        private ExcelPackage testExcelFile;
        private Mock<IAddUpdateItemManager> mockAddUpdateItemManager;

        [TestInitialize]
        public void Init()
        {
            mockLogger = new Mock<ILogger<BulkUploadManager>>();
            mockBulUploadManager = new Mock<BulkUploadManager>();
            mockSetStatusCommandHandler = new Mock<ICommandHandler<SetStatusCommand>>();
            mockGetFileContentQueryHandler = new Mock<IQueryHandler<GetFileContentParameters, GetFileContentResults>>();
            mockGetAttributeQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            mockValidationManager = new Mock<IValidationManager>();
            mockClearErrorsCommandHandler = new Mock<ICommandHandler<ClearErrorsCommand>>();
            mockSaveErrorsCommandHandler = new Mock<ICommandHandler<SaveErrorsCommand>>();
            mockPublishCommandHandler = new Mock<ICommandHandler<PublishItemUpdatesCommand>>();
            mockGetItemPropertiesFromMerchQueryHandler = new Mock<IQueryHandler<GetItemPropertiesFromMerchHierarchyParameters, MerchDependentItemPropertiesModel>>();
            mockGetItemIdFromScanCodeHandler = new Mock<IQueryHandler<GetItemIdFromScanCodeParameters, int?>>();
            mockGetAttributeModelQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            mockUpdateItemMapper = new Mock<IRowObjectToUpdateItemModelMapper>();
            mockExcelWorksheetParse = new Mock<IExcelWorksheetParser>();
            mockExcelWorksheetHeadersParser = new Mock<IExcelWorksheetHeadersParser>();
            mockExcelRowParser = new Mock<IExcelRowParser>();
            mockRowObjectsValidator = new Mock<IRowObjectsValidator>();
            mockColumnHeadersValidator = new Mock<IColumnHeadersValidator>();
            mockHierarchyCache = new Mock<IHierarchyCache>();
            mockMerchItemPropertiesCache = new Mock<IMerchItemPropertiesCache>();
            mockExcelWorkbookValidator = new Mock<IExcelWorkbookValidator>();
            mockPublishItemUpdatesCommandHandler = new Mock<ICommandHandler<PublishItemUpdatesCommand>>();
            mockAddUpdateItemManager = new Mock<IAddUpdateItemManager>();

            mockBulUploadManager = new Mock<BulkUploadManager>(mockLogger.Object,
                mockSetStatusCommandHandler.Object,
                mockClearErrorsCommandHandler.Object,
                mockSaveErrorsCommandHandler.Object,
                mockGetFileContentQueryHandler.Object,
                mockValidationManager.Object,
                mockGetItemPropertiesFromMerchQueryHandler.Object,
                mockGetItemIdFromScanCodeHandler.Object,
                mockGetAttributeModelQueryHandler.Object,
                mockUpdateItemMapper.Object,
                mockExcelWorksheetParse.Object,
                mockExcelWorksheetHeadersParser.Object,
                mockExcelRowParser.Object,
                mockRowObjectsValidator.Object,
                mockColumnHeadersValidator.Object,
                mockHierarchyCache.Object,
                mockMerchItemPropertiesCache.Object,
                mockAddItemModelMapper.Object,
                mockExcelWorkbookValidator.Object,
                mockPublishItemUpdatesCommandHandler.Object ,
                mockAddUpdateItemManager.Object
               );
            testExcelData = File.ReadAllBytes(@".\TestData\IconExport_SingleItem.xlsx");
            testExcelFile = new ExcelPackage(new MemoryStream(testExcelData));
        }

        [TestCleanup]
        public void Cleanup()
        {
            testExcelFile.Dispose();
        }

        [TestMethod]
        public void BulkUploadManager_SetActiveUpload_ShouldSetActiveUploaAndLoadExcelData()
        {
            mockGetFileContentQueryHandler.Setup(m => m.Search(It.IsAny<GetFileContentParameters>()))
                .Returns(new GetFileContentResults { Data = testExcelData });

            mockBulUploadManager.Setup(m => m.GetExcelData());

            var bulkUploadInformation = new BulkItemUploadInformation
            {
                BulkItemUploadId = 1,
                FileModeType = Enums.FileModeTypeEnum.CreateNew,
                FileName = "test.xlsx",
                FileUploadTime = DateTime.Now,
                StatusId = 0,
                UploadedBy = "TestUser"
            };
            mockBulUploadManager.Object.SetActiveUpload(bulkUploadInformation);

            mockBulUploadManager.Verify(v => v.GetExcelData(), Times.Once);

        }

        [TestMethod]
        public void BulkUPloadManger_GetAttributeData_ShouldSetAttributeData()
        {
            mockGetAttributeQueryHandler
                .Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>
                {
                    new AttributeModel() {AttributeName = "test"}
                });

            mockBulUploadManager.Object.GetAttributeData();

            Assert.IsNotNull(mockBulUploadManager.Object.attributeModels, "Attribute Models not loaded.");
            Assert.IsTrue(mockBulUploadManager.Object.attributeModels.Count == 1, "No Attribute Models Loaded.");
        }
    }
}