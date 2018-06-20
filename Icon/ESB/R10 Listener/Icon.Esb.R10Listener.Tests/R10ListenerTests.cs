using Icon.Common.DataAccess;
using Icon.Common.Email;
using Icon.Esb.ListenerApplication;
using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Constants;
using Icon.Esb.R10Listener.MessageParsers;
using Icon.Esb.R10Listener.Models;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using TIBCO.EMS;

namespace Icon.Esb.R10Listener.Tests
{
    [TestClass]
    public class R10ListenerTests
    {
        private R10Listener r10Listener;
        private ListenerApplicationSettings applicationSettings;
        private EsbConnectionSettings connectionSettings;
        private Mock<ICommandHandler<SaveR10MessageResponseCommand>> mockSaveR10MessageResponseCommandHandler;
        private Mock<IMessageParser<R10MessageResponseModel>> mockMessageParser;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<R10Listener>> mockLogger;
        private Mock<IEsbSubscriber> mockEsbSubscriber;
        private EsbMessageEventArgs args;
        private R10MessageResponseModel r10MessageResponse;

        [TestInitialize]
        public void Initialize()
        {
            applicationSettings = new ListenerApplicationSettings();
            connectionSettings = new EsbConnectionSettings();
            mockEsbSubscriber = new Mock<IEsbSubscriber>();
            mockSaveR10MessageResponseCommandHandler = new Mock<ICommandHandler<SaveR10MessageResponseCommand>>();
            mockMessageParser = new Mock<IMessageParser<R10MessageResponseModel>>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<R10Listener>>();
            args = new EsbMessageEventArgs();
            r10MessageResponse = new R10MessageResponseModel();

            r10Listener = new R10Listener(applicationSettings,
                connectionSettings,
                mockEsbSubscriber.Object,
                mockSaveR10MessageResponseCommandHandler.Object,
                mockMessageParser.Object,
                mockEmailClient.Object,
                mockLogger.Object);
        }

        [TestMethod]
        public void HandleMessage_ValidMessageResponseAndRequestSuccessIsTrue_ShouldProcessMessageResponse()
        {
            //Given
            r10MessageResponse.RequestSuccess = true;
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(r10MessageResponse)
                .Verifiable();

            //When
            r10Listener.HandleMessage(null, args);

            //Then
            mockMessageParser.Verify();
            mockSaveR10MessageResponseCommandHandler.Verify(m => m.Execute(It.IsAny<SaveR10MessageResponseCommand>()), Times.Once);

            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [TestMethod]
        public void HandleMessage_RequestSuccessIsFalse_ShouldLogError()
        {
            //Given
            r10MessageResponse.RequestSuccess = false;
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(r10MessageResponse)
                .Verifiable();

            //When
            r10Listener.HandleMessage(null, args);

            //Then
            mockMessageParser.Verify();
            mockSaveR10MessageResponseCommandHandler.Verify(m => m.Execute(It.IsAny<SaveR10MessageResponseCommand>()), Times.Once);
            
            mockLogger.Verify(m => m.Error(It.Is<string>(s => s.Contains(NotificationConstants.UnsuccessfulRequest))), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_UnsuccessfulMessageParse_ShouldLogError()
        {
            //Given
            r10MessageResponse = null;
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Throws(new Exception("Test Exception"))
                .Verifiable();

            //When
            r10Listener.HandleMessage(null, args);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockSaveR10MessageResponseCommandHandler.Verify(m => m.Execute(It.IsAny<SaveR10MessageResponseCommand>()), Times.Never);

            mockLogger.Verify(m => m.Error(It.Is<string>(s => s.Contains(NotificationConstants.UnsuccessfulParse))), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_UnsuccessfulMessageProcessing_ShouldLogError()
        {
            //Given
            r10MessageResponse = new R10MessageResponseModel
            {
                MessageId = "123",
                RequestSuccess = true
            };
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(r10MessageResponse)
                .Verifiable();
            mockSaveR10MessageResponseCommandHandler
                .Setup(m => m.Execute(It.IsAny<SaveR10MessageResponseCommand>()))
                .Throws(new Exception("Test Exception"))
                .Verifiable();

            //When
            r10Listener.HandleMessage(null, args);

            //Then
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockSaveR10MessageResponseCommandHandler.Verify();

            mockLogger.Verify(m => m.Error(It.Is<string>(s =>
                    s.Contains(NotificationConstants.UnsuccessfulMessageProcessing) &&
                    s.Contains("123") &&
                    s.Contains("Test Exception"))),
                Times.Once);
        }

        [TestMethod]
        public void HandleMessage_NoErrorAndSessionModeIsClientAcknowledge_ShouldAcknowledgeMessage()
        {
            //Given
            Mock<IEsbMessage> mockEsbMessage = new Mock<IEsbMessage>();
            args.Message = mockEsbMessage.Object;
            connectionSettings.SessionMode = SessionMode.ClientAcknowledge;

            //When
            r10Listener.HandleMessage(null, args);

            //Then
            mockEsbMessage.Verify(m => m.Acknowledge());
        }

        [TestMethod]
        public void HandleMessage_ErrorAndSessionModeIsClientAcknowledge_ShouldAcknowledgeMessage()
        {
            //Given
            Mock<IEsbMessage> mockEsbMessage = new Mock<IEsbMessage>();
            args.Message = mockEsbMessage.Object;
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>())).Throws(new Exception("Test Message"));
            connectionSettings.SessionMode = SessionMode.ClientAcknowledge;

            //When
            r10Listener.HandleMessage(null, args);

            //Then
            mockEsbMessage.Verify(m => m.Acknowledge());
        }
    }
}