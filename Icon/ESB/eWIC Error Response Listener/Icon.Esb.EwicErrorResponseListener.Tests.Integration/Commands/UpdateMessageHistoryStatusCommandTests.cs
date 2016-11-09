using Icon.RenewableContext;
using Icon.Esb.EwicErrorResponseListener.DataAccess.Commands;
using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Data.Entity;
using System.Linq;

namespace Icon.Esb.EwicErrorResponseListener.Tests.Integration.Commands
{
    [TestClass]
    public class UpdateMessageHistoryStatusCommandTests
    {
        private UpdateMessageHistoryStatusCommand command;
        private IRenewableContext<IconContext> globalContext;
        private IconContext context;
        private DbContextTransaction transaction;
        private MessageHistory testMessage;
        
        [TestInitialize]
        public void Initialize()
        {
            context = new IconContext();
            globalContext = new GlobalIconContext(context);
            command = new UpdateMessageHistoryStatusCommand(globalContext);

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
            testMessage = new TestMessageHistoryBuilder();

            context.MessageHistory.Add(testMessage);
            context.SaveChanges();

            var parameters = new UpdateMessageHistoryStatusParameters
            {
                MessageStatusId = MessageStatusTypes.Sent,
                MessageHistoryId = testMessage.MessageHistoryId
            };

            // When.
            command.Execute(parameters);

            // Then.
            var updatedMessage = context.MessageHistory.SingleOrDefault(h => testMessage.MessageHistoryId == h.MessageHistoryId);

            Assert.IsNotNull(updatedMessage);
            Assert.AreEqual(parameters.MessageStatusId, updatedMessage.MessageStatusId);
        }
    }
}
