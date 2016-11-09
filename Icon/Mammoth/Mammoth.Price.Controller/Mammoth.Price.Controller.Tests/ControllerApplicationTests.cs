using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.Email;
using Mammoth.Logging;
using Mammoth.Price.Controller.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.Price.Controller.Tests
{
    [TestClass]
    public class ControllerApplicationTests
    {
        private ControllerApplication application;
        private PriceControllerApplicationSettings settings;
        private Mock<IQueueManager<PriceEventModel>> mockQueueManager;
        private Mock<IService<PriceEventModel>> mockService;

        [TestInitialize]
        public void Initialize()
        {
            settings = new PriceControllerApplicationSettings { Regions = new List<string> { "FL" } };
            mockQueueManager = new Mock<IQueueManager<PriceEventModel>>();
            mockService = new Mock<IService<PriceEventModel>>();

            application = new ControllerApplication(settings, mockQueueManager.Object,
                new Mock<IEmailClient>().Object,
                new Mock<ILogger>().Object,
                mockService.Object);
        }

        [TestMethod]
        public void Run_NoRegions_ShouldNotProcessEvents()
        {
            //Given
            settings.Regions.Clear();

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<PriceEventModel>>()), Times.Never);
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void Run_1Region_ShouldSetCurrentRegion()
        {
            //Given
            mockQueueManager.Setup(m => m.Get())
                .Returns(new ChangeQueueEvents<PriceEventModel>());

            //When
            application.Run();

            //Then
            Assert.AreEqual("FL", settings.CurrentRegion);
        }

        [TestMethod]
        public void Run_NoPriceEventsReturned_ShouldNotProcessOrFinalizeEvents()
        {
            //Given
            mockQueueManager.Setup(m => m.Get())
                .Returns(new ChangeQueueEvents<PriceEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Once);
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Never);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<PriceEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void Run_EventsReturns_ShouldProcessAndFinalizeEvents()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.Get())
                .Returns(new ChangeQueueEvents<PriceEventModel>
                    {
                        QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                        EventModels = new List<PriceEventModel> { new PriceEventModel() }
                    })
                .Returns(new ChangeQueueEvents<PriceEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Exactly(2));
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Once);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<PriceEventModel>>()), Times.Once);
        }

        [TestMethod]
        public void Run_EventsReturnedTwice_ShouldProcessAndFinalizeEventsTwice()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.Get())
                .Returns(new ChangeQueueEvents<PriceEventModel>
                {
                    QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                    EventModels = new List<PriceEventModel> { new PriceEventModel() }
                })
                .Returns(new ChangeQueueEvents<PriceEventModel>
                {
                    QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                    EventModels = new List<PriceEventModel> { new PriceEventModel() }
                })
                .Returns(new ChangeQueueEvents<PriceEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Exactly(3));
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Exactly(2));
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<PriceEventModel>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Run_ErrorOccursWhileProcessingEvents_ShouldFinalizeEvents()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.Get())
                .Returns(new ChangeQueueEvents<PriceEventModel>
                {
                    QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                    EventModels = new List<PriceEventModel> { new PriceEventModel() }
                })
                .Returns(new ChangeQueueEvents<PriceEventModel>());
            mockService.Setup(m => m.Process(It.IsAny<List<PriceEventModel>>()))
                .Throws(new Exception("Test"));

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Exactly(2));
            mockService.Verify(m => m.Process(It.IsAny<List<PriceEventModel>>()), Times.Once);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<PriceEventModel>>()), Times.Once);
        }
    }
}
