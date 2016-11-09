using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Icon.Esb.R10Listener.Tests.Commands
{
    [TestClass]
    public class AddMessageResponseCommandHandlerTests : CommandHandlerTestBase<AddMessageResponseCommandHandler, AddMessageResponseCommand>
    {
        private MessageHistory messageHistory;

        protected override void Initialize()
        {
            commandHandler = new AddMessageResponseCommandHandler(context);
            command = new AddMessageResponseCommand();
            messageHistory = new MessageHistory
                {
                    Message = "<test>test</test>",
                    InsertDate = DateTime.Now,
                    MessageStatusId = MessageStatusTypes.Sent,
                    MessageTypeId = MessageTypes.Product
                };
        }

        [TestMethod]
        public void AddMessageResponse_ValidMessageResponse_ShouldSaveMessageResponse()
        {
            //Given
            context.Context.MessageHistory.Add(messageHistory);
            context.SaveChanges();
            command.MessageResponse = new R10MessageResponseModel
            {
                MessageHistoryId = messageHistory.MessageHistoryId,
                FailureReasonCode = "Test Failure Reason Code",
                RequestSuccess = true,
                SystemError = true,
                ResponseText = "<test>Test Response Text</test>"
            };

            //When
            commandHandler.Execute(command);
            context.SaveChanges();

            //Then
            var response = context.Context.R10MessageResponse.SingleOrDefault(r => r.MessageHistoryId == messageHistory.MessageHistoryId);

            Assert.AreEqual(command.MessageResponse.FailureReasonCode, response.FailureReasonCode);
            Assert.AreEqual(command.MessageResponse.RequestSuccess, response.RequestSuccess);
            Assert.AreEqual(command.MessageResponse.SystemError, response.SystemError);
            Assert.AreEqual(command.MessageResponse.ResponseText, response.ResponseText);
            Assert.IsNotNull(response.InsertDate);
        }
    }
}
