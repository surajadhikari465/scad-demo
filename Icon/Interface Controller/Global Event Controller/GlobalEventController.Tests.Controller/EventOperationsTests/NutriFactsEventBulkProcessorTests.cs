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

namespace GlobalEventController.Tests.Controller.EventOperationsTests
{
    [TestClass]
    public class NutriFactsEventBulkProcessorTests
    {
        private NutriFactsEventBulkProcessor processor;
        private EventQueues queues;
        private Mock<ILogger<ItemEventBulkProcessor>> mockLogger;
        private Mock<IEventServiceProvider> mockServiceProvider;
        private Mock<IBulkEventService> mockBulkEventService;
        private Mock<IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>> mockBulkGetScanCodeQuery;
        private Mock<IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>>> mockGetIconItemNutritionQueryHandler;
        private Mock<IEventArchiver> mockEventArchiver;

        [TestInitialize]
        public void InitializeData()
        {
            this.queues = new EventQueues();
            this.mockLogger = new Mock<ILogger<ItemEventBulkProcessor>>();
            this.mockServiceProvider = new Mock<IEventServiceProvider>();
            this.mockBulkGetScanCodeQuery = new Mock<IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>>();
            this.mockGetIconItemNutritionQueryHandler = new Mock<IQueryHandler<GetIconItemNutritionQuery, List<ItemNutrition>>>();
            this.mockEventArchiver = new Mock<IEventArchiver>();

            this.processor = new NutriFactsEventBulkProcessor(this.queues,
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockBulkGetScanCodeQuery.Object,
                this.mockGetIconItemNutritionQueryHandler.Object,
                this.mockEventArchiver.Object);

            this.mockBulkEventService = new Mock<IBulkEventService>();
            this.mockServiceProvider.Setup(p => p.GetBulkItemNutriFactsEventService(It.IsAny<string>())).Returns(this.mockBulkEventService.Object);
            this.mockEventArchiver.SetupGet(m => m.Events).Returns(new List<EventQueueArchive>());
            StartupOptions.NutritionEnabledRegions = new List<string>() { "FL", "MA", "PN"};
        }

        [TestMethod]
        public void ProcessNutritionEvents_NoQueuedItemEvents_LoggerInfoCalled()
        {
            // Given
            PopulateQueueWithNonNutritionEvents();

            // When
            processor.BulkProcessEvents();

            // Then
            mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(1));
            mockEventArchiver.VerifyGet(m => m.Events, Times.Never, "The EventArchiver should not be called if there are no events to process.");
        }

        [TestMethod]
        public void ProcessNutritionEvents_NutritionEventsAreQueued_EventQueueDictionaryPopulated()
        {
            // Given
            PopulateQueueWithNutritionEvents();
            int expectedRegionCount = this.queues.QueuedEvents.Select(e => e.RegionCode).Distinct().Count();
            int expectedEventCount = this.queues.QueuedEvents.Count;

            // When
            this.processor.BulkProcessEvents();

            // Then
            int actualRegionCount = this.queues.RegionToEventQueueDictionary.Count;
            Assert.IsTrue(this.queues.RegionToEventQueueDictionary.Count > 0, "The RegionToEventQueue Dictionary is not populated with data");
            Assert.AreEqual(expectedRegionCount, actualRegionCount, "The actual count of regions in the RegionToEventQueue Dictionary does not match the expected count");
            mockEventArchiver.VerifyGet(m => m.Events, Times.Exactly(expectedEventCount), "The EventArchiver was not called for each event.");
        }

