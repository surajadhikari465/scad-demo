using System;
using System.Collections.Generic;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Logging;
using Mammoth.Esb.HierarchyClassListener.Models;
using Mammoth.Esb.HierarchyClassListener.Services;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Mammoth.Esb.HierarchyClassListener.Commands;
using TIBCO.EMS;

namespace Mammoth.Esb.HierarchyClassListener.Tests
{
    [TestClass]
    public class MammothHierarchyClassListenerTests
    {
        private MammothHierarchyClassListener listener;
        private EsbConnectionSettings esbConnectionSettings;
        private ListenerApplicationSettings listenerApplicationSettings;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<IHierarchyClassService> mockHierarchyClassService;
        private Mock<ILogger<MammothHierarchyClassListener>> mockLogger;
        private Mock<IMessageParser<List<HierarchyClassModel>>> mockMessageParser;
        private Mock<IEsbSubscriber> mockSubscriber;
        private EsbMessageEventArgs eventArgs;
        private Mock<IEsbMessage> mockEsbMessage;

        [TestInitialize]
        public void Initialize()
        {
            esbConnectionSettings = new EsbConnectionSettings { SessionMode = SessionMode.ClientAcknowledge };
            listenerApplicationSettings = new ListenerApplicationSettings();
            mockEmailClient = new Mock<IEmailClient>();
            mockHierarchyClassService = new Mock<IHierarchyClassService>();
            mockLogger = new Mock<ILogger<MammothHierarchyClassListener>>();
            mockMessageParser = new Mock<IMessageParser<List<HierarchyClassModel>>>();
            mockSubscriber = new Mock<IEsbSubscriber>();

            this.listener = new MammothHierarchyClassListener(listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockLogger.Object,
                mockMessageParser.Object,
                mockHierarchyClassService.Object);

            mockEsbMessage = new Mock<IEsbMessage>();
            eventArgs = new EsbMessageEventArgs { Message = mockEsbMessage.Object };
        }

        [TestMethod]
        public void HandleMessage_SuccessfulMessage_ShouldAcknowledgeMessageAndNotLogError()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel() });

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Never);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Never);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockHierarchyClassService.Verify(m => m.AddOrUpdateHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ErrorParsingMessage_ShouldLogAndNotifyException()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Throws(new Exception());

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockHierarchyClassService.Verify(m => m.AddOrUpdateHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Never);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }

        [TestMethod]
        public void HandleMessage_ErrorCallingHierarchyClasses_ShouldLogAndNotifyException()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(It.IsAny<IEsbMessage>()))
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel() });
            mockHierarchyClassService.Setup(m => m.AddOrUpdateHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassesCommand>()))
                .Throws(new Exception());

            //When
            listener.HandleMessage(null, eventArgs);

            //Then
            mockEmailClient.Verify(m => m.Send(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
            mockLogger.Verify(m => m.Error(It.IsAny<string>()), Times.Once);
            mockMessageParser.Verify(m => m.ParseMessage(It.IsAny<IEsbMessage>()), Times.Once);
            mockHierarchyClassService.Verify(m => m.AddOrUpdateHierarchyClasses(It.IsAny<AddOrUpdateHierarchyClassesCommand>()), Times.Once);
            mockEsbMessage.Verify(m => m.Acknowledge(), Times.Once);
        }
    }
}
