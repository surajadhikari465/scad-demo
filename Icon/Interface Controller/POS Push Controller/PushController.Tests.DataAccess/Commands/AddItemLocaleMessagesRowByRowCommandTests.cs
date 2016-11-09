using Icon.Framework;
using Icon.Framework.RenewableContext;
using Icon.Testing.Builders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using PushController.DataAccess.Commands;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;

namespace PushController.Tests.DataAccess.Commands
{
    [TestClass]
    public class AddItemLocaleMessagesRowByRowCommandTests
    {
        private AddItemLocaleMessagesRowByRowCommandHandler addItemLocaleMessagesRowByRowCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private List<MessageQueueItemLocale> messagesToInsert;
        private IRMAPush irmaPushEntry;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            addItemLocaleMessagesRowByRowCommandHandler = new AddItemLocaleMessagesRowByRowCommandHandler(context);

            transaction = context.Context.Database.BeginTransaction();

            irmaPushEntry = new TestIrmaPushBuilder().Build();
            context.Context.IRMAPush.Add(irmaPushEntry);
            context.Context.SaveChanges();

            messagesToInsert = new List<MessageQueueItemLocale>();
            for (int i = 0; i < 100; i++)
            {
                messagesToInsert.Add(new TestItemLocaleMessageBuilder().WithIrmaPushId(irmaPushEntry.IRMAPushID));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void AddItemLocaleMessages_ValidListOfMessages_MessagesShouldBeSavedToTheDatabase()
        {
            // Given.
            var command = new AddItemLocaleMessagesRowByRowCommand();

            // When.
            foreach (var message in messagesToInsert)
            {
                command.Message = message;
                addItemLocaleMessagesRowByRowCommandHandler.Execute(command);
            }

            // Then.
            var insertedMessages = context.Context.MessageQueueItemLocale.Where(mq => mq.IRMAPushID == irmaPushEntry.IRMAPushID).ToList();
            Assert.AreEqual(messagesToInsert.Count, insertedMessages.Count);
        }

        [TestMethod]
        public void AddItemLocaleMessages_InvalidItem_EntityShouldBeDetachedFromTheContext()
        {
            // Given.
            var command = new AddItemLocaleMessagesRowByRowCommand
            {
                Message = new TestItemLocaleMessageBuilder()
            };

            // When.
            try { addItemLocaleMessagesRowByRowCommandHandler.Execute(command); }
            catch (Exception) { }

            // Then.
            Assert.AreEqual(EntityState.Detached, context.Context.Entry(command.Message).State);
        }
    }
}
