using BulkItemUploadProcessor.Common;
using BulkItemUploadProcessor.Common.Models;
using BulkItemUploadProcessor.DataAccess.Commands;
using BulkItemUploadProcessor.DataAccess.Queries;
using BulkItemUploadProcessor.Service.BulkUpload;
using BulkItemUploadProcessor.Service.Interfaces;
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
        private Mock<BulkUploadManager> MockBulUploadManager;
        private Mock<ILogger<BulkUploadManager>> MockLogger;
        private Mock<ICommandHandler<SetStatusCommand>> MockSetStatusCommandHandler;
        private Mock<IQueryHandler<GetFileContentParameters, GetFileContentResults>> MockGetFileContentQueryHandler;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> MockGetAttributeQueryHandler;
        private Mock<IValidationManager> MockValidationManager;
        private Mock<ICommandHandler<ClearErrorsCommand>> MockClearErrorsCommandHandler;
        private Mock<ICommandHandler<SaveErrorsCommand>> MockSaveErrorsCommandHandler;
        private Mock<ICommandHandler<PublishItemUpdatesCommand>> MockPublishCommandHandler;

        private byte[] testExcelData;
        private ExcelPackage testExcelFile;

        [TestInitialize]
        public void Init()
        {
            MockLogger = new Mock<ILogger<BulkUploadManager>>();
            MockSetStatusCommandHandler = new Mock<ICommandHandler<SetStatusCommand>>();
            MockGetFileContentQueryHandler = new Mock<IQueryHandler<GetFileContentParameters, GetFileContentResults>>();
            MockGetAttributeQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            MockValidationManager = new Mock<IValidationManager>();
            MockClearErrorsCommandHandler = new Mock<ICommandHandler<ClearErrorsCommand>>();
            MockSaveErrorsCommandHandler = new Mock<ICommandHandler<SaveErrorsCommand>>();
            MockPublishCommandHandler = new Mock<ICommandHandler<PublishItemUpdatesCommand>>();

            MockBulUploadManager = new Mock<BulkUploadManager>(MockLogger.Object,
                MockSetStatusCommandHandler.Object,
                MockPublishCommandHandler.Object,
                MockClearErrorsCommandHandler.Object,
                MockSaveErrorsCommandHandler.Object,
                MockGetFileContentQueryHandler.Object,
                MockGetAttributeQueryHandler.Object,
                MockValidationManager.Object);
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
            MockGetFileContentQueryHandler.Setup(m => m.Search(It.IsAny<GetFileContentParameters>()))
                .Returns(new GetFileContentResults { Data = testExcelData });

            MockBulUploadManager.Setup(m => m.GetExcelData());

            var bulkUploadInformation = new BulkItemUploadInformation
            {
                BulkItemUploadId = 1,
                FileModeType = Enums.FileModeTypeEnum.CreateNew,
                FileName = "test.xlsx",
                FileUploadTime = DateTime.Now,
                StatusId = 0,
                UploadedBy = "TestUser"
            };
            MockBulUploadManager.Object.SetActiveUpload(bulkUploadInformation);

            MockBulUploadManager.Verify(v => v.GetExcelData(), Times.Once);

        }

        [TestMethod]
        public void BulkUPloadManger_GetAttributeData_ShouldSetAttributeData()
        {
            MockGetAttributeQueryHandler
                .Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>
                {
                    new AttributeModel() {AttributeName = "test"}
                });

            MockBulUploadManager.Object.GetAttributeData();

            Assert.IsNotNull(MockBulUploadManager.Object.attributeModels, "Attribute Models not loaded.");
            Assert.IsTrue(MockBulUploadManager.Object.attributeModels.Count == 1, "No Attribute Models Loaded.");
        }
    }
}