using Icon.Common.Email;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using Wfm.Aws.ExtendedClient.Listener.SQS;
using Wfm.Aws.ExtendedClient.Listener.SQS.Settings;
using Wfm.Aws.ExtendedClient.SQS;
using Wfm.Aws.ExtendedClient.SQS.Model;

namespace Wfm.Aws.Tests.ExtendedClient.Listener.SQS
{
    public class TestSQSExtendedClientListener : SQSExtendedClientListener<TestSQSExtendedClientListener>
    {
        public TestSQSExtendedClientListener(
            SQSExtendedClientListenerSettings settings,
            IEmailClient emailClient,
            ISQSExtendedClient sqsExtendedClient,
            ILogger<TestSQSExtendedClientListener> logger
            ) : base(settings, emailClient, sqsExtendedClient, logger)
        {
        }

        public override void HandleMessage(SQSExtendedClientReceiveModel message)
        {
        }

        public void LogAndNotifyErrorOverride(string errorMessage, Exception ex)
        {
            LogAndNotifyError(errorMessage, ex);
        }
    }

    [TestClass]
    public class SQSExtendedClientListenerTests
    {
        private Mock<IEmailClient> emailClientMock;
        private Mock<ILogger<TestSQSExtendedClientListener>> loggerMock;
        private Mock<ISQSExtendedClient> sqsExtendedClientMock;
        private TestSQSExtendedClientListener listenerApp;
        private SQSExtendedClientReceiveModel message = new SQSExtendedClientReceiveModel()
        {
            SQSMessageID = "1"
        };

        [TestInitialize]
        public void Initialize()
        {
            emailClientMock = new Mock<IEmailClient>();
            var settings = SQSExtendedClientListenerSettings.CreateSettingsFromConfig();
            loggerMock = new Mock<ILogger<TestSQSExtendedClientListener>>();
            sqsExtendedClientMock = new Mock<ISQSExtendedClient>();
            listenerApp = new TestSQSExtendedClientListener(settings, emailClientMock.Object, sqsExtendedClientMock.Object, loggerMock.Object);
        }

        [TestMethod]
        public void HandleException_ValidCaseTest()
        {
            // Given
            Exception ex = new Exception("Test Exception");
            emailClientMock.Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>()));
            loggerMock.Setup(l => l.Error(It.IsAny<string>()));

            // When
            listenerApp.HandleException(ex, message);

            // Then
            emailClientMock.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void HandleException_InValidCaseTest()
        {
            // Given
            Exception ex = new Exception("Test Exception");
            emailClientMock.Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Test"));
            loggerMock.Setup(l => l.Error(It.IsAny<string>()));

            // When
            listenerApp.HandleException(ex, message);

            // Then
            emailClientMock.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            loggerMock.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
