using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using RegionalEventController.Controller.UpdateServices;
using RegionalEventController.Controller.Processors;
using RegionalEventController.Controller.ProcessorModules;
using RegionalEventController.DataAccess.Interfaces;
using RegionalEventController.DataAccess.Commands;
using RegionalEventController.DataAccess.Models;
using Icon.Framework;
using Icon.Logging;
using Moq;

namespace RegionalEventController.Tests.Controller.Processor_Tests
{
    [TestClass]
    public class UpdateServiceProcessorTest
    {
        private Mock<IUpdateService> mockUpdateService;

        [TestInitialize]
        public void Initialize()
        {
            this.mockUpdateService = new Mock<IUpdateService>();
        }

        [TestMethod]
        public void RunUpdateServiceProcessor_UpdateServiceProcessorIsKickedOff_UpdateBulkMethodShouldBeCalled()
        {
            // Given.
            var updateServiceProcessor = new UpdateServiceProcessor(mockUpdateService.Object);

            // When
            updateServiceProcessor.Run();

            // Then
            mockUpdateService.Verify(ms => ms.UpdateBulk(), Times.Once);
        }

        [TestMethod]
        public void RunUpdateServiceProcessor_ErrorInBulkUpdate_UpdateRowByRowMethodShouldBeCalled()
        {
            // Given.
            mockUpdateService.Setup(ms => ms.UpdateBulk()).Throws(new Exception());
            var updateServiceProcessor = new UpdateServiceProcessor(mockUpdateService.Object);

            // When
            updateServiceProcessor.Run();

            // Then
            mockUpdateService.Verify(mp => mp.UpdateBulk(), Times.Once);
            mockUpdateService.Verify(mp => mp.UpdateRowByRow(), Times.Once);
        }
    }
}