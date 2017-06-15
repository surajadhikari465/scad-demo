using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalEventController.Controller.EventOperations;
using Icon.Logging;
using Moq;
using GlobalEventController.Controller.EventServices;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Framework;
using System.Collections.Generic;
using System.Linq;
using Icon.Testing.Builders;
using System.Data.Entity;
using GlobalEventController.Common;
using GlobalEventController.DataAccess.BulkCommands;
using InterfaceController.Common;

namespace GlobalEventController.Tests.Controller.EventOperationsTests
{
	[TestClass]
	public class EventProcessorTests
	{
		private EventProcessor processor;
		private EventQueues queues;
		private Mock<ILogger<EventProcessor>> mockLogger;
		private Mock<IEventServiceProvider> mockServiceProvider;
		private Mock<IEventService> mockEventService;
		private Mock<IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>> mockBulkGetScanCodeQuery;
        private Mock<IDataIssueMessageCollector> mockDataIssueMessage;
        private Mock<IEventArchiver> mockEventArchiver;

        [TestInitialize]
        public void InitializeData()
        {
            this.queues = new EventQueues();
            this.mockLogger = new Mock<ILogger<EventProcessor>>();
            this.mockServiceProvider = new Mock<IEventServiceProvider>();
            this.mockBulkGetScanCodeQuery = new Mock<IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>>();
            this.mockDataIssueMessage = new Mock<IDataIssueMessageCollector>();
            this.mockEventArchiver = new Mock<IEventArchiver>();

            this.processor = new EventProcessor(this.queues,
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockDataIssueMessage.Object,
                this.mockEventArchiver.Object);

            this.mockEventService = new Mock<IEventService>();
            this.mockServiceProvider.Setup( p => p
                .GetEventService( It.IsAny<Enums.EventNames>(), It.IsAny<string>()))
                .Returns(mockEventService.Object);
            this.mockEventArchiver.SetupGet(m => m.Events)
                .Returns(new List<EventQueueArchive>());
        }

        [TestMethod]
		public void ProcessBrandEvents_NoBrandEventsFound_LoggerInfoCalledOnceAndReturnsNothing()
		{
			// Given
			PopulateQueueWithItemEvents();
			
			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockLogger.Verify(l => l.Info(It.IsAny<string>()),
                Times.Once, "Logger Info was not called exactly one time.");
            this.mockEventArchiver.Verify(m => m.Events,
                Times.Never);
		}

		[TestMethod]
		public void ProcessBrandEvents_BrandEventsFound_BrandEventServiceCalledForEachBrandEvents()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents
                .Count(e => e.EventId == EventTypes.BrandNameUpdate);

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockEventService.Verify(s => s.Run(),
                Times.Exactly(expectedCount), "EventService did not Run() for each event.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedCount), "The EventArchiver was not called for each event");
        }

        [TestMethod]
        public void ProcessBrandEvents_ExceptionThrownDuringOneBrandEventService_EventServiceCalledForEachQueuedEvent()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            int expectedCount = this.queues.QueuedEvents
                .Count(q => q.EventId == EventTypes.BrandNameUpdate);

            bool firstTimeCalled = true;
            this.mockEventService.Setup(e => e.Run()).Callback(() =>
            {
                if (firstTimeCalled)
                {
                    firstTimeCalled = false;
                    throw new ArgumentException();
                }
            });

            // When
            this.processor.ProcessBrandNameUpdateEvents();

            // Then
            this.mockEventService.Verify(s => s.Run(),
                Times.Exactly(expectedCount), "EventService was not called for each queued brand event.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedCount), "The EventArchiver was not called for each event");
        }

        [TestMethod]
		public void ProcessBrandEvents_BrandEventsFound_LoggerInfoCalledOnceForEachItem()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents
                .Count(e => e.EventId == EventTypes.BrandNameUpdate);
            string expectedLogInfoMsg = "Successfully processed event: IconToIrmaBrandNameUpdate. " +
                " Region = XX, ReferenceId = 1, Message = TestBrand1";

