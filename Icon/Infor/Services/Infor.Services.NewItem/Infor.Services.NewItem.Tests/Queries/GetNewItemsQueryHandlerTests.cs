using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Infor.Services.NewItem.Queries;
using System.Linq;
using Irma.Framework;
using System.Data.Entity;
using Irma.Testing.Builders;
using System.Collections.Generic;
using Infor.Services.NewItem.Models;
using Moq;
using Icon.Common.Context;
using Icon.Logging;

namespace Infor.Services.NewItem.Tests.Queries
{
    [TestClass]
    public class GetNewItemsQueryHandlerTests
    {
        private GetNewItemsQueryHandler queryHandler;
        private GetNewItemsQuery query;
        private IrmaContext context;
        private DbContextTransaction transaction;
        private int testItemChgType;
        private SubTeam testSubTeam;
        private ItemUnit testItemUnit;
        private ItemBrand testBrand;
        private ValidatedBrand testValidatedBrand;
        private NatItemClass testNatItemClass;
        private TaxClass testTaxClass;
        private List<ItemIdentifier> testItemIdentifiers;
        private List<Item> testItems;
        private List<IconItemChangeQueue> testEvents;
        private Mock<IRenewableContext<IrmaContext>> mockRenewableContext;
        private Mock<ILogger<GetNewItemsQueryHandler>> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            context = new IrmaContext("ItemCatalog_FL");
            transaction = context.Database.BeginTransaction();

            mockRenewableContext = new Mock<IRenewableContext<IrmaContext>>();
            mockRenewableContext.SetupGet(m => m.Context).Returns(context);
            mockLogger = new Mock<ILogger<GetNewItemsQueryHandler>>();

            queryHandler = new GetNewItemsQueryHandler(mockRenewableContext.Object, mockLogger.Object);
            query = new GetNewItemsQuery { Instance = 55, Region = "FL", NumberOfItemsInMessage = 100 };

            testItemChgType = context.Database.SqlQuery<int>("SELECT TOP 1 CAST(ItemChgTypeID as INT) FROM ItemChgType").First();
            testSubTeam = context.SubTeam.Add(new SubTeam { SubTeam_Name = "Test SubTeam", Dept_No = 1234, Target_Margin = 1m, EXEWarehouseSent = false });
            testItemUnit = context.ItemUnit.Add(new ItemUnit { Unit_Name = "Test Item Unit", Weight_Unit = false });
            testBrand = context.ItemBrand.Add(new ItemBrand { Brand_Name = "Test Brand" });
            testValidatedBrand = context.ValidatedBrand.Add(new ValidatedBrand { ItemBrand = testBrand, IconBrandId = 12345 });
            testNatItemClass = context.NatItemClass.Add(new NatItemClass { ClassID = 56789, ClassName = "Test National Class", NatCatID = 555 });
            testTaxClass = context.TaxClass.Add(new TaxClass { TaxClassDesc = "1111111 Test Tax Class", ExternalTaxGroupCode = "1111111" });

            context.Database.ExecuteSqlCommand("delete IconItemChangeQueue");
            context.SaveChanges();

            testItemIdentifiers = new List<ItemIdentifier>();
            testItems = new List<Item>();
            testEvents = new List<IconItemChangeQueue>();
        }

        [TestCleanup] 
        public void Cleanup()
        {
            transaction.Rollback();
            transaction.Dispose();
            context.Dispose();
        }

        [TestMethod]
        public void GetNewItemsQuery_1NewItemEvent_ShouldReturn1NewItemModel()
        {
            //Given
            SetupTestEvents(1, testItemIdentifiers, testItems, testEvents); 

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
            SetupTestEvents(10, testItemIdentifiers, testItems, testEvents);

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
            SetupTestEvents(10, testItemIdentifiers, testItems, testEvents, 33);

            //When
            var results = queryHandler.Search(query);

            //Then
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetNewItemsQuery_QueueOnlyHasFailedEvents_ShouldReturnEmptyList()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testItems, testEvents, processFailedDate: DateTime.Now);

            //When
            var results = queryHandler.Search(query);

