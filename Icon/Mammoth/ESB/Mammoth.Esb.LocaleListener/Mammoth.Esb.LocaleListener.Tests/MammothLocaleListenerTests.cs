using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Esb.LocaleListener.Commands;
using Mammoth.Esb.LocaleListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using TIBCO.EMS;

namespace Mammoth.Esb.LocaleListener.Tests
{
    [TestClass]
    public class MammothLocaleListenerTests
    {
        private MammothLocaleListener listener;
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<MammothLocaleListener>> mockLogger;
        private Mock<IMessageParser<List<LocaleModel>>> mockMessageParser;
        private Mock<ICommandHandler<AddOrUpdateLocalesCommand>> mockAddOrUpdateLocalesCommandHandler;
        private EsbMessageEventArgs eventArgs;
        private Mock<IEsbMessage> mockMessage;

        [TestInitialize]
        public void Initialize()
        {
            listenerApplicationSettings = new ListenerApplicationSettings();
            esbConnectionSettings = new EsbConnectionSettings();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<MammothLocaleListener>>();
            mockMessageParser = new Mock<IMessageParser<List<LocaleModel>>>();
            mockAddOrUpdateLocalesCommandHandler = new Mock<ICommandHandler<AddOrUpdateLocalesCommand>>();
            mockMessage = new Mock<IEsbMessage>();
            eventArgs = new EsbMessageEventArgs { Message = mockMessage.Object };

            listener = new MammothLocaleListener(listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockMessageParser.Object,
                mockAddOrUpdateLocalesCommandHandler.Object);
        }

        [TestMethod]
        public void HandleMessage_ParseSuccessful_ShouldAddOrUpdateLocales()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new List<LocaleModel> { new LocaleModel() });

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ParseNotSuccessful_ShouldLogAndNotify()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>())).Throws(new Exception());

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_UpdateNotSuccessful_ShouldLogAndNotify()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new List<LocaleModel> { new LocaleModel() });
            mockAddOrUpdateLocalesCommandHandler.Setup(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>())).Throws(new Exception());

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_SessionTypeIsNotClientAcknowledge_ShouldNotAcknowledgeMessage()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new List<LocaleModel> { new LocaleModel() });
            mockAddOrUpdateLocalesCommandHandler.Setup(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>())).Throws(new Exception());
            esbConnectionSettings.SessionMode = SessionMode.NoAcknowledge;

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockMessage.Verify(m => m.Acknowledge(), Times.Never);
        }

        [TestMethod]
        public void HandleMessage_SessionTypeIsClientAcknowledgeAndErrorsAreThrown_ShouldAlwaysAcknowledgeMessage()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>())).Returns(new List<LocaleModel> { new LocaleModel() });
            mockAddOrUpdateLocalesCommandHandler.Setup(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>())).Throws(new Exception());
            esbConnectionSettings.SessionMode = SessionMode.ClientAcknowledge;

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockAddOrUpdateLocalesCommandHandler.Verify(m => m.Execute(It.IsAny<AddOrUpdateLocalesCommand>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockMessage.Verify(m => m.Acknowledge(), Times.Once);
        }
    }
}
