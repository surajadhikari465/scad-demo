using Mammoth.Common.DataAccess;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using MammothWebApi.DataAccess.Commands;
using MammothWebApi.DataAccess.Models;
using MammothWebApi.DataAccess.Queries;
using MammothWebApi.Service.Models;
using MammothWebApi.Service.Services;
using MammothWebApi.Tests.Helpers;
using MammothWebApi.Tests.ModelBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace MammothWebApi.Tests.Services
{
    [TestClass]
    public class GetItemStorePriceAttributesServiceTests
    {
        private Mock<IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>> mockGetItemPriceQueryHandler;
        private Mock<IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>> mockGetLocalesQueryHandler;
        private GetItemStorePriceAttributesService itemStorePriceQueryService;
        private List<Locales> fakeLocaleData;
        private List<ItemStorePriceModel> fakeItemStorePriceData;

        private Locales CreateTestLocale(string region, int localeID, int businessUnit, string storeName,
            string storeAbbrev,DateTime addedDate, DateTime? modifiedDate = null)
        {
            var testLocale = new Locales
            {
                Region = region,
                LocaleID = localeID,
                BusinessUnitID = businessUnit,
                StoreName = storeName,
                StoreAbbrev = storeAbbrev,
                AddedDate = addedDate,
                ModifiedDate = modifiedDate
            };
            return testLocale;
        }

        [TestInitialize]
        public void Initialize()
        {
            this.mockGetItemPriceQueryHandler = new Mock<IQueryHandler<GetItemPriceAttributesByStoreAndScanCodeQuery, IEnumerable<ItemStorePriceModel>>>();
            this.mockGetLocalesQueryHandler = new Mock<IQueryHandler<GetLocalesByBusinessUnitsQuery, IEnumerable<Locales>>>();
    
            this.itemStorePriceQueryService = new GetItemStorePriceAttributesService(
                this.mockGetItemPriceQueryHandler.Object,
                this.mockGetLocalesQueryHandler.Object);

            this.fakeLocaleData = new List<Locales>
            {
                CreateTestLocale("MA", 10001, 11111, "TestStore1", "TSA", new DateTime(2015, 4, 24)),
                CreateTestLocale("MA", 20002, 22222, "TestStore2", "TSB", new DateTime(2009, 9, 26)),
                CreateTestLocale("SW", 30003, 33333, "TestStore3", "TSC", new DateTime(2013, 8, 8)),
                CreateTestLocale("SW", 40004, 44444, "TestStore4", "TSD", new DateTime(2016, 8, 7)),
                CreateTestLocale("SW", 40004, 55555, "TestStore5", "TSE", new DateTime(2017, 10, 30)),
            };
            this.fakeItemStorePriceData = new List<ItemStorePriceModel>
            {
                PriceTestData.CreateItemStorePriceModel(123450, "7777777771", 11111),
                PriceTestData.CreateItemStorePriceModel(123451, "7777777772", 22222),
                PriceTestData.CreateItemStorePriceModel(123452, "7777777773", 33333),
                PriceTestData.CreateItemStorePriceModel(123453, "7777777774", 44444),
                PriceTestData.CreateItemStorePriceModel(123454, "7777777775", 55555),
            };

            mockGetLocalesQueryHandler.Setup(q => q.Search(It.IsAny<GetLocalesByBusinessUnitsQuery>()))
                .Returns(fakeLocaleData);

            // since the get item price query is called for each region within the service, filter data returned by region
            mockGetItemPriceQueryHandler.Setup(q => q.Search(It.IsAny<GetItemPriceAttributesByStoreAndScanCodeQuery>()))
                .Returns((GetItemPriceAttributesByStoreAndScanCodeQuery query) => {
                    return fakeItemStorePriceData
                        .Where(isp => fakeLocaleData
                            .Where(l => l.Region == query.Region
                                && query.StoreScanCodeCollection.Select(z => z.BusinessUnitID).Contains(l.BusinessUnitID))
                            .Select(x => x.BusinessUnitID)
                            .Contains(isp.BusinessUnitID));
                });
        }

        [TestMethod]
        public void GetItemStorePriceAttributesService_GetForAllTestItems_ShouldReturnAll()
        {
            // Given.
            var testStoreItems = new List<StoreScanCodeServiceModel>()
            {
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777771", BusinessUnitId = 11111 },
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777772", BusinessUnitId = 22222 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777773", BusinessUnitId = 33333 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777774", BusinessUnitId = 44444 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777775", BusinessUnitId = 55555 },
            };
            var getItemStorePriceAttributes = new GetItemStorePriceAttributes
            {
                ItemStores = testStoreItems,
                EffectiveDate = DateTime.Today,
                IncludeFuturePrices = false,
                PriceType = null
            };

            // When.
            var resultingItemStorePriceModels = itemStorePriceQueryService.Get(getItemStorePriceAttributes);

            // Then.
            Assert.IsNotNull(resultingItemStorePriceModels);
            Assert.AreEqual(5, resultingItemStorePriceModels.Count());
        }

        [TestMethod]
        public void GetItemStorePriceAttributesService_GetForASubsetOfTestItems_ShouldReturnExpected()
        {
            // Given.
            var testStoreItems = new List<StoreScanCodeServiceModel>()
            {
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777771", BusinessUnitId = 11111 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777772", BusinessUnitId = 22222 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777773", BusinessUnitId = 33333 }
            };
            var getItemStorePriceAttributes = new GetItemStorePriceAttributes
            {
                ItemStores = testStoreItems,
                EffectiveDate = DateTime.Today,
                IncludeFuturePrices = false,
                PriceType = null
            };

            // When.
            var resultingItemStorePriceModels = itemStorePriceQueryService.Get(getItemStorePriceAttributes);

            // Then.
            Assert.IsNotNull(resultingItemStorePriceModels);
            Assert.AreEqual(3, resultingItemStorePriceModels.Count());
        }

        [TestMethod]
        public void GetItemStorePriceAttributesService_GetForNonExistentItems_ShouldReturnNone()
        {
            // Given.

            var testStoreItems = new List<StoreScanCodeServiceModel>()
            {
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777777", BusinessUnitId = 66666 }
            };
            var getItemStorePriceAttributes = new GetItemStorePriceAttributes
            {
                ItemStores = testStoreItems,
                EffectiveDate = DateTime.Today,
                IncludeFuturePrices = false,
                PriceType = null
            };

            // When.
            var resultingItemStorePriceModels = itemStorePriceQueryService.Get(getItemStorePriceAttributes);

            // Then.
            Assert.IsNotNull(resultingItemStorePriceModels);
            Assert.AreEqual(0, resultingItemStorePriceModels.Count());
        }

        [TestMethod]
        public void GetItemStorePriceAttributesService_GetWithPriceTypeNull_ShouldCallQueryWithPriceType()
        {
            // Given.
            var testStoreItems = new List<StoreScanCodeServiceModel>()
            {
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777771", BusinessUnitId = 11111 },
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777772", BusinessUnitId = 22222 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777773", BusinessUnitId = 33333 },
            };
            var getItemStorePriceAttributes = new GetItemStorePriceAttributes
            {
                ItemStores = testStoreItems,
                EffectiveDate = DateTime.Today,
                IncludeFuturePrices = false,
                PriceType = null
            };

            // When.
            var resultingItemStorePriceModels = itemStorePriceQueryService.Get(getItemStorePriceAttributes);

            // Then.
            // query should be called once per each region in the service parameter data, and should pass along the specified price type
            this.mockGetItemPriceQueryHandler
                .Verify(s => s.Search(It.Is<GetItemPriceAttributesByStoreAndScanCodeQuery>(a=> a.PriceType == null)),
                    Times.Exactly(2));
        }

        [TestMethod]
        public void GetItemStorePriceAttributesService_GetWithPriceTypeREG_ShouldCallQueryWithPriceType()
        {
            // Given.
            var testStoreItems = new List<StoreScanCodeServiceModel>()
            {
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777771", BusinessUnitId = 11111 },
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777772", BusinessUnitId = 22222 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777773", BusinessUnitId = 33333 },
            };
            var getItemStorePriceAttributes = new GetItemStorePriceAttributes
            {
                ItemStores = testStoreItems,
                EffectiveDate = DateTime.Today,
                IncludeFuturePrices = false,
                PriceType = "REG"
            };

            // When.
            var resultingItemStorePriceModels = itemStorePriceQueryService.Get(getItemStorePriceAttributes);

            // Then.
            // query should be called once per each region in the service parameter data, and should pass along the specified price type
            this.mockGetItemPriceQueryHandler
                .Verify(s => s.Search(It.Is<GetItemPriceAttributesByStoreAndScanCodeQuery>(a => a.PriceType == "REG")),
                    Times.Exactly(2));
        }

        [TestMethod]
        public void GetItemStorePriceAttributesService_GetWithPriceTypeTPR_ShouldCallQueryWithPriceType()
        {
            // Given.
            var testStoreItems = new List<StoreScanCodeServiceModel>()
            {
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777771", BusinessUnitId = 11111 },
                new StoreScanCodeServiceModel{Region = "MA", ScanCode = "7777777772", BusinessUnitId = 22222 },
                new StoreScanCodeServiceModel{Region = "SW", ScanCode = "7777777773", BusinessUnitId = 33333 },
            };
            var getItemStorePriceAttributes = new GetItemStorePriceAttributes
            {
                ItemStores = testStoreItems,
                EffectiveDate = DateTime.Today,
                IncludeFuturePrices = false,
                PriceType = "TPR"
            };

            // When.
            var resultingItemStorePriceModels = itemStorePriceQueryService.Get(getItemStorePriceAttributes);

            // Then.
            // query should be called once per each region in the service parameter data, and should pass along the specified price type
            this.mockGetItemPriceQueryHandler
                .Verify(s => s.Search(It.Is<GetItemPriceAttributesByStoreAndScanCodeQuery>(a => a.PriceType == "TPR")),
                    Times.Exactly(2));
        }
    }
}