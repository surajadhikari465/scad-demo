using Icon.RenewableContext;
using Icon.Esb.EwicAplListener.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicAplListener.Tests.Integration.Commands
{
    [TestClass]
    public class AddMessageHistoryCommandTests
    {
        private AddMessageHistoryCommand command;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private MessageHistory testMessage;

        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);

            command = new AddMessageHistoryCommand(globalContext);

            testMessage = new MessageHistory
            {
                MessageTypeId = MessageTypes.Ewic,
                MessageStatusId = MessageStatusTypes.Consumed,
                InsertDate = DateTime.Now,
                Message = String.Empty,
                InProcessBy = null,
                ProcessedDate = DateTime.Now
            };

            transaction = context.Database.BeginTransaction();
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void AddMessageHistory_ValidMessage_MessageShouldBeSavedToDatabase()
        {
            // Given.
            var parameters = new AddMessageHistoryParameters { Message = testMessage };

            // When.
            command.Execute(parameters);

            // Then.
            var savedMessage = context.MessageHistory.SingleOrDefault(m => m.MessageHistoryId == testMessage.MessageHistoryId);

            Assert.IsNotNull(savedMessage);
        }
    }
}
