using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Web.DataAccess.Commands;
using Icon.Framework;
using System.Data.Entity;
using System.Collections.Generic;
using Icon.Testing.Builders;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class SaveToMessageHistoryCommandHandlerTests
    {
        private SaveToMessageHistoryCommandHandler commandHandler;
        private IconContext context;
        private DbContextTransaction transaction;
        private List<MessageHistory> testMessages;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new SaveToMessageHistoryCommandHandler(this.context);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void SaveToMessageHistory_OneMessageToSave_OneMessageShouldBeSaved()
        {
            // Given.
            testMessages = new List<MessageHistory> { new TestMessageHistoryBuilder() };

            var command = new SaveToMessageHistoryCommand { Messages = testMessages };

            // When.
            commandHandler.Execute(command);

            // Then.
            int testMessageHistoryId = testMessages[0].MessageHistoryId;
            var newMessage = context.MessageHistory.SingleOrDefault(h => h.MessageHistoryId == testMessageHistoryId);

            Assert.IsNotNull(newMessage);
        }

        [TestMethod]
        public void SaveToMessageHistory_TwoMessagesToSave_TwoMessagesShouldBeSaved()
        {
            // Given.
            testMessages = new List<MessageHistory> { new TestMessageHistoryBuilder(), new TestMessageHistoryBuilder() };

            var command = new SaveToMessageHistoryCommand { Messages = testMessages };

            // When.
            commandHandler.Execute(command);

            // Then.
            int firstTestMessageHistoryId = testMessages[0].MessageHistoryId;
            int secondTestMessageHistoryId = testMessages[1].MessageHistoryId;

            var firstNewMessage = context.MessageHistory.SingleOrDefault(h => h.MessageHistoryId == firstTestMessageHistoryId);
            var secondNewMessage = context.MessageHistory.SingleOrDefault(h => h.MessageHistoryId == secondTestMessageHistoryId);

            Assert.IsNotNull(firstNewMessage);
            Assert.IsNotNull(secondNewMessage);
        }
    }
}