        [TestMethod]
        public void ProcessNutritionEvents_NutritionEventsAreQueued_EventQueueDictionaryListValuesAreExpected()
        {
            // Given
            PopulateQueueWithNutritionEvents();
            PopulateQueueWithNonNutritionEvents();
            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.NutritionUpdate || q.EventId == EventTypes.NutritionAdd)
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
            mockEventArchiver.VerifyGet(m => m.Events, Times.Exactly(expectedQueue.Count), "The EventArchiver was not called for each event.");
        }

        [TestMethod]
        public void ProcessNutritionEvents_NutritionEventsAreQueued_BulkNutritionEventServiceRunExecutedForEachRegion()
        {
            // Given
            PopulateQueueWithNutritionEvents();
            PopulateQueueWithNonNutritionEvents();
            int expectedCount = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.NutritionUpdate || q.EventId == EventTypes.NutritionAdd)
                .Select(q => q.RegionCode)
                .Distinct()
                .Count();
            mockGetIconItemNutritionQueryHandler.Setup(gn => gn.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());
            
            // When
            this.processor.BulkProcessEvents();

            // Then
            this.mockBulkEventService.Verify(s => s.Run(), Times.Exactly(expectedCount), "BulkNutritionEventService was not called the expected number of times");
        }

        [TestMethod]
        public void BulkProcessNutritionEvents_SuccessfullyProcessed_EventsRemovedFromEventQueueList()
        {
            // Given
            PopulateQueueWithNonNutritionEvents();
            PopulateQueueWithNutritionEvents();
            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = validatedItemModelList.Select(vi => vi.ScanCode).ToList();
            List<NutriFactsModel> itemNutritionList = validatedItemModelList.Select(vi => new NutriFactsModel() { Plu = vi.ScanCode }).ToList();

            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            mockBulkEventService.SetupGet(i => i.ItemNutriFacts).Returns(itemNutritionList);
            mockGetIconItemNutritionQueryHandler.Setup(gn => gn.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());

            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId != EventTypes.NutritionUpdate && q.EventId != EventTypes.NutritionAdd)
                .ToList();

            List<EventQueue> onlyNutritionQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.NutritionUpdate || q.EventId == EventTypes.NutritionAdd)
                .ToList();

            int numberOfRegions = onlyNutritionQueue.Select(q => q.RegionCode).Distinct().Count();

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(expectedQueue.Count, this.queues.QueuedEvents.Count, "The number of ProcessedEvents were not properly removed from the QueuedEvents.");
            Assert.IsTrue(onlyNutritionQueue.Intersect(this.queues.QueuedEvents).Count() == 0, "The QueuedEvents list match the expected event list");
            mockEventArchiver.VerifyGet(m => m.Events, Times.Exactly(numberOfRegions), "The EventArchiver was not called for each region.");
        }

        [TestMethod]
        public void BulkProcessNutritionEvents_NutritionEventsAreQueuedNoErrors_SuccessfullyProcessedEventsAddedToProcessedEventsList()
        {
            // Given
            PopulateQueueWithNonNutritionEvents();
            PopulateQueueWithNutritionEvents();

            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = new List<string>();
            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            mockGetIconItemNutritionQueryHandler.Setup(gn => gn.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());
            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.NutritionAdd || q.EventId == EventTypes.NutritionUpdate)
                .ToList();
            int numberOfRegions = expectedQueue.Select(q => q.RegionCode).Distinct().Count();

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(expectedQueue.Count, this.queues.ProcessedEvents.Count, "The number of ProcessedEvents did not match the expected number");
            Assert.IsTrue(expectedQueue.Except(this.queues.ProcessedEvents).Count() == 0, "The ProcessedEvents list match the expected event list");
        }

        [TestMethod]
        public void BulkProcessNutritionEvents_NutritionEventsAreQueuedNoErrors_ProcessedEventsListHasNoDuplicates()
        {
            // Given
            PopulateQueueWithNonNutritionEvents();
            PopulateQueueWithNutritionEvents();

            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = new List<string>();
            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            mockGetIconItemNutritionQueryHandler.Setup(gn => gn.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());
            List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.NutritionAdd || q.EventId == EventTypes.NutritionUpdate)
                .ToList();

            for (int i = 0; i < this.queues.QueuedEvents.Count; i++)
            {
                this.queues.QueuedEvents[i].QueueId = i+1;
            }

            // When
            this.processor.BulkProcessEvents();

            // Then
            Assert.AreEqual(0, this.queues.ProcessedEvents.GroupBy(e => e.QueueId).Where(g => g.Count() > 1).Count());
        }

        [TestMethod]
        public void BulkProcessNutritionEvents_ExceptionOccursForTwoQueuedEvent_EventsWithErrorAddedToFailedEventList()
        {
            // Given
            int totalEventsForOneRegion = 5;
            int expectedFailedEventsCount = 2;
            PopulateQueueWithNutritionEventsForOneRegion(totalEventsForOneRegion);

            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = validatedItemModelList.Select(vi => vi.ScanCode).ToList();

            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            List<NutriFactsModel> itemNutritionList = validatedItemModelList.Select(vi => new NutriFactsModel() { Plu = vi.ScanCode }).ToList();
            mockBulkEventService.SetupGet(i => i.ItemNutriFacts).Returns(itemNutritionList);
            mockGetIconItemNutritionQueryHandler.Setup(qh => qh.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());

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
            Assert.AreEqual(expectedFailedEventsCount, this.queues.FailedEvents.Count);
        }

        [TestMethod]
        public void BulkProcessNutritionEvents_ExceptionOccursForTwoQueuedEvent_SuccessfulEventsAreMarkedProcessed()
        {
            // Given
            int totalEventsForOneRegion = 5;
            int expectedSuccessfulEventCount = 3;
            PopulateQueueWithNutritionEventsForOneRegion(totalEventsForOneRegion);

            List<ValidatedItemModel> validatedItemModelList = GetTestValidatedItemModel();
            List<string> validatedItemsWithTax = validatedItemModelList.Select(vi => vi.ScanCode).ToList();

            mockBulkEventService.SetupGet(i => i.ValidatedItemList).Returns(validatedItemModelList);
            mockBulkEventService.SetupGet(i => i.ScanCodesWithNoTaxList).Returns(validatedItemsWithTax);
            List<NutriFactsModel> itemNutritionList = validatedItemModelList.Select(vi => new NutriFactsModel() { Plu = vi.ScanCode }).ToList();
            mockBulkEventService.SetupGet(i => i.ItemNutriFacts).Returns(itemNutritionList);
            mockGetIconItemNutritionQueryHandler.Setup(qh => qh.Handle(It.IsAny<GetIconItemNutritionQuery>())).Returns(new List<ItemNutrition>());

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

        private void PopulateQueueWithNutritionEventsForOneRegion(int numberOfEvents)
        {
            for (int i = 0; i < numberOfEvents; i++)
            {
                EventQueue eventQueue = new TestEventQueueBuilder()
                    .WithEventId(EventTypes.NutritionUpdate)
                    .WithEventMessage(String.Format("4134134{0}", i))
                    .WithRegionCode("FL").Build();
                eventQueue.EventType = new EventType { EventId = eventQueue.EventId, EventName = EventConstants.ItemNutritionAdd };
                this.queues.QueuedEvents.Add(eventQueue);
            }
        }

        private void PopulateQueueWithNutritionEventsForOneRegion()
        {
            EventQueue eventOne = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionUpdate).WithEventMessage("41341341341").WithRegionCode("FL").Build();
            eventOne.EventType = new EventType { EventId = eventOne.EventId, EventName = EventConstants.ItemNutritionAdd };
            this.queues.QueuedEvents.Add(eventOne);

            EventQueue eventTwo = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionAdd).WithEventMessage("32165432165").WithRegionCode("FL").Build();
            eventTwo.EventType = new EventType { EventId = eventTwo.EventId, EventName = EventConstants.ItemNutritionAdd };
            this.queues.QueuedEvents.Add(eventTwo);

            EventQueue eventThree = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionAdd).WithEventMessage("98765498765").WithRegionCode("FL").Build();
            eventThree.EventType = new EventType { EventId = eventThree.EventId, EventName = EventConstants.ItemNutritionUpdate };
            this.queues.QueuedEvents.Add(eventThree);

            EventQueue eventFour = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionAdd).WithEventMessage("98765498766").WithRegionCode("FL").Build();
            eventThree.EventType = new EventType { EventId = eventFour.EventId, EventName = EventConstants.ItemNutritionUpdate };
            this.queues.QueuedEvents.Add(eventThree);

            EventQueue eventFive = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionAdd).WithEventMessage("98765498767").WithRegionCode("FL").Build();
            eventThree.EventType = new EventType { EventId = eventFive.EventId, EventName = EventConstants.ItemNutritionUpdate };
            this.queues.QueuedEvents.Add(eventThree);
        }

        private void PopulateQueueWithNutritionEvents()
        {
            EventQueue eventOne = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionAdd).WithEventMessage("41341341341").WithRegionCode("FL").Build();
            eventOne.EventType = new EventType { EventId = eventOne.EventId, EventName = EventConstants.IconItemUpdatedEventName };
            this.queues.QueuedEvents.Add(eventOne);

            EventQueue eventTwo = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionUpdate).WithEventMessage("32165432165").WithRegionCode("FL").Build();
            eventTwo.EventType = new EventType { EventId = eventTwo.EventId, EventName = EventConstants.IconItemValidatedEventName };
            this.queues.QueuedEvents.Add(eventTwo);

            EventQueue eventThree = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionAdd).WithEventMessage("98765498765").WithRegionCode("MA").Build();
            eventThree.EventType = new EventType { EventId = eventThree.EventId, EventName = EventConstants.IconAlreadyValidatedNewItemRequestEventName };
            this.queues.QueuedEvents.Add(eventThree);

            EventQueue eventFour = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionUpdate).WithEventMessage("98765498765").WithRegionCode("PN").Build();
            eventFour.EventType = new EventType { EventId = eventFour.EventId, EventName = EventConstants.IconAlreadyValidatedNewItemRequestEventName };
            this.queues.QueuedEvents.Add(eventFour);

            EventQueue eventFive = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionUpdate).WithEventMessage("14725836914").WithRegionCode("MA").Build();
            eventFive.EventType = new EventType { EventId = eventFive.EventId, EventName = EventConstants.ItemNutritionAdd };
            this.queues.QueuedEvents.Add(eventFive);

            EventQueue eventSix = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionUpdate).WithEventMessage("14725836914").WithRegionCode("PN").Build();
            eventSix.EventType = new EventType { EventId = eventSix.EventId, EventName = EventConstants.ItemNutritionAdd };
            this.queues.QueuedEvents.Add(eventSix);

            EventQueue eventSeven = new TestEventQueueBuilder().WithEventId(EventTypes.NutritionAdd).WithEventMessage("36925814725").WithRegionCode("FL").Build();
            eventSeven.EventType = new EventType { EventId = eventSeven.EventId, EventName = EventConstants.ItemNutritionAdd };
            this.queues.QueuedEvents.Add(eventSeven);
        }

        private void PopulateQueueWithNonNutritionEvents()
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
    }
}
