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

        [TestInitialize]
		public void InitializeData()
		{
			this.queues = new EventQueues();
			this.mockLogger = new Mock<ILogger<EventProcessor>>();
			this.mockServiceProvider = new Mock<IEventServiceProvider>();
			this.mockBulkGetScanCodeQuery = new Mock<IQueryHandler<BulkGetValidatedItemsQuery, List<ValidatedItemModel>>>();
            this.mockDataIssueMessage = new Mock<IDataIssueMessageCollector>();

            this.processor = new EventProcessor(this.queues,
				this.mockLogger.Object,
				this.mockServiceProvider.Object,
                this.mockDataIssueMessage.Object);

			this.mockEventService = new Mock<IEventService>();
			this.mockServiceProvider.Setup(p => p.GetEventService(It.IsAny<Enums.EventNames>(), It.IsAny<string>())).Returns(mockEventService.Object);
		}

		[TestMethod]
		public void ProcessBrandEvents_NoBrandEventsFound_LoggerInfoCalledOnceAndReturnsNothing()
		{
			// Given
			PopulateQueueWithItemEvents();
			
			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once, "Logger Info was not called exactly one time.");
		}

		[TestMethod]
		public void ProcessBrandEvents_BrandEventsFound_BrandEventServiceCalledForEachBrandEvents()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents.Where(e => e.EventId == EventTypes.BrandNameUpdate).Count();

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockEventService.Verify(s => s.Run(), Times.Exactly(expectedCount), "EventService did not Run() for each event.");
		}

        [TestMethod]
        public void ProcessBrandEvents_ExceptionThrownDuringOneBrandEventService_EventServiceStillCalledForEachQueuedEvent()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            int expectedCount = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.BrandNameUpdate).Count();

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
            this.mockEventService.Verify(s => s.Run(), Times.Exactly(expectedCount), "EventService was not called for each queued brand event.");
        }

		[TestMethod]
		public void ProcessBrandEvents_BrandEventsFound_LoggerInfoCalledOnceForEachItem()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents.Where(e => e.EventId == EventTypes.BrandNameUpdate).Count();

            this.mockEventService.SetupGet(s => s.RegionalItemMessage).Returns(Enumerable.Empty<RegionalItemMessageModel>().ToList());
            var loggedInfo = new List<string>();
            this.mockLogger.Setup(log => log.Info(It.IsAny<string>())).Callback<string>(msg => loggedInfo.Add(msg));

			// When
			this.processor.ProcessBrandNameUpdateEvents();

            // Then
            this.mockLogger.Verify(
                l => l.Info("Successfully processed event: IconToIrmaBrandNameUpdate.  Region = SW, ReferenceId = 1, Message = TestBrand1"), 
                Times.Once, "Logger Info was not called once per queued event.");

            this.mockLogger.Verify(
                l => l.Info("Successfully processed event: IconToIrmaBrandNameUpdate.  Region = FL, ReferenceId = 1, Message = TestBrand1"),
                Times.Once, "Logger Info was not called once per queued event.");
        }

		[TestMethod]
		public void ProcessBrandEvents_ExceptionThrownDuringBrandEventService_LoggerErrorCalledTwice()
		{
			// Given
			EventQueue eventQueue = new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).WithEventMessage("test").Build();
			eventQueue.EventType = new EventType { EventId = EventTypes.BrandNameUpdate, EventName = "Brand Name Update" };
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockEventService.Setup(s => s.Run()).Callback(() => { throw new Exception(); });

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2), "Logger.Error was not called exactly two times.");
		}

		[TestMethod]
		public void ProcessBrandEvents_ExceptionThrownDuringOneBrandEventService_ExceptionNotThrownAndEventServiceRunCalledForEachEvent()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.BrandNameUpdate).Count();

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
			this.mockEventService.Verify(s => s.Run(), Times.Exactly(expectedCount), "The EventService was not called for each event.");
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
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2), "Logger Error not called exactly twice.");
		}

		[TestMethod]
		public void ProcessBrandEvents_NoEventServiceFoundForEventType__LoggerErrorCalledTwice()
		{
			// Given
			EventQueue eventQueue = new TestEventQueueBuilder().WithEventId(EventTypes.BrandNameUpdate).Build();
			eventQueue.EventType = new EventType { EventId = EventTypes.BrandNameUpdate, EventName = "Brand Name Update" };
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockServiceProvider = new Mock<IEventServiceProvider>();
			this.processor = new EventProcessor(this.queues, this.mockLogger.Object, this.mockServiceProvider.Object, this.mockDataIssueMessage.Object);

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2), "Logger is not called exactly twice.");
		}

		[TestMethod]
		public void ProcessBrandEvents_BrandEventsAreQueuedNoErrors_SuccessfullyProcessedEventsAddedToProcessedEventsList()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			PopulateQueueWithItemEvents();

			List<EventQueue> expectedQueue = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.BrandNameUpdate).ToList();

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			Assert.AreEqual(expectedQueue.Count, this.queues.ProcessedEvents.Count, "The number of ProcessedEvents did not match the expected number");
			Assert.IsTrue(expectedQueue.Except(this.queues.ProcessedEvents).Count() == 0, "The ProcessedEvents list match the expected event list");
		}

		[TestMethod]
		public void ProcessBrandEvents_ExceptionThrownDuringBrandEventService_EventsAddedToFailedEventsQueueList()
		{
			// Given
			PopulateQueueWithNonItemEvents();

			// Throw exception the first time, then successfully update status
			int expectedCount = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.BrandNameUpdate).Count();
			this.mockEventService.Setup(e => e.Run())
				.Callback(() =>
				{
					throw new ArgumentException();
				});

			// When
			this.processor.ProcessBrandNameUpdateEvents();

			// Then
			Assert.AreEqual(expectedCount, this.queues.FailedEvents.Count, "The events that failed were not added to the FailedEvents list properly.");
		}

        [TestMethod]
        public void ProcessBrandEvents_BrandEventsProcessedSuccessfully_EventsRemovedFromEventQueueList()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            List<EventQueue> brandEventList = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.BrandNameUpdate).ToList();

            // When
            this.processor.ProcessBrandNameUpdateEvents();

            // Then
            foreach (var brandEvent in brandEventList)
            {
                Assert.IsFalse(this.queues.QueuedEvents.Contains(brandEvent));
            }
        }

		[TestMethod]
		public void ProcessTaxEvents_NoTaxEventsFound_LoggerInfoCalledOnceAndReturnsNothing()
		{
			// Given
			PopulateQueueWithItemEvents();

			// When
			this.processor.ProcessTaxEvents();

			// Then
			mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once, "Logger.Info was not called one time.");
		}

		[TestMethod]
		public void ProcessTaxEvents_TaxEventsFound_TaxEventServiceCalledForEachQueuedTaxEvent()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.NewTaxHierarchy || q.EventId == EventTypes.TaxNameUpdate).Count();

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockEventService.Verify(s => s.Run(), Times.Exactly(expectedCount), "EventService did not run for each queued event.");
		}

        [TestMethod]
        public void ProcessTaxEvents_ExceptionThrownDuringOneTaxEventService_EventServiceStillCalledForEachQueuedEvent()
        {
            // Given
            PopulateQueueWithNonItemEvents();
            int expectedCount = this.queues.QueuedEvents
                .Where(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy).Count();

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
            this.mockEventService.Verify(s => s.Run(), Times.Exactly(expectedCount), "EventService was not called for each queued tax event.");
        }

		[TestMethod]
		public void ProcessTaxEvents_TaxEventsFound_LoggerInfoCalledOnceForEachItem()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			int expectedCount = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.NewTaxHierarchy || q.EventId == EventTypes.TaxNameUpdate).Count();
            this.mockEventService.SetupGet(s => s.RegionalItemMessage).Returns(Enumerable.Empty<RegionalItemMessageModel>().ToList());

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(expectedCount), "Logger Info did not run for each queued event.");
		}

		[TestMethod]
		public void ProcessTaxEvents_ExceptionThrownDuringTaxEventService_LoggerErrorCalledTwice()
		{
			// Given
			EventQueue eventQueue = new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).Build();
			eventQueue.EventType = new EventType { EventId = EventTypes.TaxNameUpdate, EventName = "Tax Name Update" };
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockEventService.Setup(s => s.Run()).Callback(() => { throw new ArgumentException(); });

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2), "Logger Error did not run twice after exception.");
		}

		[TestMethod]
		public void ProcessTaxEvents_EventNameNotMapped_LoggerErrorCalledTwice()
		{
			// Given
			EventQueue eventQueue = new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).Build();
			eventQueue.EventType = new EventType { EventId = EventTypes.TaxNameUpdate, EventName = "test" };
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockEventService.Setup(s => s.Run()).Callback(() => { throw new ArgumentException(); });

			// When
			this.processor.ProcessTaxEvents();

			// Then
			this.mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2), "Logger Error was not called two time.");
		}

		[TestMethod]
		public void ProcessTaxEvents_NoEventServiceFoundForEventType__LoggerErrorCalledTwice()
		{
			// Given
			EventQueue eventQueue = new TestEventQueueBuilder().WithEventId(EventTypes.TaxNameUpdate).Build();
			eventQueue.EventType = new EventType { EventId = EventTypes.TaxNameUpdate, EventName = "Tax Name Update" };
			this.queues.QueuedEvents.Add(eventQueue);
			this.mockServiceProvider = new Mock<IEventServiceProvider>();
			this.processor = new EventProcessor(this.queues, this.mockLogger.Object, this.mockServiceProvider.Object, this.mockDataIssueMessage.Object);

			// When
			this.processor.ProcessTaxEvents();

			// Then
			mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2), "Logger is not called exactly twice.");
		}

		[TestMethod]
		public void ProcessTaxEvents_TaxEventsAreQueuedNoErrors_SuccessfullyProcessedEventsAddedToProcessedEventsList()
		{
			// Given
			PopulateQueueWithNonItemEvents();
			PopulateQueueWithItemEvents();

			List<EventQueue> expectedQueue = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy).ToList();

			// When
			this.processor.ProcessTaxEvents();

			// Then
			Assert.AreEqual(expectedQueue.Count, this.queues.ProcessedEvents.Count, "The number of ProcessedEvents did not match the expected number");
			Assert.IsTrue(expectedQueue.Except(this.queues.ProcessedEvents).Count() == 0, "The ProcessedEvents list match the expected event list");
		}

		[TestMethod]
		public void ProcessTaxEvents_ExceptionThrownDuringTaxEventService_EventsAddedToFailedEventsQueueList()
		{
			// Given
			PopulateQueueWithNonItemEvents();

			// Throw exception the first time, then successfully update status
			int expectedCount = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy).Count();
			this.mockEventService.Setup(e => e.Run())
				.Callback(() =>
				{
					throw new ArgumentException();
				});

			// When
			this.processor.ProcessTaxEvents();

			// Then
			Assert.AreEqual(expectedCount, this.queues.FailedEvents.Count, "The events that failed were not added to the FailedEvents list properly.");
		}

		[TestMethod]
        public void ProcessTaxEvents_TaxEventsProcessedSuccessfully_EventsRemovedFromEventQueueList()
		{
			// Given
			PopulateQueueWithNonItemEvents();
            List<EventQueue> taxEventsList = this.queues.QueuedEvents.Where(q => q.EventId == EventTypes.TaxNameUpdate || q.EventId == EventTypes.NewTaxHierarchy).ToList();

			// When
			this.processor.ProcessTaxEvents();

			// Then
			foreach (var taxEvent in taxEventsList)
			{
				Assert.IsFalse(this.queues.QueuedEvents.Contains(taxEvent));
			}
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
	}
}
