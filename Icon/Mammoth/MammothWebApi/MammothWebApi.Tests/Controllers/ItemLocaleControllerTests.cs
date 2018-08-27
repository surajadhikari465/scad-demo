using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.Controllers;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Models;
using MammothWebApi.Service.Services;
using MammothWebApi.Tests.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Http.Results;
using Dapper;
using System.Data.SqlClient;
using MammothWebApi.DataAccess.Commands;

namespace MammothWebApi.Tests.Controllers
{
    [TestClass]
    public class ItemLocaleControllerTests
    {
        private Mock<IUpdateService<AddUpdateItemLocale>> mockItemLocaleService;
        private Mock<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>> mockGetAllBusinessUnitsQueryHandler;
        private Mock<IUpdateService<DeauthorizeItemLocale>> mockDeauthorizeItemLocaleService;
        private Mock<ILogger> mockLogger;
        private ItemLocaleController itemLocaleController;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockItemLocaleService = new Mock<IUpdateService<AddUpdateItemLocale>>();
            this.mockGetAllBusinessUnitsQueryHandler = new Mock<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>>();
            this.mockDeauthorizeItemLocaleService = new Mock<IUpdateService<DeauthorizeItemLocale>>();
            this.mockLogger = new Mock<ILogger>();
            this.itemLocaleController = new ItemLocaleController(this.mockItemLocaleService.Object, 
                this.mockGetAllBusinessUnitsQueryHandler.Object, 
                this.mockDeauthorizeItemLocaleService.Object,
                this.mockLogger.Object);
        }

