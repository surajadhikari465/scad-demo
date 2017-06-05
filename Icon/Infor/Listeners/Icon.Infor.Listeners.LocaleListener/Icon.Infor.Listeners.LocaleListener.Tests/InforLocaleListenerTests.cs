using System;
using System.Text;
using System.Collections.Generic;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.MessageParsers;
using Icon.Infor.Listeners.LocaleListener.Models;
using Icon.Common.DataAccess;
using Icon.Infor.Listeners.LocaleListener.Commands;
using Icon.Esb.ListenerApplication;
using Icon.Esb;
using Icon.Esb.Subscriber;
using Icon.Common.Email;
using Icon.Logging;
using Moq;

namespace Icon.Infor.Listeners.LocaleListener.Tests
{
    /// <summary>
    /// Summary description for InforLocaleListenerTests
    /// </summary>
    [TestClass]
    public class InforLocaleListenerTests
    {
        private InforLocaleListener listener;
        private Mock<ICommandHandler<AddOrUpdateLocalesCommand>> mockAddOrUpdateLocalesCommandHandler;
        private Mock<ICommandHandler<ArchiveLocaleMessageCommand>> mockArchiveLocaleMessageCommandHandler;
        private Mock<IEmailClient> mockEmailClient;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<ICommandHandler<GenerateLocaleMessagesCommand>> mockGenerateLocaleMessagesCommandHandler;
        private ListenerApplicationSettings listenerApplicationSettings;
        private Mock<ILogger<InforLocaleListener>> mockLogger;
        private Mock<IMessageParser<LocaleModel>> mockMessageParser;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEsbMessage> mockEsbMessage;

        [TestInitialize]
        public void Initialize()
        {
            mockAddOrUpdateLocalesCommandHandler = new Mock<ICommandHandler<AddOrUpdateLocalesCommand>>();
            mockArchiveLocaleMessageCommandHandler = new Mock<ICommandHandler<ArchiveLocaleMessageCommand>>();
            mockEmailClient = new Mock<IEmailClient>();
            esbConnectionSettings = new EsbConnectionSettings();
            mockGenerateLocaleMessagesCommandHandler = new Mock<ICommandHandler<GenerateLocaleMessagesCommand>>();
            listenerApplicationSettings = new ListenerApplicationSettings();
            mockLogger = new Mock<ILogger<InforLocaleListener>>();
            mockMessageParser = new Mock<IMessageParser<LocaleModel>>();
            mockSubscriber = new Mock<IEsbSubscriber>();

            listener = new InforLocaleListener(
                mockMessageParser.Object,
                mockAddOrUpdateLocalesCommandHandler.Object,
                mockGenerateLocaleMessagesCommandHandler.Object,
                mockArchiveLocaleMessageCommandHandler.Object,
                listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object);

            mockEsbMessage = new Mock<IEsbMessage>();
        }

        [TestMethod]
        public void HandleMessage_SuccessfullyParsesMessage_CallsCommandHandlers()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new LocaleModel());

            //When
            listener.HandleMessage(null, new EsbMessageEventArgs { Message = mockEsbMessage.Object });

            //Then
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
            mockGenerateLocaleMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<GenerateLocaleMessagesCommand>()), Times.Once);
            mockArchiveLocaleMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveLocaleMessageCommand>()), Times.Once);
        }
    }
}
