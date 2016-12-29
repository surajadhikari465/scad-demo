using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalEventController.Controller.EventOperations;
using Moq;
using GlobalEventController.Controller.EventServices;
using Icon.Logging;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Queries;
using GlobalEventController.Common;
using System.Collections.Generic;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Framework;
using Icon.Testing.Builders;
using System.Linq;
using System.Data.Entity;
using System.Collections;
using Icon.Common.Email;
using MoreLinq;

namespace GlobalEventController.Tests.Controller.EventOperationsTests
{
    [TestClass]
    public class ItemEventBulkProcessorTests
    {
        private ItemEventBulkProcessor processor;
        private EventQueues queues;
        private Mock<ILogger<ItemEventBulkProcessor>> mockLogger;
        private Mock<IEventServiceProvider> mockServiceProvider;
        private Mock<IBulkEventService> mockBulkEventService;
        private Mock<IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>> mockBulkGetScanCodeQuery;
        private Mock<IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>>> mockGetIconItemNutritionQueryHandler;
        private Mock<IEmailClient> mockEmailClient;
        private DataIssueMessageCollector dataIssueMessages;
        private Mock<IEventArchiver> mockEventArchiver;

        [TestInitialize]
        public void InitializeData()
        {
            this.queues = new EventQueues();
            this.mockLogger = new Mock<ILogger<ItemEventBulkProcessor>>();
            this.mockServiceProvider = new Mock<IEventServiceProvider>();
            this.mockBulkGetScanCodeQuery = new Mock<IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>>();
            this.mockGetIconItemNutritionQueryHandler = new Mock<IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>>>();
            this.mockEmailClient = new Mock<IEmailClient>();
            this.dataIssueMessages = new DataIssueMessageCollector(mockEmailClient.Object);
            this.mockEventArchiver = new Mock<IEventArchiver>();

            this.processor = new ItemEventBulkProcessor(this.queues,
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockBulkGetScanCodeQuery.Object,
                this.mockGetIconItemNutritionQueryHandler.Object,
                dataIssueMessages,
                mockEventArchiver.Object);

            this.mockBulkEventService = new Mock<IBulkEventService>();
            this.mockServiceProvider.Setup(p => p.GetBulkItemEventService(It.IsAny<string>())).Returns(this.mockBulkEventService.Object);
            this.mockEventArchiver.SetupGet(m => m.Events).Returns(new List<EventQueueArchive>());
            StartupOptions.NutritionEnabledRegions = new List<string>();
        }

        [TestMethod]
        public void BulkProcessEvents_NoQueuedItemEvents_LoggerInfoCalledAndItemBulkServiceRunIsNotCalled()
        {
            // Given
            PopulateQueueWithNonItemEvents();

            // When
            processor.BulkProcessEvents();

            // Then
            mockLogger.Verify(l => l.Info(It.Is<string>(s => s == "There are no item events to process.")), Times.Exactly(1));
            mockBulkEventService.Verify(s => s.Run(), Times.Never);
            mockEventArchiver.VerifyGet(m => m.Events, Times.Never, "The EventArchiver should not be called if there are no events to process.");
        }

        [TestMethod]
        public void BulkProcessEvents_ItemEventsAreQueued_EventQueueDictionaryPopulated()
        {
            // Given
            PopulateQueueWithItemEvents();
            int expectedCount = this.queues.QueuedEvents.Select(e => e.RegionCode).Distinct().Count();

            // When
            this.processor.BulkProcessEvents();

            // Then
            int actualCount = this.queues.RegionToEventQueueDictionary.Count;
            Assert.IsTrue(this.queues.RegionToEventQueueDictionary.Count > 0, "The RegionToEventQueue Dictionary is not populated with data");
            Assert.AreEqual(expectedCount, actualCount, "The actual count of regions in the RegionToEventQueue Dictionary does not match the expected count");
        }

