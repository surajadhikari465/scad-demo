using Icon.Framework;
using Icon.Testing.Builders;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass]
    public class ApplyMerchTaxAssociationToItemsCommandHandlerTests
    {
        private ApplyMerchTaxAssociationToItemsCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private HierarchyClassTrait merchDefaultTaxTrait;
        private HierarchyClass testMerchandiseClass;
        private HierarchyClass originalTaxClass;
        private HierarchyClass updatedTaxClass;
        private HierarchyClass overridenTaxClass;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();

            testMerchandiseClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Merchandise).WithHierarchyLevel(HierarchyLevels.SubBrick);
            originalTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);
            updatedTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);
            overridenTaxClass = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Tax).WithHierarchyLevel(HierarchyLevels.Tax);

            commandHandler = new ApplyMerchTaxAssociationToItemsCommandHandler(context);

            transaction = context.Database.BeginTransaction();

            context.HierarchyClass.Add(testMerchandiseClass);
            context.HierarchyClass.Add(originalTaxClass);
            context.HierarchyClass.Add(updatedTaxClass);
            context.HierarchyClass.Add(overridenTaxClass);
            context.SaveChanges();

            merchDefaultTaxTrait = new HierarchyClassTrait
            {
                traitID = Traits.MerchDefaultTaxAssociatation,
                traitValue = originalTaxClass.hierarchyClassID.ToString(),
                hierarchyClassID = testMerchandiseClass.hierarchyClassID
            };

            context.HierarchyClassTrait.Add(merchDefaultTaxTrait);
            context.SaveChanges();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        private void AssociateItems(List<Item> items)
        {
            context.Item.AddRange(items);
            context.SaveChanges();

            ItemHierarchyClass testItemMerchandise;
            ItemHierarchyClass testItemTax;

            foreach (var item in items)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = originalTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax
                };
            }

            context.SaveChanges();
        }

        [TestMethod]
        public void ApplyMerchTaxAssociationToItemsCommand_NoItemsAssociatedToSubBrick_NoItemsShouldHaveMappedTaxClass()
        {
            // Given.
            var command = new ApplyMerchTaxAssociationToItemsCommand
            {
                MerchandiseHierarchyClassId = testMerchandiseClass.hierarchyClassID,
                TaxHierarchyClassId = updatedTaxClass.hierarchyClassID
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            bool itemsAssociatedToMappedTax = context.ItemHierarchyClass.Any(ihc => ihc.hierarchyClassID == updatedTaxClass.hierarchyClassID);

            Assert.IsFalse(itemsAssociatedToMappedTax);
        }

        [TestMethod]
        public void ApplyMerchTaxAssociationToItemsCommand_NoCurrentMapping_AllAssociatedItemsShouldBeMappedToNewTaxClass()
        {
            // Given.
            context.HierarchyClassTrait.Remove(merchDefaultTaxTrait);
            context.SaveChanges();

            var random = new Random(DateTime.Now.Second);

            var testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            context.Item.AddRange(testItems);
            context.SaveChanges();

            ItemHierarchyClass testItemMerchandise;
            ItemHierarchyClass testItemTax;

            foreach (var item in testItems)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = overridenTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };

                item.ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax
                };
            }

            context.SaveChanges();

            var command = new ApplyMerchTaxAssociationToItemsCommand
            {
                MerchandiseHierarchyClassId = testMerchandiseClass.hierarchyClassID,
                TaxHierarchyClassId = updatedTaxClass.hierarchyClassID
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var itemsAssociatedToNewlyMappedTax = context.ItemHierarchyClass.Where(ihc => ihc.hierarchyClassID == updatedTaxClass.hierarchyClassID).ToList();

            Assert.AreEqual(testItems.Count, itemsAssociatedToNewlyMappedTax.Count);
        }

        [TestMethod]
        public void ApplyMerchTaxAssociationToItemsCommand_OneItemAssociatedToSubBrick_OneItemShouldHaveMappedTaxClass()
        {
            // Given.
            var testItems = new List<Item>
            {
                new TestItemBuilder().Build()
            };

            AssociateItems(testItems);

            var command = new ApplyMerchTaxAssociationToItemsCommand
            {
                MerchandiseHierarchyClassId = testMerchandiseClass.hierarchyClassID,
                TaxHierarchyClassId = updatedTaxClass.hierarchyClassID
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var itemsAssociatedToMappedTax = context.ItemHierarchyClass.Where(ihc => ihc.hierarchyClassID == updatedTaxClass.hierarchyClassID).ToList();

            Assert.AreEqual(itemsAssociatedToMappedTax.Count, testItems.Count);
        }

        [TestMethod]
        public void ApplyMerchTaxAssociationToItemsCommand_ThreeItemsAssociatedToSubBrick_ThreeItemsShouldHaveMappedTaxClass()
        {
            // Given.
            var random = new Random(DateTime.Now.Second);

            var testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            AssociateItems(testItems);

            var command = new ApplyMerchTaxAssociationToItemsCommand
            {
                MerchandiseHierarchyClassId = testMerchandiseClass.hierarchyClassID,
                TaxHierarchyClassId = updatedTaxClass.hierarchyClassID
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var itemsAssociatedToMappedTax = context.ItemHierarchyClass.Where(ihc => ihc.hierarchyClassID == updatedTaxClass.hierarchyClassID).ToList();

            Assert.AreEqual(itemsAssociatedToMappedTax.Count, testItems.Count);
        }

        [TestMethod]
        public void ApplyMerchTaxAssociationToItemsCommand_TwoItemsWithOverriddenTaxAndTwoItemsWithUnmappedTax_TwoItemsShouldHaveMappedTaxClass()
        {
            // Given.
            var random = new Random(DateTime.Now.Second);

            var testItems = new List<Item>
            {
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString()),
                new TestItemBuilder().WithScanCode(random.Next(0000000001, 2000000000).ToString())
            };

            AssociateItems(new List<Item> { testItems[0], testItems[1] });

            context.Item.Add(testItems[2]);
            context.Item.Add(testItems[3]);
            context.SaveChanges();

            ItemHierarchyClass testItemMerchandise;
            ItemHierarchyClass testItemTax;

            for (int i = 2; i <= 3; i++)
            {
                testItemMerchandise = new ItemHierarchyClass { hierarchyClassID = testMerchandiseClass.hierarchyClassID, localeID = Locales.WholeFoods };
                testItemTax = new ItemHierarchyClass { hierarchyClassID = overridenTaxClass.hierarchyClassID, localeID = Locales.WholeFoods };

                testItems[i].ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    testItemMerchandise,
                    testItemTax
                };
            }

            context.SaveChanges();

            var command = new ApplyMerchTaxAssociationToItemsCommand
            {
                MerchandiseHierarchyClassId = testMerchandiseClass.hierarchyClassID,
                TaxHierarchyClassId = updatedTaxClass.hierarchyClassID
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var overridenItems = context.ItemHierarchyClass.Where(ihc => ihc.hierarchyClassID == overridenTaxClass.hierarchyClassID).ToList();
            var itemsAssociatedToNewlyMappedTax = context.ItemHierarchyClass.Where(ihc => ihc.hierarchyClassID == updatedTaxClass.hierarchyClassID).ToList();
            var itemsAssociatedToOriginalTax = context.ItemHierarchyClass.Where(ihc => ihc.hierarchyClassID == originalTaxClass.hierarchyClassID).ToList();

            Assert.AreEqual(overridenItems.Count, 2);
            Assert.AreEqual(itemsAssociatedToNewlyMappedTax.Count, 2);
            Assert.AreEqual(itemsAssociatedToOriginalTax.Count, 0);
        }

        [TestMethod]
        public void ApplyMerchTaxAssociationToItemsCommand_TaxClassIsUpdated_MessageShouldBeSentToEsb()
        {
            // Given.
            var testItems = new List<Item>
            {
                new TestItemBuilder().WithValidationDate(DateTime.Now.ToString())
            };

            AssociateItems(testItems);

            AddRequiredInformationForProductMessage(testItems[0]);

            var command = new ApplyMerchTaxAssociationToItemsCommand
            {
                MerchandiseHierarchyClassId = testMerchandiseClass.hierarchyClassID,
                TaxHierarchyClassId = updatedTaxClass.hierarchyClassID
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testScanCode = testItems[0].ScanCode.Single().scanCode;
            var message = context.MessageQueueProduct.SingleOrDefault(q => q.ScanCode == testScanCode);

            Assert.AreEqual(message.TaxClassName, updatedTaxClass.hierarchyClassName);            
        }

        [TestMethod]
        public void ApplyMerchTaxAssociationToItemsCommand_TaxClassIsUpdated_EventShouldBeGeneratedForIrma()
        {
            // Given.
             var testItems = new List<Item>
            {
                new TestItemBuilder().WithValidationDate(DateTime.Now.ToString())
            };

            AssociateItems(testItems);

            AddRequiredInformationForItemEvent(testItems[0]);

            var command = new ApplyMerchTaxAssociationToItemsCommand
            {
                MerchandiseHierarchyClassId = testMerchandiseClass.hierarchyClassID,
                TaxHierarchyClassId = updatedTaxClass.hierarchyClassID
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            string testScanCode = testItems[0].ScanCode.Single().scanCode;
            var itemEvent = context.EventQueue.SingleOrDefault(q => q.EventMessage == testScanCode);

            Assert.IsNotNull(itemEvent);
        }

        private void AddRequiredInformationForItemEvent(Item item)
        {
            IRMAItemSubscription subscription = new TestIrmaItemSubscriptionBuilder().WithIdentifier(item.ScanCode.Single().scanCode);

            context.IRMAItemSubscription.Add(subscription);
            context.SaveChanges();
        }

        private void AddRequiredInformationForProductMessage(Item item)
        {
            HierarchyClass brand = new TestHierarchyClassBuilder().WithHierarchyId(Hierarchies.Brands).WithHierarchyLevel(HierarchyLevels.Brand);
            context.HierarchyClass.Add(brand);
            context.SaveChanges();

            brand.HierarchyClassTrait = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = brand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                }
            };

            context.SaveChanges();

            var itemBrand = new ItemHierarchyClass
            {
                itemID = item.itemID,
                localeID = Locales.WholeFoods,
                hierarchyClassID = brand.hierarchyClassID
            };

            item.ItemHierarchyClass.Add(itemBrand);
            context.SaveChanges();

            testMerchandiseClass.HierarchyClassTrait = new List<HierarchyClassTrait>
            {
                new HierarchyClassTrait
                {
                    hierarchyClassID = brand.hierarchyClassID,
                    traitID = Traits.SentToEsb,
                    traitValue = DateTime.Now.ToString()
                },
                new HierarchyClassTrait
                {
                    hierarchyClassID = brand.hierarchyClassID,
                    traitID = Traits.MerchFinMapping,
                    traitValue = "Grocery (1000)"
                }
            };

            context.SaveChanges();
        }
    }
}
