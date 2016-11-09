using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TlogController.Controller.Processors;
using TlogController.Controller.ProcessorModules;
using System;
using System.Collections.Generic;
using TlogController.DataAccess.Models;
using Icon.Logging;
using TlogController.DataAccess.BulkCommands;

namespace TlogController.Tests.Controller.Processor_Tests
{
    [TestClass]
    public class IrmaTlogProcessorUnitTest
    {
        private Mock<IIconTlogProcessorModule> mockIconTlogProcessorModule;
        private Mock<IIrmaTlogProcessorModule> mockIrmaTlogProcessorModule;

        [TestInitialize]
        public void Initialize()
        {
            mockIconTlogProcessorModule = new Mock<IIconTlogProcessorModule>();
            mockIrmaTlogProcessorModule = new Mock<IIrmaTlogProcessorModule>();
            
        }

        [TestMethod]
        public void UpdateIrmaSalesSumByItem_ErrorInBulkUpdate_FallbackMethodShouldBeCalled()
        {
            // Given.
            mockIrmaTlogProcessorModule.Setup(mt => mt.PushSalesSumByitemDataInBulkToIrma(It.IsAny<IrmaTlog>())).Throws(new Exception());

            var irmaTlogProcessor = new IrmaTlogProcessor(mockIconTlogProcessorModule.Object, mockIrmaTlogProcessorModule.Object, new IrmaTlog());

            // When.
            irmaTlogProcessor.UpdateSalesSumByitem();

            // Then.
            mockIrmaTlogProcessorModule.Verify(mb => mb.PushSalesSumByitemDataInBulkToIrma(It.IsAny<IrmaTlog>()), Times.AtLeastOnce);
            mockIrmaTlogProcessorModule.Verify(mb => mb.PushSalesSumByitemDataTransactionByTransactionToIrma(It.IsAny<IrmaTlog>()), Times.AtLeastOnce);
        }

        [TestMethod]
        public void UpdateTlogReprocessRequests_ErrorInBulkUpdate_FallbackMethodShouldBeCalled()
        {
            // Given.
            mockIrmaTlogProcessorModule.Setup(mt => mt.PushTlogReprocessRequestsInBulkToIrma(It.IsAny<IrmaTlog>())).Throws(new Exception());

            var irmaTlogProcessor = new IrmaTlogProcessor(mockIconTlogProcessorModule.Object, mockIrmaTlogProcessorModule.Object, new IrmaTlog());

            // When.
            irmaTlogProcessor.PopulateTlogReprocessRequests();

            // Then.
            mockIrmaTlogProcessorModule.Verify(mb => mb.PushTlogReprocessRequestsInBulkToIrma(It.IsAny<IrmaTlog>()), Times.AtLeastOnce);
            mockIrmaTlogProcessorModule.Verify(mb => mb.PushTlogReprocessRequestsOneByOneToIrma(It.IsAny<IrmaTlog>()), Times.AtLeastOnce);
        }
    }
}
