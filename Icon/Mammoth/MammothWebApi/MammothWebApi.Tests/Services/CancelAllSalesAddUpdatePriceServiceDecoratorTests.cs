using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mammoth.Common.DataAccess.CommandQuery;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.DataAccess.Models;
using System.Collections.Generic;
using MammothWebApi.Service.Services;
using MammothWebApi.Service.Decorators;
using MammothWebApi.Service.Models;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class CancelAllSalesAddUpdatePriceServiceDecoratorTests
    {
        private Mock<IQueryHandler<GetPricesByScanCodeAndStoreQuery, List<ItemPriceModel>>> mockGetPricesQuery;
        private Mock<IService<AddUpdatePrice>> mockAddUpdatePriceService;
        private CancelAllSalesAddUpdatePriceServiceDecorator cancelAllSalesDecorator;
        private AddUpdatePrice addUpdatePriceData;

        [TestInitialize]
        public void InitializeTest()
        {
            this.mockGetPricesQuery = new Mock<IQueryHandler<GetPricesByScanCodeAndStoreQuery, List<ItemPriceModel>>>();
            this.mockAddUpdatePriceService = new Mock<IService<AddUpdatePrice>>();
            this.cancelAllSalesDecorator = new CancelAllSalesAddUpdatePriceServiceDecorator(this.mockAddUpdatePriceService.Object,
                this.mockGetPricesQuery.Object);

            this.addUpdatePriceData = new AddUpdatePrice();
        }

        [TestMethod]
        public void CancelAllSalesDecorator_NoCancelAllSalesToProcess_NoRowsAddedToServiceData()
        {
            // Given
            int expectedCount = 3;
            this.addUpdatePriceData.Prices = BuildPriceServiceModel(numberOfItems: expectedCount);

            // When
            this.cancelAllSalesDecorator.Handle(this.addUpdatePriceData);

            // Then
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices.Count == expectedCount)), Times.Once);

            for (int i = 0; i < expectedCount; i++)
            {
                this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices[i].ScanCode == this.addUpdatePriceData.Prices[i].ScanCode
                && aup.Prices[i].BusinessUnitId == this.addUpdatePriceData.Prices[i].BusinessUnitId
                && aup.Prices[i].EndDate == this.addUpdatePriceData.Prices[i].EndDate
                && aup.Prices[i].PriceType == this.addUpdatePriceData.Prices[i].PriceType
                && aup.Prices[i].StartDate == this.addUpdatePriceData.Prices[i].StartDate
                && aup.Prices[i].Price == this.addUpdatePriceData.Prices[i].Price
                && aup.Prices[i].Multiple == this.addUpdatePriceData.Prices[i].Multiple
                && aup.Prices[i].PriceUom == this.addUpdatePriceData.Prices[i].PriceUom
                && aup.Prices[i].Region == this.addUpdatePriceData.Prices[i].Region
                && aup.Prices[i].CancelAllSales == this.addUpdatePriceData.Prices[i].CancelAllSales
                && aup.Prices[i].CurrencyCode == this.addUpdatePriceData.Prices[i].CurrencyCode)), Times.Once);
            }
        }

        [TestMethod]
        public void CancelAllSalesDecorator_NoCancelAllSalesToProcess_ShouldNotExecuteGetPricesByScanCodeAndStoreQuery()
        {
            // Given
            int expectedCount = 3;
            this.addUpdatePriceData.Prices = BuildPriceServiceModel(numberOfItems: expectedCount);

            // When
            this.cancelAllSalesDecorator.Handle(this.addUpdatePriceData);

            // Then
            this.mockGetPricesQuery.Verify(q => q.Search(It.IsAny<GetPricesByScanCodeAndStoreQuery>()), Times.Never);
        }

        [TestMethod]
        public void CancelAllSalesDecorator_CancelAllSales_WithNoStackedSalesToCancel_ShouldNotUpdateAdditionalPrices()
        {
            int startingPriceRowsCount = 2;
            this.addUpdatePriceData.Prices = BuildPriceServiceModel(numberOfItems: startingPriceRowsCount);
            this.addUpdatePriceData.Prices[0].CancelAllSales = true;

            ItemPriceModel extraSalePrice = new ItemPriceModel
            {
                BusinessUnitId = this.addUpdatePriceData.Prices[0].BusinessUnitId,
                CurrencyCode = this.addUpdatePriceData.Prices[0].CurrencyCode,
                EndDate = this.addUpdatePriceData.Prices[0].EndDate,
                Multiple = this.addUpdatePriceData.Prices[0].Multiple,
                Price = this.addUpdatePriceData.Prices[0].Price,
                PriceType = this.addUpdatePriceData.Prices[0].PriceType,
                PriceUom = this.addUpdatePriceData.Prices[0].PriceUom,
                Region = this.addUpdatePriceData.Prices[0].Region,
                ScanCode = this.addUpdatePriceData.Prices[0].ScanCode,
                StartDate = this.addUpdatePriceData.Prices[0].StartDate
            };

            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByScanCodeAndStoreQuery>()))
                .Returns(new List<ItemPriceModel> { extraSalePrice });

            // When
            this.cancelAllSalesDecorator.Handle(this.addUpdatePriceData);

            // Then
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices.Count == startingPriceRowsCount)), Times.Once);            
        }

        [TestMethod]
        public void CancelAllSalesDecorator_CancelAllSalesRowsToProcessWithActiveSales_ShouldUpdateStackedPrices()
        {
            // Given
            int startingPriceRowsCount = 2;
            this.addUpdatePriceData.Prices = BuildPriceServiceModel(numberOfItems: startingPriceRowsCount);
            this.addUpdatePriceData.Prices[0].CancelAllSales = true;

            ItemPriceModel extraSalePrice = new ItemPriceModel
            {
                BusinessUnitId = this.addUpdatePriceData.Prices[0].BusinessUnitId,
                CurrencyCode = this.addUpdatePriceData.Prices[0].CurrencyCode,
                EndDate = this.addUpdatePriceData.Prices[0].EndDate?.AddDays(20),
                Multiple = this.addUpdatePriceData.Prices[0].Multiple,
                Price = this.addUpdatePriceData.Prices[0].Price - 2,
                PriceType = "EDV",
                PriceUom = this.addUpdatePriceData.Prices[0].PriceUom,
                Region = this.addUpdatePriceData.Prices[0].Region,
                ScanCode = this.addUpdatePriceData.Prices[0].ScanCode,
                StartDate = this.addUpdatePriceData.Prices[0].StartDate.AddDays(-15)
            };
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByScanCodeAndStoreQuery>()))
                .Returns(new List<ItemPriceModel>{ extraSalePrice });

            // When
            this.cancelAllSalesDecorator.Handle(this.addUpdatePriceData);

            // Then
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices.Count == startingPriceRowsCount + 1)), Times.Once);
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices[2].ScanCode == this.addUpdatePriceData.Prices[0].ScanCode
                && aup.Prices[2].BusinessUnitId == this.addUpdatePriceData.Prices[0].BusinessUnitId
                && aup.Prices[2].EndDate == this.addUpdatePriceData.Prices[0].EndDate
                && aup.Prices[2].PriceType == extraSalePrice.PriceType
                && aup.Prices[2].StartDate == extraSalePrice.StartDate
                && aup.Prices[2].Price == extraSalePrice.Price
                && aup.Prices[2].Multiple == extraSalePrice.Multiple
                && aup.Prices[2].PriceUom == extraSalePrice.PriceUom
                && aup.Prices[2].Region == extraSalePrice.Region
                && aup.Prices[2].CancelAllSales
                && aup.Prices[2].CurrencyCode == extraSalePrice.CurrencyCode)), Times.Once);
        }

        [TestMethod]
        public void CancelAllSalesDecorator_CancelAllSalesRowsToProcessWithActiveSalesSamePriceType_ShouldUpdateStackedPrice()
        {
            // Given
            int startingPriceRowsCount = 2;
            this.addUpdatePriceData.Prices = BuildPriceServiceModel(numberOfItems: startingPriceRowsCount);
            this.addUpdatePriceData.Prices[0].CancelAllSales = true;

            ItemPriceModel extraSalePrice = new ItemPriceModel
            {
                BusinessUnitId = this.addUpdatePriceData.Prices[0].BusinessUnitId,
                CurrencyCode = this.addUpdatePriceData.Prices[0].CurrencyCode,
                EndDate = this.addUpdatePriceData.Prices[0].EndDate?.AddDays(20),
                Multiple = this.addUpdatePriceData.Prices[0].Multiple,
                Price = this.addUpdatePriceData.Prices[0].Price - 2,
                PriceType = "SAL",
                PriceUom = this.addUpdatePriceData.Prices[0].PriceUom,
                Region = this.addUpdatePriceData.Prices[0].Region,
                ScanCode = this.addUpdatePriceData.Prices[0].ScanCode,
                StartDate = this.addUpdatePriceData.Prices[0].StartDate.AddDays(-15)
            };
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByScanCodeAndStoreQuery>()))
                .Returns(new List<ItemPriceModel> { extraSalePrice });

            // When
            this.cancelAllSalesDecorator.Handle(this.addUpdatePriceData);

            // Then
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices.Count == startingPriceRowsCount + 1)), Times.Once);
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices[2].ScanCode == this.addUpdatePriceData.Prices[0].ScanCode
                && aup.Prices[2].BusinessUnitId == this.addUpdatePriceData.Prices[0].BusinessUnitId
                && aup.Prices[2].EndDate == this.addUpdatePriceData.Prices[0].EndDate
                && aup.Prices[2].PriceType == extraSalePrice.PriceType
                && aup.Prices[2].StartDate == extraSalePrice.StartDate
                && aup.Prices[2].Price == extraSalePrice.Price
                && aup.Prices[2].Multiple == extraSalePrice.Multiple
                && aup.Prices[2].PriceUom == extraSalePrice.PriceUom
                && aup.Prices[2].Region == extraSalePrice.Region
                && aup.Prices[2].CancelAllSales
                && aup.Prices[2].CurrencyCode == extraSalePrice.CurrencyCode)), Times.Once);
        }

        [TestMethod]
        public void CancelAllSalesDecorator_CancelAllSalesRowsToProcessWithNoOtherExistingActiveSales_ShouldNotUpdateAdditionalStackedPrice()
        {
            // Given
            int startingPriceRowsCount = 2;
            this.addUpdatePriceData.Prices = BuildPriceServiceModel(numberOfItems: startingPriceRowsCount);
            this.addUpdatePriceData.Prices[0].CancelAllSales = true;

            ItemPriceModel extraSalePrice = new ItemPriceModel
            {
                BusinessUnitId = this.addUpdatePriceData.Prices[0].BusinessUnitId,
                CurrencyCode = this.addUpdatePriceData.Prices[0].CurrencyCode,
                EndDate = this.addUpdatePriceData.Prices[0].EndDate?.AddDays(-60),
                Multiple = this.addUpdatePriceData.Prices[0].Multiple,
                Price = this.addUpdatePriceData.Prices[0].Price - 2,
                PriceType = "ISS",
                PriceUom = this.addUpdatePriceData.Prices[0].PriceUom,
                Region = this.addUpdatePriceData.Prices[0].Region,
                ScanCode = this.addUpdatePriceData.Prices[0].ScanCode,
                StartDate = this.addUpdatePriceData.Prices[0].StartDate.AddDays(-75)
            };
            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByScanCodeAndStoreQuery>()))
                .Returns(new List<ItemPriceModel> { extraSalePrice });

            // When
            this.cancelAllSalesDecorator.Handle(this.addUpdatePriceData);

            // Then
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices.Count == startingPriceRowsCount)), Times.Once);
        }

        [TestMethod]
        public void CancelAllSalesDecorator_CancelAllSales_ShouldUpdateOnlyScanCodeStorePricesSpecified()
        {
            // Given
            // cancel all sales for: 
            // Store 11111, ScanCode 777777777770
            // Store 11112, ScanCode 777777777771
            // Database has stacked prices:
            // Store 11111, ScanCode 777777777770 yes
            // Store 11111, ScanCode 777777777771 no
            // Store 11112, ScanCode 777777777771 yes
            // Store 11112, ScanCode 777777777770 no

            this.addUpdatePriceData.Prices = new List<PriceServiceModel>
            {
                new PriceServiceModel
                {
                    BusinessUnitId = 11111,
                    ScanCode = "7777777770",
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    PriceUom = "EA",
                    Region = "NC",
                    CurrencyCode = "USD",
                    CancelAllSales = true
                },
                new PriceServiceModel
                {
                    BusinessUnitId = 11112,
                    ScanCode = "7777777771",
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    PriceUom = "EA",
                    Region = "NC",
                    CurrencyCode = "USD",
                    CancelAllSales = true
                }
            };

            var stackedPrices = new List<ItemPriceModel>
            {
                new ItemPriceModel
                {
                    BusinessUnitId = 11111,
                    ScanCode = "7777777770",
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                    StartDate = DateTime.Today.AddDays(-20),
                    EndDate = DateTime.Today.AddDays(20),
                    PriceUom = "EA",
                    Region = "NC",
                    CurrencyCode = "USD",
                },
                new ItemPriceModel
                {
                    BusinessUnitId = 11112,
                    ScanCode = "7777777771",
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                   StartDate = DateTime.Today.AddDays(-20),
                    EndDate = DateTime.Today.AddDays(20),
                    PriceUom = "EA",
                    Region = "NC",
                    CurrencyCode = "USD",
                },
                new ItemPriceModel
                {
                    BusinessUnitId = 11111,
                    ScanCode = "7777777771",
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                    StartDate = DateTime.Today.AddDays(-20),
                    EndDate = DateTime.Today.AddDays(20),
                    PriceUom = "EA",
                    Region = "NC",
                    CurrencyCode = "USD",
                },
                new ItemPriceModel
                {
                    BusinessUnitId = 11112,
                    ScanCode = "7777777770",
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                    StartDate = DateTime.Today.AddDays(-20),
                    EndDate = DateTime.Today.AddDays(20),
                    PriceUom = "EA",
                    Region = "NC",
                    CurrencyCode = "USD",
                },
                
            };

            this.mockGetPricesQuery.Setup(q => q.Search(It.IsAny<GetPricesByScanCodeAndStoreQuery>()))
                .Returns(stackedPrices);

            // When
            this.cancelAllSalesDecorator.Handle(this.addUpdatePriceData);

            // Then
            this.mockAddUpdatePriceService.Verify(ps => ps.Handle(It.Is<AddUpdatePrice>(aup =>
                aup.Prices.Count == 4)), Times.Once);
        }

        private List<PriceServiceModel> BuildPriceServiceModel(int numberOfItems)
        {
            var prices = new List<PriceServiceModel>();

            for (int i = 0; i < numberOfItems; i++)
            {
                PriceServiceModel model = new PriceServiceModel
                {
                    BusinessUnitId = 11111,
                    ScanCode = String.Format("777777777{0}", i.ToString()),
                    Price = 1.99M,
                    PriceType = "SAL",
                    Multiple = 1,
                    StartDate = DateTime.Today,
                    EndDate = DateTime.Today.AddDays(1),
                    PriceUom = "EA",
                    Region = "NC",
                    CurrencyCode = "USD",
                    CancelAllSales = false
                };

                prices.Add(model);
            }

            return prices;
        }
    }
}
