using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.R10Listener.Commands;
using Moq;
using Icon.Common.DataAccess;
using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using Icon.Esb.R10Listener.Constants;

namespace Icon.Esb.R10Listener.Tests.Commands
{
    [TestClass]
    public class ProcessFailedR10MessageResponseCommandHandlerTests : CommandHandlerTestBase<ProcessFailedR10MessageResponseCommandHandler, ProcessFailedR10MessageResponseCommand>
    {
        private Mock<ICommandHandler<ResendMessageQueueEntriesCommand>> mockResendMessageQueueEntriesCommandHandler;
        private Mock<ICommandHandler<ResendMessageCommand>> mockResendMessageCommandHandler;
        private MessageHistory messageHistory;

        [TestInitialize]
        protected override void Initialize()
        {
            mockResendMessageQueueEntriesCommandHandler = new Mock<ICommandHandler<ResendMessageQueueEntriesCommand>>();
            mockResendMessageCommandHandler = new Mock<ICommandHandler<ResendMessageCommand>>();

            commandHandler = new ProcessFailedR10MessageResponseCommandHandler(context,
                mockResendMessageQueueEntriesCommandHandler.Object,
                mockResendMessageCommandHandler.Object);
            command = new ProcessFailedR10MessageResponseCommand { MessageResponse = new R10MessageResponseModel() };

            messageHistory = new MessageHistory
            {
                Message = "<test>test</test>",
                MessageStatusId = MessageStatusTypes.Sent,
                MessageTypeId = MessageTypes.Product,
                InsertDate = DateTime.Now
            };
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException), "No MessageHistory found with MessageHistoryId equal to -1")]
        public void ProcessFailedR10MessageResponse_MessageHistoryDoesNotExist_ShouldThrowException()
        {
            //Given
            command.MessageResponse.MessageHistoryId = -1;

            //When
            commandHandler.Execute(command);
        }

        [TestMethod]
        public void ProcessFailedR10MessageResponse_MessageHistoryExists_ShouldMarkMessageHistoryAsFailed()
        {
            //Given
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            command.MessageResponse.MessageHistoryId = messageHistory.MessageHistoryId;
            command.MessageResponse.SystemError = true;

            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            mockResendMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageCommand>()), Times.Never);
            mockResendMessageQueueEntriesCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageQueueEntriesCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessFailedR10MessageResponse_MessageHistoryIsSystemTimeOut_ShouldResendMessage()
        {
            //Given
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            command.MessageResponse.MessageHistoryId = messageHistory.MessageHistoryId;
            command.MessageResponse.SystemError = true;
            command.MessageResponse.FailureReasonCode = FailureReasonCodes.SystemTimeOut;

            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            mockResendMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageCommand>()), Times.Once);
            mockResendMessageQueueEntriesCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageQueueEntriesCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessFailedR10MessageResponse_ProductMessageIsNotSystemErrorAndHasThresholdExceedsErrors_ShouldResendMessageQueueEntries()
        {
            //Given
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();

            command.MessageResponse.MessageHistoryId = messageHistory.MessageHistoryId;
            command.MessageResponse.SystemError = false;
            command.MessageResponse.FailureReasonCode = Constants.BusinessErrorCodes.ThresholdExceededError;

            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            mockResendMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageCommand>()), Times.Never);
            mockResendMessageQueueEntriesCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageQueueEntriesCommand>()), Times.Once);
        }

        [TestMethod]
        public void ProcessFailedR10MessageResponse_ProductMessageIsNotSystemErrorAndDoesNotHaveThresholdExceedsErrors_ShouldNotResendMessageQueueEntries()
        {
            //Given
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();

            command.MessageResponse.MessageHistoryId = messageHistory.MessageHistoryId;
            command.MessageResponse.SystemError = false;
            command.MessageResponse.FailureReasonCode = "Test Failure Reason Code";

            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            mockResendMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageCommand>()), Times.Never);
            mockResendMessageQueueEntriesCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageQueueEntriesCommand>()), Times.Never);
        }

        [TestMethod]
        public void ProcessFailedR10MessageResponse_ProductMessageIsNotSystemErrorAndFailureReasonCodeIsNull_ShouldNotResendMessageQueueEntries()
        {
            //Given
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();

            command.MessageResponse.MessageHistoryId = messageHistory.MessageHistoryId;
            command.MessageResponse.SystemError = false;
            command.MessageResponse.FailureReasonCode = null;

            //When
            commandHandler.Execute(command);

            //Then
            Assert.AreEqual(MessageStatusTypes.Failed, messageHistory.MessageStatusId);
            mockResendMessageCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageCommand>()), Times.Never);
            mockResendMessageQueueEntriesCommandHandler.Verify(m => m.Execute(It.IsAny<ResendMessageQueueEntriesCommand>()), Times.Never);
        }
    }
}
