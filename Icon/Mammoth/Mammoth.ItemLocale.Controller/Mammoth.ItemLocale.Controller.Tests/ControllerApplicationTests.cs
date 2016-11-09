using Mammoth.Common.ControllerApplication;
using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.ControllerApplication.Services;
using Mammoth.Common.Email;
using Mammoth.ItemLocale.Controller.DataAccess.Models;
using Mammoth.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Mammoth.ItemLocale.Controller.Tests
{
    [TestClass]
    public class ControllerApplicationTests
    {
        private ControllerApplication application;
        private ItemLocaleControllerApplicationSettings settings;
        private Mock<IQueueManager<ItemLocaleEventModel>> mockQueueManager;
        private Mock<IService<ItemLocaleEventModel>> mockService;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void Initialize()
        {
            settings = new ItemLocaleControllerApplicationSettings { Regions = new List<string> { "FL" } };
            mockQueueManager = new Mock<IQueueManager<ItemLocaleEventModel>>();
            mockService = new Mock<IService<ItemLocaleEventModel>>();
            mockLogger = new Mock<ILogger>();

            application = new ControllerApplication(settings, mockQueueManager.Object, mockService.Object, 
                new Mock<IEmailClient>().Object, mockLogger.Object);
        }

        [TestMethod]
        public void Run_NoRegions_ShouldNotProcessEvents()
        {
            //Given
            settings.Regions.Clear();

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<ItemLocaleEventModel>>()), Times.Never);
            mockService.Verify(m => m.Process(It.IsAny<List<ItemLocaleEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void Run_1Region_ShouldSetCurrentRegion()
        {
            //Given
            mockQueueManager.Setup(m => m.Get())
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>());

            //When
            application.Run();

            //Then
            Assert.AreEqual("FL", settings.CurrentRegion);
        }

        [TestMethod]
        public void Run_NoItemLocaleEventsReturned_ShouldNotProcessOrFinalizeEvents()
        {
            //Given
            mockQueueManager.Setup(m => m.Get())
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Once);
            mockService.Verify(m => m.Process(It.IsAny<List<ItemLocaleEventModel>>()), Times.Never);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<ItemLocaleEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void Run_EventsReturns_ShouldProcessAndFinalizeEvents()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.Get())
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>
                {
                    QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                    EventModels = new List<ItemLocaleEventModel> { new ItemLocaleEventModel() }
                })
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Exactly(2));
            mockService.Verify(m => m.Process(It.IsAny<List<ItemLocaleEventModel>>()), Times.Once);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<ItemLocaleEventModel>>()), Times.Once);
        }

        [TestMethod]
        public void Run_EventsReturnedTwice_ShouldProcessAndFinalizeEventsTwice()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.Get())
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>
                {
                    QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                    EventModels = new List<ItemLocaleEventModel> { new ItemLocaleEventModel() }
                })
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>
                {
                    QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                    EventModels = new List<ItemLocaleEventModel> { new ItemLocaleEventModel() }
                })
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Exactly(3));
            mockService.Verify(m => m.Process(It.IsAny<List<ItemLocaleEventModel>>()), Times.Exactly(2));
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<ItemLocaleEventModel>>()), Times.Exactly(2));
        }

        [TestMethod]
        public void Run_ErrorOccursWhileProcessingEvents_ShouldFinalizeEvents()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.Get())
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>
                {
                    QueuedEvents = new List<EventQueueModel> { new EventQueueModel() },
                    EventModels = new List<ItemLocaleEventModel> { new ItemLocaleEventModel() }
                })
                .Returns(new ChangeQueueEvents<ItemLocaleEventModel>());
            mockService.Setup(m => m.Process(It.IsAny<List<ItemLocaleEventModel>>()))
                .Throws(new Exception("Test"));

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(), Times.Exactly(2));
            mockService.Verify(m => m.Process(It.IsAny<List<ItemLocaleEventModel>>()), Times.Once);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<ChangeQueueEvents<ItemLocaleEventModel>>()), Times.Once);
        }
    }
}