            this.mockEventService.SetupGet(s => s.RegionalItemMessage)
                .Returns(Enumerable.Empty<RegionalItemMessageModel>().ToList());
            var loggedInfo = new List<string>();
            this.mockLogger.Setup(log => log.Info(It.IsAny<string>()))
                .Callback<string>(msg => loggedInfo.Add(msg));

			// When
			this.processor.ProcessBrandNameUpdateEvents();

            // Then
            this.mockLogger.Verify( l => l.Info(expectedLogInfoMsg.Replace("XX", "SW")),
                Times.Once, "Logger Info was not called once per queued event.");
            this.mockLogger.Verify( l => l.Info(expectedLogInfoMsg.Replace("XX", "FL")),
                Times.Once, "Logger Info was not called once per queued event.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedCount), "The EventArchiver was not called for each event");
        }

        [TestMethod]
		public void ProcessBrandEvents_ExceptionThrownDuringBrandEventService_LoggerErrorCalledTwice()
		{
            // Given
            EventQueue eventQueue = new TestEventQueueBuilder()
                .WithEventId(EventTypes.BrandNameUpdate).WithEventMessage("test").Build();
			eventQueue.EventType = new EventType
            {
                EventId = EventTypes.BrandNameUpdate,
                EventName = "Brand Name Update"
            };
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockEventService.Setup(s => s.Run()).Callback(() => { throw new Exception(); });

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()),
                Times.Exactly(2), "Logger.Error was not called exactly two times.");
            this.mockEventArchiver.Verify(m => m.Events, Times.Once);
        }

        [TestMethod]
		public void ProcessBrandEvents_ExceptionThrownDuringOneBrandEventService_ExceptionNotThrownAndEventServiceRunCalledForEachEvent()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents
                .Count(q => q.EventId == EventTypes.BrandNameUpdate);

			bool firstTimeCalled = true;
			this.mockEventService.Setup(s => s.Run()).Callback(() =>
				{
					if (firstTimeCalled)
					{
						firstTimeCalled = false;
						throw new ArgumentException();
					}
				});

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockEventService.Verify(s => s.Run(),
                Times.Exactly(expectedCount), "The EventService was not called for each event.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedCount), "The EventArchiver was not called for each event");
        }

        [TestMethod]
		public void ProcessBrandEvents_EventNameNotMapped_LoggerErrorCalledTwice()
		{
            // Given
            EventQueue eventQueue = new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).Build();
            eventQueue.EventType = new EventType { EventId = 10, EventName = "test" };
            this.queues.QueuedEvents.Add(eventQueue);

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()),
                Times.Exactly(2), "Logger Error not called exactly twice.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Once, "The EventArchiver was not called for each event");
        }

        [TestMethod]
		public void ProcessBrandEvents_NoEventServiceFoundForEventType_LoggerErrorCalledTwice()
		{
			// Given
			EventQueue eventQueue = new TestEventQueueBuilder()
                .WithEventId(EventTypes.BrandNameUpdate).Build();
			eventQueue.EventType = new EventType
            {
                EventId = EventTypes.BrandNameUpdate,
                EventName = "Brand Name Update"
            };
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockServiceProvider = new Mock<IEventServiceProvider>();
            this.mockEventArchiver = new Mock<IEventArchiver>();
            this.mockEventArchiver.SetupGet(m => m.Events)
                .Returns(new List<EventQueueArchive>());
			this.processor = new EventProcessor(this.queues,
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockDataIssueMessage.Object,
                mockEventArchiver.Object);

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			mockLogger.Verify(l => l.Error(It.IsAny<string>()),
                Times.Exactly(2), "Logger is not called exactly twice.");
		}

		[TestMethod]
		public void ProcessBrandEvents_BrandEventsAreQueuedNoErrors_SuccessfullyProcessedEventsAddedToProcessedEventsList()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			PopulateQueueWithItemEvents();

			List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.BrandNameUpdate).ToList();

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			Assert.AreEqual(expectedQueue.Count, this.queues.ProcessedEvents.Count,
                "The number of ProcessedEvents did not match the expected number");
			Assert.IsTrue(expectedQueue.Except(this.queues.ProcessedEvents).Count() == 0,
                "The ProcessedEvents list match the expected event list");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedQueue.Count), "The EventArchiver was not called for each event");
        }

        [TestMethod]
		public void ProcessBrandEvents_ExceptionThrownDuringBrandEventService_FailedEventsAddedToFailedEventsQueueList()
		{
			// Given
			PopulateQueueWithNonItemEvents();

			// Throw exception the first time, then successfully update status
			int expectedCount = this.queues.QueuedEvents
                .Count(q => q.EventId == EventTypes.BrandNameUpdate);
            this.mockEventService.Setup(e => e.Run()).Callback(() =>
               {
                   throw new ArgumentException();
               });

            // When
            this.processor.ProcessBrandNameUpdateEvents();

			// Then
			Assert.AreEqual(expectedCount,
                this.queues.FailedEvents.Count, "The events that failed were not added to the FailedEvents list properly.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(this.queues.FailedEvents.Count), "The EventArchiver was not called for each event");
        }

        [TestMethod]
        public void ProcessBrandEvents_BrandEventsProcessedSuccessfully_EventsRemovedFromEventQueueList()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            List<EventQueue> brandEventList = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.BrandNameUpdate).ToList();

            // When
            this.processor.ProcessBrandNameUpdateEvents();

            // Then
            foreach (var brandEvent in brandEventList)
            {
                Assert.IsFalse(this.queues.QueuedEvents.Contains(brandEvent));
            }
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(brandEventList.Count), "The EventArchiver was not called for each event");
        }

        [TestMethod]
		public void ProcessTaxEvents_NoTaxEventsFound_LoggerInfoCalledOnceAndReturnsNothing()
		{
			// Given
			PopulateQueueWithItemEvents();

			// When
			this.processor.ProcessTaxEvents();

			// Then
			mockLogger.Verify(l => l.Info(It.IsAny<string>()),
                Times.Once, "Logger.Info was not called one time.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Never, "The EventArchiver should not be called if there are not events to process.");
        }

        [TestMethod]
		public void ProcessTaxEvents_TaxEventsFound_TaxEventServiceCalledForEachQueuedTaxEvent()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents
                .Count(q => q.EventId == EventTypes.NewTaxHierarchy || q.EventId == EventTypes.TaxNameUpdate);

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockEventService.Verify(s => s.Run(),
                Times.Exactly(expectedCount), "EventService did not run for each queued event.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedCount), "The EventArchiver was not called for each event.");
        }

        [TestMethod]
        public void ProcessTaxEvents_ExceptionThrownDuringOneTaxEventService_EventServiceStillCalledForEachQueuedEvent()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            int expectedCount = this.queues.QueuedEvents
                .Count(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy);

            bool firstTimeCalled = true;
            this.mockEventService.Setup(e => e.Run()).Callback(() =>
            {
                if (firstTimeCalled)
                {
                    firstTimeCalled = false;
                    throw new ArgumentException();
                }
            });

            // When
            this.processor.ProcessTaxEvents();

            // Then
            this.mockEventService.Verify(s => s.Run(),
                Times.Exactly(expectedCount), "EventService was not called for each queued tax event.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedCount), "The EventArchiver was not called for each event.");
        }

        [TestMethod]
		public void ProcessTaxEvents_TaxEventsFound_LoggerInfoCalledOnceForEachItem()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents
                .Count(q => q.EventId == EventTypes.NewTaxHierarchy || q.EventId == EventTypes.TaxNameUpdate);
            this.mockEventService.SetupGet(s => s.RegionalItemMessage)
                .Returns(Enumerable.Empty<RegionalItemMessageModel>().ToList());

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockLogger.Verify(l => l.Info(It.IsAny<string>()),
                Times.Exactly(expectedCount), "Logger Info did not run for each queued event.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(expectedCount), "The EventArchiver was not called for each event.");
        }

        [TestMethod]
		public void ProcessTaxEvents_ExceptionThrownDuringTaxEventService_LoggerErrorCalledTwice()
		{
            // Given
            EventQueue eventQueue = CreateEvent(EventTypes.TaxNameUpdate, EventConstants.IconToIrmaTaxClassUpdate);
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockEventService.Setup(s => s.Run())
                .Callback(() => { throw new ArgumentException(); });

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()),
                Times.Exactly(2), "Logger Error did not run twice after exception.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Once, "The EventArchiver was not called for each event.");
        }

        [TestMethod]
		public void ProcessTaxEvents_EventNameNotMapped_LoggerErrorCalledTwice()
		{
            // Given
            EventQueue eventQueue = CreateEvent(EventTypes.TaxNameUpdate, "test");
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockEventService.Setup(s => s.Run())
                .Callback(() => { throw new ArgumentException(); });

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()),
                Times.Exactly(2), "Logger Error was not called two time.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Once, "The EventArchiver was not called for each event.");
        }

        [TestMethod]
		public void ProcessTaxEvents_NoEventServiceFoundForEventType__LoggerErrorCalledTwice()
		{
			// Given
            EventQueue eventQueue = CreateEvent(EventTypes.TaxNameUpdate, EventConstants.IconToIrmaTaxClassUpdate);
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockServiceProvider = new Mock<IEventServiceProvider>();
            this.mockEventArchiver = new Mock<IEventArchiver>();
            this.mockEventArchiver.SetupGet(m => m.Events)
                .Returns(new List<EventQueueArchive>());
			this.processor = new EventProcessor(this.queues,
                this.mockLogger.Object,
                this.mockServiceProvider.Object,
                this.mockDataIssueMessage.Object,
                mockEventArchiver.Object);

			// When
			this.processor.ProcessTaxEvents();

			// Then
			mockLogger.Verify(l => l.Error(It.IsAny<string>()),
                Times.Exactly(2), "Logger is not called exactly twice.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Once, "The EventArchiver was not called for each event.");
        }

        [TestMethod]
		public void ProcessTaxEvents_TaxEventsAreQueuedNoErrors_SuccessfullyProcessedEventsAddedToProcessedEventsList()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			PopulateQueueWithItemEvents();

			List<EventQueue> expectedQueue = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy)
                .ToList();

			// When
			this.processor.ProcessTaxEvents();

			// Then
			Assert.AreEqual(expectedQueue.Count, this.queues.ProcessedEvents.Count,
                "The number of ProcessedEvents did not match the expected number");
			Assert.IsTrue(expectedQueue.Except(this.queues.ProcessedEvents).Count() == 0,
                "The ProcessedEvents list match the expected event list");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(this.queues.ProcessedEvents.Count), "The EventArchiver was not called for each event.");
        }

        [TestMethod]
		public void ProcessTaxEvents_ExceptionThrownDuringTaxEventService_EventsAddedToFailedEventsQueueList()
		{
			// Given
			PopulateQueueWithNonItemEvents();

			// Throw exception the first time, then successfully update status
			int expectedCount = this.queues.QueuedEvents
                .Count(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy);
			this.mockEventService.Setup(e => e.Run()) .Callback(() =>
				{
					throw new ArgumentException();
				});

			// When
			this.processor.ProcessTaxEvents();

			// Then
			Assert.AreEqual(expectedCount, this.queues.FailedEvents.Count,
                "The events that failed were not added to the FailedEvents list properly.");
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(this.queues.FailedEvents.Count), "The EventArchiver was not called for each event.");
        }

        [TestMethod]
        public void ProcessTaxEvents_TaxEventsProcessedSuccessfully_EventsRemovedFromEventQueueList()
		{
			// Given
			PopulateQueueWithNonItemEvents();
            List<EventQueue> taxEventsList = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy)
                .ToList();

			// When
			this.processor.ProcessTaxEvents();

			// Then
			foreach (var taxEvent in taxEventsList)
			{
				Assert.IsFalse(this.queues.QueuedEvents.Contains(taxEvent));
			}
            this.mockEventArchiver.VerifyGet(m => m.Events,
                Times.Exactly(taxEventsList.Count), "The EventArchiver was not called for each event.");
        }

        private EventQueue CreateEvent(int id, string name)
        {
            EventQueue eventQueue = new TestEventQueueBuilder()
                .WithEventId(id)
                .Build();
            eventQueue.EventType = new EventType
            {
                EventId = eventQueue.EventId,
                EventName = name
            };
            return eventQueue;
        }

        private EventQueue CreateEvent(int id, string message, string region, string name, int? eventReferenceId = null)
        {
            EventQueue eventQueue = new TestEventQueueBuilder()
                .WithEventId(id)
                .WithEventMessage(message)
                .WithRegionCode(region)
                .Build();
            if (eventReferenceId.HasValue)
            {
                eventQueue.EventReferenceId = eventReferenceId.Value;
            }
            eventQueue.EventType = new EventType
            {
                EventId = eventQueue.EventId,
                EventName = name
            };
            return eventQueue;
        }

        private void PopulateQueueWithItemEvents()
		{
            var eventOne = CreateEvent(
                    id: EventTypes.ItemUpdate,
                    message: "41341341341",
                    region: "FL",
                    name: EventConstants.IconItemUpdatedEventName); 
            this.queues.QueuedEvents.Add(eventOne);

            var eventTwo = CreateEvent(
                    id: EventTypes.ItemValidation,
                    message: "32165432165",
                    region: "FL",
                    name: EventConstants.IconItemValidatedEventName);
			this.queues.QueuedEvents.Add(eventTwo);

            var eventThree = CreateEvent(
                    id: EventTypes.NewIrmaItem,
                    message: "98765498765",
                    region: "MA",
                    name: EventConstants.IconAlreadyValidatedNewItemRequestEventName);
			this.queues.QueuedEvents.Add(eventThree);

            var eventFour = CreateEvent(
                    id: EventTypes.NewIrmaItem,
                    message: "98765498765",
                    region: "PN",
                    name: EventConstants.IconAlreadyValidatedNewItemRequestEventName);
            this.queues.QueuedEvents.Add(eventFour);

            var eventFive = CreateEvent(
                    id: EventTypes.ItemValidation,
                    message: "14725836914",
                    region: "MA",
                    name: EventConstants.IconItemValidatedEventName);
            this.queues.QueuedEvents.Add(eventFive);

            var eventSix = CreateEvent(
                    id: EventTypes.ItemValidation,
                    message: "14725836914",
                    region: "PN",
                    name: EventConstants.IconItemValidatedEventName);
            this.queues.QueuedEvents.Add(eventSix);

            var eventSeven = CreateEvent(
                    id: EventTypes.ItemValidation,
                    message: "36925814725",
                    region: "FL",
                    name: EventConstants.IconItemValidatedEventName);
            this.queues.QueuedEvents.Add(eventSeven);
		}

		private void PopulateQueueWithNonItemEvents()
		{
            var eventOne = CreateEvent(
                    id: EventTypes.BrandNameUpdate,
                    message: "TestBrand1",
                    region: "SW",
                    name: EventConstants.IconToIrmaBrandNameUpdate,
                    eventReferenceId: 1);
            this.queues.QueuedEvents.Add(eventOne);

            var eventTwo = CreateEvent(
                    id: EventTypes.TaxNameUpdate,
                    message: "5552222",
                    region: "SW",
                    name: EventConstants.IconToIrmaTaxClassUpdate);
            this.queues.QueuedEvents.Add(eventTwo);

            var eventThree = CreateEvent(
                    id: EventTypes.BrandNameUpdate,
                    message: "TestBrand1",
                    region: "FL",
                    name: EventConstants.IconToIrmaBrandNameUpdate,
                    eventReferenceId: 1);
            this.queues.QueuedEvents.Add(eventThree);

            var eventFour = CreateEvent(
                    id: EventTypes.TaxNameUpdate,
                    message: "5552222",
                    region: "MW",
                    name: EventConstants.IconToIrmaTaxClassUpdate);
            this.queues.QueuedEvents.Add(eventFour);

            var eventFive = CreateEvent(
                    id: EventTypes.NewTaxHierarchy,
                    message: "4444444",
                    region: "FL",
                    name: EventConstants.IconToIrmaTaxClassUpdate);
            this.queues.QueuedEvents.Add(eventFive);
		}
	}
}
