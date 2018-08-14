using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.Controllers;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Models;
using MammothWebApi.Service.Services;
using MammothWebApi.Tests.Helpers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web.Http.Results;

namespace MammothWebApi.Tests.Controllers
{
    [TestClass]
    public class PriceControllerTests
    {
        private PriceController controller;
        private Mock<IUpdateService<AddUpdatePrice>> mockAddUpdatePriceService;
        private Mock<IUpdateService<DeletePrice>> mockDeletePriceService;
        private Mock<IUpdateService<CancelAllSales>> mockCancelAllSalesService;
        private Mock<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>> mockGetAllBusinessUnitsQueryHandler;
        private Mock<IQueryService<GetItemStorePriceAttributes, IEnumerable<ItemStorePriceModel>>> mockGetItemStorePriceService;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void InitializeTests()
        {
            this.mockLogger = new Mock<ILogger>();
            this.mockAddUpdatePriceService = new Mock<IUpdateService<AddUpdatePrice>>();
            this.mockDeletePriceService = new Mock<IUpdateService<DeletePrice>>();
            this.mockCancelAllSalesService = new Mock<IUpdateService<CancelAllSales>>();

            this.mockGetAllBusinessUnitsQueryHandler = new Mock<IQueryHandler<GetAllBusinessUnitsQuery, List<int>>>();
            this.mockGetItemStorePriceService = new Mock<IQueryService<GetItemStorePriceAttributes, IEnumerable<ItemStorePriceModel>>>();
            this.controller = new PriceController(
                this.mockAddUpdatePriceService.Object,
                this.mockDeletePriceService.Object,
                this.mockCancelAllSalesService.Object,
                this.mockGetAllBusinessUnitsQueryHandler.Object,
                this.mockGetItemStorePriceService.Object,
                this.mockLogger.Object);
        }

        [TestMethod]
        public void PriceControllerAddOrUpdatePrices_PricesListIsNull_ReturnsBadRequest()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = null;
            var expectedMessage = "There were no prices submitted or the format of the data could not be read.";

            // When
            var result = this.controller.AddOrUpdatePrices(prices) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
            Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void PriceControllerAddOrUpdatePrices_PricesListIsNull_LoggerWarnCalled()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = null;

