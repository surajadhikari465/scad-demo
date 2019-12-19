using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.NewItem.Queries;
using System.Linq;
using Irma.Framework;
using System.Data.Entity;
using Irma.Testing.Builders;
using System.Collections.Generic;
using Services.NewItem.Models;
using Moq;
using Icon.Common.Context;
using Icon.Logging;
using Icon.Framework;

namespace Services.NewItem.Tests.Queries
{
    [TestClass]
    public class GetNewItemsQueryHandlerTests
    {
        private GetNewItemsQueryHandler queryHandler;
        private GetNewItemsQuery query;
        private IrmaContext irmaContext;
        private IconContext iconContext;
        private DbContextTransaction irmaTransaction;
        private DbContextTransaction iconTransaction;
        private int testItemChgType;
        private SubTeam testSubTeam;
        private ItemUnit testItemUnit;
        private ItemBrand testBrand;
        private ValidatedBrand testValidatedBrand;
        private NatItemClass testNatItemClass;
        private TaxClass testTaxClass;
        private List<ItemIdentifier> testItemIdentifiers;
        private List<Irma.Framework.Item> testIrmaItems;
        private List<Icon.Framework.Item> testIconItems;
        private List<IconItemChangeQueue> testEvents;
        private Mock<IRenewableContext<IrmaContext>> mockIrmaRenewableContext;
        private Mock<IRenewableContext<IconContext>> mockIconRenewableContext;
        private Mock<ILogger<GetNewItemsQueryHandler>> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            irmaContext = new IrmaContext("ItemCatalog_FL");
            iconContext = new IconContext();

            irmaTransaction = irmaContext.Database.BeginTransaction();
            iconTransaction = iconContext.Database.BeginTransaction();

            mockIrmaRenewableContext = new Mock<IRenewableContext<IrmaContext>>();
            mockIrmaRenewableContext.SetupGet(m => m.Context).Returns(irmaContext);
            mockIconRenewableContext = new Mock<IRenewableContext<IconContext>>();
            mockIconRenewableContext.SetupGet(m => m.Context).Returns(iconContext);
            mockLogger = new Mock<ILogger<GetNewItemsQueryHandler>>();

            queryHandler = new GetNewItemsQueryHandler(mockIrmaRenewableContext.Object, mockIconRenewableContext.Object, mockLogger.Object);
            query = new GetNewItemsQuery { Instance = 55, Region = "FL", NumberOfItemsInMessage = 100 };

            testItemChgType = irmaContext.Database.SqlQuery<int>("SELECT TOP 1 CAST(ItemChgTypeID as INT) FROM ItemChgType").First();
            testSubTeam = irmaContext.SubTeam.Add(new SubTeam { SubTeam_Name = "Test SubTeam", Dept_No = 1234, Target_Margin = 1m, EXEWarehouseSent = false });
            testItemUnit = irmaContext.ItemUnit.Add(new ItemUnit { Unit_Name = "Test Item Unit", Weight_Unit = false });
            testBrand = irmaContext.ItemBrand.Add(new ItemBrand { Brand_Name = "Test Brand" });
            testValidatedBrand = irmaContext.ValidatedBrand.Add(new ValidatedBrand { ItemBrand = testBrand, IconBrandId = 12345 });
            testNatItemClass = irmaContext.NatItemClass.Add(new NatItemClass { ClassID = 56789, ClassName = "Test National Class", NatCatID = 555 });
            testTaxClass = irmaContext.TaxClass.Add(new TaxClass { TaxClassDesc = "1111111 Test Tax Class", ExternalTaxGroupCode = "1111111" });

            irmaContext.Database.ExecuteSqlCommand("delete IconItemChangeQueue");
            irmaContext.SaveChanges();

            testItemIdentifiers = new List<ItemIdentifier>();
            testIrmaItems = new List<Irma.Framework.Item>();
            testEvents = new List<IconItemChangeQueue>();
            testIconItems = new List<Icon.Framework.Item>();
        }

        [TestCleanup]
        public void Cleanup()
        {
            irmaTransaction.Rollback();
            irmaTransaction.Dispose();
            irmaContext.Dispose();
            iconTransaction.Rollback();
            iconTransaction.Dispose();
            iconContext.Dispose();
        }

        [TestMethod]
        public void GetNewItemsQuery_1NewItemEvent_ShouldReturn1NewItemModel()
        {
            //Given
            SetupTestEvents(1, testItemIdentifiers, testIrmaItems, testIconItems, testEvents);

            //When
            var results = queryHandler.Search(query).ToList();

            //Then            
            Assert.AreEqual(1, results.Count);
            AssertResultsAreEqualToTestEvents(results);
        }

