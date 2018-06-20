using Dapper;
using Icon.Esb.R10Listener.Commands;
using Icon.Esb.R10Listener.Infrastructure.DataAccess;
using Icon.Esb.R10Listener.Models;
using Icon.Framework;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;

namespace Icon.Esb.R10Listener.Tests.Commands
{
    [TestClass]
    public class SaveR10MessageResponseCommandHandlerTests : CommandHandlerTestBase<SaveR10MessageResponseCommandHandler, SaveR10MessageResponseCommand>
    {
        protected override void Initialize()
        {
            commandHandler = new SaveR10MessageResponseCommandHandler(new DbFactory());
            command = new SaveR10MessageResponseCommand();
        }

        [TestMethod]
        public void SaveR10MessageResponse_ValidMessageResponse_ShouldSaveMessageResponse()
        {
            //Given
            command.R10MessageResponseModel = new R10MessageResponseModel
            {
                MessageId = "Test",
                FailureReasonCode = "Test Failure Reason Code",
                RequestSuccess = true,
                SystemError = true,
                ResponseText = "<test>Test Response Text</test>"
            };

            //When
            commandHandler.Execute(command);

            //Then
            var response = sqlConnection.QueryFirst<R10MessageResponseModel>("SELECT * FROM app.MessageResponseR10 WHERE MessageId = @MessageId", new { command.R10MessageResponseModel.MessageId });

            Assert.AreEqual(command.R10MessageResponseModel.FailureReasonCode, response.FailureReasonCode);
            Assert.AreEqual(command.R10MessageResponseModel.RequestSuccess, response.RequestSuccess);
            Assert.AreEqual(command.R10MessageResponseModel.SystemError, response.SystemError);
            Assert.AreEqual(command.R10MessageResponseModel.ResponseText, response.ResponseText);
        }
    }
}