        [TestMethod]
        public void BulkProcessEvents_ItemEventsAreQueued_EventQueueDictionaryListValuesAreExpected()
        {
            // Given
            PopulateQueueWithItemEvents();
            PopulateQueueWithNonItemEvents();
            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .ToList();
            int expectedFlCount = expectedQueue.Where(q => q.RegionCode == "FL").Count();
            int expectedMaCount = expectedQueue.Where(q => q.RegionCode == "MA").Count();
            int expectedPnCount = expectedQueue.Where(q => q.RegionCode == "PN").Count();

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(expectedFlCount, this.queues.RegionToEventQueueDictionary["FL"].Count, "The actual count of queued events for FL does not match the expected count");
            Assert.AreEqual(expectedMaCount, this.queues.RegionToEventQueueDictionary["MA"].Count, "The actual count of queued events for MA does not match the expected count");
            Assert.AreEqual(expectedPnCount, this.queues.RegionToEventQueueDictionary["PN"].Count, "The actual count of queued events for PN does not match the expected count");
        }

        [TestMethod]
        public void BulkProcessEvents_ItemEventsAreQueued_BulkItemEventServiceRunExecutedForEachRegion()
        {
            // Given
            PopulateQueueWithItemEvents();
            PopulateQueueWithNonItemEvents();
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());
            int expectedCount = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .Select(q => q.RegionCode)
                .Distinct()
                .Count();

            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(new List<ValidatedItemModel>());
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());

            // When
            this.processor.BulkProcessEvents();

