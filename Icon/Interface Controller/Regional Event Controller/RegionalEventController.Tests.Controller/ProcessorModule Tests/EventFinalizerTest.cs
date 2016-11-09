using System;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.DataAccess.Models;
using Icon.Common.Email;
using RegionalEventController.DataAccess.Commands;
using Icon.Logging;
using Irma.Framework;
using Icon.Testing.Builders;

namespace RegionalEventController.Tests.Controller
{
    [TestClass]
    public class EventFinalizerTest
    {
        private IrmaContext context = null;
        private EventFinalizer eventFinalizer;
        private List<IrmaNewItem> eventQueues;
        private Mock<IEmailClient> mockEmailClient;

        [TestInitialize]
        public void Initialize()
        {
            eventQueues = new List<IrmaNewItem>();
            mockEmailClient = new Mock<IEmailClient>();
        }

        [TestMethod]
        public void HandleFailedEvents_EventsWithExceptionExist_ShouldSendEmailOfEventsWithException()
        {
            //Given
            List<IrmaNewItem> eventQueues = buildQueue("Test failure.");

            eventFinalizer = buildEventFinalizer(eventQueues);

            //When
            eventFinalizer.HandleFailedEvents();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void HandleFailedEvents_EventsWithExceptionDontExist_ShouldNotSendEmailOfExceptionEvents()
        {
            //Given
            List<IrmaNewItem> eventQueues = buildQueue(String.Empty);

            eventFinalizer = buildEventFinalizer(eventQueues);

            //When
            eventFinalizer.HandleFailedEvents();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void HandleFailedEvents_EventsMarkedAsInvalidExist_ShouldNotSendEmailOfExceptionEvents()
        {
            //Given
            List<IrmaNewItem> eventQueues = buildQueue("Test Exception");
            eventQueues.ForEach(i => i.IsInvalidQueueEntry = true);

            eventFinalizer = buildEventFinalizer(eventQueues);

            //When
            eventFinalizer.HandleFailedEvents();

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        private EventFinalizer buildEventFinalizer(List<IrmaNewItem> queue)
        {
            return new EventFinalizer(
                eventQueues,
                mockEmailClient.Object,
                new DeleteNewItemsFromIrmaQueueCommandHandler(new Mock<ILogger<DeleteNewItemsFromIrmaQueueCommandHandler>>().Object, context));
        }

        private List<IrmaNewItem> buildQueue(string failedReason)
        {
            eventQueues = new List<Icon.Testing.CustomModels.IrmaNewItem>
            {
                new TestIrmaNewItemBuilder().WithFailureReason(failedReason).Build()
            }.ConvertAll(n => new RegionalEventController.DataAccess.Models.IrmaNewItem
            {
                QueueId = n.QueueId,
                RegionCode = n.RegionCode,
                ProcessedByController = n.ProcessedByController,
                IrmaTaxClass = n.IrmaTaxClass,
                IrmaItemKey = n.IrmaItemKey,
                Identifier = n.Identifier,
                IconItemId = n.IconItemId,
                IrmaItem = n.IrmaItem,
                FailureReason = n.FailureReason
            });

            return eventQueues;
        }
    }
}
