using System.Linq;
using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Queries;
using Icon.Framework;
using System.Data.Entity;
using Icon.Testing.Builders;
using System.Collections.Generic;

namespace Icon.Web.Tests.Integration.Queries
{
    [TestClass] [Ignore]
    public class GetDefaultTaxClassMismatchesQueryTests
    {
        private GetDefaultTaxClassMismatchesQuery query;
        private GetDefaultTaxClassMismatchesParameters parameters;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClassTrait merchDefaultTaxTrait;
        private HierarchyClass testMerchandiseClass;
        private HierarchyClass testParentMerchandiseClass;
        private HierarchyClass testBrand;
        private HierarchyClass defaultTaxClass;
        private HierarchyClass updatedTaxClass;
        private HierarchyClass overridenTaxClass;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            testMerchandiseClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.SubBrick);
            testParentMerchandiseClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.Gs1Brick);
            testBrand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyLevel(HierarchyLevels.Brand);
            defaultTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);
            updatedTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);
            overridenTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);

            query = new GetDefaultTaxClassMismatchesQuery(context);
            parameters = new GetDefaultTaxClassMismatchesParameters();

            transaction = context.Database.BeginTransaction();

            context.HierarchyClass.Add(testMerchandiseClass);
            context.HierarchyClass.Add(testParentMerchandiseClass);
            context.HierarchyClass.Add(testBrand);
            context.HierarchyClass.Add(defaultTaxClass);
            context.HierarchyClass.Add(updatedTaxClass);
            context.HierarchyClass.Add(overridenTaxClass);
            context.SaveChanges();

            testMerchandiseClass.hierarchyParentClassID = testParentMerchandiseClass.hierarchyClassID;
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void CreateDefaultMapping()
        {
            merchDefaultTaxTrait = new HierarchyClassTrait
            {
                traitID = Traits.MerchDefaultTaxAssociatation,
                traitValue = defaultTaxClass.hierarchyClassID.ToString(),
                hierarchyClassID = testMerchandiseClass.hierarchyClassID
            };

            context.HierarchyClassTrait.Add(merchDefaultTaxTrait);
            context.SaveChanges();
        }

        private void CreateSecondaryMapping(HierarchyClass merchandiseClass, HierarchyClass taxClass)
        {
            var merchDefaultTaxTrait = new HierarchyClassTrait
            {
                traitID = Traits.MerchDefaultTaxAssociatation,
                traitValue = taxClass.hierarchyClassID.ToString(),
                hierarchyClassID = merchandiseClass.hierarchyClassID
            };

            context.HierarchyClassTrait.Add(merchDefaultTaxTrait);
            context.SaveChanges();
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_NoDefaultMappingsExist_NoResultsShouldBeReturned()
        {
            // Given.
            var merchDefaultTaxTraits = context.HierarchyClassTrait.Where(hct => hct.traitID == Traits.MerchDefaultTaxAssociatation).ToList();
            context.HierarchyClassTrait.RemoveRange(merchDefaultTaxTraits);
            context.SaveChanges();

            // When.
            var results = query.Search(parameters);

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_MappingExistsWithNoOverrides_NoResultsShouldBeReturned()
        {
            // Given.
            CreateDefaultMapping();

            // When.
            var results = query.Search(parameters).Where(q => q.MerchandiseLineage.Split('|')[1] == testMerchandiseClass.hierarchyClassName.ToString()).ToList();

            // Then.
            Assert.AreEqual(0, results.Count);
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_MappingExistsWithThreeOverridesAndZeroDefault_ThreeItemsShouldBeReturned()
        {
            // Given.
            CreateDefaultMapping();

            var random = new Random(DateTime.Now.Second);

            var testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();

            ItemHierarchyClass testItemMerchandise;
            ItemHierarchyClass testItemTax;
            ItemHierarchyClass testItemBrand;

            foreach (var item in testItems)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = overridenTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemBrand = new ItemHierarchyClass { hierarchyClassID = testBrand.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax,
                    testItemBrand
                };
            }

            context.SaveChanges();

            // When.
            var results = query.Search(parameters).Where(q => q.MerchandiseLineage.Split('|')[1] == testMerchandiseClass.hierarchyClassName.ToString()).ToList();

            // Then.
            Assert.AreEqual(testItems.Count, results.Count);
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_MappingExistsWithThreeOverridesAndThreeDefault_ThreeItemsShouldBeReturned()
        {
            // Given.
            CreateDefaultMapping();

            var random = new Random(DateTime.Now.Second);

            var defaultItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            var overridenItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            context.Item.AddRange(defaultItems);
            context.Item.AddRange(overridenItems);
            context.SaveChanges();

            ItemHierarchyClass testItemMerchandise;
            ItemHierarchyClass testItemTax;
            ItemHierarchyClass testItemBrand;

            foreach (var item in overridenItems)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = overridenTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemBrand = new ItemHierarchyClass { hierarchyClassID = testBrand.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax,
                    testItemBrand
                };
            }

            foreach (var item in defaultItems)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = defaultTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemBrand = new ItemHierarchyClass { hierarchyClassID = testBrand.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax,
                    testItemBrand
                };
            }

            context.SaveChanges();

            // When.
            var results = query.Search(parameters).Where(q => q.MerchandiseLineage.Split('|')[1] == testMerchandiseClass.hierarchyClassName.ToString()).ToList();

            // Then.
            Assert.AreEqual(overridenItems.Count, results.Count);
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_TwoMappingsExistWithThreeOverridesAndZeroDefault_SixItemsShouldBeReturned()
        {
            // Given.
            CreateDefaultMapping();

            HierarchyClass merchandiseClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.SubBrick);
            HierarchyClass taxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);

            context.HierarchyClass.Add(merchandiseClass);
            context.HierarchyClass.Add(taxClass);
            context.SaveChanges();

            CreateSecondaryMapping(merchandiseClass, taxClass);

            var random = new Random(DateTime.Now.Second);

            var itemsForFirstMapping = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            var itemsForSecondMapping = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            context.Item.AddRange(itemsForFirstMapping);
            context.Item.AddRange(itemsForSecondMapping);
            context.SaveChanges();

            ItemHierarchyClass testItemMerchandise;
            ItemHierarchyClass testItemTax;
            ItemHierarchyClass testItemBrand;

            foreach (var item in itemsForFirstMapping)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = overridenTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemBrand = new ItemHierarchyClass { hierarchyClassID = testBrand.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax,
                    testItemBrand
                };
            }

            foreach (var item in itemsForSecondMapping)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = merchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = overridenTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemBrand = new ItemHierarchyClass { hierarchyClassID = testBrand.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax,
                    testItemBrand
                };
            }

            context.SaveChanges();

            // When.
            var results = query.Search(parameters);
            var firstMappingMismatches = results.Where(q => q.MerchandiseLineage.Split('|')[1] == testMerchandiseClass.hierarchyClassName.ToString()).ToList();
            var secondMappingMismatches = results.Where(q => q.MerchandiseLineage.Split('|')[1] == merchandiseClass.hierarchyClassName.ToString()).ToList();

            // Then.
            Assert.AreEqual(itemsForFirstMapping.Count, firstMappingMismatches.Count);
            Assert.AreEqual(itemsForSecondMapping.Count, secondMappingMismatches.Count);
        }

        [TestMethod]
        public void GetDefaultTaxClassMismatchesQuery_TwoMappingsExistWithThreeOverridesAndThreeDefault_SixItemsShouldBeReturned()
        {
            // Given.
            CreateDefaultMapping();

            HierarchyClass merchandiseClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.SubBrick);
            HierarchyClass taxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);

            context.HierarchyClass.Add(merchandiseClass);
            context.HierarchyClass.Add(taxClass);
            context.SaveChanges();

            CreateSecondaryMapping(merchandiseClass, taxClass);

            var random = new Random(DateTime.Now.Second);

            var itemsForFirstMapping = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            var itemsForSecondMapping = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            context.Item.AddRange(itemsForFirstMapping);
            context.Item.AddRange(itemsForSecondMapping);
            context.SaveChanges();

            ItemHierarchyClass testItemMerchandise;
            ItemHierarchyClass testItemTax;
            ItemHierarchyClass testItemBrand;

            int i = 2;
            foreach (var item in itemsForFirstMapping)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = (i % 2 == 0) ? overridenTaxClass.hierarchyClassID : defaultTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemBrand = new ItemHierarchyClass { hierarchyClassID = testBrand.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax,
                    testItemBrand
                };

                i++;
            }

            foreach (var item in itemsForSecondMapping)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = merchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = (i % 2 == 0) ? overridenTaxClass.hierarchyClassID : taxClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemBrand = new ItemHierarchyClass { hierarchyClassID = testBrand.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax,
                    testItemBrand
                };

                i++;
            }

            context.SaveChanges();

            // When.
            var results = query.Search(parameters);
            var firstMappingMismatches = results.Where(q => q.MerchandiseLineage.Split('|')[1] == testMerchandiseClass.hierarchyClassName.ToString()).ToList();
            var secondMappingMismatches = results.Where(q => q.MerchandiseLineage.Split('|')[1] == merchandiseClass.hierarchyClassName.ToString()).ToList();

            // Then.
            Assert.AreEqual(itemsForFirstMapping.Count / 2, firstMappingMismatches.Count);
            Assert.AreEqual(itemsForSecondMapping.Count / 2, secondMappingMismatches.Count);
        }
    }
}
