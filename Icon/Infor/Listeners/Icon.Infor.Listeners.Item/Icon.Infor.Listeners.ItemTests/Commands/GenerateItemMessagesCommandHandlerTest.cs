using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Commands;
using Icon.Infor.Listeners.Item.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;
using System.Transactions;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class GenerateItemMessagesCommandHandlerTest
    {
        private GenerateItemMessagesCommandHandler commandHandler;
        private IconContext context;
        private IconDbContextFactory contextFactory;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            contextFactory = new IconDbContextFactory();
            context = new IconContext();

            commandHandler = new GenerateItemMessagesCommandHandler(contextFactory);
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void GenerateItemMessages_WhenInforHasOneItem_ThenGenerateItemMessages()
        {
            //Given
            var scanCode = "123456789999";
            Assert.IsFalse(context.ScanCode.Any(sc => sc.scanCode == scanCode), "Scan code already exists. Unable to run test.");

            var item = BuildAndSaveItem(scanCode);
            var expectedItemId = item.itemID;
            var subscriptions = AddIrmaItemSubscription(scanCode, new List<string> { "MW", "FL" });

            var expectedItem = new ItemModel
            {
                ItemId = expectedItemId
            };

            //When
            commandHandler.Execute(new GenerateItemMessagesCommand
                {
                    Items = new List<ItemModel>{ expectedItem }
                });

            //Then
            var eventQueue = this.context.EventQueue.Where(mqp => mqp.EventReferenceId == item.itemID).ToList();
            var messageQueueProduct = this.context.MessageQueueProduct.Where(mqp => mqp.ItemId == item.itemID).ToList();

            Assert.AreEqual(subscriptions.Count, eventQueue.Count());
            Assert.AreEqual(1, messageQueueProduct.Count());
        }

        private List<IRMAItemSubscription> AddIrmaItemSubscription(string scanCode, List<string> regions)
        {
            var subscriptions = regions
                .Select(r => new IRMAItemSubscription
                {
                    identifier = scanCode,
                    regioncode = r
                }).ToList();

            context.IRMAItemSubscription.AddRange(subscriptions);
            context.SaveChanges();

            return subscriptions;
        }

        private Framework.Item BuildAndSaveItem(string scanCode)
        {
            var brand = BuildAndSaveHierarchyClass(Hierarchies.Brands, HierarchyLevels.Brand, "Infor Test Brand");
            HierarchyClass merch = BuildAndSaveHierarchyClass(Hierarchies.Merchandise, HierarchyLevels.SubBrick, "Infor Test Merch");
            HierarchyClass tax = BuildAndSaveHierarchyClass(Hierarchies.Tax, HierarchyLevels.Tax, "Infor Test Tax");
            HierarchyClass financial = BuildAndSaveHierarchyClass(Hierarchies.Financial, HierarchyLevels.Financial, "Infor Test Financial");

            Framework.Item item = new Framework.Item
            {
                itemTypeID = ItemTypes.RetailSale,
                ScanCode = new List<ScanCode> { new ScanCode { scanCode = scanCode, scanCodeTypeID = ScanCodeTypes.Upc } },
                ItemHierarchyClass = new List<ItemHierarchyClass>
                {
                    new ItemHierarchyClass { hierarchyClassID = brand.hierarchyClassID, localeID = Locales.WholeFoods },
                    new ItemHierarchyClass { hierarchyClassID = merch.hierarchyClassID, localeID = Locales.WholeFoods },
                    new ItemHierarchyClass { hierarchyClassID = tax.hierarchyClassID, localeID = Locales.WholeFoods },
                    new ItemHierarchyClass { hierarchyClassID = financial.hierarchyClassID, localeID = Locales.WholeFoods }
                },
                ItemTrait = new List<ItemTrait>
                {
                    new ItemTrait { traitID = Traits.ProductDescription, traitValue = "Infor Test ProductDescription", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PosDescription, traitValue = "Infor Test PosDescription", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.PackageUnit, traitValue = "Infor Test PackageUnit", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.RetailSize, traitValue = "Infor Test RetailSize", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.RetailUom, traitValue = "Infor Test RetailUom", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.FoodStampEligible, traitValue = "Infor Test FoodStampEligible", localeID = Locales.WholeFoods },
                    new ItemTrait { traitID = Traits.ValidationDate, traitValue = "Infor Test ValidationDate", localeID = Locales.WholeFoods },
                }
            };

            context.Item.Add(item);
            context.SaveChanges();

            return item;
        }

        private HierarchyClass BuildAndSaveHierarchyClass(int hierarchyId, int hierarchyLevel, string name)
        {
            HierarchyClass hierarchyClass = new HierarchyClass
            {
                hierarchyClassName = name,
                hierarchyID = hierarchyId,
                hierarchyLevel = hierarchyLevel
            };

            context.HierarchyClass.Add(hierarchyClass);
            context.SaveChanges();

            return hierarchyClass;
        }
    }
}
