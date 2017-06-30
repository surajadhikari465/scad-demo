using Icon.Common.Context;
using Icon.Esb.Subscriber;
using Icon.Framework;
using Icon.Infor.Listeners.Item.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace Icon.Infor.Listeners.Item.Tests.Commands
{
    [TestClass]
    public class ArchiveMessageCommandHandlerTests
    {
        private ArchiveMessageCommandHandler commandHandler;
        private ArchiveMessageCommand command;
        private IconContext context;
        private Mock<IEsbMessage> mockEsbMessage;
        private IconDbContextFactory contextFactory;
        private TransactionScope transaction;

        [TestInitialize]
        public void Initialize()
        {
            transaction = new TransactionScope();
            contextFactory = new IconDbContextFactory();
            context = new IconContext();

            mockEsbMessage = new Mock<IEsbMessage>();

            commandHandler = new ArchiveMessageCommandHandler(contextFactory);
            command = new ArchiveMessageCommand { Message = mockEsbMessage.Object };
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Dispose();
        }

        [TestMethod]
        public void ArchiveMessage_MessageExists_MessageShouldBeSavedToDatabase()
        {
            //Given
            var message = File.ReadAllText(@"TestMessages/ArchiveMessageTestMessage.xml");
            var inforMessageId = Guid.NewGuid();

            mockEsbMessage.SetupGet(m => m.MessageText).Returns(message);
            mockEsbMessage.Setup(m => m.GetProperty("IconMessageID")).Returns(inforMessageId.ToString());

            //When
            commandHandler.Execute(command);

            //Then
            var messageHistory = context.InforMessageHistory.FirstOrDefault(m => m.InforMessageId == inforMessageId);
            Assert.IsNotNull(messageHistory);
        }
    }
}
