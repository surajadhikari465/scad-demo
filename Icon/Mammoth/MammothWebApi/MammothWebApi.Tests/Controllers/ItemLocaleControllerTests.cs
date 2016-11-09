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

namespace MammothWebApi.Tests.Controllers
{
    [TestClass]
    public class ItemLocaleControllerTests
    {
        private Mock<IService<AddUpdateItemLocale>> mockItemLocaleService;
        private Mock<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>> mockGetAllBusinessUnitsQueryHandler;
        private Mock<ILogger> mockLogger;
        private ItemLocaleController itemLocaleController;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockItemLocaleService = new Mock<IService<AddUpdateItemLocale>>();
            this.mockGetAllBusinessUnitsQueryHandler = new Mock<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>>();
            this.mockLogger = new Mock<ILogger>();
            this.itemLocaleController = new ItemLocaleController(this.mockItemLocaleService.Object, 
                this.mockGetAllBusinessUnitsQueryHandler.Object, 
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
        public void ItemLocaleController_ExceptionsDuringAddUpdate_ReturnsInternalServerError()
        {
            // Given
            List<ItemLocaleModel> itemLocales = new List<ItemLocaleModel>();
            itemLocales.Add(new TestItemLocaleModelBuilder().Build());
            this.mockItemLocaleService.Setup(s => s.Handle(It.IsAny<AddUpdateItemLocale>())).Throws<NullReferenceException>();

            // When
            var response = this.itemLocaleController.AddOrUpdateItemLocale(itemLocales) as InternalServerErrorResult;

            // Then
            Assert.IsNotNull(response, "The InternalServerError response is null.");
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
                    TmDiscount = true
                };

                itemLocaleList.Add(itemLocale);
            }

            using (var file = new StreamWriter(@"C:\Temp\testItemLocalejson.txt"))
            {
                string json = JsonConvert.SerializeObject(itemLocaleList);
                file.WriteLine(json);
            }
        }
    }
}
