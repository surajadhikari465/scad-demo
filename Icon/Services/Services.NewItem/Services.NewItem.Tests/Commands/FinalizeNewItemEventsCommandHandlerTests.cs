using Icon.Common.Context;
using Icon.Logging;
using Irma.Framework;
using Irma.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Services.NewItem.Commands;
using Services.NewItem.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace Services.NewItem.Tests.Commands
{
    [TestClass]
    public class FinalizeNewItemEventsCommandHandlerTests
    {
        private FinalizeNewItemEventsCommandHandler commandHandler;
        private FinalizeNewItemEventsCommand command;
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
        private Mock<ILogger<FinalizeNewItemEventsCommandHandler>> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            context = new IrmaContext("ItemCatalog_FL");
            transaction = context.Database.BeginTransaction();

            mockRenewableContext = new Mock<IRenewableContext<IrmaContext>>();
            mockRenewableContext.SetupGet(m => m.Context).Returns(context);
            mockLogger = new Mock<ILogger<FinalizeNewItemEventsCommandHandler>>();

            commandHandler = new FinalizeNewItemEventsCommandHandler(mockRenewableContext.Object, mockLogger.Object);
            command = new FinalizeNewItemEventsCommand { Instance = 55 };

            testItemChgType = context.Database.SqlQuery<int>("SELECT TOP 1 CAST(ItemChgTypeID as INT) FROM ItemChgType").First();
            testSubTeam = context.SubTeam.Add(new SubTeam { SubTeam_Name = "Test SubTeam", SubDept_No = 1234, Target_Margin = 1m, EXEWarehouseSent = false });
            testItemUnit = context.ItemUnit.Add(new ItemUnit { Unit_Name = "Test Item Unit", Weight_Unit = false });
            testBrand = context.ItemBrand.Add(new ItemBrand { Brand_Name = "Test Brand" });
            testValidatedBrand = context.ValidatedBrand.Add(new ValidatedBrand { ItemBrand = testBrand, IconBrandId = 12345 });
            testNatItemClass = context.NatItemClass.Add(new NatItemClass { ClassID = 56789, ClassName = "Test National Class", NatCatID = 555 });
            testTaxClass = context.TaxClass.Add(new TaxClass { TaxClassDesc = "111111 Test Tax Class", ExternalTaxGroupCode = "1111111" });

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
        public void FinalizeNewItemEvents_NewItemsIsEmpty_ShouldNotCallStoredProcedure()
        {
            //Given
            Mock<IEnumerable<NewItemModel>> mockNewItems = new Mock<IEnumerable<NewItemModel>>();
            mockNewItems.Setup(m => m.GetEnumerator())
                .Returns(new List<NewItemModel>().GetEnumerator());
            command.NewItems = mockNewItems.Object;

            //When
            commandHandler.Execute(command);

            //Then
            mockNewItems.Verify(m => m.GetEnumerator(), Times.Exactly(2));
        }

        [TestMethod]
        public void FinalizeNewItemEvents_MessagesExist_ShouldDeleteMessages()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testItems, testEvents, command.Instance);

            command.NewItems = testEvents.Select(e => new NewItemModel
            {
                QueueId = e.QID
            });

            //When
            commandHandler.Execute(command);

            //Then
            List<int> queueIds = command.NewItems.Select(i => i.QueueId).ToList();
            Assert.IsFalse(context.IconItemChangeQueue.Any(q => queueIds.Contains(q.QID)));
        }

        [TestMethod]
        public void FinalizeNewItemEvents_ErrorOccured_ShouldFailEvents()
        {
            //Given
            SetupTestEvents(10, testItemIdentifiers, testItems, testEvents, command.Instance);

            command.NewItems = testEvents.Select(e => new NewItemModel
            {
                QueueId = e.QID
            });
            command.ErrorOccurred = true;

            //When
            commandHandler.Execute(command);

            //Then
            List<int> queueIds = command.NewItems.Select(i => i.QueueId).ToList();
            var events = context.IconItemChangeQueue.AsNoTracking().Where(q => queueIds.Contains(q.QID)).ToList();

            Assert.AreEqual(events.Count, command.NewItems.Count());
            foreach (var newItemEvent in events)
            {
                Assert.IsNotNull(newItemEvent.ProcessFailedDate);
            }
        }

        [TestMethod]
        public void FinalizeNewItemEvents_OtherEventsExistNotInListOfNewItems_ShouldOnlyDeleteEventsInNewItems()
        {
            //Given
            SetupTestEvents(20, testItemIdentifiers, testItems, testEvents, command.Instance);
            command.NewItems = testEvents.Take(10).Select(e => new NewItemModel
            {
                QueueId = e.QID
            });

            //When
            commandHandler.Execute(command);

            //Then
            List<int> queueIds = command.NewItems.Select(i => i.QueueId).ToList();
            var events = context.IconItemChangeQueue.AsNoTracking().Where(q => queueIds.Contains(q.QID)).ToList();

            Assert.AreEqual(10, command.NewItems.Count());
            foreach (var newItemEvent in events)
            {
                Assert.IsNull(newItemEvent.ProcessFailedDate);
                Assert.IsNull(newItemEvent.InProcessBy);
            }
        }

        [TestMethod]
        public void FinalizeNewItemEvents_EventsAreLeftInProcessAfterFinalize_ShouldFailInProcessEvents()
        {
            SetupTestEvents(10, testItemIdentifiers, testItems, testEvents, command.Instance);
            command.NewItems = testEvents.Select(e => new NewItemModel
            {
                QueueId = e.QID
            });
            SetupTestEvents(15, new List<ItemIdentifier>(), new List<Item>(), new List<IconItemChangeQueue>(), command.Instance);

            //When
            commandHandler.Execute(command);

            //Then
            var stringInstanceId = command.Instance.ToString();
            var events = context.IconItemChangeQueue.AsNoTracking().ToList();

            Assert.AreEqual(15, events.Count);
            foreach (var newItemEvent in events)
            {
                Assert.IsNotNull(newItemEvent.ProcessFailedDate);
                Assert.IsNull(newItemEvent.InProcessBy);
            }
        }

        private void SetupTestEvents(int numberOfEvents, List<ItemIdentifier> itemIdentifiers, List<Item> items, List<IconItemChangeQueue> events, int? instanceId = null, DateTime? processFailedDate = null)
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
                if (processFailedDate.HasValue)
                {
                    newItemEvent = newItemEvent.WithProcessFailedDate(processFailedDate.Value);
                }
                events.Add(newItemEvent);
            }
            context.IconItemChangeQueue.AddRange(events);
            context.SaveChanges();
        }

    }
}