            // Then
            this.mockBulkEventService.Verify(s => s.Run(), Times.Exactly(expectedCount), "BulkItemEventService was not called the expected number of times");
            mockEventArchiver.VerifyGet(m => m.Events, Times.Exactly(expectedCount), "The EventArchiver was not called for each region.");
        }

        [TestMethod]
        public void BulkProcessEvents_SuccessfullyProcessed_EventsRemovedFromEventQueueList()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            PopulateQueueWithItemEvents();
            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = validatedItemModelList.Select(vi => vi.ScanCode).ToList();

            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());

            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId != EventTypes.ItemUpdate && q.EventId != EventTypes.ItemValidation && q.EventId != EventTypes.NewIrmaItem)
                .ToList();

            List<EventQueue> onlyItemQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .ToList();

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(expectedQueue.Count, this.queues.QueuedEvents.Count, "The number of ProcessedEvents were not properly removed from the QueuedEvents.");
            Assert.IsTrue(onlyItemQueue.Intersect(this.queues.QueuedEvents).Count() == 0, "The QueuedEvents list match the expected event list");
        }


        [TestMethod]
        public void BulkProcessEvents_MissingTaxClass_EventsAreRemovedFromEventQueueList()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            PopulateQueueWithItemEvents();
            List<string> validateItemsWithTax = GetTestValidatedItemModelWithTax();
            List<ValidatedItemModel> validateItems = GetTestValidatedItemModel();
            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validateItems);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validateItemsWithTax);
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());
            //int noOfMissingTaxClassCount = validateItems.Count - validateItemsWithTax.Count;

            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId != EventTypes.ItemUpdate && q.EventId != EventTypes.ItemValidation && q.EventId != EventTypes.NewIrmaItem)
                .ToList();

            List<EventQueue> onlyItemQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .ToList();

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(validateItems.Count, this.queues.ProcessedEvents.Count, "The number of ProcessedEvents were not properly removed from the QueuedEvents.");
            Assert.IsTrue(onlyItemQueue.Intersect(this.queues.QueuedEvents).Count() == 0, "The QueuedEvents list match the expected event list");
        }

        [TestMethod]
        public void BulkProcessEvents_DataIssuesIdentifiedByBulkEventService_EventsAreRemovedFromEventQueueList()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            PopulateQueueWithItemEvents();
            List<ValidatedItemModel> validateItems = GetTestValidatedItemModel();
            List<string> validateItemsWithTax = GetTestValidatedItemModelWithTax();
            List<RegionalItemMessageModel> regionalItemMessages = GetTestRegionalItemMessageModel();
            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validateItems);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(new List<string>());
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(regionalItemMessages);

            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId != EventTypes.ItemUpdate && q.EventId != EventTypes.ItemValidation && q.EventId != EventTypes.NewIrmaItem)
                .ToList();

            List<EventQueue> onlyItemQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .ToList();

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(validateItems.Count, this.queues.ProcessedEvents.Count, "The number of ProcessedEvents were not properly removed from the QueuedEvents.");
            Assert.IsTrue(onlyItemQueue.Intersect(this.queues.QueuedEvents).Count() == 0, "The QueuedEvents list match the expected event list");
        }

        [TestMethod]
        public void BulkProcessEvents_ItemEventsAreProcessedWithNoErrors_SuccessfullyProcessedEventsAddedToProcessedEventsList()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            PopulateQueueWithItemEvents();

            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = new List<string>();
            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());
            mockGetIconItemNutritionQueryHandler.Setup(gn => gn.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());
            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .ToList();

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(expectedQueue.Count, this.queues.ProcessedEvents.Count, "The number of ProcessedEvents did not match the expected number");
            Assert.IsTrue(expectedQueue.Except(this.queues.ProcessedEvents).Count() == 0, "The ProcessedEvents list match the expected event list");
        }

        [TestMethod]
        public void BulkProcessEvents_ItemEventsQueued_LoggerInfoCalledTwoTimes()
        {
            // Given
            PopulateQueueWithItemEventsForOneRegion();

            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = validatedItemModelList.Select(vi => vi.ScanCode).ToList();

            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());
            // When
            this.processor.BulkProcessEvents();

            // Then
            this.mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void BulkProcessEvents_ExceptionOccursForTwoQueuedEvent_EventWithErrorAddedToFailedEventList()
        {
            // Given
            int totalEventsForOneRegion = 5;
            int expectedFailedEventCount = 2;
            PopulateQueueWithItemEventsForOneRegion(totalEventsForOneRegion);
            int eventCount = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .Count();

            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(new List<ValidatedItemModel>());
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());
            int count = 0;
            this.mockBulkEventService.Setup(s => s.Run()).Callback(() =>
            {
                count++;
                if (count % 2 == 1 || count == totalEventsForOneRegion + 1) // fail every other one and the last one
                    throw new ArgumentException();
            });

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(expectedFailedEventCount, this.queues.FailedEvents.Count);
            mockEventArchiver.VerifyGet(m => m.Events, Times.Exactly(4), "The EventArchiver was not called for each region.");
        }


        [TestMethod]
        public void BulkProcessEvents_ExceptionOccursForTwoQueuedEvent_ShouldAddSuccessfulEventsToProcessedList()
        {
            // Given
            int totalEventsForOneRegion = 5;
            int expectedSuccessfulEventCount = 3;
            PopulateQueueWithItemEventsForOneRegion(totalEventsForOneRegion);
            int eventCount = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .Count();
            
            mockBulkEventService.SetupGet(i => i.RegionalItemMessage).Returns(new List<RegionalItemMessageModel>());
            int count = 0;
            this.mockBulkEventService.Setup(s => s.Run()).Callback(() =>
            {
                count++;
                if (count % 2 == 1 || count == totalEventsForOneRegion + 1) // fail every other one and the last one
                    throw new ArgumentException();
            });

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(expectedSuccessfulEventCount, this.queues.ProcessedEvents.Count);
        }

        [TestMethod]
        public void BulkProcesItemEvents_ItemEventsAreQueuedNoErrors_ProcessedEventsListHasNoDuplicates()
        {
            // Given
            PopulateQueueWithItemEvents();
            PopulateQueueWithNonItemEvents();

            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = new List<string>();
            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            mockGetIconItemNutritionQueryHandler.Setup(gn => gn.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());
            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.ItemUpdate || q.EventId == EventTypes.ItemValidation || q.EventId == EventTypes.NewIrmaItem)
                .ToList();

            for (int i = 0; i < this.queues.QueuedEvents.Count; i++)
            {
                this.queues.QueuedEvents[i].QueueId = i + 1;
            }

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(0, this.queues.ProcessedEvents.GroupBy(e => e.QueueId).Where(g => g.Count() > 1).Count());
        }

        private void PopulateQueueWithItemEventsForOneRegion(int numberOfEvents)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                EventQueue eventQueue = new TestEventQueueBuilder()
                    .WithEventId(EventTypes.ItemUpdate)
                    .WithEventMessage(String.Format("4134134{0}", i))
                    .WithRegionCode("FL").Build();
                eventQueue.EventType = new EventType { EventId = eventQueue.EventId, EventName = EventConstants.IconItemUpdatedEventName };
                this.queues.QueuedEvents.Add(eventQueue);
            }
        }

        private void PopulateQueueWithItemEventsForOneRegion()
        {
            EventQueue eventOne = new TestEventQueueBuilder().WithEventId(EventTypes.ItemUpdate).WithEventMessage("41341341341").WithRegionCode("FL").Build();
            eventOne.EventType = new EventType { EventId = eventOne.EventId, EventName = EventConstants.IconItemUpdatedEventName };
            this.queues.QueuedEvents.Add(eventOne);

            EventQueue eventTwo = new TestEventQueueBuilder().WithEventId(EventTypes.ItemValidation).WithEventMessage("32165432165").WithRegionCode("FL").Build();
            eventTwo.EventType = new EventType { EventId = eventTwo.EventId, EventName = EventConstants.IconItemValidatedEventName };
            this.queues.QueuedEvents.Add(eventTwo);

            EventQueue eventThree = new TestEventQueueBuilder().WithEventId(EventTypes.NewIrmaItem).WithEventMessage("98765498765").WithRegionCode("FL").Build();
            eventThree.EventType = new EventType { EventId = eventThree.EventId, EventName = EventConstants.IconAlreadyValidatedNewItemRequestEventName };
            this.queues.QueuedEvents.Add(eventThree);
        }

        private void PopulateQueueWithItemEvents()
        {
            EventQueue eventOne = new TestEventQueueBuilder().WithEventId(EventTypes.ItemUpdate).WithEventMessage("41341341341").WithRegionCode("FL").Build();
            eventOne.EventType = new EventType { EventId = eventOne.EventId, EventName = EventConstants.IconItemUpdatedEventName };
            this.queues.QueuedEvents.Add(eventOne);

            EventQueue eventTwo = new TestEventQueueBuilder().WithEventId(EventTypes.ItemValidation).WithEventMessage("32165432165").WithRegionCode("FL").Build();
            eventTwo.EventType = new EventType { EventId = eventTwo.EventId, EventName = EventConstants.IconItemValidatedEventName };
            this.queues.QueuedEvents.Add(eventTwo);

            EventQueue eventThree = new TestEventQueueBuilder().WithEventId(EventTypes.NewIrmaItem).WithEventMessage("98765498765").WithRegionCode("MA").Build();
            eventThree.EventType = new EventType { EventId = eventThree.EventId, EventName = EventConstants.IconAlreadyValidatedNewItemRequestEventName };
            this.queues.QueuedEvents.Add(eventThree);

            EventQueue eventFour = new TestEventQueueBuilder().WithEventId(EventTypes.NewIrmaItem).WithEventMessage("98765498765").WithRegionCode("PN").Build();
            eventFour.EventType = new EventType { EventId = eventFour.EventId, EventName = EventConstants.IconAlreadyValidatedNewItemRequestEventName };
            this.queues.QueuedEvents.Add(eventFour);

            EventQueue eventFive = new TestEventQueueBuilder().WithEventId(EventTypes.ItemValidation).WithEventMessage("14725836914").WithRegionCode("MA").Build();
            eventFive.EventType = new EventType { EventId = eventFive.EventId, EventName = EventConstants.IconItemValidatedEventName };
            this.queues.QueuedEvents.Add(eventFive);

            EventQueue eventSix = new TestEventQueueBuilder().WithEventId(EventTypes.ItemValidation).WithEventMessage("14725836914").WithRegionCode("PN").Build();
            eventSix.EventType = new EventType { EventId = eventSix.EventId, EventName = EventConstants.IconItemValidatedEventName };
            this.queues.QueuedEvents.Add(eventSix);

            EventQueue eventSeven = new TestEventQueueBuilder().WithEventId(EventTypes.ItemValidation).WithEventMessage("36925814725").WithRegionCode("FL").Build();
            eventSeven.EventType = new EventType { EventId = eventSeven.EventId, EventName = EventConstants.IconItemValidatedEventName };
            this.queues.QueuedEvents.Add(eventSeven);
        }

        private void PopulateQueueWithNonItemEvents()
        {
            EventQueue eventOne = new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).WithEventReferenceId(1).WithEventMessage("TestBrand1").WithRegionCode("SW").Build();
            eventOne.EventType = new EventType { EventId = eventOne.EventId, EventName = EventConstants.IconToIrmaBrandNameUpdate };
            this.queues.QueuedEvents.Add(eventOne);

            EventQueue eventTwo = new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).WithEventMessage("5552222").WithRegionCode("SW").Build();
            eventTwo.EventType = new EventType { EventId = eventTwo.EventId, EventName = EventConstants.IconToIrmaTaxClassUpdate };
            this.queues.QueuedEvents.Add(eventTwo);

            EventQueue eventThree = new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).WithEventReferenceId(1).WithEventMessage("TestBrand1").WithRegionCode("FL").Build();
            eventThree.EventType = new EventType { EventId = eventThree.EventId, EventName = EventConstants.IconToIrmaBrandNameUpdate };
            this.queues.QueuedEvents.Add(eventThree);

            EventQueue eventFour = new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).WithEventMessage("5552222").WithRegionCode("MW").Build();
            eventFour.EventType = new EventType { EventId = eventFour.EventId, EventName = EventConstants.IconToIrmaTaxClassUpdate };
            this.queues.QueuedEvents.Add(eventFour);

            EventQueue eventFive = new TestEventQueueBuilder().WithEventId(EventTypes.NewTaxHierarchy).WithEventMessage("4444444").WithRegionCode("FL").Build();
            eventFive.EventType = new EventType { EventId = eventFive.EventId, EventName = EventConstants.IconToIrmaNewTaxClass };
            this.queues.QueuedEvents.Add(eventFive);
        }

        private List<string> GetTestValidatedItemModelWithTax()
        {
            List<string> validatedItems = new List<string>();
            validatedItems.Add("36925814725");

            return validatedItems;
        }

        private List<ValidatedItemModel> GetTestValidatedItemModel()
        {
            List<ValidatedItemModel> validatedItems = new List<ValidatedItemModel>();
            validatedItems.Add(new ValidatedItemModel { ScanCode = "41341341341" });
            validatedItems.Add(new ValidatedItemModel { ScanCode = "32165432165" });
            validatedItems.Add(new ValidatedItemModel { ScanCode = "98765498765" });
            validatedItems.Add(new ValidatedItemModel { ScanCode = "98765498765" });
            validatedItems.Add(new ValidatedItemModel { ScanCode = "14725836914" });
            validatedItems.Add(new ValidatedItemModel { ScanCode = "14725836914" });
            validatedItems.Add(new ValidatedItemModel { ScanCode = "36925814725" });

            return validatedItems;
        }

        private List<RegionalItemMessageModel> GetTestRegionalItemMessageModel()
        {
            List<RegionalItemMessageModel> regionalItemMessages = new List<RegionalItemMessageModel>();
            regionalItemMessages.Add(new RegionalItemMessageModel {  Identifier = "41341341341", Message = "Tax Class doesn’t exist with tax name - 0001111 Test Tax", RegionCode = "FL" });
            regionalItemMessages.Add(new RegionalItemMessageModel { Identifier = "98765498765", Message = "Tax Class doesn’t exist with tax name - 0001111 Test Tax", RegionCode = "FL" });
            regionalItemMessages.Add(new RegionalItemMessageModel { Identifier = "41341341341", Message = "Retail UOM doesn't exist with UOM abbreviation - XXX", RegionCode = "FL" });
            regionalItemMessages.Add(new RegionalItemMessageModel { Identifier = "98765498765", Message = "Retail UOM doesn't exist with UOM abbreviation - XXX", RegionCode = "FL" });
            regionalItemMessages.Add(new RegionalItemMessageModel { Identifier = "14725836914", Message = "National Class doesn't exist with ClassId - 0000", RegionCode = "FL" });
         
            return regionalItemMessages;
        }
    }
}
