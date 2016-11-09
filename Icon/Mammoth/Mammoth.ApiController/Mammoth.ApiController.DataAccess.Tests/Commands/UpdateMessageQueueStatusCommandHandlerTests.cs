using Icon.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using Mammoth.Common.DataAccess;
using Mammoth.Common.Testing.Builders;
using Mammoth.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MammothCommands = Mammoth.ApiController.DataAccess.Commands;

namespace Mammoth.ApiController.DataAccess.Tests.Commands
{
    [TestClass]
    public class UpdateMessageQueueStatusCommandHandlerTests : CommandHandlerTestBase<MammothCommands.UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>, UpdateMessageQueueStatusCommand<MessageQueueItemLocale>, MammothContext>
    {
        protected override void Initialize()
        {
            commandHandler = new MammothCommands.UpdateMessageQueueStatusCommandHandler<MessageQueueItemLocale>(new GlobalContext<MammothContext>(context));
        }

        [TestMethod]
        public void UpdateMessageQueueStatus_MessageQueuesExist_ShouldSetMessageStatus()
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

            command.MessageStatusId = MessageStatusTypes.Staged;
            command.QueuedMessages = messageQueues;

            //When
            commandHandler.Execute(command);

            //Then
            foreach (var messageQueue in messageQueues)
            {
                Assert.AreEqual(MessageStatusTypes.Staged, messageQueue.MessageStatusId);
            }
        }
    }
}