            //Then
            Assert.IsFalse(results.Any());
        }

        [TestMethod]
        public void GetNewItemsQuery_OrganicIsTrue_ShouldReturnOrganicTrue()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testItems, testEvents, organic: true);

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
            var failedTestItems = new List<Item>();
            var failedTestEvents = new List<IconItemChangeQueue>();
            SetupTestEvents(10, failedTestItemIdentifiers, failedTestItems, failedTestEvents, processFailedDate: DateTime.Now);

            SetupTestEvents(7, testItemIdentifiers, testItems, testEvents);

            var otherInstanceTestIdentifiers = new List<ItemIdentifier>();
            var otherInstanceTestItems = new List<Item>();
            var otherInstanceTestEvents = new List<IconItemChangeQueue>();
            SetupTestEvents(10, otherInstanceTestIdentifiers, otherInstanceTestItems, otherInstanceTestEvents, 33);
            
            //When
            var results = queryHandler.Search(query).ToList();

            //Then
            AssertResultsAreEqualToTestEvents(results);
        }

        private void SetupTestEvents(int numberOfEvents, List<ItemIdentifier> itemIdentifiers, List<Item> items, List<IconItemChangeQueue> events, int? instanceId = null, DateTime? processFailedDate = null, bool organic = false)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                var itemIdentifier = new TestItemIdentifierBuilder()
                    .WithIdentifier("1234567890" + i.ToString())
                    .WithDefault_Identifier(1)
                    .Build();
                var item = new TestIrmaDbItemBuilder()
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
                items.Add(item);
            }

            context.Item.AddRange(items);
            context.SaveChanges();

            foreach (var item in items)
            {
                var newItemEvent = new TestIconItemChangeQueueBuilder()
                       .WithItemKey(item.Item_Key)
                       .WithIdentifier(item.ItemIdentifier.First().Identifier)
                       .WithItemChgTypeId((byte)testItemChgType);
                if (instanceId.HasValue)
                {
                    newItemEvent = newItemEvent.WithInProcessBy(instanceId.Value.ToString());
                }
                if(processFailedDate.HasValue)
                {
                    newItemEvent = newItemEvent.WithProcessFailedDate(processFailedDate.Value);
                }
                events.Add(newItemEvent);
            }
            context.IconItemChangeQueue.AddRange(events);
            context.SaveChanges();
        }

        private void AssertResultsAreEqualToTestEvents(List<NewItemModel> results)
        {
            Assert.AreEqual(testItems.Count, results.Count);
            for (int i = 0; i < results.Count; i++)
            {
                var result = results[i];
                var testEvent = testEvents[i];
                var testItemIdentifier = testItemIdentifiers[i];
                var testItem = testItems[i];

                Assert.AreEqual(query.Region, result.Region);
                Assert.AreEqual(testEvent.QID, result.QueueId);
                Assert.AreEqual(testItem.Item_Key, result.ItemKey);
                Assert.AreEqual(testItemIdentifier.Identifier, result.ScanCode);
                Assert.AreEqual(true, result.IsDefaultIdentifier);
                Assert.AreEqual(testItem.Item_Description, result.ItemDescription);
                Assert.AreEqual(testItem.POS_Description, result.PosDescription);
                Assert.AreEqual(testBrand.Brand_Name, result.BrandName);
                Assert.AreEqual(testValidatedBrand.IconBrandId, result.IconBrandId);
                Assert.AreEqual(testItem.Package_Desc1, result.PackageUnit);
                Assert.AreEqual(testItem.Package_Desc2, result.RetailSize);
                Assert.AreEqual(testItemUnit.Unit_Abbreviation, result.RetailUom);
                Assert.AreEqual(testItem.Food_Stamps, result.FoodStampEligible);
                Assert.AreEqual(testSubTeam.SubTeam_Name, result.SubTeamName);
                Assert.AreEqual(testSubTeam.Dept_No.ToString(), result.SubTeamNumber);
                Assert.AreEqual(testNatItemClass.ClassID.ToString(), result.NationalClassCode);
                Assert.AreEqual(testTaxClass.ExternalTaxGroupCode, result.TaxClassCode);
                Assert.AreEqual(testEvent.InsertDate, result.QueueInsertDate);
                Assert.AreEqual(testItem.Organic, result.Organic);
            }
        }
    }
}
