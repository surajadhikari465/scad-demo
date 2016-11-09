using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.ApiController.Controller;
using Moq;
using Icon.Logging;
using Icon.Common.Email;
using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Common;
using Icon.Esb.Producer;

namespace Icon.ApiController.Tests
{
    [TestClass]
    public class ApiControllerBaseTests
    {
        private ApiControllerBase controller;

        private Mock<ILogger<ApiControllerBase>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IHistoryProcessor> mockHistoryProcessor;
        private Mock<IQueueProcessor> mockQueueProcessor;
        private Mock<IEsbProducer> mockProducer;

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ApiControllerBase>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockHistoryProcessor = new Mock<IHistoryProcessor>();
            mockQueueProcessor = new Mock<IQueueProcessor>();
            mockProducer = new Mock<IEsbProducer>();
            mockProducer.SetupGet(m => m.IsConnected).Returns(true);

            ControllerType.Type = "Product";
            controller = new ApiControllerBase(mockLogger.Object, mockEmailClient.Object, mockHistoryProcessor.Object, mockQueueProcessor.Object, mockProducer.Object);
        }

        [TestMethod]
        public void ExecuteHistoryProcessor_UnhandledExceptionOccurs_AlertEmailShouldBeSent()
        {
            // Given.
            mockHistoryProcessor.Setup(p => p.ProcessMessageHistory()).Throws(new Exception());

            // When.
            controller.Execute();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockQueueProcessor.Verify(p => p.ProcessMessageQueue(), Times.Never);
            mockProducer.Verify(p => p.Dispose(), Times.Once);
        }

        [TestMethod]
        public void ExecuteQueueProcessor_UnhandledExceptionOccurs_AlertEmailShouldBeSent()
        {
            // Given.
            mockQueueProcessor.Setup(p => p.ProcessMessageQueue()).Throws(new Exception());

            // When.
            controller.Execute();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockProducer.Verify(p => p.Dispose(), Times.Once);
        }

        [TestMethod]
        public void ExecuteQueueProcessor_ProducerNotConnected_ShouldNotCallDispose()
        {
            // Given.
            mockProducer.SetupGet(m => m.IsConnected).Returns(false);

            // When.
            controller.Execute();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockProducer.Verify(p => p.Dispose(), Times.Never);
        }
    }
}
