using Icon.Framework;
using Icon.Logging;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetPluMappingQueryTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private Item item1;
        private Item item2;
        private Item item3;
        private HierarchyClass testBrand;
        private int itemId1;
        private int itemId2;
        private int itemId3;
        private int brandClassId;
        private PLUMap pluMap1;
        private PLUMap pluMap2;
        private List<Item> items;
        private List<ScanCode> scanCodes;
        private List<ItemTrait> itemTraits;
        private List<ItemHierarchyClass> itemHierarchy;
        private GetPluMappingQuery pluQuery;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            items = new List<Item>();
            item1 = new Item { itemTypeID = 1 };
            item2 = new Item { itemTypeID = 1 };
            item3 = new Item { itemTypeID = 1 };
            testBrand = new HierarchyClass { hierarchyID = 2, hierarchyClassName = "Integration Test Brand" };
            context = new IconContext();
            context.Database.Connection.Open();

            transaction = context.Database.BeginTransaction();
            context.Item.Add(item1);
            context.Item.Add(item2);
            context.Item.Add(item3);
            context.HierarchyClass.Add(testBrand);
            context.SaveChanges();
            itemId1 = item1.itemID;
            itemId2 = item2.itemID;
            itemId3 = item3.itemID;
            brandClassId = testBrand.hierarchyClassID;

            int productDescriptionTraitId = context.Trait.Single(t => t.traitCode == TraitCodes.ProductDescription).traitID;

            scanCodes = new List<ScanCode>
            {
                new ScanCode { itemID = itemId1, scanCode = "22999941000", scanCodeTypeID = 2 },
                new ScanCode { itemID = itemId2, scanCode = "22999942000", scanCodeTypeID = 2 },
                new ScanCode { itemID = itemId3, scanCode = "22999943000", scanCodeTypeID = 1 }
            };

            itemTraits = new List<ItemTrait>
            {
                new ItemTrait { itemID = itemId1, traitID = productDescriptionTraitId, traitValue = "IntegrationTestDescription1", localeID = 1 },
                new ItemTrait { itemID = itemId2, traitID = productDescriptionTraitId, traitValue = "TestIntegrationDescription2", localeID = 1 },
                new ItemTrait { itemID = itemId3, traitID = productDescriptionTraitId, traitValue = "TestIntegrationDescription3", localeID = 1 }
            };

            itemHierarchy = new List<ItemHierarchyClass>
            {
                new ItemHierarchyClass { itemID = itemId1, hierarchyClassID = brandClassId },
                new ItemHierarchyClass { itemID = itemId2, hierarchyClassID = brandClassId },
                new ItemHierarchyClass { itemID = itemId3, hierarchyClassID = brandClassId }
            };

            pluMap1 = new PLUMap
            {
                itemID = itemId1,
                flPLU = "29999999100",
                maPLU = "29999999200",
                mwPLU = "29876599100",
                naPLU = "23433399100",
                ncPLU = "24544499100",
                nePLU = "27777799100",
                pnPLU = "28888899100",
                rmPLU = "24242499100",
                soPLU = "21212199100",
                spPLU = "27877799100",
                swPLU = "27777799100",
                ukPLU = "23362499100"
            };

            pluMap2 = new PLUMap
            {
                itemID = itemId2,
                flPLU = "23216599100",
                maPLU = "29562199100",
                mwPLU = "29876599100",
                naPLU = "23433399100",
                ncPLU = "25621399100",
                nePLU = "24544499100",
                pnPLU = "27777799100",
                rmPLU = "24242499100",
                soPLU = "25555699100",
                spPLU = "29999899100",
                swPLU = "27777799100",
                ukPLU = "21111199100"
            };

            context.PLUMap.Add(pluMap1);
            context.PLUMap.Add(pluMap2);
            context.ScanCode.AddRange(scanCodes);
            context.ItemTrait.AddRange(itemTraits);
            context.ItemHierarchyClass.AddRange(itemHierarchy);
            context.SaveChanges();

            pluQuery = new GetPluMappingQuery(this.context);
            mockLogger = new Mock<ILogger>();
        }

        [TestCleanup]
        public void CleanUp()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
                transaction = null;
            }

            context.Dispose();
        }

        [TestMethod]
        public void PluMappingQuerySearch_NationalPlu_ReturnsResultsWithPartialPluSearch()
        {
            // Given
            GetPluMappingParameters parameters = new GetPluMappingParameters
            {
                NationalPlu = "2299994"
            };

            // When
            var pluList = pluQuery.Search(parameters);

            // Then
            var expectedCount = 2;
            var actualCount = pluList.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void PluMappingQuerySearch_PartialPluDescription_ReturnsResultsWherePluDescriptionContains()
        {
            // Given
            GetPluMappingParameters parameters = new GetPluMappingParameters
            {
                PluDescription = "TestIntegrat"
            };

            // When
            var pluList = pluQuery.Search(parameters);

            // Then
            var expectedCount = 1;
            var actualCount = pluList.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void PluMappingQuerySearch_PartialPluDescription_ShouldNotReturnResultsForUpcTypeScanCodes()
        {
            // Given
            GetPluMappingParameters parameters = new GetPluMappingParameters
            {
                PluDescription = "TestIntegrat"
            };

            // When
            var pluList = pluQuery.Search(parameters);

            // Then
            var expectedCount = 1;
            var actualCount = pluList.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void PluMappingQuerySearch_RegionalPlu_ReturnsResultsWhereMappedRegionalPluContains()
        {
            // Given
            GetPluMappingParameters parameters = new GetPluMappingParameters
            {
                RegionalPlu = "29999999100"
            };

            // When
            var pluList = pluQuery.Search(parameters);

            // Then
            var expectedCount = 1;
            var actualCount = pluList.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void PluMappingQuerySearch_RegionalPlu_ReturnsMultipleRowsWhereRegionsShareMappedPlu()
        {
            // Given
            GetPluMappingParameters parameters = new GetPluMappingParameters
            {
                RegionalPlu = "27777799100"
            };

            // When
            var pluList = pluQuery.Search(parameters);

            // Then
            var expectedCount = 2;
            var actualCount = pluList.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void PluMappingQuerySearch_PartialBrandName_ReturnsResultsThatContainBrandName()
        {
            // Given
            GetPluMappingParameters parameters = new GetPluMappingParameters
            {
                BrandName = "Integration Test B"
            };

            // When
            var pluList = pluQuery.Search(parameters);

            // Then
            var expectedCount = 2;
            var actualCount = pluList.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void PluMappingQuerySearch_MultipleSearchParameters_ReturnsFilteredRowsWithAllSearchCriteria()
        {
            // Given
            GetPluMappingParameters parameters = new GetPluMappingParameters
            {
                NationalPlu = "22999941000",
                RegionalPlu = "24544499100",
                BrandName = "Integration Test Brand",
                PluDescription = "IntegrationTestDes"
            };

            // When
            var pluList = pluQuery.Search(parameters);

            // Then
            var expectedCount = 1;
            var actualCount = pluList.Count();

            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}
