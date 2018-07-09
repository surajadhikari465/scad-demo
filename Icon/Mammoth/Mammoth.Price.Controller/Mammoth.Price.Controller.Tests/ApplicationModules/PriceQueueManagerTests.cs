using Mammoth.Common.ControllerApplication.Models;
using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Logging;
using Mammoth.Price.Controller.ApplicationModules;
using Mammoth.Price.Controller.Common;
using Mammoth.Price.Controller.DataAccess.Commands;
using Mammoth.Price.Controller.DataAccess.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Mammoth.Price.Controller.Tests.ApplicationModules
{
    [TestClass]
    public class PriceQueueManagerTests
    {
        private PriceQueueManager queueManager;
        private PriceControllerApplicationSettings settings;
        private Mock<IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>>> mockUpdateAndGetEventQueueInProcessQueryHandler;
        private Mock<ICommandHandler<DeleteEventQueueCommand>> mockDeleteEventQueueCommandHandler;
        private Mock<ILogger> mockLogger;

        [TestInitialize]
        public void InitializeTest()
        {
            this.settings = new PriceControllerApplicationSettings
            {
                Instance = 7777,
                ControllerName = "Price",
                CurrentRegion = "FL",
                MaxNumberOfRowsToMark = 100,
                Regions = new List<string> { "FL" },
                NonAlertErrors = new List<string>()
            };
            mockUpdateAndGetEventQueueInProcessQueryHandler = new Mock<IQueryHandler<UpdateAndGetEventQueueInProcessParameters, List<EventQueueModel>>>();
            mockDeleteEventQueueCommandHandler = new Mock<ICommandHandler<DeleteEventQueueCommand>>();
            mockLogger = new Mock<ILogger>();

            this.queueManager = new PriceQueueManager(
                this.settings,
                mockUpdateAndGetEventQueueInProcessQueryHandler.Object,
                mockDeleteEventQueueCommandHandler.Object,
                mockLogger.Object);
        }

        [TestMethod]
        public void GetEvents_NoErrors_ReturnsQueryResults()
        {
            //Given
            mockUpdateAndGetEventQueueInProcessQueryHandler.Setup(m => m.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>()))
                .Returns(new List<EventQueueModel>
                {
                    new EventQueueModel(),
                    new EventQueueModel(),
                    new EventQueueModel()
                });

            //When
            var events = queueManager.GetEvents();

            //Then
            Assert.AreEqual(3, events.Count);
        }

        [TestMethod]
        public void GetEvents_ErrorOccurs_ReturnsEmptyList()
        {
            //Given
            mockUpdateAndGetEventQueueInProcessQueryHandler.Setup(m => m.Search(It.IsAny<UpdateAndGetEventQueueInProcessParameters>()))
                .Throws(new Exception());

            //When
            var events = queueManager.GetEvents();

            //Then
            Assert.AreEqual(0, events.Count);
            mockLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }

        [TestMethod]
        public void DeleteInProcessEvents_NoErrorOccurs_CallsDeleteCommandHandler()
        {
            //When
            queueManager.DeleteInProcessEvents();

            //Then
            mockDeleteEventQueueCommandHandler.Verify(m => m.Execute(It.IsAny<DeleteEventQueueCommand>()), Times.Once);
        }

        [TestMethod]
        public void DeleteInProcessEvents_ErrorOccurs_ExceptionIsCaught()
        {
            //Given
            mockDeleteEventQueueCommandHandler.Setup(m => m.Execute(It.IsAny<DeleteEventQueueCommand>()))
                .Throws(new Exception());

            //When
            queueManager.DeleteInProcessEvents();

            //Then
            mockDeleteEventQueueCommandHandler.Verify(m => m.Execute(It.IsAny<DeleteEventQueueCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>(), It.IsAny<Exception>()), Times.Once);
        }
    }
}
