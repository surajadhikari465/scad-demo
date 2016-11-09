using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Framework;
using Icon.ApiController.DataAccess.Commands;
using MammothCommands = Mammoth.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using System.Collections.Generic;
using Mammoth.Common.Testing.Builders;
using System.Linq;

namespace Mammoth.ApiController.DataAccess.Tests.Commands
{
    [TestClass]
    public class UpdateMessageQueueProcessedDateCommandHandlerTests : CommandHandlerTestBase<MammothCommands.UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>, UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>, MammothContext>
    {
        protected override void Initialize()
        {
            commandHandler = new MammothCommands.UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>(new GlobalContext<MammothContext>(context));
        }

        [TestMethod]
        public void UpdateMessageQueueProcessedDate_MessageQueuesExist_ShouldUpdateProcessedDate()
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

            command.MessagesToUpdate = messageQueues;
            command.ProcessedDate = DateTime.Now;

            //When
            commandHandler.Execute(command);

            //Then
            var updatedMessageQueues = context.MessageQueueItemLocales.AsNoTracking().Where(mq=>mq.ScanCode == "12345").ToList();
            foreach (var messageQueue in updatedMessageQueues)
            {
                Assert.AreEqual(command.ProcessedDate, messageQueue.ProcessedDate);
            }
        }
    }
}
