using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Mammoth.Framework;
using Icon.ApiController.DataAccess.Commands;
using MammothCommands = Mammoth.ApiController.DataAccess.Commands;
using Icon.Common.Context;
using System.Linq;
using Mammoth.Common.DataAccess;
using System.Data.Entity.Infrastructure;
using System.Xml;

namespace Mammoth.ApiController.DataAccess.Tests.Commands
{
    [TestClass]
    public class SaveToMessageHistoryCommandHandlerTests : CommandHandlerTestBase<MammothCommands.SaveToMessageHistoryCommandHandler, SaveToMessageHistoryCommand<MessageHistory>, MammothContext>
    {
        protected override void Initialize()
        {
            commandHandler = new MammothCommands.SaveToMessageHistoryCommandHandler(new GlobalContext<MammothContext>(context));
        }

        [TestMethod]
        public void SaveToMessageHistory_MessageDoesntExist_ShouldSaveMessage()
        {
            //Given
            command.Message = new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.ItemLocale,
                Message = "<test>test</test>"
            };

            //When
            commandHandler.Execute(command);

            //Then
            var messageHistory = context.MessageHistories
                .AsNoTracking()
                .First(mh => mh.MessageHistoryId == command.Message.MessageHistoryId);
            Assert.AreEqual(command.Message.MessageTypeId, messageHistory.MessageTypeId);
            Assert.AreEqual(command.Message.MessageStatusId, messageHistory.MessageStatusId);
            Assert.AreEqual(command.Message.Message, messageHistory.Message);
        }

        [TestMethod]
        [ExpectedException(typeof(DbUpdateException), AllowDerivedTypes = true)]
        public void SaveToMessageHistory_MessageContainsIllegalXmlCharacter_ShouldThrowException()
        {
            //Given
            command.Message = new MessageHistory
            {
                MessageStatusId = MessageStatusTypes.Ready,
                MessageTypeId = MessageTypes.ItemLocale,
                Message = "<test>This text contains a hidden character that is illegal for xml.</test>"
            };

            //When
            commandHandler.Execute(command);
        }
    }
}