        [TestMethod]
        public void GetNewItemsQuery_10NewItemEvents_ShouldReturn10NewItemModel()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testIrmaItems, testIconItems, testEvents);

            //When
            var results = queryHandler.Search(query)
                .OrderBy(i => i.ScanCode)
                .ToList();

            //Then
            AssertResultsAreEqualToTestEvents(results);
        }

        [TestMethod]
        public void GetNewItemsQuery_QueueIsEmpty_ShouldReturnEmptyList()
        {
            //When
            var results = queryHandler.Search(query);

            //Then
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetNewItemsQuery_QueueOnlyHasEventsThatAreInProcessByOtherInstances_ShouldReturnEmptyList()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testIrmaItems, testIconItems, testEvents, 33);

            //When
            var results = queryHandler.Search(query);

            //Then
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetNewItemsQuery_QueueOnlyHasFailedEvents_ShouldReturnEmptyList()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testIrmaItems, testIconItems, testEvents, processFailedDate: DateTime.Now);

            //When
            var results = queryHandler.Search(query);

            //Then
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetNewItemsQuery_OrganicIsTrue_ShouldReturnOrganicTrue()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testIrmaItems, testIconItems, testEvents, organic: true);

            //When
            var results = queryHandler.Search(query).ToList();

            //Then
            AssertResultsAreEqualToTestEvents(results);
        }

        [TestMethod]
        public void GetNewItemsQuery_QueueHasEventsThatAreProcessableAsWellAsFailedEventsAndEventsInProcessByOtherInstances_ShouldReturnOnlyTheReadyEvents()
        {
            //Given
            var failedTestItemIdentifiers = new List<ItemIdentifier>();
            var failedTestIrmaItems = new List<Irma.Framework.Item>();
            var failedTestEvents = new List<IconItemChangeQueue>();
            var failedTestIconItems = new List<Icon.Framework.Item>();
            SetupTestEvents(10, failedTestItemIdentifiers, failedTestIrmaItems, failedTestIconItems, failedTestEvents, processFailedDate: DateTime.Now);

            SetupTestEvents(7, testItemIdentifiers, testIrmaItems, testIconItems, testEvents);

            var otherInstanceTestIdentifiers = new List<ItemIdentifier>();
            var otherInstanceTestIrmaItems = new List<Irma.Framework.Item>();
            var otherInstanceTestEvents = new List<IconItemChangeQueue>();
            var otherInstanceTestIconItems = new List<Icon.Framework.Item>();
            SetupTestEvents(10, otherInstanceTestIdentifiers, otherInstanceTestIrmaItems, otherInstanceTestIconItems, otherInstanceTestEvents, 33);

            //When
            var results = queryHandler.Search(query).ToList();

            //Then
            AssertResultsAreEqualToTestEvents(results);
        }

        [TestMethod]
        public void GetNewItemsQuery_ScanCodeIsAssociatedToDeletedItem_ShouldOnlyGetEventForActiveItem()
        {
            //Given
            SetupTestEvents(1, testItemIdentifiers, testIrmaItems, testIconItems, testEvents);

            var deletedIdentifier = new TestItemIdentifierBuilder()
                .WithIdentifier(testItemIdentifiers.First().Identifier)
                .WithDefault_Identifier(1)
                .WithDeleted_Identifier(1)
                .Build();
            var deletedIten = new TestIrmaDbItemBuilder()
                .WithItemIdentifier(deletedIdentifier)
                .WithRetail_Sale(true)
                .WithSubTeam(testSubTeam)
                .WithItemUnit3(testItemUnit)
                .WithItemUnit4(testItemUnit)
                .WithBrand_ID(testBrand.Brand_ID)
                .WithClassID(testNatItemClass.ClassID)
                .WithTaxClassID(testTaxClass.TaxClassID)
                .WithItem_Description("Deleted Item Description")
                .WithPOS_Description("Deleted POS Description")
                .WithPackage_Desc1(3)
                .WithPackage_Desc2(2.1m)
                .WithFood_Stamps(true)
                .WithDeleted_Item(true)
                .Build();
            irmaContext.Item.Add(deletedIten);
            irmaContext.SaveChanges();

            //When
            var results = queryHandler.Search(query).ToList();

            //Then            
            Assert.AreEqual(1, results.Count);
            AssertResultsAreEqualToTestEvents(results);
        }

        [TestMethod]
        public void GetNewItemsQuery_ItemDoesNotExistInIcon_IconItemIdShouldBeNull()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testIrmaItems, testIconItems, testEvents, organic: true, addItemsToIcon: false);

            //When
            var results = queryHandler.Search(query).ToList();

            //Then
            foreach (var item in results)
            {
                Assert.IsNull(item.IconItemId);
            }
            AssertResultsAreEqualToTestEvents(results);
        }

        private void SetupTestEvents(
            int numberOfEvents, 
            List<ItemIdentifier> itemIdentifiers, 
            List<Irma.Framework.Item> irmaItems, 
            List<Icon.Framework.Item> iconItems, 
            List<IconItemChangeQueue> events, 
            int? instanceId = null, 
            DateTime? processFailedDate = null, 
            bool organic = false,
            bool addItemsToIcon = true)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                var itemIdentifier = new TestItemIdentifierBuilder()
                    .WithIdentifier("1234567890" + i.ToString())
                    .WithDefault_Identifier(1)
                    .Build();
                var irmaItem = new TestIrmaDbItemBuilder()
                    .WithItemIdentifier(itemIdentifier)
                    .WithRetail_Sale(true)
                    .WithSubTeam(testSubTeam)
                    .WithItemUnit3(testItemUnit)
                    .WithItemUnit4(testItemUnit)
                    .WithBrand_ID(testBrand.Brand_ID)
                    .WithClassID(testNatItemClass.ClassID)
                    .WithTaxClassID(testTaxClass.TaxClassID)
                    .WithItem_Description("Test Item Description " + i.ToString())
                    .WithPOS_Description("Test POS Description " + i.ToString())
                    .WithPackage_Desc1(3 + i)
                    .WithPackage_Desc2(2.1m + i)
                    .WithFood_Stamps(true)
                    .WithOrganic(organic)
                    .Build();
                itemIdentifiers.Add(itemIdentifier);
                irmaItems.Add(irmaItem);

                if (addItemsToIcon && !iconContext.ScanCode.Any(sc => sc.scanCode == itemIdentifier.Identifier))
                {
                    var iconItem = new Icon.Framework.Item
                    {
                        itemTypeID = ItemTypes.RetailSale,
                        ScanCode = new List<ScanCode>
                        {
                            new ScanCode
                            {
                                scanCode = itemIdentifier.Identifier,
                                scanCodeTypeID = ScanCodeTypes.Upc
                            }
                        }
                    };
                    iconItems.Add(iconItem);
                }
            }

            irmaContext.Item.AddRange(irmaItems);
            irmaContext.SaveChanges();
            iconContext.Item.AddRange(iconItems);
            iconContext.SaveChanges();

            foreach (var item in irmaItems)
            {
                var newItemEvent = new TestIconItemChangeQueueBuilder()
                       .WithItemKey(item.Item_Key)
                       .WithIdentifier(item.ItemIdentifier.First().Identifier)
                       .WithItemChgTypeId((byte)testItemChgType);
                if (instanceId.HasValue)
                {
                    newItemEvent = newItemEvent.WithInProcessBy(instanceId.Value.ToString());
                }
                if (processFailedDate.HasValue)
                {
                    newItemEvent = newItemEvent.WithProcessFailedDate(processFailedDate.Value);
                }
                events.Add(newItemEvent);
            }
            irmaContext.IconItemChangeQueue.AddRange(events);
            irmaContext.SaveChanges();
        }

        private void AssertResultsAreEqualToTestEvents(List<NewItemModel> results)
        {
            Assert.AreEqual(testIrmaItems.Count, results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                var testEvent = testEvents[i];
                var testItemIdentifier = testItemIdentifiers[i];
                var testIrmaItem = testIrmaItems[i];
                var testIconItem = iconContext.ScanCode.FirstOrDefault(sc => sc.scanCode == testItemIdentifier.Identifier);

                Assert.AreEqual(query.Region, result.Region);
                Assert.AreEqual(testEvent.QID, result.QueueId);
                Assert.AreEqual(testIrmaItem.Item_Key, result.ItemKey);
                Assert.AreEqual(testItemIdentifier.Identifier, result.ScanCode);
                Assert.AreEqual(true, result.IsDefaultIdentifier);
                Assert.AreEqual(testIrmaItem.Item_Description, result.ItemDescription);
                Assert.AreEqual(testIrmaItem.POS_Description, result.PosDescription);
                Assert.AreEqual(testBrand.Brand_Name, result.BrandName);
                Assert.AreEqual(testValidatedBrand.IconBrandId, result.IconBrandId);
                Assert.AreEqual(testIrmaItem.Package_Desc1, result.PackageUnit);
                Assert.AreEqual(testIrmaItem.Package_Desc2, result.RetailSize);
                Assert.AreEqual(testItemUnit.Unit_Abbreviation, result.RetailUom);
                Assert.AreEqual(testIrmaItem.Food_Stamps, result.FoodStampEligible);
                Assert.AreEqual(testSubTeam.SubTeam_Name, result.SubTeamName);
                Assert.AreEqual(testSubTeam.Dept_No.ToString(), result.SubTeamNumber);
                Assert.AreEqual(testNatItemClass.ClassID.ToString(), result.NationalClassCode);
                Assert.AreEqual(testTaxClass.ExternalTaxGroupCode, result.TaxClassCode);
                Assert.AreEqual(testEvent.InsertDate, result.QueueInsertDate);
                Assert.AreEqual(testIrmaItem.Organic, result.Organic);
                if (testIconItem == null)
                {
                    Assert.IsNull(result.IconItemId);
                }
                else
                {
                    Assert.AreEqual(testIconItem.scanCode, result.ScanCode);
                    Assert.AreEqual(testIconItem.itemID, result.IconItemId.Value);
                }
            }
        }
    }
}
