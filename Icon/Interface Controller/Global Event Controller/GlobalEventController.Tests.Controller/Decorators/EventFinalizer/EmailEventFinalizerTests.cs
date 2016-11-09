using GlobalEventController.Common;
using GlobalEventController.Controller.Decorators.EventFinalizer;
using GlobalEventController.Controller.EventOperations;
using Icon.Common.Email;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace GlobalEventController.Tests.Controller.Decorators.EventFinalizer
{
    [TestClass]
    public class EmailEventFinalizerTests
    {
        private EventFinalizerEmailDecorator emailEventFinalizer;
        private Mock<IEventQueues> mockEventQueues;
        private Mock<IEventFinalizer> mockEventFinalizer;
        private Mock<IEmailClient> mockEmailClient;

        [TestInitialize]
        public void Initialize()
        {
            mockEventQueues = new Mock<IEventQueues>();
            mockEventFinalizer = new Mock<IEventFinalizer>();
            mockEmailClient = new Mock<IEmailClient>();

            emailEventFinalizer = new EventFinalizerEmailDecorator(mockEventFinalizer.Object,
                mockEventQueues.Object,
                mockEmailClient.Object);
        }

        [TestMethod]
        public void HandleFailedEvents_FailedEventsExist_ShouldSendEmailOfFailedEvents()
        {
            //Given
            mockEventQueues.SetupGet(m => m.FailedEvents)
                .Returns(new List<FailedEvent> { new FailedEvent { Event = new TestEventQueueBuilder(), FailureReason = "TEST" } });

            //When
            emailEventFinalizer.HandleFailedEvents();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockEventFinalizer.Verify(m => m.HandleFailedEvents(), Times.Once);
            mockEventQueues.VerifyGet(m => m.FailedEvents, Times.Exactly(2));
        }

        [TestMethod]
        public void HandleFailedEvents_FailedEventsDontExist_ShouldNotSendEmailOfFailedEvents()
        {
            //Given
            mockEventQueues.SetupGet(m => m.FailedEvents)
                .Returns(new List<FailedEvent>());

            //When
            emailEventFinalizer.HandleFailedEvents();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockEventFinalizer.Verify(m => m.HandleFailedEvents(), Times.Once);
            mockEventQueues.VerifyGet(m => m.FailedEvents, Times.Once);
        }

        [TestMethod]
        public void DeleteEvents_ShouldCallWrappedFinalizersDeleteEvents()
        {
            //When
            emailEventFinalizer.DeleteEvents();

            //Then
            mockEventFinalizer.Verify(m => m.DeleteEvents(), Times.Once);
        }
    }
}
