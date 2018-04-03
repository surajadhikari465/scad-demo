using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using Mammoth.Price.Controller.ApplicationModules;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Models;
using Mammoth.Price.Controller.EventProcessors;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.Tests
{
    [TestClass]
    public class ControllerApplicationTests
    {
        private ControllerApplication application;
        private PriceControllerApplicationSettings settings;
        private Mock<IQueueManager> mockQueueManager;
        private Mock<IEventProcessor<PriceEventModel>> mockPriceEventProcessor;
        private Mock<IEventProcessor<CancelAllSalesEventModel>> mockCancelAllSalesEventProcessor;
        private Mock<ICommandHandler<ArchiveEventsCommand>> mockArchiveEventsCommandHandler;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            settings = new PriceControllerApplicationSettings { Regions = new List<string> { "FL" } };
            mockQueueManager = new Mock<IQueueManager>();
            mockPriceEventProcessor = new Mock<IEventProcessor<PriceEventModel>>();
            mockCancelAllSalesEventProcessor = new Mock<IEventProcessor<CancelAllSalesEventModel>>();
            mockArchiveEventsCommandHandler = new Mock<ICommandHandler<ArchiveEventsCommand>>();
            mockLogger = new Mock<ILogger>();

            application = new ControllerApplication(
                settings,
                mockQueueManager.Object,
                mockPriceEventProcessor.Object,
                mockCancelAllSalesEventProcessor.Object,
                mockArchiveEventsCommandHandler.Object,
                mockLogger.Object);
        }

        [TestMethod]
        public void Run_NoRegions_ShouldNotProcessEvents()
        {
            //Given
            settings.Regions.Clear();

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.GetEvents(), Times.Never);
            mockQueueManager.Verify(m => m.DeleteInProcessEvents(), Times.Never);
            mockPriceEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Never);
            mockCancelAllSalesEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Never);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Never);
        }

        [TestMethod]
        public void Run_1Region_ShouldSetCurrentRegion()
        {
            //Given
            mockQueueManager.Setup(m => m.GetEvents()).Returns(new List<EventQueueModel>());

            //When
            application.Run();

            //Then
            Assert.AreEqual("FL", settings.CurrentRegion);
        }

        [TestMethod]
        public void Run_NoEventsReturned_ShouldNotProcessEvents()
        {
            //Given
            mockQueueManager.Setup(m => m.GetEvents()).Returns(new List<EventQueueModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.DeleteInProcessEvents(), Times.Never);
            mockPriceEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Never);
            mockCancelAllSalesEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Never);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Never);
        }

        [TestMethod]
        public void Run_EventsReturns_ShouldProcessEvents()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.GetEvents())
                .Returns(new List<EventQueueModel> { new EventQueueModel() })
                .Returns(new List<EventQueueModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.DeleteInProcessEvents(), Times.Once);
            mockPriceEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Once);
            mockCancelAllSalesEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Once);
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Once);
        }

        [TestMethod]
        public void Run_EventsReturnedTwice_ShouldProcessAndFinalizeEventsTwice()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.GetEvents())
               .Returns(new List<EventQueueModel> { new EventQueueModel() })
               .Returns(new List<EventQueueModel> { new EventQueueModel() })
               .Returns(new List<EventQueueModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.DeleteInProcessEvents(), Times.Exactly(2));
            mockPriceEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Exactly(2));
            mockCancelAllSalesEventProcessor.Verify(m => m.ProcessEvents(It.IsAny<List<EventQueueModel>>()), Times.Exactly(2));
            mockArchiveEventsCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveEventsCommand>()), Times.Exactly(2));
        }
    }
}
