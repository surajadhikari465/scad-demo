﻿using System;
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

namespace Icon.Infor.Listeners.HierarchyClass.Tests
{
    [TestClass]
    public class HierarchyClassListenerTests
    {
        private HierarchyClassListener listener;
        private Mock<IMessageParser<IEnumerable<HierarchyClassModel>>> mockMessageParser;
        private Mock<IHierarchyClassService> mockService;
        private ListenerApplicationSettings listenerApplicationSettings;
        private EsbConnectionSettings esbConnectionSettings;
        private Mock<IEsbSubscriber> mockSubscriber;
        private Mock<IEmailClient> mockEmailClient;
        private Mock<ILogger<HierarchyClassListener>> mockLogger;
        private Mock<IHierarchyClassListenerNotifier> mockNotifier;
        private Mock<ICollectionValidator<HierarchyClassModel>> mockValidator;
        private Mock<ICommandHandler<ArchiveHierarchyClassesCommand>> mockArchiveHierarchyClassesCommandHandler;
        private Mock<ICommandHandler<ArchiveMessageCommand>> mockArchiveMessageCommandHandler;
        private Mock<IRenewableContext<IconContext>> mockGlobalContext;

        private EsbMessageEventArgs args;
        private Mock<IEsbMessage> mockMessage;

        [TestInitialize]
        public void Initialize()
        {
            mockMessageParser = new Mock<IMessageParser<IEnumerable<HierarchyClassModel>>>();
            mockService = new Mock<IHierarchyClassService>();
            listenerApplicationSettings = new ListenerApplicationSettings();
            esbConnectionSettings = new EsbConnectionSettings();
            mockSubscriber = new Mock<IEsbSubscriber>();
            mockEmailClient = new Mock<IEmailClient>();
            mockLogger = new Mock<ILogger<HierarchyClassListener>>();
            mockNotifier = new Mock<IHierarchyClassListenerNotifier>();
            mockValidator = new Mock<ICollectionValidator<HierarchyClassModel>>();
            mockArchiveHierarchyClassesCommandHandler = new Mock<ICommandHandler<ArchiveHierarchyClassesCommand>>();
            mockArchiveMessageCommandHandler = new Mock<ICommandHandler<ArchiveMessageCommand>>();
            mockGlobalContext = new Mock<IRenewableContext<IconContext>>();

            listener = new HierarchyClassListener(
                mockMessageParser.Object,
                mockValidator.Object,
                new[] { mockService.Object },
                mockGlobalContext.Object,
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
                .Returns(new List<HierarchyClassModel> { new HierarchyClassModel() });
            esbConnectionSettings.SessionMode = SessionMode.ClientAcknowledge;

            //When
            listener.HandleMessage(null, args);

            //Then
            mockService.Verify(m => m.ProcessHierarchyClassMessages(It.IsAny<IEnumerable<HierarchyClassModel>>()), Times.Once);
            mockMessage.Verify(m => m.Acknowledge(), Times.Once);
        }
    }
}
