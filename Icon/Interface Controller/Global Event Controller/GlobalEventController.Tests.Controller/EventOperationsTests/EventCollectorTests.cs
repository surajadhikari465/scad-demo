using GlobalEventController.Controller.EventOperations;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using GlobalEventController.DataAccess.Queries;
using Icon.Common;
using Icon.Framework;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Data.Entity;

namespace GlobalEventController.Tests.Controller.EventOperationsTests
{
    [TestClass]
    public class EventCollectorTests
    {
        private EventQueues queues;
        private Mock<ILogger<EventCollector>> logger;
        private Mock<IQueryHandler<BulkUpdateEventQueueInProcessCommand, List<EventQueueCustom>>> bulkUpdateEventsHandler;
        private EventCollector collector;

        [TestInitialize]
        public void Initialize()
        {
            this.queues = new EventQueues();
            this.logger = new Mock<ILogger<EventCollector>>();
            this.bulkUpdateEventsHandler = new Mock<IQueryHandler<BulkUpdateEventQueueInProcessCommand, List<EventQueueCustom>>>();

            this.collector = new EventCollector(this.queues, this.logger.Object, this.bulkUpdateEventsHandler.Object);
            this.queues.QueuedEvents = new List<EventQueue>();
            ConfigurationManager.AppSettings["MaxQueueEntriesToProcess"] = null;
        }

        [TestMethod]
        public void GetEvents_NoErrorsOccurr_LoggingInfoCalledTwice()
        {
            // Given
            SetupBulkUpdateEventQueueInProcessToReturnEmptyList();

            // When
            this.collector.GetEvents();

            // Then
            this.logger.Verify(l => l.Info(It.IsAny<string>()), Times.Exactly(2), "The Logger.Info was not called twice.");
        }

        [TestMethod]
        public void GetEvents_MaxQueueEntriesToProcessConfigNotFound_LoggingErrorCalledOnce()
        {
            // Given
            SetupBulkUpdateEventQueueInProcessToReturnEmptyList();

            // When
            this.collector.GetEvents();

            // Then
            this.logger.Verify(l => l.Error(It.IsAny<string>()), Times.Once, "Logger.Error not called once.");
        }

        [TestMethod]
        public void GetEvents_MaxQueueEntriesToProcessConfigNotFound_BulkUpdateEventQueueInProcessStillCalledOneTime()
        {
            // Given
            SetupBulkUpdateEventQueueInProcessToReturnEmptyList();

            // When
            this.collector.GetEvents();

            // Then
            this.bulkUpdateEventsHandler.Verify(bu => bu.Handle(It.IsAny<BulkUpdateEventQueueInProcessCommand>()), Times.Once,
                "BulkUpdateEventQueueInProcessCommand not called one time.");
        }

        [TestMethod]
        public void GetEvents_EventsAreRegisteredMaxQueueEntriesToProcessFound_BulkUpdateEventQueueInProcessCommandCalledOnetime()
        {
            // Given
            SetupBulkUpdateEventQueueInProcessToReturnEmptyList();
            ConfigurationManager.AppSettings["MaxQueueEntriesToProcess"] = "1000";

            // When
            this.collector.GetEvents();

            // Then
            this.bulkUpdateEventsHandler.Verify(bu => bu.Handle(It.IsAny<BulkUpdateEventQueueInProcessCommand>()), Times.Once,
                "BulkUpdateEventQueueInProcessCommand not called one time.");
        }

        [TestMethod]
        [ExpectedException(typeof(Exception))]
        public void GetEvents_ExceptionThrownDuringBulkUpdateEventQueueInProcess_ExceptionThrown()
        {
            // Given
            SetupBulkUpdateEventQueueInProcessToThrowException();

            // When
            this.collector.GetEvents();

            // Then
            // Expected that the exception is bubbled up and thrown
        }

        [TestMethod]
        public void GetEvents_EventsAreRegistered_QueuedEventsListIsPopulated()
        {
            // Given
            List<EventQueueCustom> eventQueueCustom = BuildEventQueueCustomList();
            this.bulkUpdateEventsHandler.Setup(e => e.Handle(It.IsAny<BulkUpdateEventQueueInProcessCommand>())).Returns(eventQueueCustom);

            // When
            this.collector.GetEvents();
            List<EventQueue> expected = new List<EventQueue>();
            expected = eventQueueCustom.Select(c => new EventQueue
                {
                    EventId = c.EventId,
                    EventMessage = c.EventMessage,
                    EventReferenceId = c.EventReferenceId,
                    InProcessBy = c.InProcessBy,
                    RegionCode = c.RegionCode,
                    InsertDate = c.InsertDate,
                    ProcessFailedDate = c.ProcessFailedDate,
                    QueueId = c.QueueId,
                    EventType = new EventType { EventId = c.EventId, EventName = c.EventName }
                }).ToList();

            // Then
            for (int i = 0; i < expected.Count; i++)
			{
                Assert.AreEqual(expected[i].EventId, this.queues.QueuedEvents[i].EventId, "EventId did not match expected value.");
                Assert.AreEqual(expected[i].EventMessage, this.queues.QueuedEvents[i].EventMessage, "EventMessage did not match expected value.");
                Assert.AreEqual(expected[i].InProcessBy, this.queues.QueuedEvents[i].InProcessBy, "InProcessBy did not match expected value.");
                Assert.AreEqual(expected[i].RegionCode, this.queues.QueuedEvents[i].RegionCode, "RegionCode did not match expected value.");
                Assert.AreEqual(expected[i].InsertDate, this.queues.QueuedEvents[i].InsertDate, "InsertDate did not match expected value.");
                Assert.AreEqual(expected[i].ProcessFailedDate, this.queues.QueuedEvents[i].ProcessFailedDate, "ProcessFailedDate did not match expected value.");
                Assert.AreEqual(expected[i].QueueId, this.queues.QueuedEvents[i].QueueId, "QueueId did not match expected value.");
                Assert.AreEqual(expected[i].EventType.EventName, this.queues.QueuedEvents[i].EventType.EventName, "EventType.EventName did not match expected value.");
			}
        }


        private void SetupBulkUpdateEventQueueInProcessToReturnEmptyList()
        {
            this.bulkUpdateEventsHandler.Setup(e => e.Handle(It.IsAny<BulkUpdateEventQueueInProcessCommand>())).Returns(new List<EventQueueCustom>());
        }

        private void SetupBulkUpdateEventQueueInProcessToThrowException()
        {
            this.bulkUpdateEventsHandler.Setup(e => e.Handle(It.IsAny<BulkUpdateEventQueueInProcessCommand>())).Throws(new Exception());
        }

        private List<EventQueueCustom> BuildEventQueueCustomList()
        {
            List<EventQueueCustom> eventQueueCustom = new List<EventQueueCustom>();
            eventQueueCustom.Add(new EventQueueCustom
            {
                EventId = EventTypes.BrandNameUpdate,
                EventMessage = "test",
                EventReferenceId = 1,
                EventName = "Brand Name Update",
                RegionCode = "SW",
                QueueId = 1
            });

            eventQueueCustom.Add(new EventQueueCustom
            {
                EventId = EventTypes.ItemUpdate,
                EventMessage = "test",
                EventReferenceId = 2,
                EventName = "Item Update",
                RegionCode = "SW",
                QueueId = 2
            });

            eventQueueCustom.Add(new EventQueueCustom
            {
                EventId = EventTypes.TaxNameUpdate,
                EventMessage = "test",
                EventReferenceId = 1,
                EventName = "Tax Name Update",
                RegionCode = "MA",
                QueueId = 3
            });

            return eventQueueCustom;
        }
    }
}
