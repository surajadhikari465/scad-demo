using AttributePublisher.DataAccess.Models;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.MessageBuilders;
using AttributePublisher.Operations;
using AttributePublisher.Services;
using Esb.Core.MessageBuilders;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace AttributePublisher.Tests.Unit.Operations
{
    [TestClass]
    public class BuildMessageOperationTests
    {
        private BuildMessageOperation operation;
        private AttributePublisherServiceSettings settings;
        private Mock<IMessageHeaderBuilder> mockMessageHeaderBuilder;
        private Mock<IMessageBuilder<List<AttributeModel>>> mockMessageBuilder;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockNext;

        [TestInitialize]
        public void Initialize()
        {
            settings = new AttributePublisherServiceSettings { RecordsPerMessage = 1 };
            mockMessageHeaderBuilder = new Mock<IMessageHeaderBuilder>();
            mockNext = new Mock<IOperation<AttributePublisherServiceParameters>>();
            mockMessageBuilder = new Mock<IMessageBuilder<List<AttributeModel>>>();
            operation = new BuildMessageOperation(mockNext.Object, mockMessageBuilder.Object, mockMessageHeaderBuilder.Object, settings);
        }

        [TestMethod]
        public void BuildMessageOperation_Execute_BuildsMessageGivenModels()
        {
            //Given 
            var parameters = new AttributePublisherServiceParameters
            {
                Attributes = new List<AttributeModel>
                {
                    new AttributeModel()
                }
            };
            mockMessageBuilder.Setup(m => m.BuildMessage(It.IsAny<List<AttributeModel>>()))
                .Returns("Test");
            mockMessageHeaderBuilder.Setup(m => m.BuildHeader())
                .Returns(new Dictionary<string, string> { { "Test", "Test" } });

            //When
            operation.Execute(parameters);

            //Then
            Assert.AreEqual(1, parameters.AttributeMessages.Count);
            Assert.AreEqual("Test", parameters.AttributeMessages[0].Message);
            Assert.AreEqual("Test", parameters.AttributeMessages[0].MessageHeaders["Test"]);
            Assert.IsNotNull(parameters.AttributeMessages[0].MessageId);
            Assert.AreEqual(1, parameters.AttributeMessages[0].Attributes.Count);
        }
    }
}
