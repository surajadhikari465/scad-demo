using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.Controllers;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Models;
using MammothWebApi.Service.Services;
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
