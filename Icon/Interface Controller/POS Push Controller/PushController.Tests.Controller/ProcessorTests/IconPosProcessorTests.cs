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
    public class IconPosProcessorTests
    {
        private IconPosProcessor processor;
        private Mock<ILogger<IconPosProcessor>> mockLogger;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IIconPosDataProcessingModule> mockEsbModule;
        private Mock<IIconPosDataProcessingModule> mockUdmModule;

        [TestInitialize]
        public void Initialize()
        {
            EmailAlertApprover.Reset();
        }

        [TestMethod]
        public void ProcessPosDataForEsb_UnhandledExceptionOccurs_AlertEmailShouldBeSent()
        {
            // Given.
            mockLogger = new Mock<ILogger<IconPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockEsbModule = new Mock<IIconPosDataProcessingModule>();
            mockUdmModule = new Mock<IIconPosDataProcessingModule>();

            mockEsbModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IconPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                mockEsbModule.Object,
                mockUdmModule.Object);

            // When.
            processor.ProcessPosDataForEsb();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ProcessPosDataForUdm_UnhandledExceptionOccurs_AlertEmailShouldBeSent()
        {
            // Given.
            mockLogger = new Mock<ILogger<IconPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockEsbModule = new Mock<IIconPosDataProcessingModule>();
            mockUdmModule = new Mock<IIconPosDataProcessingModule>();

            mockUdmModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IconPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                mockEsbModule.Object,
                mockUdmModule.Object);

            // When.
            processor.ProcessPosDataForUdm();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ProcessPosDataForEsb_EmailAlertsSentFirstTime_ShouldSendEmailAlert()
        {
            // Given.
            mockLogger = new Mock<ILogger<IconPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockEsbModule = new Mock<IIconPosDataProcessingModule>();
            mockUdmModule = new Mock<IIconPosDataProcessingModule>();

            mockEsbModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IconPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                mockEsbModule.Object,
                mockUdmModule.Object);

            // When.
            processor.ProcessPosDataForEsb();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ProcessPosDataForEsb_TwoEmailAlertsSentQuickly_ShouldOnlySendOneAlert()
        {
            // Given.
            mockLogger = new Mock<ILogger<IconPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockEsbModule = new Mock<IIconPosDataProcessingModule>();
            mockUdmModule = new Mock<IIconPosDataProcessingModule>();

            mockEsbModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IconPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                  mockEsbModule.Object,
                mockUdmModule.Object);

            // When.
            processor.ProcessPosDataForEsb();
            processor.ProcessPosDataForEsb();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ProcessPosDataForUdm_EmailAlertsSentFirstTime_ShouldSendEmailAlert()
        {
            // Given.
            mockLogger = new Mock<ILogger<IconPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockEsbModule = new Mock<IIconPosDataProcessingModule>();
            mockUdmModule = new Mock<IIconPosDataProcessingModule>();

            mockUdmModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IconPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                  mockEsbModule.Object,
                mockUdmModule.Object);

            // When.
            processor.ProcessPosDataForUdm();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void ProcessPosDataForUdm_TwoEmailAlertsSentQuickly_ShouldOnlySendOneAlert()
        {
            // Given.
            mockLogger = new Mock<ILogger<IconPosProcessor>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockEsbModule = new Mock<IIconPosDataProcessingModule>();
            mockUdmModule = new Mock<IIconPosDataProcessingModule>();

            mockUdmModule.Setup(m => m.Execute()).Throws(new TimeoutException());

            processor = new IconPosProcessor(
                mockLogger.Object,
                mockEmailClient.Object,
                  mockEsbModule.Object,
                mockUdmModule.Object);

            // When.
            processor.ProcessPosDataForUdm();
            processor.ProcessPosDataForUdm();

            // Then.
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
