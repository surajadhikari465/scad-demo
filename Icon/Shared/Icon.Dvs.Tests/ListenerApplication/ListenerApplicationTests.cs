using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using Icon.Common.Email;
using Icon.Dvs.ListenerApplication;
using Icon.Dvs.Model;
using Icon.Dvs.Subscriber;
using Icon.Logging;
using Moq;

namespace Icon.Dvs.Tests.ListenerApplication
{
    public class TestListenerApplication: ListenerApplication<TestListenerApplication>
    {
        public TestListenerApplication(
            DvsListenerSettings settings,
            IDvsSubscriber subscriber,
            IEmailClient emailClient,
            ILogger<TestListenerApplication> logger
            ): base(settings, subscriber, emailClient, logger)
        {
        }

        public override void HandleMessage(DvsMessage message)
        {
        }

        public void LogAndNotifyErrorOverride(string errorMessage, Exception ex)
        {
            LogAndNotifyError(errorMessage, ex);
        }
    }

    [TestClass]
    public class ListenerApplicationTests
    {
        private Mock<IEmailClient> emailClient;
        private Mock<ILogger<TestListenerApplication>> logger;
        private Mock<IDvsSubscriber> subscriber;

        private TestListenerApplication listenerApp;

        [TestInitialize]
        public void Initialize()
        {
            emailClient = new Mock<IEmailClient>();
            var settings = DvsListenerSettings.CreateSettingsFromConfig();
            logger = new Mock<ILogger<TestListenerApplication>>();
            subscriber = new Mock<IDvsSubscriber>();

            listenerApp = new TestListenerApplication(settings, subscriber.Object, emailClient.Object, logger.Object);
        }

        [TestMethod]
        public void HandleException_ValidCaseTest()
        {
            // Given
            Exception ex = new Exception("Test Exception");
            emailClient.Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>()));
            logger.Setup(l => l.Error(It.IsAny<string>()));

            // When
            listenerApp.HandleException(ex, TestResources.GetDvsMessage());

            // Then
            emailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            logger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(2));
        }

        [TestMethod]
        public void HandleException_InValidCaseTest()
        {
            // Given
            Exception ex = new Exception("Test Exception");
            emailClient.Setup(e => e.Send(It.IsAny<string>(), It.IsAny<string>())).Throws(new Exception("Test"));
            logger.Setup(l => l.Error(It.IsAny<string>()));

            // When
            listenerApp.HandleException(ex, TestResources.GetDvsMessage());

            // Then
            emailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            logger.Verify(l => l.Error(It.IsAny<string>()), Times.Exactly(3));
        }
    }
}
