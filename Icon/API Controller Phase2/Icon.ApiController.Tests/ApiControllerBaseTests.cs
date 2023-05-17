using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.ApiController.Controller;
using Moq;
using Icon.Logging;
using Icon.Common.Email;
using Icon.ApiController.Controller.HistoryProcessors;
using Icon.ApiController.Controller.QueueProcessors;
using Icon.ApiController.Common;

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

        [TestInitialize]
        public void Initialize()
        {
            mockLogger = new Mock<ILogger<ApiControllerBase>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockHistoryProcessor = new Mock<IHistoryProcessor>();
            mockQueueProcessor = new Mock<IQueueProcessor>();

            ControllerType.Type = "Product";
            controller = new ApiControllerBase(mockLogger.Object, mockEmailClient.Object, mockHistoryProcessor.Object, mockQueueProcessor.Object);
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
            
        }
    }
}
