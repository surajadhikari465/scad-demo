using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothCommands = Mammoth.ApiController.DataAccess.Commands;
using Mammoth.Framework;
using Icon.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using Mammoth.Common.Testing.Builders;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.ApiController.DataAccess.Tests.Commands
{
    [TestClass]
    public class MarkQueuedEntriesAsInProcessCommandHandlerTests : CommandHandlerTestBase<MammothCommands.MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>, MarkQueuedEntriesAsInProcessCommand<MessageQueueItemLocale>, MammothContext>
    {
        protected override void Initialize()
        {
            commandHandler = new MammothCommands.MarkQueuedEntriesAsInProcessCommandHandler<MessageQueueItemLocale>(new MammothContextFactory());
            context.Database.ExecuteSqlCommand("truncate table esb.MessageQueueItemLocale;");
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_MessageQueuesExist_ShouldMarkMessageQueuesInProcess()
        {
            //Given
            List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder()
            };
            context.MessageQueueItemLocales.AddRange(messageQueues);
            context.SaveChanges();

            command.Instance = 55;
            command.LookAhead = 100;

            //When
            commandHandler.Execute(command);

            //Then
            var actualMessageQueues = context.MessageQueueItemLocales.AsNoTracking().ToList();
            Assert.AreEqual(3, actualMessageQueues.Count);
            foreach (var messageQueue in actualMessageQueues)
            {
                Assert.AreEqual(command.Instance, messageQueue.InProcessBy);
            }
        }

        [TestMethod]
        public void MarkQueuedEntriesAsInProcess_LookAheadIsLessThanNumberOfMessageQueues_ShouldMarkMessageQueuesInProcess()
        {
            //Given
            List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder()
            };
            context.MessageQueueItemLocales.AddRange(messageQueues);
            context.SaveChanges();

            command.Instance = 55;
            command.LookAhead = 2;

            //When
            commandHandler.Execute(command);

            //Then
            var actualMessageQueues = context.MessageQueueItemLocales.AsNoTracking().ToList();
            Assert.AreEqual(3, actualMessageQueues.Count);
            foreach (var messageQueue in actualMessageQueues.Take(2))
            {
                Assert.AreEqual(command.Instance, messageQueue.InProcessBy);
            }
            Assert.AreEqual(null, actualMessageQueues.Last().InProcessBy);
        }
    }
}
