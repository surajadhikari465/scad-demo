using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Icon.Esb.R10Listener.Commands;
using Icon.Common.DataAccess;
using Moq;
using Icon.Framework;
using Icon.Esb.R10Listener.Context;
using Icon.Esb.R10Listener.Models;

namespace Icon.Esb.R10Listener.Tests.Commands
{
    [TestClass]
    public class ProcessR10MessageResponseCommandHandlerTests : CommandHandlerTestBase<ProcessR10MessageResponseCommandHandler, ProcessR10MessageResponseCommand>
    {
        private Mock<ICommandHandler<AddMessageResponseCommand>> mockAddMessageResponseCommandHandler;
        private Mock<ICommandHandler<ProcessFailedR10MessageResponseCommand>> mockProcessFailedR10MessageResponseCommandHandler;
        private Mock<IRenewableContext<IconContext>> mockContext;

        [TestInitialize]
        protected override void Initialize()
        {
            mockAddMessageResponseCommandHandler = new Mock<ICommandHandler<AddMessageResponseCommand>>();
            mockProcessFailedR10MessageResponseCommandHandler = new Mock<ICommandHandler<ProcessFailedR10MessageResponseCommand>>();
            mockContext = new Mock<IRenewableContext<IconContext>>();

            commandHandler = new ProcessR10MessageResponseCommandHandler(mockContext.Object,
                mockAddMessageResponseCommandHandler.Object,
                mockProcessFailedR10MessageResponseCommandHandler.Object);
            command = new ProcessR10MessageResponseCommand { MessageResponse = new R10MessageResponseModel() };
        }

        [TestMethod]
        public void ProcessR10MessageResponse_RequestSuccessful_ShouldAddMessageResponse()
        {
            //Given
            command.MessageResponse.RequestSuccess = true;

            //When
            commandHandler.Execute(command);

            //Then
            mockAddMessageResponseCommandHandler.Verify(m => m.Execute(It.IsAny<AddMessageResponseCommand>()), Times.Once);
            mockProcessFailedR10MessageResponseCommandHandler.Verify(m => m.Execute(It.IsAny<ProcessFailedR10MessageResponseCommand>()), Times.Never);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            mockContext.Verify(m => m.Refresh(), Times.Once);
        }

        [TestMethod]
        public void ProcessR10MessageResponse_RequestNotSuccessful_ShouldProcessFailedR10MessageResponse()
        {
            //Given
            command.MessageResponse.RequestSuccess = false;

            //When
            commandHandler.Execute(command);

            //Then
            mockAddMessageResponseCommandHandler.Verify(m => m.Execute(It.IsAny<AddMessageResponseCommand>()), Times.Once);
            mockProcessFailedR10MessageResponseCommandHandler.Verify(m => m.Execute(It.IsAny<ProcessFailedR10MessageResponseCommand>()), Times.Once);
            mockContext.Verify(m => m.SaveChanges(), Times.Once);
            mockContext.Verify(m => m.Refresh(), Times.Once);
        }
    }
}
