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
    public class AddPriceMessagesRowByRowCommandTests
    {
        private AddPriceMessagesRowByRowCommandHandler addPriceMessagesRowByRowCommandHandler;
        private GlobalIconContext context;
        private DbContextTransaction transaction;
        private List<MessageQueuePrice> messagesToInsert;
        private IRMAPush irmaPushEntry;

        [TestInitialize]
        public void Initialize()
        {
            context = new GlobalIconContext(new IconContext());
            addPriceMessagesRowByRowCommandHandler = new AddPriceMessagesRowByRowCommandHandler(context);
            
            transaction = context.Context.Database.BeginTransaction();

            irmaPushEntry = new TestIrmaPushBuilder().Build();
            context.Context.IRMAPush.Add(irmaPushEntry);
            context.Context.SaveChanges();

            messagesToInsert = new List<MessageQueuePrice>();
            for (int i = 0; i < 100; i++)
            {
                messagesToInsert.Add(new TestPriceMessageBuilder().WithIrmaPushId(irmaPushEntry.IRMAPushID));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {
            transaction.Rollback();
        }

        [TestMethod]
        public void AddPriceMessages_ValidListOfMessages_MessagesShouldBeSavedToTheDatabase()
        {
            // Given.
            var command = new AddPriceMessagesRowByRowCommand();
            
            // When.
            foreach (var message in messagesToInsert)
            {
                command.Message = message;
                addPriceMessagesRowByRowCommandHandler.Execute(command);
            }

            // Then.
            var insertedMessages = context.Context.MessageQueuePrice.Where(mq => mq.IRMAPushID == irmaPushEntry.IRMAPushID).ToList();
            Assert.AreEqual(messagesToInsert.Count, insertedMessages.Count);
        }

        [TestMethod]
        public void AddPriceMessages_InvalidItem_EntityShouldBeDetachedFromTheContext()
        {
            // Given.
            var command = new AddPriceMessagesRowByRowCommand
            {
                Message = new TestPriceMessageBuilder()
            };

            // When.
            try { addPriceMessagesRowByRowCommandHandler.Execute(command); }
            catch (Exception) { }

            // Then.
            Assert.AreEqual(EntityState.Detached, context.Context.Entry(command.Message).State);
        }
    }
}
