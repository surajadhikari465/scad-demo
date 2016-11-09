using Vim.Common.ControllerApplication;
using Vim.Common.ControllerApplication.Services;
using Vim.Locale.Controller.DataAccess.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vim.Locale.Controller.Tests
{
    [TestClass]
    public class ControllerApplicationTests
    {
        private ControllerApplication application;
        private Mock<IQueueManager<LocaleEventModel>> mockQueueManager;
        private Mock<IService<LocaleEventModel>> mockService;

        [TestInitialize]
        public void Initialize()
        {
            mockQueueManager = new Mock<IQueueManager<LocaleEventModel>>();
            mockService = new Mock<IService<LocaleEventModel>>();

            application = new ControllerApplication(mockQueueManager.Object, mockService.Object);
        }

        [TestMethod]
        public void Run_NoLocaleEventsReturned_ShouldNotProcessOrFinalizeEvents()
        {
            //Given
            mockQueueManager.Setup(m => m.Get(It.IsAny<List<int>>()))
                .Returns(new List<LocaleEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(It.IsAny<List<int>>()), Times.Once);
            mockService.Verify(m => m.Process(It.IsAny<List<LocaleEventModel>>()), Times.Never);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<List<LocaleEventModel>>()), Times.Never);
        }

        [TestMethod]
        public void Run_EventsReturns_ShouldProcessAndFinalizeEvents()
        {
            //Given
            mockQueueManager.SetupSequence(m => m.Get(It.IsAny<List<int>>()))
                .Returns(new List<LocaleEventModel> { new LocaleEventModel() })
                .Returns(new List<LocaleEventModel>());

            //When
            application.Run();

            //Then
            mockQueueManager.Verify(m => m.Get(It.IsAny<List<int>>()), Times.Exactly(2));
            mockService.Verify(m => m.Process(It.IsAny<List<LocaleEventModel>>()), Times.Once);
            mockQueueManager.Verify(m => m.Finalize(It.IsAny<List<LocaleEventModel>>()), Times.Once);
        }
    }
}
