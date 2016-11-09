using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ItemMovementListener.Commands;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.IO;
using TIBCO.EMS;

namespace Icon.Esb.ItemMovement.Tests
{
    [TestClass]
    public class ItemMovementListenerTests
    {
        private ItemMovementListener.ItemMovementListener listener;
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<ItemMovementListener.ItemMovementListener>> mockLogger;
        private Mock<ICommandHandler<SaveItemMovementTransactionCommand>> mockSaveItemMovementTransactionCommandHandler;
        private ItemMovementListenerSettings listenerSettings;

        [TestInitialize]
        public void Initialize()
        {
            listenerApplicationSettings = new ListenerApplicationSettings();
            esbConnectionSettings = new EsbConnectionSettings();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<ItemMovementListener.ItemMovementListener>>();
            mockSaveItemMovementTransactionCommandHandler = new Mock<ICommandHandler<SaveItemMovementTransactionCommand>>();
            listenerSettings = new ItemMovementListenerSettings
            {
                MessageQueueSize = 1,
                PerformanceLoggingEnabled = false
            };

            listener = new ItemMovementListener.ItemMovementListener(
                listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockSaveItemMovementTransactionCommandHandler.Object,
                listenerSettings
            );
        }

        [TestMethod]
        public void ItemMovementListener_PerformanceLoggingIsDisabled_LoggerShouldNotBeCalled()
        {
            // Given.
            var xml = File.ReadAllText(@"TestMessages\Sample ItemMovement Message.xml");
            EsbMessage message = new EsbMessage(new TextMessage(null, xml));

            listenerSettings.PerformanceLoggingEnabled = false;

            // When.
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = message });

            // Then.
            mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void ItemMovementListener_PerformanceLoggingIsEnabled_LoggerShouldBeCalled()
        {
            // Given.
            var xml = File.ReadAllText(@"TestMessages\Sample ItemMovement Message.xml");
            EsbMessage message = new EsbMessage(new TextMessage(null, xml));

            listenerSettings.PerformanceLoggingEnabled = true;

            // When.
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = message });

            // Then.
            mockLogger.Verify(l => l.Info(It.IsAny<string>()), Times.Once);
        }
    }
}
