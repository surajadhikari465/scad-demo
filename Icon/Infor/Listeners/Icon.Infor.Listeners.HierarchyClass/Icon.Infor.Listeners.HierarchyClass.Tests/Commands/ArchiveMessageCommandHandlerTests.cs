using Icon.Common.Context;
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
    public class ArchiveMessageCommandHandlerTests
    {
        private ArchiveMessageCommandHandler commandHandler;
        private ArchiveMessageCommand command;
        private Mock<IRenewableContext<IconContext>> mockGlobalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private Mock<IEsbMessage> mockEsbMessage;
        private Guid testInforMessageId;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            transaction = context.Database.BeginTransaction();

            mockGlobalContext = new Mock<IRenewableContext<IconContext>>();
            mockGlobalContext.SetupGet(m => m.Context).Returns(context);

            commandHandler = new ArchiveMessageCommandHandler(mockGlobalContext.Object);
            command = new ArchiveMessageCommand();

            testInforMessageId = Guid.NewGuid();
            mockEsbMessage = new Mock<IEsbMessage>();
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID")).Returns(testInforMessageId.ToString());

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
            var messageHistory = context.InforMessageHistory.Single(mh => mh.InforMessageId == testInforMessageId);
            Assert.IsNotNull(messageHistory);
            Assert.AreEqual(MessageTypes.InforHierarchy, messageHistory.MessageTypeId);
            Assert.AreEqual(MessageStatusTypes.Consumed, messageHistory.MessageStatusId);
            Assert.AreEqual(messageText, messageHistory.Message);
        }
    }
}
