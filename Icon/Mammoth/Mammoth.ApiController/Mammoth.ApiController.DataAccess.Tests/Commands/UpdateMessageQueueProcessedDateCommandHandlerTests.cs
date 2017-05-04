using Icon.ApiController.DataAccess.Commands;
using Mammoth.Common.Testing.Builders;
using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using MammothCommands = Mammoth.ApiController.DataAccess.Commands;

namespace Mammoth.ApiController.DataAccess.Tests.Commands
{
    [TestClass]
    public class UpdateMessageQueueProcessedDateCommandHandlerTests : CommandHandlerTestBase<MammothCommands.UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>, UpdateMessageQueueProcessedDateCommand<MessageQueueItemLocale>, MammothContext>
    {
        protected override void Initialize()
        {
            commandHandler = new MammothCommands.UpdateMessageQueueProcessedDateCommandHandler<MessageQueueItemLocale>(new MammothContextFactory());
        }

        [TestMethod]
        public void UpdateMessageQueueProcessedDate_MessageQueuesExists_ShouldUpdateProcessedDate()
        {
            //Given
            var testScanCode = "9888888888888";
            List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder().WithScanCode(testScanCode),
                new TestMessageQueueItemLocaleBuilder().WithScanCode(testScanCode),
                new TestMessageQueueItemLocaleBuilder().WithScanCode(testScanCode)
            };
            context.MessageQueueItemLocales.AddRange(messageQueues);
            context.SaveChanges();

            command.MessagesToUpdate = messageQueues;
            command.ProcessedDate = DateTime.Now;

            //When
            commandHandler.Execute(command);

            //Then
            var updatedMessageQueues = context.MessageQueueItemLocales.AsNoTracking().Where(mq=>mq.ScanCode == "9888888888888").ToList();
            foreach (var messageQueue in updatedMessageQueues)
            {
                Assert.AreEqual(command.ProcessedDate, messageQueue.ProcessedDate);
            }
        }
    }
}
