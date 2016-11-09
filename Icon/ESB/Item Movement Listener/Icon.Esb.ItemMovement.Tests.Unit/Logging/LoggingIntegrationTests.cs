using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ItemMovementListener.Commands;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.IO;
using System.Linq;
using TIBCO.EMS;
using NLog;
using Icon.Logging;

namespace Icon.Esb.ItemMovement.Tests.Logging
{
    [TestClass]
    public class LoggingIntegrationTests
    {
        private IconContext context;
        private ItemMovementListener.ItemMovementListener listener;
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private NLogLogger<ItemMovementListener.ItemMovementListener> logger;
        private Mock<ICommandHandler<SaveItemMovementTransactionCommand>> mockSaveItemMovementTransactionCommandHandler;
        private ItemMovementListenerSettings listenerSettings;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            listenerApplicationSettings = new ListenerApplicationSettings();
            esbConnectionSettings = new EsbConnectionSettings();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            logger = new NLogLogger<ItemMovementListener.ItemMovementListener>();
            mockSaveItemMovementTransactionCommandHandler = new Mock<ICommandHandler<SaveItemMovementTransactionCommand>>();
            listenerSettings = new ItemMovementListenerSettings
            {
                MessageQueueSize = 1,
                PerformanceLoggingEnabled = true
            };

            listener = new ItemMovementListener.ItemMovementListener(
                listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                logger,
                mockSaveItemMovementTransactionCommandHandler.Object,
                listenerSettings
            );
        }

        [TestMethod]
        public void ItemMovementListener_PerformanceLoggingIsEnabled_LogShouldBeWrittenToDatabase()
        {
            // Given.
            var now = DateTime.Now.Subtract(TimeSpan.FromMilliseconds(5000));

            var xml = File.ReadAllText(@"TestMessages\Sample ItemMovement Message.xml");
            EsbMessage message = new EsbMessage(new TextMessage(null, xml));

            // When.
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = message });

            // Then.

            Assert.IsTrue(
                context.PerformanceLog
                    .OrderByDescending(l => l.PerformanceLogID)
                    .First()
                    .InsertDate > now);                    
        }
    }
}