        [TestMethod]
        public void ItemLocaleController_NoExceptionsDuringAddUpdate_ReturnsOkResponse()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());
            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>()))
                .Returns(new List<int>(itemLocales.Select(il => il.BusinessUnitId)));

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as OkResult;

            // Then
            Assert.IsNotNull(response, "The OkResult response is null.");
        }

        [TestMethod]
        public void ItemLocaleController_NonSqlExceptionsDuringAddUpdate_ReturnsInternalServerErrorWithExceptionResponse()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());
            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>())).Returns(new List<int>());
            InvalidOperationException invalidOperationException = new InvalidOperationException("Test Invalid Operation Exception", new Exception("Test Inner Exception"));
            this.mockItemLocaleService.Setup(s => s.Handle(It.IsAny<AddUpdateItemLocale>())).Throws(invalidOperationException);

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as ExceptionResult;

            // Then
            Assert.IsNotNull(response, "The InternalServerError with Exception response is null.");
            Assert.AreEqual(invalidOperationException.Message, response.Exception.Message);
            Assert.AreEqual(invalidOperationException.InnerException, response.Exception.InnerException);
        }

        [TestMethod]
        public void ItemLocaleController_SqlExceptionsDuringAddUpdate_ReturnsInternalServerErrorWithExceptionResponse()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());
            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>())).Returns(new List<int>());
            SqlException sqlException = CreateSqlException();
            this.mockItemLocaleService.Setup(s => s.Handle(It.IsAny<AddUpdateItemLocale>())).Throws(sqlException);

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as ExceptionResult;

            // Then
            Assert.IsNotNull(response, "The InternalServerError with Sql Exception response is null.");
            Assert.AreEqual(sqlException.Message, response.Exception.Message);
            Assert.AreEqual(sqlException.InnerException, response.Exception.InnerException);
        }

        [TestMethod]
        public void ItemLocaleController_ItemLocaleListIsNull_ShouldLogWarning()
        {
            // Given
            List<ItemLocaleModel> itemLocales = null;

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales);

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "The object passed is either null or does not contain any rows.")),
                Times.Once);
        }

        [TestMethod]
        public void ItemLocaleController_ItemLocaleListIsNull_ReturnsBadRequestResponse()
        {
            // Given
            List<ItemLocaleModel> itemLocales = null;
            string expectedMessage = "The object sent is either null or does not contain any rows.";

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(response, "The BadRequestErrorMessageResult response is null.");
            Assert.AreEqual(expectedMessage, response.Message, "The BadRequestErrorMessageResult Message did not match the expected error message.");
        }

        [TestMethod]
        public void ItemLocaleController_ItemLocaleListCountIsZero_ShouldLogWarning()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as BadRequestErrorMessageResult;

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "The object passed is either null or does not contain any rows.")),
                Times.Once);
        }

        [TestMethod]
        public void ItemLocaleController_ItemLocaleListCountIsZero_ReturnsBadRequestResponse()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            string expectedMessage = "The object sent is either null or does not contain any rows.";

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(response, "The BadRequestErrorMessageResult response is null.");
            Assert.AreEqual(expectedMessage, response.Message, "The BadRequestErrorMessageResult Message did not match the expected error message.");
        }

        [TestMethod]
        public void ItemLocaleController_ValidItemLocaleData_AddUpdateServiceCalledOneTime()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());

            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>()))
                .Returns(new List<int>(itemLocales.Select(il => il.BusinessUnitId)));

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as OkResult;

            // Then
            this.mockItemLocaleService.Verify(s => s.Handle(It.IsAny<AddUpdateItemLocale>()), Times.Once);
        }

        [TestMethod]
        public void ItemLocaleController_ItemLocaleListIsNullDuringDeauthorizeItemLocale_ShouldLogWarning()
        {
            // Given
            List<ItemLocaleModel> itemLocales = null;

            // When
            var response = this.itemLocaleController.DeauthorizeItemLocale(itemLocales);

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "The object passed is either null or does not contain any rows.")),
                Times.Once);
            this.mockDeauthorizeItemLocaleService.Verify(s => s.Handle(It.IsAny<DeauthorizeItemLocale>()), Times.Never);
        }

        [TestMethod]
        public void ItemLocaleController_ItemLocaleListCountIsZeroDuringDeauthorizeItemLocale_ShouldLogWarning()
        {
            // Given
            List<ItemLocaleModel> itemLocales = null;

            // When
            var response = this.itemLocaleController.DeauthorizeItemLocale(itemLocales);

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "The object passed is either null or does not contain any rows.")),
                Times.Once);
            this.mockDeauthorizeItemLocaleService.Verify(s => s.Handle(It.IsAny<DeauthorizeItemLocale>()), Times.Never);
        }

        [TestMethod]
        public void ItemLocaleController_ItemLocaleListIsValidDuringDeauthorizeItemLocale_ShouldCallService()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());

            // When
            var response = this.itemLocaleController.DeauthorizeItemLocale(itemLocales);

            // Then
            this.mockDeauthorizeItemLocaleService.Verify(s => s.Handle(It.IsAny<DeauthorizeItemLocale>()), Times.Once);
            this.mockItemLocaleService.Verify(s => s.Handle(It.IsAny<AddUpdateItemLocale>()), Times.Once);
        }

        [TestMethod]
        public void ItemLocaleController_SqlExceptionsDuringDeauthorizeItemLocale_ReturnsInternalServerErrorWithExceptionResponse()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());        
            SqlException sqlException = CreateSqlException();
            this.mockDeauthorizeItemLocaleService.Setup(s => s.Handle(It.IsAny<DeauthorizeItemLocale>())).Throws(sqlException);

            // When
            var response = this.itemLocaleController.DeauthorizeItemLocale(itemLocales) as ExceptionResult;

            // Then
            Assert.IsNotNull(response, "The InternalServerError with Sql Exception response is null.");
            Assert.AreEqual(sqlException.Message, response.Exception.Message);
            Assert.AreEqual(sqlException.InnerException, response.Exception.InnerException);
        }

        [TestMethod]
        public void ItemLocaleController_ValidItemLocaleDataIncludingIrmaItemKey_AddUpdateServiceCalledWithExpectedData()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            var expectedItemLocaleModel = new TestItemLocaleModelBuilder().Build();
            var expectedIrmaItemKey = expectedItemLocaleModel.IrmaItemKey;
            var expectedDefaultScanCode = expectedItemLocaleModel.DefaultScanCode;
            itemLocales.Add(expectedItemLocaleModel);

            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>()))
                .Returns(new List<int>(itemLocales.Select(il => il.BusinessUnitId)));

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as OkResult;

            // Then
            this.mockItemLocaleService.Verify(s => s.Handle(It.Is<AddUpdateItemLocale>(il=>
                (expectedIrmaItemKey == il.ItemLocales.First().IrmaItemKey) && (expectedDefaultScanCode == il.ItemLocales.First().DefaultScanCode))
                ), Times.Once);
        }

        //[TestMethod]
        public void BuildTestItemLocaleJsonFile()
        {
            int rows = 1;
            List<ItemLocaleModel> itemLocaleList = new List<ItemLocaleModel>();


            for (int i = 0; i < rows; i++)
            {
                ItemLocaleModel itemLocale = new ItemLocaleModel
                {
                    AgeRestriction = null,
                    Authorized = true,
                    BusinessUnitId = 10006,
                    CaseDiscount = true,
                    ChicagoBaby = null,
                    ColorAdded = null,
                    CountryOfProcessing = null,
                    Discontinued = false,
                    ElectronicShelfTag = null,
                    Exclusive = null,
                    LabelTypeDescription = "LRG",
                    LinkedItem = null,
                    LocalItem = false,
                    Locality = null,
                    MSRP = 3.99M,
                    NumberOfDigitsSentToScale = null,
                    Origin = null,
                    ProductCode = null,
                    Region = "NC",
                    RestrictedHours = false,
                    RetailUnit = "EACH",
                    ScaleExtraText = null,
                    ScanCode = String.Format("994824037{0}", i),
                    SignDescription = String.Format("Test ItemLocale Sign Desc {0}", i),
                    SignRomanceLong = String.Format("Test Long Sign Romance Text {0}", i),
                    SignRomanceShort = String.Format("Test Short Sign Romance Text {0}", i),
                    TagUom = null,
                    TmDiscount = true,
                    DefaultScanCode = false
                };

                itemLocaleList.Add(itemLocale);
            }

            using (var file = new StreamWriter(@"C:\Temp\testItemLocalejson.txt"))
            {
                string json = JsonConvert.SerializeObject(itemLocaleList);
                file.WriteLine(json);
            }
        }

        private SqlException CreateSqlException()
        {
            SqlException exception = null;
            try
            {
                SqlConnection connection = new SqlConnection(@"Data Source=.;Initial Catalog=FAIL;Connection Timeout=1");
                connection.Open();
            }
            catch (SqlException sqlException)
            {
                exception = sqlException;
            }

            return exception;
        }
    }
}
