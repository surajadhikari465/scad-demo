using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using GlobalEventController.Controller.EventOperations;
using SubteamEventController.Controller.EventOperations;
using Moq;
using GlobalEventController.DataAccess.Commands;
using GlobalEventController.DataAccess.BulkCommands;
using GlobalEventController.DataAccess.Infrastructure;
using Icon.Logging;
using Icon.Framework;
using Icon.Testing.Builders;
using GlobalEventController.Controller.Models;
using Icon.Common.Email;
using System.Collections.Generic;

namespace SSubteamEventController.Tests.Controller.EventOperations
{
    [TestClass]
    public class SubTeamEventFinalizerTests
    {
        private SubTeamEventFinalizer emailEventFinalizer;
        private Mock<IEventQueues> mockEventQueues;
        private Mock<IEventFinalizer> mockEventFinalizer;
        private Mock<IEmailClient> mockEmailClient;

        [TestInitialize]
        public void Initialize()
        {
            mockEventQueues = new Mock<IEventQueues>();
            mockEventFinalizer = new Mock<IEventFinalizer>();
            mockEmailClient = new Mock<IEmailClient>();

            emailEventFinalizer = new SubTeamEventFinalizer(mockEventFinalizer.Object,
                mockEventQueues.Object,
                mockEmailClient.Object);
        }

        [TestMethod]
        public void SubTeamEventFinalizer_FailedEventsExist_ShouldSendEmailOfFailedEvents()
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
        public void SubTeamEventFinalizer_FailedEventsDontExist_ShouldNotSendEmailOfFailedEvents()
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
        public void SubTeamEventFinalizer_DeleteEvents_ShouldCallWrappedFinalizersDeleteEvents()
        {
            //When
            emailEventFinalizer.DeleteEvents();

            //Then
            mockEventFinalizer.Verify(m => m.DeleteEvents(), Times.Once);
        }
    }
}
