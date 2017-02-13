using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using MammothCommands = Mammoth.ApiController.DataAccess.Commands;
using Icon.ApiController.DataAccess.Commands;
using Mammoth.Framework;
using Icon.Common.Context;
using Mammoth.Common.DataAccess;

namespace Mammoth.ApiController.DataAccess.Tests.Commands
{
    [TestClass]
    public class UpdateMessageHistoryStatusCommandHandlerTests : CommandHandlerTestBase<MammothCommands.UpdateMessageHistoryStatusCommandHandler, UpdateMessageHistoryStatusCommand<MessageHistory>, MammothContext>
    {
        protected override void Initialize()
        {
            commandHandler = new MammothCommands.UpdateMessageHistoryStatusCommandHandler(new MammothContextFactory());
        }

        [TestMethod]
        public void UpdateMessageHistoryStatus_MessageHistoryExists_ShouldUpdateStatus()
        {
            //Given
            var messageHistory = new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.ItemLocale,
                Message = "<test>test</test>"
            };
            context.MessageHistories.Add(messageHistory);
            context.SaveChanges();

            command.Message = messageHistory;
            command.MessageStatusId = MessageStatusTypes.Sent;

            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(MessageStatusTypes.Sent, messageHistory.MessageStatusId);
        }
    }
}
