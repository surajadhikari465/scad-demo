using Icon.RenewableContext;
using Icon.Common.Email;
using Icon.Esb.EwicAplListener.Common.Models;
using Icon.Esb.EwicAplListener.MessageParsers;
using Icon.Esb.EwicAplListener.NewAplProcessors;
using Icon.Esb.EwicAplListener.StorageServices;
using Icon.Esb.ListenerApplication;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;

namespace Icon.Esb.EwicAplListener.Tests.Unit
{
    [TestClass]
    public class EwicListenerTests
    {
        private EwicAplListener listener;
        private Mock<IRenewableContext<IconContext>> mockGlobalContext;
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<EwicAplListener>> mockLogger;
        private Mock<IMessageParser<AuthorizedProductListModel>> mockMessageParser;
        private Mock<IAplStorageService> mockStorageService;
        private Mock<INewAplProcessor> mockBusinessLogicProcessor;

        [TestInitialize]
        public void Initialize()
        {
            listenerApplicationSettings = EwicAplListenerApplicationSettings.CreateDefaultSettings<EwicAplListenerApplicationSettings>("eWIC Listener");
            esbConnectionSettings = EsbConnectionSettings.CreateSettingsFromNamedConnectionConfig("listener");
            
            mockGlobalContext = new Mock<IRenewableContext<IconContext>>();
            mockMessageParser = new Mock<IMessageParser<AuthorizedProductListModel>>();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<EwicAplListener>>();
            mockStorageService = new Mock<IAplStorageService>();
            mockBusinessLogicProcessor = new Mock<INewAplProcessor>();

            listener = new EwicAplListener(
                mockGlobalContext.Object,
                listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockMessageParser.Object,
                mockStorageService.Object,
                mockBusinessLogicProcessor.Object);
        }

        [TestMethod]
        public void HandleMessage_OnMessageArrival_MessageShouldBeParsed()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new AuthorizedProductListModel());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageParserThrowsException_ExceptionShouldBeLoggedAndNotificationSent()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Throws(new Exception());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_MessageParsedSuccessfully_StorageServiceShouldBeCalled()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new AuthorizedProductListModel());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockStorageService.Verify(s => s.Save(It.IsAny<AuthorizedProductListModel>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_StorageServiceThrowsException_ExceptionShouldBeLoggedAndNotificationSent()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new AuthorizedProductListModel());
            mockStorageService.Setup(s => s.Save(It.IsAny<AuthorizedProductListModel>())).Throws(new Exception());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockStorageService.Verify(s => s.Save(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_AplSavedSuccessfully_AutoMappingProcessShouldExecute()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new AuthorizedProductListModel());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockStorageService.Verify(s => s.Save(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockBusinessLogicProcessor.Verify(s => s.ApplyMappings(It.IsAny<AuthorizedProductListModel>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_AutoMappingCompletesSuccessfully_AutoExclusionsProcessShouldExecute()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new AuthorizedProductListModel());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockStorageService.Verify(s => s.Save(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockBusinessLogicProcessor.Verify(s => s.ApplyMappings(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockBusinessLogicProcessor.Verify(s => s.ApplyExclusions(It.IsAny<AuthorizedProductListModel>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_AutoMappingProcessThrowsException_ExceptionShouldBeLoggedAndNotificationSent()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new AuthorizedProductListModel());
            mockBusinessLogicProcessor.Setup(p => p.ApplyMappings(It.IsAny<AuthorizedProductListModel>())).Throws(new Exception());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockStorageService.Verify(s => s.Save(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockBusinessLogicProcessor.Verify(s => s.ApplyMappings(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockBusinessLogicProcessor.Verify(s => s.ApplyExclusions(It.IsAny<AuthorizedProductListModel>()), Times.Never);
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_AutoExclusionProcessThrowsException_ExceptionShouldBeLoggedAndNotificationSent()
        {
            // Given.
            mockMessageParser.Setup(p => p.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new AuthorizedProductListModel());
            mockBusinessLogicProcessor.Setup(p => p.ApplyExclusions(It.IsAny<AuthorizedProductListModel>())).Throws(new Exception());

            // When.
            listener.HandleMessage(new object(), new EsbMessageEventArgs { Message = new Mock<IEsbMessage>().Object });

            // Then.
            mockMessageParser.Verify(p => p.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockStorageService.Verify(s => s.Save(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockBusinessLogicProcessor.Verify(s => s.ApplyMappings(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockBusinessLogicProcessor.Verify(s => s.ApplyExclusions(It.IsAny<AuthorizedProductListModel>()), Times.Once);
            mockLogger.Verify(l => l.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(e => e.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
