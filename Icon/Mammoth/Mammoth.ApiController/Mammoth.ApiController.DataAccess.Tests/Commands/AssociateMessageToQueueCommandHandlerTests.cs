using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Framework;
using Icon.ApiController.DataAccess.Commands;
using MammothCommands = Mammoth.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using System.Linq;
using System.Collections.Generic;
using Mammoth.Common.Testing;
using Mammoth.Common.Testing.Builders;
using System.Data.SqlClient;
using Icon.Common;
using System.Data;
using Mammoth.Common.DataAccess;

namespace Mammoth.ApiController.DataAccess.Tests.Commands
{
    [TestClass]
    public class AssociateMessageToQueueCommandHandlerTests : CommandHandlerTestBase<MammothCommands.AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>, AssociateMessageToQueueCommand<MessageQueueItemLocale, MessageHistory>, MammothContext>
    {
        protected override void Initialize()
        {
            commandHandler = new MammothCommands.AssociateMessageToQueueCommandHandler<MessageQueueItemLocale>(new MammothContextFactory());
        }

        [TestMethod]
        public void AssociateMessageToQueue_MessageQueuesAndMessageHistoryExist_ShouldAssociateMessageToQueues()
        {
            //Given
            var messageHistory = new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.ItemLocale,
                Message = "<test>test</test>"
            };
            context.MessageHistories.Add(messageHistory);
            List<MessageQueueItemLocale> messageQueues = new List<MessageQueueItemLocale>
            {
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder(),
                new TestMessageQueueItemLocaleBuilder()
            };
            context.MessageQueueItemLocales.AddRange(messageQueues);
            context.SaveChanges();

            command.QueuedMessages = messageQueues;
            command.MessageHistory = messageHistory;

            //When
            commandHandler.Execute(command);

            //Then
            var actualMessageQueues = context.MessageQueueItemLocales.AsNoTracking().Where(mq=>mq.MessageHistoryId == messageHistory.MessageHistoryId).ToList();
            Assert.IsNotNull(actualMessageQueues);
            Assert.IsTrue(actualMessageQueues.Count == 3);
            foreach (var messageQueue in actualMessageQueues)
            {
                Assert.AreEqual(messageHistory.MessageHistoryId, messageQueue.MessageHistoryId);
            }
        }
    }
}
