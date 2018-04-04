using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetItemsByBulkScanCodeSearchQueryTests
    {
        private GetItemsByBulkScanCodeSearchQuery query;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<string> searchedScanCodes;
        private HierarchyClass testBrand;
        private HierarchyClass testMerchandise;
        private HierarchyClass testTax;
        private List<Item> testItems;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            query = new GetItemsByBulkScanCodeSearchQuery(this.context);

            searchedScanCodes = new List<string>
            {
                "4444443333",
                "333333355555",
                "88882222222",
                "abcdefg",
                "998989898"
            };

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void StageTestHierarchies()
        {
            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands);
            testMerchandise = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise);
            testTax = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax);

            context.HierarchyClass.Add(testBrand);
            context.HierarchyClass.Add(testMerchandise);
            context.HierarchyClass.Add(testTax);

            context.SaveChanges();
        }

        private void StageTestItems()
        {
            testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(searchedScanCodes[0])
                    .WithBrandAssociation(testBrand.hierarchyClassID)
                    .WithSubBrickAssociation(testMerchandise.hierarchyClassID)
                    .WithTaxClassAssociation(testTax.hierarchyClassID),

                new TestItemBuilder().WithScanCode(searchedScanCodes[1])
                    .WithBrandAssociation(testBrand.hierarchyClassID)
                    .WithSubBrickAssociation(testMerchandise.hierarchyClassID)
                    .WithTaxClassAssociation(testTax.hierarchyClassID),

                new TestItemBuilder().WithScanCode(searchedScanCodes[2])
                    .WithBrandAssociation(testBrand.hierarchyClassID)
                    .WithSubBrickAssociation(testMerchandise.hierarchyClassID)
                    .WithTaxClassAssociation(testTax.hierarchyClassID)
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetItemsByBulkScanCodeSearch_FiveScanCodesSearchedAndThreeFound_ThreeItemsShouldBeReturned()
        {
            // Given.
            StageTestHierarchies();
            StageTestItems();

            var parameters = new GetItemsByBulkScanCodeSearchParameters
            {
                ScanCodes = searchedScanCodes
            };

            // When.
            var items = query.Search(parameters);

            // Then.
            var foundScanCodes = searchedScanCodes.Intersect(testItems.Select(i => i.ScanCode.Single().scanCode)).ToList();

            Assert.AreEqual(foundScanCodes.Count, items.Count);
        }

        [TestMethod]
        public void GetItemsByBulkScanCodeSearch_ResultsAreReturned_ModelValuesShouldMatchDatabaseValues()
        {
            // Given.
            StageTestHierarchies();
            StageTestItems();

            var parameters = new GetItemsByBulkScanCodeSearchParameters
            {
                ScanCodes = searchedScanCodes
            };

            // When.
            var items = query.Search(parameters);

            // Then.
            var expectedItem = testItems[0];
            var actualItem = items[0];
            
            var expectedItemId = expectedItem.itemID;
            var expectedScanCode = expectedItem.ScanCode.Single().scanCode;
            var expectedBrandHierarchyClass = expectedItem.ItemHierarchyClass.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Brands).HierarchyClass;
            var expectedProductDescription = expectedItem.ItemTrait.Single(it => it.traitID == Traits.ProductDescription).traitValue;
            var expectedPosDescription = expectedItem.ItemTrait.Single(it => it.traitID == Traits.PosDescription).traitValue;
            var expectedPackageUnit = expectedItem.ItemTrait.Single(it => it.traitID == Traits.PackageUnit).traitValue;
            var expectedFoodStampEligible = expectedItem.ItemTrait.Single(it => it.traitID == Traits.FoodStampEligible).traitValue;
            var expectedPosScaleTare = expectedItem.ItemTrait.Single(it => it.traitID == Traits.PosScaleTare).traitValue;
            var expectedRetailSize = expectedItem.ItemTrait.Single(it => it.traitID == Traits.RetailSize).traitValue;
            var expectedRetailUom = expectedItem.ItemTrait.Single(it => it.traitID == Traits.RetailUom).traitValue;
            var expectedDeliverySystem = expectedItem.ItemTrait.Single(it => it.traitID == Traits.DeliverySystem).traitValue;
            var expectedMerchandiseHierarchyClass = expectedItem.ItemHierarchyClass.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Merchandise).HierarchyClass;
            var expectedTaxHierarchyClass = expectedItem.ItemHierarchyClass.Single(ihc => ihc.HierarchyClass.hierarchyID == Hierarchies.Tax).HierarchyClass;
            var expectedValidationStatus = expectedItem.ItemTrait.SingleOrDefault(it => it.traitID == Traits.ValidationDate);
            var expectedDepartmentSale = expectedItem.ItemTrait.SingleOrDefault(it => it.traitID == Traits.DepartmentSale);
            var expectedHiddenItem = expectedItem.ItemTrait.SingleOrDefault(it => it.traitID == Traits.HiddenItem);

            Assert.AreEqual(expectedItemId, actualItem.ItemId);
            Assert.AreEqual(expectedScanCode, actualItem.ScanCode);
            Assert.AreEqual(expectedBrandHierarchyClass.hierarchyClassID, actualItem.BrandHierarchyClassId);
            Assert.AreEqual(expectedBrandHierarchyClass.hierarchyClassName, actualItem.BrandName);
            Assert.AreEqual(expectedProductDescription, actualItem.ProductDescription);
            Assert.AreEqual(expectedPosDescription, actualItem.PosDescription);
            Assert.AreEqual(expectedPackageUnit, actualItem.PackageUnit);
            Assert.AreEqual(expectedFoodStampEligible, actualItem.FoodStampEligible);
            Assert.AreEqual(expectedPosScaleTare, actualItem.PosScaleTare);
            Assert.AreEqual(expectedRetailSize, actualItem.RetailSize);
            Assert.AreEqual(expectedRetailUom, actualItem.RetailUom);
            Assert.AreEqual(expectedDeliverySystem, actualItem.DeliverySystem);
            Assert.AreEqual(expectedMerchandiseHierarchyClass.hierarchyClassID, actualItem.MerchandiseHierarchyClassId);
            Assert.AreEqual(expectedMerchandiseHierarchyClass.hierarchyClassName, actualItem.MerchandiseHierarchyName);
            Assert.AreEqual(expectedTaxHierarchyClass.hierarchyClassID, actualItem.TaxHierarchyClassId);
            Assert.AreEqual(expectedTaxHierarchyClass.hierarchyClassName, actualItem.TaxHierarchyName);
            Assert.AreEqual(expectedHiddenItem, actualItem.HiddenItem);
            Assert.IsNull(expectedValidationStatus);
            Assert.IsNull(expectedDepartmentSale);
        }
    }
}
