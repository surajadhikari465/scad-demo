using Icon.Framework;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass]
    public class GetIrmaItemsQueryTests
    {
        private IconContext context;
        private DbContextTransaction transaction;
        private List<IRMAItem> irmaItems;
        private GetIrmaItemsQuery query;
        private int currentCountMW;
        private int currentCountSW;
        private string testTaxName;
        private int testTaxClassId;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            this.currentCountMW = this.context.IRMAItem.Where(i => i.regioncode == "MW").Count();
            this.currentCountSW = this.context.IRMAItem.Where(i => i.regioncode == "SW").Count();

            transaction = context.Database.BeginTransaction();
            HierarchyClass taxClass = context.HierarchyClass.First(hc => hc.hierarchyID == Hierarchies.Tax && hc.HierarchyClassTrait.Any(hct => hct.traitID == Traits.TaxRomance));
            testTaxName = taxClass.HierarchyClassTrait.First(hct => hct.traitID == Traits.TaxRomance).traitValue;
            testTaxClassId = taxClass.hierarchyClassID;
            
            irmaItems = new List<IRMAItem>
            {
                new IRMAItem
                {
                    identifier = "99889988777",
                    defaultIdentifier = true,
                    regioncode = "MW",
                    brandName = "IRMA Test Brand",
                    itemDescription = "IRMA Test Item Description 1",
                    posDescription = "IRMA Test Pos Description 1",
                    packageUnit = 3,
                    foodStamp = true,
                    posScaleTare = 0,
                    departmentSale = true,
                    insertDate = DateTime.Now,
                    retailSize = 5,
                    retailUom = "EACH",
                    DeliverySystem = "CAP",
                    taxClassID = taxClass.hierarchyClassID
                },
                new IRMAItem
                {
                    identifier = "99889988778",
                    defaultIdentifier = true,
                    regioncode = "MW",
                    brandName = "IRMA Test Brand",
                    itemDescription = "IRMA Test Item Description 2",
                    posDescription = "IRMA Test Pos Description 2",
                    packageUnit = 3,
                    foodStamp = true,
                    posScaleTare = 0,
                    departmentSale = true,
                    insertDate = DateTime.Now,
                    retailSize = 5,
                    retailUom = "EACH",
                    DeliverySystem = "CAP"
                },
                new IRMAItem
                {
                    identifier = "99889988779",
                    defaultIdentifier = true,
                    regioncode = "MW",
                    brandName = "IRMA Testing Brand",
                    itemDescription = "IRMA Test Item Description 3",
                    posDescription = "IRMA Test Pos Description 3",
                    packageUnit = 1,
                    foodStamp = true,
                    posScaleTare = 0,
                    departmentSale = true,
                    insertDate = DateTime.Now,
                    retailSize = 5,
                    retailUom = "EACH",
                    DeliverySystem = "CAP"
                },
                new IRMAItem
                {
                    identifier = "99889988760",
                    defaultIdentifier = true,
                    regioncode = "MW",
                    brandName = "IRMA Testing Brand",
                    itemDescription = "IRMA Test Item Description 4",
                    posDescription = "IRMA Test Pos Description 4",
                    packageUnit = 1,
                    foodStamp = true,
                    posScaleTare = 0,
                    departmentSale = true,
                    insertDate = DateTime.Now,
                    retailSize = 5,
                    retailUom = "EACH",
                    DeliverySystem = "CAP",
                    taxClassID = 0
                },
            };

            context.IRMAItem.AddRange(irmaItems);
            context.SaveChanges();

            query = new GetIrmaItemsQuery(this.context);
        }
        
        [TestCleanup]
        public void DeleteTestData()
        {
                transaction.Rollback();
            }

        [TestMethod]
        public void GetIrmaItemsQuery_PartialScanCode_ReturnsItemsStartingWithPartialScanCode()
        {
            // Given
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                Identifier = "9988998877"
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = 3;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetIrmaItemsQuery_PartialScanCode_ReturnsItemsContainsPartialScanCode()
        {
            // Given
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                Identifier = "8899887",
                PartialScanCode = true
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = 4;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }


        [TestMethod]
        public void GetIrmaItemsQuery_MatchingRegionCode_ShouldReturnItemsWithMatchingRegionCode()
        {
            // Given
            string region = "MW";
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                RegionCode = region
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = currentCountMW + irmaItems.Count;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetIrmaItemsQuery_NonMatchingRegionCode_ShouldReturnNoItems()
        {
            // Given
            string region = "SW";
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                RegionCode = region
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = currentCountSW;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetIrmaItemsQuery_BrandName_ReturnsItemsWithBrandNameThatContainsSearchBrandName()
        {
            // Given
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                Brand = "IRMA Test Br"
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetIrmaItemsQuery_PartialProductDescription_ReturnsItemsContainingProductDescription()
        {
            // Given
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                ItemDescription = "IRMA Test Item Des",
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = 4;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetIrmaItemsQuery_ScanCodeBrandDescriptionParametersSupplied_FiltersResultsBasedOnParameters()
        {
            // Given
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                Identifier = "9988998877",
                Brand = "IRMA Testing Brand",
                ItemDescription = "IRMA Test Item Descri",
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetIrmaItemsQuery_UndefinedTax_ReturnsItemsWithContainsInvalidTaxClass()
        {
            // Given
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                Identifier = "99",
                TaxRomanceName = "Undefined"
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetIrmaItemsQuery_TaxClass_ReturnsAllItemsWithSearchTaxClass()
        {
            // Given
            GetIrmaItemsParameters parameters = new GetIrmaItemsParameters
            {
                TaxRomanceName = testTaxName
            };

            // When
            var items = query.Search(parameters);

            // Then
            int expectedCount = context.IRMAItem.Count(ii => ii.taxClassID == testTaxClassId);
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
            Assert.IsFalse(items.Any(ii => ii.taxClassID != testTaxClassId));
        }
    }
}

