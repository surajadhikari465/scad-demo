﻿using Icon.Common.Context;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.HierarchyClass.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Infor.Listeners.HierarchyClass.Tests.Commands
{
    [TestClass]
    public class ArchiveMessageCommandHandlerTests : BaseHierarchyClassesCommandTest
    {
        private ArchiveMessageCommandHandler commandHandler;
        private ArchiveMessageCommand command;
        private Mock<IEsbMessage> mockEsbMessage;
        private Guid messageId;

        [TestInitialize]
        public void Initialize()
        {
            commandHandler = new ArchiveMessageCommandHandler(mockRenewableContext.Object);
            command = new ArchiveMessageCommand();

            messageId = Guid.NewGuid();
            mockEsbMessage = new Mock<IEsbMessage>();
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID")).Returns(messageId.ToString());

            command.Message = mockEsbMessage.Object;
        }

        [TestMethod]
        public void ArchiveMessage_MessageExists_ShouldSaveMessage()
        {
            //Given
            var messageText = File.ReadAllText(@"TestMessages/ArchiveMessageTestMessage.xml");
            mockEsbMessage.Setup(m => m.MessageText)
                .Returns(messageText);

            //When
            commandHandler.Execute(command);

            //Then
            var messageHistory = context.InforMessageHistory.Single(mh => mh.InforMessageId == messageId);
            Assert.IsNotNull(messageHistory);
            Assert.AreEqual(MessageTypes.InforHierarchy, messageHistory.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Consumed, messageHistory.MessageStatusId);
            Assert.AreEqual(messageText, messageHistory.Message);
        }
    }
}
