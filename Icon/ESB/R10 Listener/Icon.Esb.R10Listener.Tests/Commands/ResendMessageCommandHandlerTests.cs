using Icon.Esb.R10Listener.Commands;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Icon.Esb.R10Listener.Tests.Commands
{
    [TestClass]
    public class ResendMessageCommandHandlerTests : CommandHandlerTestBase<ResendMessageCommandHandler, ResendMessageCommand>
    {
        private R10ListenerApplicationSettings applicationSettings;
        private MessageHistory messageHistory;
        private string testMessage = "<test>Test</test>";

        protected override void Initialize()
        {
            applicationSettings = new R10ListenerApplicationSettings { ResendMessageCount = 1 };
            commandHandler = new ResendMessageCommandHandler(context, applicationSettings);
            command = new ResendMessageCommand();
            messageHistory = new MessageHistory
            {
                Message = testMessage,
                MessageStatusId = MessageStatusTypes.Failed,
                MessageTypeId = MessageTypes.Product,
                InsertDate = DateTime.Now
            };
        }

        [TestMethod]
        public void ResendMessage_ResendMessageCountIs0_ShouldNotCreateNewResendStatus()
        {
            //Given
            applicationSettings.ResendMessageCount = 0;
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            command.MessageHistoryId = messageHistory.MessageHistoryId;

            //When
            commandHandler.Execute(new ResendMessageCommand
                {
                    MessageHistoryId = command.MessageHistoryId
                });
            context.SaveChanges();

            //Then
            Assert.IsFalse(context.Context.MessageResendStatus.Any(mrs => mrs.MessageHistoryId == command.MessageHistoryId));
        }

        [TestMethod]
        public void ResendMessage_ResendMessageCountIsLessThan0_ShouldNotCreateNewResendStatus()
        {
            //Given
            applicationSettings.ResendMessageCount = -1;
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            command.MessageHistoryId = messageHistory.MessageHistoryId;

            //When
            commandHandler.Execute(new ResendMessageCommand
            {
                MessageHistoryId = command.MessageHistoryId
            });
            context.SaveChanges();

            //Then
            Assert.IsFalse(context.Context.MessageResendStatus.Any(mrs => mrs.MessageHistoryId == command.MessageHistoryId));
        }

        [TestMethod]
        public void ResendMessage_NumberOfResendsIsGreaterThanResendMessageCount_ShouldNotIncrementResendStatusOrChangeMessageStatus()
        {
            //Given
            applicationSettings.ResendMessageCount = 4;
            messageHistory.MessageStatusId = MessageStatusTypes.Sent;
            context.Context.MessageHistory.Add(messageHistory);
            context.Context.MessageResendStatus.Add(new MessageResendStatus
            {
                MessageHistory = messageHistory,
                InsertDate = DateTime.Now,
                NumberOfResends = 4
            });
            context.SaveChanges();
            command.MessageHistoryId = messageHistory.MessageHistoryId;

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            var messageResendStatus = context.Context.MessageResendStatus.FirstOrDefault(mrs => mrs.MessageHistoryId == command.MessageHistoryId);
            Assert.IsNotNull(messageResendStatus);
            Assert.AreEqual(4, messageResendStatus.NumberOfResends);
            Assert.AreEqual(MessageStatusTypes.Sent, messageHistory.MessageStatusId);
        }

        [TestMethod]
        public void ResendMessage_MessageHistoryDoesNotExist_ShouldNotCreateNewResendStatus()
        {
            //Given
            command.MessageHistoryId = -1;

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.IsFalse(context.Context.MessageResendStatus.Any(mrs => mrs.MessageHistoryId == command.MessageHistoryId));
        }

        [TestMethod]
        public void ResendMessage_MessageHistoryStatusIsReady_ShouldNotCreateNewResendStatus()
        {
            //Given
            messageHistory.MessageStatusId = MessageStatusTypes.Ready;
            context.Context.MessageHistory.Add(messageHistory);
            command.MessageHistoryId = messageHistory.MessageHistoryId;

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            Assert.IsFalse(context.Context.MessageResendStatus.Any(mrs => mrs.MessageHistoryId == command.MessageHistoryId));
        }

        [TestMethod]
        public void ResendMessage_MessageResendStatusDoesNotExist_ShouldAddResendStatusWithResendSetTo1()
        {
            //Given
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            command.MessageHistoryId = messageHistory.MessageHistoryId;

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            var messageResendStatus = context.Context.MessageResendStatus.FirstOrDefault(mrs => mrs.MessageHistoryId == command.MessageHistoryId);
            Assert.IsNotNull(messageResendStatus);
            Assert.AreEqual(1, messageResendStatus.NumberOfResends);
            Assert.AreEqual(MessageStatusTypes.Ready, messageHistory.MessageStatusId);
        }

        [TestMethod]
        public void ResendMessage_MessageResendStatusNotExist_ShouldAddResendStatusAndIncrementNumberOfResends()
        {
            //Given
            applicationSettings.ResendMessageCount = 5;
            context.Context.MessageHistory.Add(messageHistory);
            context.Context.MessageResendStatus.Add(new MessageResendStatus
                {
                    MessageHistory = messageHistory,
                    InsertDate = DateTime.Now,                    
                    NumberOfResends = 4
                });
            context.SaveChanges();
            command.MessageHistoryId = messageHistory.MessageHistoryId;

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            var messageResendStatus = context.Context.MessageResendStatus.FirstOrDefault(mrs => mrs.MessageHistoryId == command.MessageHistoryId);
            Assert.IsNotNull(messageResendStatus);
            Assert.AreEqual(5, messageResendStatus.NumberOfResends);
            Assert.AreEqual(MessageStatusTypes.Ready, messageHistory.MessageStatusId);
        }
    }
}
