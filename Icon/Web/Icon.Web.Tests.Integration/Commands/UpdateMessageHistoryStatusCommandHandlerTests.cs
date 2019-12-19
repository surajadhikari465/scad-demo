using System.Linq;
using Icon.Framework;
using Icon.Web.DataAccess.Commands;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Data.Entity;
using Icon.Testing.Builders;

namespace Icon.Web.Tests.Integration.Commands
{
    [TestClass] [Ignore]
    public class UpdateMessageHistoryStatusCommandHandlerTests
    {
        private UpdateMessageHistoryStatusCommandHandler commandHandler;
        
        private IconContext context;
        private DbContextTransaction transaction;
        private List<MessageHistory> testMessages;
        private List<int> testMessagesById;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            commandHandler = new UpdateMessageHistoryStatusCommandHandler(this.context);

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void UpdateMessageHistoryStatus_StatusUpdatedToSent_MessageStatusShouldBeSent()
        {
            // Given.
            testMessages = new List<MessageHistory>
            {
                new TestMessageHistoryBuilder(),
                new TestMessageHistoryBuilder(),
                new TestMessageHistoryBuilder()
            };

            context.MessageHistory.AddRange(testMessages);
            context.SaveChanges();

            testMessagesById = new List<int>
            {
                testMessages[0].MessageHistoryId,
                testMessages[1].MessageHistoryId,
                testMessages[2].MessageHistoryId
            };

            var command = new UpdateMessageHistoryStatusCommand
            {
                MessageStatusId = MessageStatusTypes.Sent,
                MessageHistoriesById = testMessagesById
            };

            // When.
            commandHandler.Execute(command);

            // Then.
            var updatedMessages = context.MessageHistory.Where(h => testMessagesById.Contains(h.MessageHistoryId)).ToList();

            bool allMessagesAreSentStatus = updatedMessages.TrueForAll(m => m.MessageStatusId == MessageStatusTypes.Sent);

            Assert.IsTrue(allMessagesAreSentStatus);
        }
    }
}