            // When
            var result = this.controller.AddOrUpdatePrices(prices) as BadRequestErrorMessageResult;

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "Did not receive any prices to add or update.")),
                "Logger Warn was not called properly.");
        }

        [TestMethod]
        public void PriceControllerAddOrUpdatePrices_PricesListCountIsZero_ReturnsBadRequest()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = new List<MammothWebApi.Models.PriceModel>();
            var expectedMessage = "There were no prices submitted or the format of the data could not be read.";

            // When
            var result = this.controller.AddOrUpdatePrices(prices) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
            Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void PriceControllerAddOrUpdatePrices_PricesListCountIsZero_LoggerWarnedCalled()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = new List<MammothWebApi.Models.PriceModel>();

            // When
            var result = this.controller.AddOrUpdatePrices(prices) as BadRequestErrorMessageResult;

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "Did not receive any prices to add or update.")),
                "Logger Warn was not called properly.");
        }

        [TestMethod]
        public void PriceControllerAddOrUpdatePrices_ValidPriceModelList_ReturnsCreatedResponse()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = BuildPriceModel(numberOfItems: 3);
            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>()))
                .Returns(new List<int>(prices.Select(p => p.BusinessUnitId)));

            // When
            var result = this.controller.AddOrUpdatePrices(prices) as OkResult;

            // Then
            Assert.IsNotNull(result, "An Ok (Http 200) response was not returned as expected.");
        }

        [TestMethod]
        public void PriceControllerAddOrUpdatePrices_NonSqlExceptionDuringService_ReturnsInternalServerErrorWithExceptionDetails()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = BuildPriceModel(numberOfItems: 3);
            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>())).Returns(new List<int>());
            InvalidOperationException invalidOperationException = new InvalidOperationException("Test Invalid Operation Exception", new Exception("Test Inner Exception"));
            this.mockAddUpdatePriceService.Setup(s => s.Handle(It.IsAny<AddUpdatePrice>())).Throws(invalidOperationException);

            // When
            var result = this.controller.AddOrUpdatePrices(prices) as ExceptionResult;

            // Then
            Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
            Assert.AreEqual(invalidOperationException.Message, result.Exception.Message);
            Assert.AreEqual(invalidOperationException.InnerException, result.Exception.InnerException);

        }

        [TestMethod]
        public void PriceControllerDeletePrices_NonSqlExceptionDuringService_ReturnsInternalServerErrorWithExceptionDetails()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = BuildPriceModel(numberOfItems: 3);
            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>())).Returns(new List<int>());
            InvalidOperationException invalidOperationException = new InvalidOperationException("Test Invalid Operation Exception", new Exception("Test Inner Exception"));
            this.mockDeletePriceService.Setup(s => s.Handle(It.IsAny<DeletePrice>())).Throws(invalidOperationException);

            // When
            var result = this.controller.DeletePrices(prices) as ExceptionResult;

            // Then
            Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
            Assert.AreEqual(invalidOperationException.Message, result.Exception.Message);
            Assert.AreEqual(invalidOperationException.InnerException, result.Exception.InnerException);
        }

        [TestMethod]
        public void PriceControllerPrices_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = BuildPriceModel(numberOfItems: 3);
            SqlException sqlException = CreateSqlException();
            this.mockDeletePriceService.Setup(s => s.Handle(It.IsAny<DeletePrice>())).Throws(sqlException);

            // When
            var result = this.controller.DeletePrices(prices) as ExceptionResult;

            // Then
            Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
            Assert.AreEqual(sqlException.Message, result.Exception.Message);
            Assert.AreEqual(sqlException.InnerException, result.Exception.InnerException);
        }

        [TestMethod]
        public void PriceControllerAddOrUpdatePrices_SqlExceptionDuringService_ReturnsInternalServerErrorWithSqlExceptionDetails()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = BuildPriceModel(numberOfItems: 3);
            this.mockGetAllBusinessUnitsQueryHandler.Setup(h => h.Search(It.IsAny<GetAllBusinessUnitsQuery>())).Returns(new List<int>());
            SqlException sqlException = CreateSqlException();
            this.mockAddUpdatePriceService.Setup(s => s.Handle(It.IsAny<AddUpdatePrice>())).Throws(sqlException);

            // When
            var result = this.controller.AddOrUpdatePrices(prices) as ExceptionResult;

            // Then
            Assert.IsNotNull(result, "The InternalServerError with Exception response is null.");
            Assert.AreEqual(sqlException.Message, result.Exception.Message);
            Assert.AreEqual(sqlException.InnerException, result.Exception.InnerException);
        }

        [TestMethod]
        public void PriceControllerDeletePrices_ValidPriceModelList_ReturnsOkResponse()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = BuildPriceModel(numberOfItems: 5);

            // When
            var result = this.controller.DeletePrices(prices) as OkResult;

            // THen
            Assert.IsNotNull(result, "An Ok (Http 200) response was not returned as expected.");
        }

        [TestMethod]
        public void PriceControllerDeletePrices_PricesListIsNull_ReturnsBadRequest()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = null;
            var expectedMessage = "There were no prices submitted or the format of the data could not be read.";

            // When
            var result = this.controller.DeletePrices(prices) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
            Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void PriceControllerDeletePrices_PricesListIsNull_LoggerWarnCalled()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = null;

            // When
            var result = this.controller.DeletePrices(prices) as BadRequestErrorMessageResult;

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "Did not receive any prices to rollback (delete).")),
                "Logger Warn was not called properly.");
        }

        [TestMethod]
        public void PriceControllerDeletePrices_PricesListCountIsZero_ReturnsBadRequest()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = new List<MammothWebApi.Models.PriceModel>();
            var expectedMessage = "There were no prices submitted or the format of the data could not be read.";

            // When
            var result = this.controller.DeletePrices(prices) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
            Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void PriceControllerDeletePrices_PricesListCountIsZero_LoggerWarnedCalled()
        {
            // Given
            List<MammothWebApi.Models.PriceModel> prices = new List<MammothWebApi.Models.PriceModel>();

            // When
            var result = this.controller.DeletePrices(prices) as BadRequestErrorMessageResult;

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "Did not receive any prices to rollback (delete).")),
                "Logger Warn was not called properly.");
        }

        [TestMethod]
        public void PriceControllerCancelAllSales_CancelAllSalesModelListIsNull_ReturnsBadRequest()
        {
            // Given
            List<MammothWebApi.Models.CancelAllSalesModel> cancelAllSalesList = null;
            var expectedMessage = "There were no cancel all sales submitted or the format of the data could not be read.";

            // When
            var result = this.controller.CancelAllSales(cancelAllSalesList) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
            Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void PriceControllerCancelAllSales_CancelAllSalesModelListZeroCount_ReturnsBadRequest()
        {
            // Given
            List<MammothWebApi.Models.CancelAllSalesModel> cancelAllSalesList = new List<MammothWebApi.Models.CancelAllSalesModel>();
            var expectedMessage = "There were no cancel all sales submitted or the format of the data could not be read.";

            // When
            var result = this.controller.CancelAllSales(cancelAllSalesList) as BadRequestErrorMessageResult;

            // Then
            Assert.IsNotNull(result, "The request did not return a BadRequestErrorMessageResult.");
            Assert.AreEqual(expectedMessage, result.Message, "The actual message did not match the expected message.");
        }

        [TestMethod]
        public void PriceControllerCancelAllSales_CancelAllSalesModelListZeroCount_LoggerCalled()
        {
            // Given
            List<MammothWebApi.Models.CancelAllSalesModel> cancelAllSalesList = new List<MammothWebApi.Models.CancelAllSalesModel>();

            // When
            var result = this.controller.CancelAllSales(cancelAllSalesList) as BadRequestErrorMessageResult;

            // Then
            this.mockLogger.Verify(l => l.Warn(It.Is<string>(s => s == "Did not receive any cancel all sale data.")),
                "Logger Warn was not called properly.");
        }

        [TestMethod]
        public void PriceControllerCancelAllSales_ValidCancelAllSalesModelList_ReturnsOKResponse()
        {
            // Given
            List<MammothWebApi.Models.CancelAllSalesModel> cancelAllSalesList = BuildCancelAllSalesModel();

            // When
            var result = this.controller.CancelAllSales(cancelAllSalesList) as OkResult;

            // Then
            Assert.IsNotNull(result, "An Ok (Http 200) response was not returned as expected.");
        }

        [TestMethod]
        public void PriceControllerGetPrices_SinglePriceRequestWithInvalidModel_ReturnsBadRequestErrorMessageResult()
        {
            // Given
            var priceRequestModel = new PriceRequestModel()
            {
                BusinessUnitId = 11111,
                ScanCode = "1234567890123456",
                EffectiveDate = DateTime.Now.Date
            };
            //manually add an error to the controller's model state to simulate an invalid model
            this.controller.ModelState.AddModelError("ScanCode", "ScanCode max length 13");

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult),
                "A BadRequestErrorMessageResult response was not returned as expected.");
        }

        [TestMethod]
        public void PriceControllerGetPrices_PriceCollectionRequestWithInvalidModel_ReturnsBadRequestErrorMessageResult()
        {
            // Given
            int numItems = 3;
            var testStoreItems = new List<StoreItem>(numItems);
            for (int i = 1; i < numItems + 1; i++)
            {
                int testBizUnit = 11111 * i;
                string testScanCode = $"777777777{i}";
                testStoreItems.Add(new StoreItem { BusinessUnitId = testBizUnit, ScanCode = testScanCode });
            }
            var priceRequestModel = new PriceCollectionRequestModel()
            {
                StoreItems = testStoreItems,
                IncludeFuturePrices = false
            };
            //manually add an error to the controller's model state to simulate an invalid model
            this.controller.ModelState.AddModelError("StoreItems", "StoreItems is required");

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            Assert.IsInstanceOfType(response, typeof(BadRequestErrorMessageResult),
                "A BadRequestErrorMessageResult response was not returned as expected.");
        }

        [TestMethod]
        public void PriceControllerGetPrices_SinglePriceRequest_ReturnsJsonContentResponse()
        {
            // Given
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelReg(50001, "7777777770", 11111, 5.67M, DateTime.Now.AddDays(-7))
            };
            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var priceRequestModel = new PriceRequestModel()
            {
                BusinessUnitId = 11111,
                ScanCode = "7777777770",
                EffectiveDate = DateTime.Now.Date
            };
            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<ItemStorePriceModel>>),
                "A JsonResult response was not returned as expected.");
        }

        [TestMethod]
        public void PriceControllerGetPrices_SinglePriceRequest_ReturnsContentWithItemStorePriceModel()
        {
            // Given
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelReg(50001, "7777777770", 22222, 4.44M, DateTime.Now.AddDays(-4))
            };
            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var priceRequestModel = new PriceRequestModel()
            {
                BusinessUnitId = 22222,
                ScanCode = "7777777770",
                EffectiveDate = DateTime.Now.Date
            };
            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<ItemStorePriceModel>>));
            var returnedPrices = (response as JsonResult<IEnumerable<ItemStorePriceModel>>).Content.ToList();
            Assert.IsNotNull(returnedPrices);
            Assert.AreEqual(1, returnedPrices.Count);
            Assert.AreEqual(22222, returnedPrices[0].BusinessUnitID);
            Assert.AreEqual("7777777770", returnedPrices[0].ScanCode);
            Assert.AreEqual(50001, returnedPrices[0].ItemId);
            Assert.AreEqual(4.44M, returnedPrices[0].Price);
            Assert.AreEqual(DateTime.Now.AddDays(-4).Date, returnedPrices[0].StartDate);
            Assert.AreEqual(true, returnedPrices[0].Authorized);
        }

        [TestMethod]
        public void PriceControllerGetPrices_SinglePriceRequest_WithPriceTypeREG_CallsServiceWithREGParameter()
        {
            // Given
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelReg(50001, "7777777777", 33333, 0.98M, DateTime.Now.AddDays(-5))
            };
            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var priceRequestModel = new PriceRequestModel()
            {
                BusinessUnitId = 33333,
                ScanCode = "7777777777",
                EffectiveDate = DateTime.Now.Date,
                PriceType = "REG"
            };

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            this.mockGetItemStorePriceService
                .Verify(s => s.Get(It.Is<GetItemStorePriceAttributes>(q => q.PriceType == "REG")), Times.Once);
        }

        [TestMethod]
        public void PriceControllerGetPrices_SinglePriceRequest_WithPriceTypeTPR_CallsServiceWithTPRParameter()
        {
            // Given
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelTpr(50001, "7777777774", 11111, 14.04M, DateTime.Now.AddDays(-2), DateTime.Now.AddDays(4))
            };
            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var priceRequestModel = new PriceRequestModel()
            {
                BusinessUnitId = 11111,
                ScanCode = "7777777774",
                EffectiveDate = DateTime.Now.Date,
                PriceType = "TPR"
            };

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            this.mockGetItemStorePriceService
                .Verify(s => s.Get(It.Is<GetItemStorePriceAttributes>(q => q.PriceType == "TPR")), Times.Once);
        }

        [TestMethod]
        public void PriceControllerGetPrices_PriceCollectionRequest_ReturnsJsonContentResponse()
        {
            // Given
            //int numItems = 3;
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelReg(50001, $"7777777771", 11111, 4.44M, DateTime.Now.AddDays(-2)),
                BuildItemStorePriceModelReg(50002, $"7777777772", 22222, 5.55M, DateTime.Now.AddDays(-3)),
                BuildItemStorePriceModelReg(50003, $"7777777773", 33333, 6.66M, DateTime.Now.AddDays(-1)),
             };
            //var testItemStorePriceModels = new List<ItemStorePriceModel>(numItems);

            //testItemStorePriceModels.Add(
            //    BuildItemStorePriceModelReg(50001, $"7777777771", 11111, 4.44M, DateTime.Now.AddDays(-2)));
            //testItemStorePriceModels.Add(
            //    BuildItemStorePriceModelReg(50002, $"7777777772", 22222, 5.55M, DateTime.Now.AddDays(-3)));
            //testItemStorePriceModels.Add(
            //    BuildItemStorePriceModelReg(50003, $"7777777773", 33333, 6.66M, DateTime.Now.AddDays(-1)));

            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var testStoreItems = ItemStorePriceModelToStoreItem(testItemStorePriceModels);
            var priceRequestModel = new PriceCollectionRequestModel()
            {
                StoreItems = testStoreItems,
                IncludeFuturePrices = false
            };
            //this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
            //    .Returns(testItemStorePriceModels);

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            var returnedPrices = (response as JsonResult<IEnumerable<ItemStorePriceModel>>).Content.ToList();
            Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<ItemStorePriceModel>>),
                "A JsonResult response was not returned as expected.");
        }

        [TestMethod]
        public void PriceControllerGetPrices_PriceCollectionRequest_ReturnsContentWithItemStorePriceModels()
        {
            // Given
            int numItems = 3;
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelReg(50001, $"7777777771", 11111, 4.44M, DateTime.Now.AddDays(-2)),
                BuildItemStorePriceModelReg(50002, $"7777777772", 22222, 5.55M, DateTime.Now.AddDays(-3)),
                BuildItemStorePriceModelTpr(50001, $"7777777771", 11111, 3.33M, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(3)),
             };
            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var testStoreItems = ItemStorePriceModelToStoreItem(testItemStorePriceModels);

            var priceRequestModel = new PriceCollectionRequestModel()
            {
                StoreItems = testStoreItems,
                IncludeFuturePrices = false
            };

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            Assert.IsInstanceOfType(response, typeof(JsonResult<IEnumerable<ItemStorePriceModel>>));
            var returnedPrices = (response as JsonResult<IEnumerable<ItemStorePriceModel>>).Content.ToList();
            Assert.IsNotNull(returnedPrices);
            Assert.AreEqual(numItems, returnedPrices.Count);
            for (int i = 0; i< numItems; i++)
            {
                Assert.AreEqual(testItemStorePriceModels[i].BusinessUnitID, returnedPrices[i].BusinessUnitID);
                Assert.AreEqual(testItemStorePriceModels[i].ScanCode, returnedPrices[i].ScanCode);
                Assert.AreEqual(testItemStorePriceModels[i].ItemId, returnedPrices[i].ItemId);
                Assert.AreEqual(testItemStorePriceModels[i].Price, returnedPrices[i].Price);
                Assert.AreEqual(testItemStorePriceModels[i].StartDate, returnedPrices[i].StartDate);
                Assert.AreEqual(testItemStorePriceModels[i].PriceType, returnedPrices[i].PriceType);
                Assert.AreEqual(testItemStorePriceModels[i].Authorized, returnedPrices[i].Authorized);
            }
        }

        [TestMethod]
        public void PriceControllerGetPrices_PriceCollectionRequest_WithPriceTypeREG_CallsServiceWithREGParameter()
        {
            // Given
            //int numItems = 3;
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelReg(50001, $"7777777771", 11111, 4.44M, DateTime.Now.AddDays(-2)),
                BuildItemStorePriceModelReg(50002, $"7777777772", 22222, 5.55M, DateTime.Now.AddDays(-3)),
                BuildItemStorePriceModelTpr(50001, $"7777777771", 22222, 3.33M, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(3)),
             };
            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var testStoreItems = ItemStorePriceModelToStoreItem(testItemStorePriceModels);
            var priceRequestModel = new PriceCollectionRequestModel()
            {
                StoreItems = testStoreItems,
                IncludeFuturePrices = false,
                PriceType = "REG"
            };

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            this.mockGetItemStorePriceService
                .Verify(s => s.Get(It.Is<GetItemStorePriceAttributes>(q => q.PriceType == "REG")), Times.Once);
        }

        [TestMethod]
        public void PriceControllerGetPrices_PriceCollectionRequest_WithPriceTypeTPR_CallsServiceWithTPRParameter()
        {
            // Given
            var testItemStorePriceModels = new List<ItemStorePriceModel>
            {
                BuildItemStorePriceModelTpr(50001, $"7777777771", 11111, 2.98M, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(3)),
                BuildItemStorePriceModelTpr(50001, $"7777777772", 22222, 2.98M, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(3)),
                BuildItemStorePriceModelTpr(50001, $"7777777773", 33333, 2.98M, DateTime.Now.AddDays(-1), DateTime.Now.AddDays(3))
            };
            this.mockGetItemStorePriceService.Setup(s => s.Get(It.IsAny<GetItemStorePriceAttributes>()))
                .Returns(testItemStorePriceModels);

            var testStoreItems = ItemStorePriceModelToStoreItem(testItemStorePriceModels);
            var priceRequestModel = new PriceCollectionRequestModel()
            {
                StoreItems = testStoreItems,
                IncludeFuturePrices = false,
                PriceType = "TPR"
            };

            // When
            var response = this.controller.GetPrices(priceRequestModel);

            // Then
            this.mockGetItemStorePriceService
                .Verify(s => s.Get(It.Is<GetItemStorePriceAttributes>( q=> q.PriceType == "TPR")), Times.Once);
        }

        private ItemStorePriceModel BuildItemStorePriceModelReg(int itemID, string scanCode, int businessUnit,
            decimal price, DateTime startDate)
        {
            return BuildItemStorePriceModel(itemID, scanCode, businessUnit, price, startDate);
        }

        private ItemStorePriceModel BuildItemStorePriceModelTpr(int itemID, string scanCode, int businessUnit,
            decimal price, DateTime startDate, DateTime endDate)
        {
            return BuildItemStorePriceModel(itemID, scanCode, businessUnit, price, startDate, endDate,
                "TPR", "MSAL");
        }

        private ItemStorePriceModel BuildItemStorePriceModel(int itemID, string scanCode, int businessUnit,
            decimal price, DateTime startDate, DateTime? endDate = null,
            string priceType = "REG", string priceTypeAttributem = "REG")
        {
            return PriceTestData.CreateItemStorePriceModel(itemID, scanCode, businessUnit,
                price, priceType, priceTypeAttributem, startDate, endDate);           
        }

        private List<StoreItem> ItemStorePriceModelToStoreItem(IList<ItemStorePriceModel> storePriceModels)
        {
            var storeItems = new List<StoreItem>(storePriceModels.Count);
            if (storePriceModels.Any())
            {
                return storePriceModels
                    .Select(p=> new StoreItem { BusinessUnitId = p.BusinessUnitID , ScanCode = p.ScanCode })
                    .ToList();
            }
            return storeItems;
        }

        private List<MammothWebApi.Models.CancelAllSalesModel> BuildCancelAllSalesModel()
        {
            List<MammothWebApi.Models.CancelAllSalesModel> cancelAllSalesList = new List<MammothWebApi.Models.CancelAllSalesModel>();
            MammothWebApi.Models.CancelAllSalesModel cancelAllSalesEventModel = new MammothWebApi.Models.CancelAllSalesModel()
            {
                BusinessUnitId = 1,
                Region = "FL",
                ScanCode = "7777777771"
            };

            cancelAllSalesList.Add(cancelAllSalesEventModel);

            return cancelAllSalesList;
        }

        private List<MammothWebApi.Models.PriceModel> BuildPriceModel(int numberOfItems)
        {
            var prices = new List<MammothWebApi.Models.PriceModel>();

            for (int i = 0; i < numberOfItems; i++)
            {
                MammothWebApi.Models.PriceModel model = new MammothWebApi.Models.PriceModel
                {
                    BusinessUnitId = 11111,
                    ScanCode = string.Format("777777777{0}", i.ToString()),
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                    StartDate = new DateTime(2015, 9, 30),
                    EndDate = new DateTime(2015, 10, 5),
                    PriceUom = "EA",
                    Region = "FL",
                    CurrencyCode = "USD"
                };

                prices.Add(model);
            }

            return prices;
        }

        public void BuildPriceJsonFile()
        {
            int rows = 1;
            List<MammothWebApi.Models.PriceModel> prices = new List<MammothWebApi.Models.PriceModel>();

            for (int i = 0; i < rows; i++)
            {
                MammothWebApi.Models.PriceModel price = new MammothWebApi.Models.PriceModel
                {
                    ScanCode = string.Format("994824192{0}", i.ToString()),
                    BusinessUnitId = 10006,
                    Multiple = 1,
                    Price = 4.99M,
                    PriceType = "REG",
                    StartDate = new DateTime(2015, 11, 11),
                    EndDate = null,
                    PriceUom = "EA",
                    CurrencyCode = "USD",
                    Region = "NC"
                };

                prices.Add(price);
            }

            using (var file = new StreamWriter(@"C:\Temp\testPriceJson.txt"))
            {
                string json = JsonConvert.SerializeObject(prices);
                file.WriteLine(json);
            }
        }

        public void BuildSalePriceJsonFile()
        {
            int rows = 5;
            List<MammothWebApi.Models.PriceModel> prices = new List<MammothWebApi.Models.PriceModel>();

            for (int i = 0; i < rows; i++)
            {
                MammothWebApi.Models.PriceModel price = new MammothWebApi.Models.PriceModel
                {
                    ScanCode = string.Format("994824192{0}", i.ToString()),
                    BusinessUnitId = 10130,
                    Multiple = 1,
                    Price = 2.99M,
                    PriceType = "SAL",
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(8),
                    PriceUom = "EA",
                    CurrencyCode = "USD",
                    Region = "FL",
                };

                prices.Add(price);
            }

            using (var file = new StreamWriter(@"C:\Temp\TestSalePriceJson.txt"))
            {
                string json = JsonConvert.SerializeObject(prices);
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
