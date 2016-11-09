using GlobalEventController.Common;
using GlobalEventController.Controller.EventOperations;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Logging;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace GlobalEventController.Tests.Controller.EventOperationsTests
{
    [TestClass]
    public class EventFinalizerTests
    {
        private EventFinalizer finalizer;
        private Mock<ILogger<EventFinalizer>> mockLogger;
        private Mock<ICommandHandler<UpdateEventQueueFailuresCommand>> mockUpdateEventQueueFailures;
        private Mock<ICommandHandler<BulkDeleteEventQueueCommand>> mockBulkDeleteEventQueue;
        private EventQueues queues;

        [TestInitialize]
        public void InitializeData()
        {
            this.queues = new EventQueues();
            this.mockLogger = new Mock<ILogger<EventFinalizer>>();
            this.mockUpdateEventQueueFailures = new Mock<ICommandHandler<UpdateEventQueueFailuresCommand>>();
            this.mockBulkDeleteEventQueue = new Mock<ICommandHandler<BulkDeleteEventQueueCommand>>();
            this.finalizer = new EventFinalizer(this.queues, this.mockLogger.Object, this.mockUpdateEventQueueFailures.Object, this.mockBulkDeleteEventQueue.Object);
        }

        [TestMethod]
        public void HandleFailedEvents_FailedEventsListIsEmpty_UpdateEventQueueFailuresCommandNotCalled()
        {
            // Given
            // Nothing to setup.

            // When
            this.finalizer.HandleFailedEvents();

            // Then
            this.mockUpdateEventQueueFailures.Verify(u => u.Handle(It.IsAny<UpdateEventQueueFailuresCommand>()), Times.Never, "The UpdateEventQueueCommandHandler was called unexpectedly.");
        }

        [TestMethod]
        public void HandleFailedEvents_FailedEventsListIsEmpty_LoggerInfoNotCalled()
        {
            // Given
            // Nothing to setup.

            // When
            this.finalizer.HandleFailedEvents();

            // Then
            this.mockLogger.Verify(u => u.Info(It.IsAny<string>()), Times.Never, "The Logger.Info was called unexpectedly.");
        }

        [TestMethod]
        public void HandleFailedEvents_FailedEventsAreQueued_UpdateEventQueueFailuresCommandHandlerCalledOneTime()
        {
            // Given
            PopulateFailedEventQueue();

            // When
            this.finalizer.HandleFailedEvents();

            // Then
            this.mockUpdateEventQueueFailures.Verify(u => u.Handle(It.IsAny<UpdateEventQueueFailuresCommand>()), Times.Once, 
                "The UpdateEventQueueCommandHandler was not called exactly one time.");
        }

        [TestMethod]
        public void HandleFailedEvents_FailedEventsAreQueued_LoggerInfoCalledOneTime()
        {
            // Given
            PopulateFailedEventQueue();

            // When
            this.finalizer.HandleFailedEvents();

            // Then
            this.mockLogger.Verify(u => u.Info(It.IsAny<string>()), Times.Once, "The Logger.Info was not called exactly one time.");
        }

        [TestMethod]
        public void HandleFailedEvents_ExceptionThrownDuringUpdateEventQueueFailureCommand_LoggerErrorCalledTwoTimes()
        {
            // Given
            PopulateFailedEventQueue();
            this.mockUpdateEventQueueFailures.Setup(u => u.Handle(It.IsAny<UpdateEventQueueFailuresCommand>())).Throws(new ArgumentException());

            // When
            this.finalizer.HandleFailedEvents();

            // Then
            this.mockLogger.Verify(u => u.Error(It.IsAny<string>()), Times.Exactly(2), "The Logger.Error was not called exactly two times.");
        }

        [TestMethod]
        public void DeleteEvents_FailedEventsListIsEmpty_DeleteEventQueueCommandNotCalled()
        {
            // Given
            // No setup needed.

            // When
            this.finalizer.DeleteEvents();

            // Then
            this.mockBulkDeleteEventQueue.Verify(d => d.Handle(It.IsAny<BulkDeleteEventQueueCommand>()), Times.Never,
                "The BulkDeleteEventQueueCommand was called unexpectedly.");
        }

        [TestMethod]
        public void DeleteEvents_FailedEventsListIsEmpty_LoggerInfoNotCalled()
        {
            // Given
            // No setup needed.

            // When
            this.finalizer.DeleteEvents();

            // Then
            this.mockLogger.Verify(u => u.Info(It.IsAny<string>()), Times.Never, "The Logger.Info was called unexpectedly.");
        }

        [TestMethod]
        public void HandleFailedEvents_ProcessEventsAreQueued_BulkDeleteEventsCommandHandlerCalledOneTime()
        {
            // Given
            PopulateProcessedEventQueue();

            // When
            this.finalizer.DeleteEvents();

            // Then
            this.mockBulkDeleteEventQueue.Verify(d => d.Handle(It.IsAny<BulkDeleteEventQueueCommand>()), Times.Once,
                "The BulkDeleteEventQueueCommand was called exactly one time.");
        }

        [TestMethod]
        public void HandleFailedEvents_ProcessEventsAreQueued_LoggerInfoCalledOneTime()
        {
            // Given
            PopulateProcessedEventQueue();

            // When
            this.finalizer.DeleteEvents();

            // Then
            this.mockLogger.Verify(u => u.Info(It.IsAny<string>()), Times.Once, "The Logger.Info was not called exactly one time.");
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void HandleFailedEvents_ExceptionThrownDuringBulkDeleteEventCommand_ExceptionThrown()
        {
            // Given
            PopulateProcessedEventQueue();
            this.mockBulkDeleteEventQueue.Setup(u => u.Handle(It.IsAny<BulkDeleteEventQueueCommand>())).Throws(new ArgumentException());

            // When
            this.finalizer.DeleteEvents();

            // Then
            // Expected Exception to be thrown by EventFinalizer
        }

        private void PopulateFailedEventQueue()
        {
            this.queues.FailedEvents.Add(new FailedEvent(new TestEventQueueBuilder().WithRegionCode("SW"), null));
            this.queues.FailedEvents.Add(new FailedEvent(new TestEventQueueBuilder().WithRegionCode("MA"), null));
            this.queues.FailedEvents.Add(new FailedEvent(new TestEventQueueBuilder().WithRegionCode("MW"), null));
            this.queues.FailedEvents.Add(new FailedEvent(new TestEventQueueBuilder().WithRegionCode("NA"), null));
            this.queues.FailedEvents.Add(new FailedEvent(new TestEventQueueBuilder().WithRegionCode("FL"), null));
        }

        private void PopulateProcessedEventQueue()
        {
            this.queues.ProcessedEvents.Add(new TestEventQueueBuilder().WithRegionCode("SW"));
            this.queues.ProcessedEvents.Add(new TestEventQueueBuilder().WithRegionCode("MA"));
            this.queues.ProcessedEvents.Add(new TestEventQueueBuilder().WithRegionCode("MW"));
            this.queues.ProcessedEvents.Add(new TestEventQueueBuilder().WithRegionCode("NA"));
            this.queues.ProcessedEvents.Add(new TestEventQueueBuilder().WithRegionCode("FL"));
        }
    }
}