using Icon.Common.Email;
using Icon.Logging;
using InterfaceController.Common;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Controller.ProcessorModules;
using PushController.Controller.Processors;
using System;

namespace PushController.Tests.Controller.ProcessorTests
{
    [TestClass]
    public class IrmaPosProcessorTests
    {
        private IrmaPosProcessor processor;
        private Mock<ILogger<IrmaPosProcessor>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IIrmaPosDataProcessingModule> mockStagingModule;

        [TestInitialize]
        public void Initialize()
        {
            EmailAlertApprover.Reset();
        }

        [TestMethod]
        public void StageIrmaPosData_UnhandledExceptionOccurs_AlertEmailShouldBeSent()
        {
            // Given.
            mockLogger = new Mock<ILogger<IrmaPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockStagingModule = new Mock<IIrmaPosDataProcessingModule>();
            
            mockStagingModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IrmaPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                mockStagingModule.Object);

            // When.
            processor.StageIrmaPosData();           

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StageIrmaPosData_EmailAlertsSentFirstTime_ShouldSendEmailAlert()
        {
            // Given.
            mockLogger = new Mock<ILogger<IrmaPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockStagingModule = new Mock<IIrmaPosDataProcessingModule>();

            mockStagingModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IrmaPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                mockStagingModule.Object);

            // When.
            processor.StageIrmaPosData();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void StageIrmaPosData_TwoEmailAlertsSentQuickly_ShouldOnlySendOneAlert()
        {
            // Given.
            mockLogger = new Mock<ILogger<IrmaPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockStagingModule = new Mock<IIrmaPosDataProcessingModule>();

            mockStagingModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IrmaPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                mockStagingModule.Object);

            // When.
            processor.StageIrmaPosData();
            processor.StageIrmaPosData();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
