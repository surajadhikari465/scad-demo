using System;
using System.Collections.Generic;
using Icon.Common.Email;
using Icon.Esb;
using Icon.Esb.ListenerApplication;
using Icon.Esb.MessageParsers;
using Icon.Esb.Subscriber;
using Icon.Infor.Listeners.HierarchyClass.Models;
using Icon.Logging;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using TIBCO.EMS;
using Icon.Infor.Listeners.HierarchyClass.Notifier;
using Icon.Infor.Listeners.HierarchyClass.Validators;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Icon.Common.DataAccess;
using Icon.Common.Context;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.EsbService;

namespace Icon.Infor.Listeners.HierarchyClass.Tests
{
    [TestClass]
    public class HierarchyClassListenerTests
    {
        private HierarchyClassListener listener;
        private Mock<IMessageParser<IEnumerable<InforHierarchyClassModel>>> mockMessageParser;
        private Mock<IHierarchyClassService> mockService;
        private Mock<HierarchyClassEsbService> mockEsbService;  
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<HierarchyClassListener>> mockLogger;
        private Mock<IHierarchyClassListenerNotifier> mockNotifier;
        private Mock<ICollectionValidator<InforHierarchyClassModel>> mockValidator;
        private Mock<ICommandHandler<ArchiveHierarchyClassesCommand>> mockArchiveHierarchyClassesCommandHandler;
        private Mock<ICommandHandler<ArchiveMessageCommand>> mockArchiveMessageCommandHandler;

        private EsbMessageEventArgs args;
        private Mock<IEsbMessage> mockMessage;

        [TestInitialize]
        public void Initialize()
        {
            mockMessageParser = new Mock<IMessageParser<IEnumerable<InforHierarchyClassModel>>>();
            mockService = new Mock<IHierarchyClassService>();
            mockEsbService = new Mock<HierarchyClassEsbService>();
            listenerApplicationSettings = new ListenerApplicationSettings();
            esbConnectionSettings = new EsbConnectionSettings();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<HierarchyClassListener>>();
            mockNotifier = new Mock<IHierarchyClassListenerNotifier>();
            mockValidator = new Mock<ICollectionValidator<InforHierarchyClassModel>>();
            mockArchiveHierarchyClassesCommandHandler = new Mock<ICommandHandler<ArchiveHierarchyClassesCommand>>();
            mockArchiveMessageCommandHandler = new Mock<ICommandHandler<ArchiveMessageCommand>>();

            listener = new HierarchyClassListener(
                mockMessageParser.Object,
                mockValidator.Object,
                new[] { mockService.Object },
                mockEsbService.Object,
                mockArchiveHierarchyClassesCommandHandler.Object,
                mockArchiveMessageCommandHandler.Object,
                listenerApplicationSettings,
                esbConnectionSettings,
                mockSubscriber.Object,
                mockEmailClient.Object,
                mockNotifier.Object,
                mockLogger.Object);

            mockMessage = new Mock<IEsbMessage>();
            args = new EsbMessageEventArgs();
            args.Message = mockMessage.Object;
        }

        [TestMethod]
        public void HandleMessage_ValidMessage_ShouldCallServiceAndAcknowledgeMessage()
        {
            //Given
            mockMessageParser.Setup(m => m.ParseMessage(mockMessage.Object))
                .Returns(new List<InforHierarchyClassModel> { new InforHierarchyClassModel() });
            esbConnectionSettings.SessionMode = SessionMode.ClientAcknowledge;

            //When
            listener.HandleMessage(null, args);

            //Then
            mockService.Verify(m => m.ProcessHierarchyClassMessages(It.IsAny<IEnumerable<InforHierarchyClassModel>>()), Times.Once);
            mockMessage.Verify(m => m.Acknowledge(), Times.Once);
        }
    }
}
