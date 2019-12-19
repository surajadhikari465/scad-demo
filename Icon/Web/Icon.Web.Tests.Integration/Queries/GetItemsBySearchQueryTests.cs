using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Tests.Common.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Globalization;
using System.Linq;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetItemsBySearchQueryTests
    {
        private GetItemsBySearchQuery getItemsBySearchQuery;
        private GetItemsBySearchParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;
        private Item item1;
        private Item item2;
        private Item item3;
        private HierarchyClass testBrand;
        private List<int> itemsById;
        private List<ItemTrait> itemTraits;
        private List<ItemHierarchyClass> itemHierarchyClasses;
        private HierarchyClass testTaxClass;
        private string testTaxRomance;
        private HierarchyClass testMerch;
        private HierarchyClass testNationalClass;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            testBrand = new HierarchyClass { hierarchyID = 2, hierarchyClassName = "GetItemsBySearchQuery Integration Test Brand" };
            testMerch = new HierarchyClass { hierarchyID = Hierarchies.Merchandise, hierarchyClassName = "GetItemsBySearchQuery Integration Test Merch" };
            testNationalClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.National,
                hierarchyClassName = "GetItemsBySearchQuery Integration Test National",
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait {traitID = Traits.NationalClassCode, traitValue = "Test National Class" }
                }
            };
            testTaxClass = new HierarchyClass
            {
                hierarchyID = Hierarchies.Tax,
                hierarchyClassName = "GetItemsBySearchQuery-Test Tax",
                HierarchyClassTrait = new List<HierarchyClassTrait>
                {
                    new HierarchyClassTrait { traitID = Traits.TaxRomance, traitValue = "GetItemsBySearchQuery-Test TaxRomance" }
                }
            };
            context.HierarchyClass.Add(testTaxClass);
            context.HierarchyClass.Add(testBrand);
            context.HierarchyClass.Add(testMerch);
            context.HierarchyClass.Add(testNationalClass);
            context.SaveChanges();

            item1 = new Item
            {
                ItemTypeId = 1,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = "11111155549", scanCodeTypeID = 1, localeID = 1 } }
            };
            item2 = new Item
            {
                ItemTypeId = 1,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = "11111155545", scanCodeTypeID = 1, localeID = 1 } }
            };
            item3 = new Item
            {
                ItemTypeId = 1,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = "11111155536", scanCodeTypeID = 1, localeID = 1 } }
            };
            context.Item.Add(item1);
            context.Item.Add(item2);
            context.Item.Add(item3);
            context.SaveChanges();

            itemTraits = new List<ItemTrait>
            {
                new ItemTrait { itemID = item1.ItemId, traitID = Traits.ProductDescription, traitValue = "GetItemsBySearchQuery Product Description1", localeID = 1 },
                new ItemTrait { itemID = item2.ItemId, traitID = Traits.ProductDescription, traitValue = "GetItemsBySearchQuery Product Description2", localeID = 1 },
                new ItemTrait { itemID = item3.ItemId, traitID = Traits.ProductDescription, traitValue = "GetItemsBySearchQuery Product Description3", localeID = 1 },
                new ItemTrait { itemID = item1.ItemId, traitID = Traits.ValidationDate, traitValue = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture), localeID = 1 },
                new ItemTrait { itemID = item2.ItemId, traitID = Traits.ValidationDate, traitValue = DateTime.UtcNow.ToString("yyyy-MM-dd HH:mm:ss.fffffff", CultureInfo.InvariantCulture), localeID = 1 },
                new ItemTrait { itemID = item1.ItemId, traitID = Traits.PosDescription, traitValue = "GetItemsBySearchQuery Pos Description1", localeID = 1 },
                new ItemTrait { itemID = item1.ItemId, traitID = Traits.PackageUnit, traitValue = "9", localeID = 1 },
                new ItemTrait { itemID = item1.ItemId, traitID = Traits.FoodStampEligible, traitValue = "1", localeID = 1 },
                new ItemTrait { itemID = item1.ItemId, traitID = Traits.PosScaleTare, traitValue = "1.4", localeID = 1 },
                new ItemTrait { itemID = item1.ItemId, traitID = Traits.DepartmentSale, traitValue = "1", localeID = 1 },
                new ItemTrait { itemID = item2.ItemId, traitID = Traits.PackageUnit, traitValue = "10", localeID = 1 },
                new ItemTrait { itemID = item2.ItemId, traitID = Traits.FoodStampEligible, traitValue = "1", localeID = 1 },
                new ItemTrait { itemID = item3.ItemId, traitID = Traits.PosScaleTare, traitValue = "1.4", localeID = 1 },
                new ItemTrait { itemID = item3.ItemId, traitID = Traits.FoodStampEligible, traitValue = "0", localeID = 1 }
            };
            itemHierarchyClasses = new List<ItemHierarchyClass>
            {
                new ItemHierarchyClass { itemID = item1.ItemId, hierarchyClassID = testBrand.hierarchyClassID },
                new ItemHierarchyClass { itemID = item2.ItemId, hierarchyClassID = testBrand.hierarchyClassID },
                new ItemHierarchyClass { itemID = item3.ItemId, hierarchyClassID = testBrand.hierarchyClassID },
                new ItemHierarchyClass { itemID = item2.ItemId, hierarchyClassID = testTaxClass.hierarchyClassID },
                new ItemHierarchyClass { itemID = item3.ItemId, hierarchyClassID = testTaxClass.hierarchyClassID },
                new ItemHierarchyClass { itemID = item1.ItemId, hierarchyClassID = testMerch.hierarchyClassID },
                new ItemHierarchyClass { itemID = item1.ItemId, hierarchyClassID = testNationalClass.hierarchyClassID }
            };

            context.ItemTrait.AddRange(itemTraits);
            context.ItemHierarchyClass.AddRange(itemHierarchyClasses);
            context.SaveChanges();

            getItemsBySearchQuery = new GetItemsBySearchQuery(this.context);

            testTaxRomance = testTaxClass.HierarchyClassTrait.First(hct => hct.traitID == Traits.TaxRomance).traitValue;
            itemsById = new List<int> { item1.ItemId, item2.ItemId, item3.ItemId };
        }

        [TestCleanup]
        public void Cleanup()
        {
            if (transaction != null)
            {
                transaction.Rollback();
                transaction.Dispose();
            }

            context.Dispose();
        }

        [TestMethod]
        public void GetItemsBySearchQuery_SearchOnScanCode_ReturnsItemsStartingWithScanCode()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ScanCode = "1111115554"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 2;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PartialScanCodeIsTrue_ReturnsItemsContainingScanCode()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ScanCode = "11111555",
                PartialScanCode = true
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 3;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ExactBrandName_ReturnsItemsContainingExactBrandName()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                BrandName = "GetItemsBySearchQuery Integration Test Brand"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 3;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ExactBrandNameAndNoItemsHaveBrandName_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                BrandName = "NotExistingBrand"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 0;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PartialBrandName_ReturnsItemsContainingPartialBrandName()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                BrandName = "GetItemsBySearchQuery Integration",
                PartialBrandName = true
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 3;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PartialBrandNameAndNoItemsHaveBrandName_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                BrandName = "NotExistingBrand",
                PartialBrandName = true
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 0;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PartialProductDescription_ReturnsItemsContainingProductDescription()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "Query Product Description1"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ProductDescriptionAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "NonExisting"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 0;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PartialPosDescription_ReturnsItemsContainingPosDescription()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PosDescription = "Query Pos Description1"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PartialPosDescriptionAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PosDescription = "NonExisting"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 0;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PackageUnit_ReturnsItemsWithSamePackageUnit()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PackageUnit = "9",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PackageUnitAndNoItemsMatcth_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PackageUnit = "500",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_FoodStampEligibleIsYes_ReturnsItemsWithTrueFoodStampEligible()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                FoodStampEligible = "Yes",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }
        
        [TestMethod]
        public void GetItemsBySearchQuery_FoodStampEligibleIsNo_ReturnsItemsWithFalseFoodStampEligible()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                FoodStampEligible = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PosScaleTare_ReturnsItemsWithPosScaleTare()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PosScaleTare = "1.4",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PosScaleTareAndNoItemsMatcth_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PosScaleTare = "500",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_RetailSize_ReturnsItemsWithSameRetailSize()
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.RetailSize, traitValue = "15", itemID = item1.ItemId, localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                RetailSize = "15",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_RetailSizeAndNoItemsMatcth_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                RetailSize = "500",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_RetailUom_ReturnsItemsWithSameRetailUom()
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.RetailUom, traitValue = UomCodes.SquareFoot, itemID = item1.ItemId, localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                RetailUom = UomCodes.SquareFoot,
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_RetailUomAndNoItemsMatcth_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                RetailSize = "500",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DeliverySystemAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                DeliverySystem = "500",
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_MerchandiseHierarchy_ReturnsItemsWithSameMerchandiseHierarchy()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                MerchandiseHierarchy = testMerch.hierarchyClassName
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_MerchandiseHierarchyAndNoItemsMatcth_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                MerchandiseHierarchy = "NonExisting"
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_TaxRomance_ReturnsItemsContainingTaxRomance()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                TaxRomance = "GetItemsBySearchQuery"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 2;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_TaxRomanceAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                TaxRomance = "NonExisting"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 0;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_NationalClass_ReturnsItemsContainingNationalClass()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                NationalClass = "Integration Test National"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_NationalClassAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                NationalClass = "NonExisting"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 0;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DepartmentSaleIsYes_ReturnsDepartmentSaleItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                DepartmentSale = "Yes"
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }
        
        [TestMethod]
        public void GetItemsBySearchQuery_DepartmentSaleIsNo_ReturnsNonDepartmentSaleItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                DepartmentSale = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_AllStatus_ReturnsBothValidatedAndNonValidatedItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ScanCode = "111111555",
                SearchStatus = SearchStatus.All
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 3;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_LoadedStatus_ReturnsOnlyLoadedItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ScanCode = "111111555",
                SearchStatus = SearchStatus.Loaded
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ValidatedStatus_ReturnsOnlyValidatedItems()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ScanCode = "111111555",
                SearchStatus = SearchStatus.Validated
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 2;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_HiddenItemStatusIsAll_ReturnsVisibleAndHiddenItems()
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.HiddenItem, traitValue = "1", localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.HiddenItem, traitValue = "0", localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                HiddenItemStatus = HiddenStatus.All,
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 3;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_HiddenItemStatusIsHidden_ReturnsHiddenItems()
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.HiddenItem, traitValue = "1", localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.HiddenItem, traitValue = "0", localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                HiddenItemStatus = HiddenStatus.Hidden
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 1;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        
        [TestMethod]
        public void GetItemsBySearchQuery_HiddenItemStatusIsVisible_ReturnsVisibleItems()
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.HiddenItem, traitValue = "1", localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.HiddenItem, traitValue = "0", localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                HiddenItemStatus = HiddenStatus.Visible,
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_CreatedDate_ReturnsItemsWithSameCreatedDate()
        {
            // Given
            var createdDate = "1970-01-01";
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.InsertDate, traitValue = createdDate, localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.InsertDate, traitValue = createdDate, localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                CreatedDate = createdDate
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_CreatedDateAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            var createdDate = "1970-01-01";
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.InsertDate, traitValue = "1971-01-01", localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.InsertDate, traitValue = "1971-01-01", localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                CreatedDate = createdDate
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_LastModifiedUser_ReturnsItemsThatContainLastModifiedUser()
        {
            // Given
            var user = @"Namespace\TestUser";
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedUser, traitValue = user, localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedUser, traitValue = user, localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                LastModifiedUser = "TestUser"
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_LastModifiedUserAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedUser, traitValue = "TestUser", localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedUser, traitValue = "TestUser", localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                LastModifiedUser = "NotExist"
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ModifiedDate_ReturnsItemsThatContainsModifiedDate()
        {
            // Given
            var modifiedDate = "1970-01-01";
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedDate, traitValue = modifiedDate, localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedDate, traitValue = modifiedDate, localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                LastModifiedDate = modifiedDate
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ModifiedDateAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            var modifiedDate = "1970-01-01";
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedDate, traitValue = "1971-01-01", localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.ModifiedDate, traitValue = "1971-01-01", localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                LastModifiedDate = modifiedDate
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_FoodStampsEligibleAndProdDescription_ReturnsReturnsResultsBasedOnParameters()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                FoodStampEligible = "Yes",
                ProductDescription = "GetItemsBySearchQuery Product Description"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 2;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_FoodStampsNotEligibleAndProdDescription_ReturnsReturnsResultsBasedOnParameters()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                FoodStampEligible = "No",
                ProductDescription = "GetItemsBySearchQuery Product Description"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PosScaleTareAndProdDescription_ReturnsReturnsResultsBasedOnParameterse()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PosScaleTare = "1.4",
                ProductDescription = "GetItemsBySearchQuery Product Description"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 2;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PackageUnitAndProdDescription_ReturnsResultsBasedOnParameters()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                PackageUnit = "9",
                ProductDescription = "GetItemsBySearchQuery Product Description"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DepartmentSaleAndProdDescription_ReturnsReturnsResultsBasedOnParameters()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                DepartmentSale = "Yes",
                ProductDescription = "GetItemsBySearchQuery Product Description"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_NotDepartmentSaleWithProductDescription_ReturnsReturnsResultsBasedOnParameters()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                DepartmentSale = "No",
                ProductDescription = "GetItemsBySearchQuery Product Description"
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 2;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ScanCodeBrandDescriptionParametersSupplied_FiltersResultsBasedOnParameters()
        {
            // Given
            parameters = new GetItemsBySearchParameters
            {
                ScanCode = "111111555",
                BrandName = "GetItemsBySearchQuery Integration",
                ProductDescription = "Query Product Description1",
                PartialBrandName = true
            };

            // When
            var result = getItemsBySearchQuery.Search(parameters);

            // Then
            int expectedCount = 1;
            int actualCount = result.Items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_AirChilledIsYes_ReturnsItemsWithAirChilled()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAirChilled(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAirChilled(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAirChilled(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                AirChilled = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_AirChilledIsNo_ReturnsItemsWithoutAirChilled()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAirChilled(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAirChilled(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAirChilled(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                AirChilled = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_AnimalWelfareRating_ReturnsItemsWithSameAnimalWelfareRating()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAnimalWelfareRating("Step5Plus"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAnimalWelfareRating("Step5Plus"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithAnimalWelfareRating("Step5"));
            
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                AnimalWelfareRating = "Step 5+"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_BiodynamicIsYes_ReturnsItemsWithBiodynamic()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithBiodynamic(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithBiodynamic(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithBiodynamic(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Biodynamic = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [Ignore]        
        public void GetItemsBySearchQuery_BiodynamicIsNo_ReturnsItemsWithoutBiodynamic()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithBiodynamic(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithBiodynamic(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithBiodynamic(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Biodynamic = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_CheeseRawIsYes_ReturnsItemsWithCheeseRaw()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseRaw(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseRaw(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseRaw(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                CheeseRaw = "Yes"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_CheeseRawIsNo_ReturnsItemsWithoutCheeseRaw()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseRaw(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseRaw(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseRaw(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                CheeseRaw = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DryAgedIsYes_ReturnsItemsWithDryAged()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithDryAged(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithDryAged(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithDryAged(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                DryAged = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_DryAgedIsNo_ReturnsItemsWithoutDryAged()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithDryAged(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithDryAged(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithDryAged(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                DryAged = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_FreeRangeIsYes_ReturnsItemsWithFreeRange()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithFreeRange(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithFreeRange(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithFreeRange(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                FreeRange = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_FreeRangeIsNo_ReturnsItemsWithoutFreeRange()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithFreeRange(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithFreeRange(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithFreeRange(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                FreeRange = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_GrassFedIsYes_ReturnsItemsWithGrassFed()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGrassFed(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGrassFed(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGrassFed(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                GrassFed = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_GrassFedIsNo_ReturnsItemsWithoutGrassFed()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGrassFed(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGrassFed(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGrassFed(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                GrassFed = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_MadeInHouseIsYes_ReturnsItemsWithMadeInHouse()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMadeInHouse(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMadeInHouse(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMadeInHouse(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                MadeInHouse = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_MadeInHouseIsNo_ReturnsItemsWithoutMadeInHouse()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMadeInHouse(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMadeInHouse(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMadeInHouse(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                MadeInHouse = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_MscIsYes_ReturnsItemsWithMsc()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMsc(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMsc(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMsc(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Msc = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_MscIsNo_ReturnsItemsWithoutMsc()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMsc(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMsc(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithMsc(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Msc = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PastureRaisedIsYes_ReturnsItemsWithPastureRaised()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPastureRaised(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPastureRaised(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPastureRaised(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                PastureRaised = "Yes"
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_PastureRaisedIsNo_ReturnsItemsWithoutPastureRaised()
        {
            // Given.
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPastureRaised(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPastureRaised(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPastureRaised(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                PastureRaised = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            // When.
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then.
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PremiumBodyCareIsYes_ReturnsItemsWithPremiumBodyCare()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPremiumBodyCare(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPremiumBodyCare(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPremiumBodyCare(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                PremiumBodyCare = "Yes",
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_PremiumBodyCareIsNo_ReturnsItemsWithoutPremiumBodyCare()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPremiumBodyCare(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPremiumBodyCare(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithPremiumBodyCare(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                PremiumBodyCare = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_VegetarianIsYes_ReturnsItemsWithVegetarian()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVegetarian(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVegetarian(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVegetarian(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Vegetarian = "Yes"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_VegetarianIsNo_ReturnsItemsWithoutVegetarian()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVegetarian(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVegetarian(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVegetarian(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Vegetarian = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_WholeTradeIsYes_ReturnsItemsWithWholeTrad()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithWholeTrade(true));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithWholeTrade(true));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithWholeTrade(false));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                WholeTrade = "Yes"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]        
        public void GetItemsBySearchQuery_WholeTradeIsNo_ReturnsItemsWithoutWholeTrade()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithWholeTrade(false));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithWholeTrade(false));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithWholeTrade(true));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                WholeTrade = "No",
                BrandName = "GetItemsBySearchQuery Integration Test Brand",
                PageIndex = 0,
                PageSize = 10
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_MilkType_ReturnsItemsWithSameMilkType()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseMilkType("BuffaloMilk"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseMilkType("BuffaloMilk"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithCheeseMilkType("CowMilk"));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                MilkType = "Buffalo Milk"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_EcoScaleRating_ReturnsItemsWithSameEcoScaleRating()
        {

            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithEcoScaleRating("PremiumYellow"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithEcoScaleRating("PremiumYellow"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithEcoScaleRating("UltraPremiumGreen"));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                EcoScaleRating = "Premium/Yellow"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_SeafoodFreshOrFrozen_ReturnsItemsWithSameSeafoodFreshOrFrozen()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithSeafoodFreshOrFrozen("PreviouslyFrozen"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithSeafoodFreshOrFrozen("PreviouslyFrozen"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithSeafoodFreshOrFrozen("Fresh"));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                SeafoodFreshOrFrozen = "Previously Frozen"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_SeafoodCatchType_ReturnsItemsWithSameSeafoodCatchType()
        {
            //Given
            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithSeafoodCatchType("Wild"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithSeafoodCatchType("Wild"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithSeafoodCatchType("FarmRaised"));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                SeafoodCatchType = "Wild"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_GlutenFreeAgency_ReturnsItemsWithSameGlutenFreeAgency()
        {
            //Given
            HierarchyClass hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName("Test Agency")
                .WithGlutenFreeTrait("1");

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGlutenFreeAgencyName("TestGLutenFreeAgency"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGlutenFreeAgencyName("TestGlutenGreeAgrncy1"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithGlutenFreeAgencyName(null));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                GlutenFreeAgency = hierarchyClass.hierarchyClassName
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_KosherAgency_ReturnsItemsWithSameKosherAgency()
        {
            //Given
            HierarchyClass hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName("Test Agency")
                .WithKosherTrait("1");

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithKosherAgencyName("TestKosherAgency"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithKosherAgencyName("TestKosherAgency"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithKosherAgencyName(null));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                KosherAgency = hierarchyClass.hierarchyClassName
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_NonGmoAgency_ReturnsItemsWithSameNonGmoAgency()
        {
            //Given
            HierarchyClass hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName("Test Agency")
                .WithNonGmoTrait("1");

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithNonGmoAgencyName("TestNonGmoAgency"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithNonGmoAgencyName("TestNonGmoAgency"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithNonGmoAgencyName(null));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                NonGmoAgency = hierarchyClass.hierarchyClassName
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_OrganicAgency_ReturnsItemsWithSameOrganicAgency()
        {
            //Given
            HierarchyClass hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName("Test Agency")
                .WithOrganicTrait("1");

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithOrganicAgencyName("TestOrganicAgency"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithOrganicAgencyName("TestOrganicAgency"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithOrganicAgencyName(null));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                OrganicAgency = hierarchyClass.hierarchyClassName
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_VeganAgency_ReturnsItemsWithSameVeganAgency()
        {
            //Given
            HierarchyClass hierarchyClass = new TestHierarchyClassBuilder()
                .WithHierarchyId(Hierarchies.CertificationAgencyManagement)
                .WithHierarchyClassName("Test Agency")
                .WithVeganTrait("1");

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            item1.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVeganAgencyName("TestVeganAgency"));
            item2.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVeganAgencyName("TestVeganAgency"));
            item3.ItemSignAttribute.Add(new TestItemSignAttributeBuilder().WithVeganAgencyName(null));

            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                VeganAgency = hierarchyClass.hierarchyClassName
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(2, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_Notes_ReturnsItemsThatContainNotes()
        {
            // Given
            var notes = @"Notes Notes Notes TestNotes Notes Notes Notes";
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.Notes, traitValue = notes, localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.Notes, traitValue = notes, localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Notes = "TestNotes"
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_NotesAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = Traits.Notes, traitValue = "TestNotes", localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = Traits.Notes, traitValue = "TestNotes", localeID = Locales.WholeFoods });
            context.SaveChanges();

            parameters = new GetItemsBySearchParameters
            {
                Notes = "NotExist"
            };

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DrainedWeightUom_ReturnsItemsThatMatchDrainedWeightUom()
        {
            // Given
            parameters = new GetItemsBySearchParameters { DrainedWeightUom = "ShouldExist" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.DrainedWeightUom, parameters.DrainedWeightUom);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DrainedWeightUomAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { CaseinFree = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.CaseinFree, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_CaseinFree_ReturnsItemsThatMatchCaseinFree()
        {
            // Given
            parameters = new GetItemsBySearchParameters { CaseinFree = "1" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.CaseinFree, parameters.CaseinFree);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_CaseinFreeAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { CaseinFree = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.CaseinFree, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DrainedWeight_ReturnsItemsThatMatchDrainedWeight()
        {
            // Given
            parameters = new GetItemsBySearchParameters { DrainedWeight = "TestDrainedWeight" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.DrainedWeight, parameters.DrainedWeight);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_DrainedWeightAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "TestDrainedWeight";
            parameters = new GetItemsBySearchParameters { DrainedWeight = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.DrainedWeight, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_NutritionRequired_ReturnsItemsThatMatchNutritionRequired()
        {
            // Given
            parameters = new GetItemsBySearchParameters { NutritionRequired = "1" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.NutritionRequired, parameters.NutritionRequired);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_NutritionRequiredAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { NutritionRequired = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.NutritionRequired, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_OrganicPersonalCare_ReturnsItemsThatMatchOrganicPersonalCare()
        {
            // Given
            parameters = new GetItemsBySearchParameters { OrganicPersonalCare = "1" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.OrganicPersonalCare, parameters.OrganicPersonalCare);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_OrganicPersonalCareAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { OrganicPersonalCare = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.OrganicPersonalCare, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_Hemp_ReturnsItemsThatMatchHemp()
        {
            // Given
            parameters = new GetItemsBySearchParameters { Hemp = "1" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.Hemp, parameters.Hemp);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_HempAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { Hemp = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.Hemp, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_FairTradeCertified_ReturnsItemsThatMatchFairTradeCertified()
        {
            // Given
            parameters = new GetItemsBySearchParameters { FairTradeCertified = "1" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.FairTradeCertified, parameters.FairTradeCertified);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_FairTradeCertifiedAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { FairTradeCertified = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.FairTradeCertified, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_AlcoholByVolume_ReturnsItemsThatMatchAlcoholByVolume()
        {
            // Given
            parameters = new GetItemsBySearchParameters { AlcoholByVolume = "1" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.AlcoholByVolume, parameters.AlcoholByVolume);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_AlcoholByVolumeAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { AlcoholByVolume = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.AlcoholByVolume, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_MainProductName_ReturnsItemsThatContainMainProductName()
        {
            // Given
            parameters = new GetItemsBySearchParameters { MainProductName = "Main Product" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.MainProductName, "Test Main Product Name");
        }

        [TestMethod]
        public void GetItemsBySearchQuery_MainProductNameAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { MainProductName = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.MainProductName, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ProductFlavorType_ReturnsItemsThatContainProductFlavorType()
        {
            // Given
            parameters = new GetItemsBySearchParameters { ProductFlavorType = "Product Flavor" };
            GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(Traits.ProductFlavorType, "Test Product Flavor Type");
        }

        [TestMethod]
        public void GetItemsBySearchQuery_ProductFlavorTypeAndNoItemsMatch_ReturnsNoItems()
        {
            // Given
            string traitValue = "1";
            parameters = new GetItemsBySearchParameters { ProductFlavorType = "NotExists" };
            GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(Traits.ProductFlavorType, traitValue);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_SortColumnIsProductDescriptionAndSortOrderIsDesc_ShouldOrderResultsByProductDescription()
        {
            //Given
            var parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "GetItemsBySearchQuery",
                SortColumn = "ProductDescription",
                SortOrder = "desc"
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(3, items.Count);
            var orderedProductDescriptions = new List<string>
            {
                item3.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue,
                item2.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue,
                item1.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue
            }
            .OrderByDescending(s => s)
            .ToList();
            Assert.AreEqual(orderedProductDescriptions[0], items[0].ProductDescription);
            Assert.AreEqual(orderedProductDescriptions[1], items[1].ProductDescription);
            Assert.AreEqual(orderedProductDescriptions[2], items[2].ProductDescription);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_SortColumnAndSortOrderAreNull_ShouldOrderResultsByScanCode()
        {
            //Given
            var parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "GetItemsBySearchQuery",
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(3, items.Count);
            var orderedProductDescriptions = new List<string>
            {
                item3.ScanCode.First().scanCode,
                item2.ScanCode.First().scanCode,
                item1.ScanCode.First().scanCode
            }
            .OrderBy(s => s)
            .ToList();
            Assert.AreEqual(orderedProductDescriptions[0], items[0].ScanCode);
            Assert.AreEqual(orderedProductDescriptions[1], items[1].ScanCode);
            Assert.AreEqual(orderedProductDescriptions[2], items[2].ScanCode);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PageSizeAndPageIndexAreSupplied_ShouldReturnNumberOfItemsEqualToPageSizeAndAtThePageIndex()
        {
            //Given
            var parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "GetItemsBySearchQuery",
                PageIndex = 1,
                PageSize = 1
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(1, items.Count);
            Assert.AreEqual(item2.ItemId, items[0].ItemId);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PageSizeAndPageIndexAreNull_ShouldReturnAllMatchingItems()
        {
            //Given
            var parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "GetItemsBySearchQuery",
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(3, items.Count);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_PagingAndSortingParametersAreSupplied_ShouldPageAndSortItems()
        {
            //Given
            var parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "GetItemsBySearchQuery",
                SortColumn = "ProductDescription",
                SortOrder = "desc",
                PageIndex = 0,
                PageSize = 2
            };

            //When
            var items = getItemsBySearchQuery.Search(parameters).Items;

            //Then
            Assert.AreEqual(2, items.Count);
            var orderedProductDescriptions = new List<string>
            {
                item3.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue,
                item2.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue,
                item1.ItemTrait.First(it => it.traitID == Traits.ProductDescription).traitValue
            }
            .OrderByDescending(s => s)
            .ToList();
            Assert.AreEqual(orderedProductDescriptions[0], items[0].ProductDescription);
            Assert.AreEqual(orderedProductDescriptions[1], items[1].ProductDescription);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_GetCountOnlyIsTrue_ShouldReturnTheCountOfTheTotalNumberOfResults()
        {
            //Given
            var parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "GetItemsBySearchQuery",
                SortColumn = "ProductDescription",
                GetCountOnly = true
            };

            //When
            var result = getItemsBySearchQuery.Search(parameters);

            //Then
            Assert.IsNull(result.Items);
            Assert.AreEqual(3, result.ItemsCount);
        }

        [TestMethod]
        public void GetItemsBySearchQuery_GetCountOnlyIsFalse_ShouldReturnItems()
        {
            //Given
            var parameters = new GetItemsBySearchParameters
            {
                ProductDescription = "GetItemsBySearchQuery",
                SortColumn = "ProductDescription",
                GetCountOnly = false
            };

            //When
            var result = getItemsBySearchQuery.Search(parameters);

            //Then
            Assert.AreEqual(3, result.Items.Count);
            Assert.AreEqual(0, result.ItemsCount);
        }

        public void GetItemsBySearchQuery_TraitsMatchSearchCriteria_ShouldReturnItemsThatMatchSearchCriteria(int traitID, string traitValue)
        {
            // Given
            item1.ItemTrait.Add(new ItemTrait { traitID = traitID, traitValue = traitValue, localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = traitID, traitValue = traitValue, localeID = Locales.WholeFoods });
            context.SaveChanges();

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 2;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }

        public void GetItemsBySearchQuery_TraitsDontMatchSearchCriteria_ShouldReturnNoItems(int traitID, string traitValue)
        {
            item1.ItemTrait.Add(new ItemTrait { traitID = traitID, traitValue = traitValue, localeID = Locales.WholeFoods });
            item2.ItemTrait.Add(new ItemTrait { traitID = traitID, traitValue = traitValue, localeID = Locales.WholeFoods });
            context.SaveChanges();

            // When
            var items = getItemsBySearchQuery.Search(parameters).Items.Where(i => itemsById.Contains(i.ItemId)).ToList();

            // Then
            int expectedCount = 0;
            int actualCount = items.Count;
            Assert.AreEqual(expectedCount, actualCount);
        }
    }
}