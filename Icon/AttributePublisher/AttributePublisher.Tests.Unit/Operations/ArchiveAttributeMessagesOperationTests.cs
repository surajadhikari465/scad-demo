using AttributePublisher.DataAccess.Commands;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Operations;
using AttributePublisher.Services;
using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace AttributePublisher.Tests.Unit.Operations
{
    [TestClass]
    public class ArchiveAttributeMessagesOperationTests
    {
        private ArchiveAttributeMessagesOperation operation;
        private Mock<ICommandHandler<ArchiveMessagesCommand>> mockArchiveMessagesCommandHandler;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockNext;

        [TestInitialize]
        public void Initialize()
        {
            mockNext = new Mock<IOperation<AttributePublisherServiceParameters>>();
            mockArchiveMessagesCommandHandler = new Mock<ICommandHandler<ArchiveMessagesCommand>>();
            operation = new ArchiveAttributeMessagesOperation(mockNext.Object, mockArchiveMessagesCommandHandler.Object);
        }

        [TestMethod]
        public void ArchiveAttributeMessagesOperation_Execute_CallsCommandHandler()
        {
            //When
            operation.Execute(new AttributePublisher.Services.AttributePublisherServiceParameters());

            //Then
            mockArchiveMessagesCommandHandler.Verify(m => m.Execute(It.IsAny<ArchiveMessagesCommand>()), Times.Once);
        }
    }
}
