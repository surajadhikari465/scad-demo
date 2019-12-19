using AttributePublisher.DataAccess.Models;
using AttributePublisher.DataAccess.Queries;
using AttributePublisher.Infrastructure.Operations;
using AttributePublisher.Operations;
using AttributePublisher.Services;
using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace AttributePublisher.Tests.Unit.Operations
{
    [TestClass]
    public class GetAttributesOperationTests
    {
        private GetAttributesOperation operation;
        private Mock<IOperation<AttributePublisherServiceParameters>> mockNext;
        private Mock<IQueryHandler<GetAttributesParameters, List<AttributeModel>>> mockGetAttributesQueryHandler;
        private AttributePublisherServiceSettings settings;

        [TestInitialize]
        public void Initialize()
        {
            mockNext = new Mock<IOperation<AttributePublisherServiceParameters>>();
            settings = new AttributePublisherServiceSettings();
            mockGetAttributesQueryHandler = new Mock<IQueryHandler<GetAttributesParameters, List<AttributeModel>>>();
            operation = new GetAttributesOperation(mockNext.Object, mockGetAttributesQueryHandler.Object, settings);
        }

        [TestMethod]
        public void GetAttributesOperation_AttributesAreReturned_SetsContinueProcessingToTrue()
        {
            //Given
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<GetAttributesParameters>()))
                .Returns(new List<AttributeModel> { new AttributeModel() });
            var parameters = new AttributePublisherServiceParameters();

            //When
            operation.Execute(parameters);

            //Then
            mockGetAttributesQueryHandler.Verify(m => m.Search(It.IsAny<GetAttributesParameters>()), Times.Once);
            Assert.AreEqual(1, parameters.Attributes.Count);
            Assert.IsTrue(parameters.ContinueProcessing);
        }

        [TestMethod]
        public void GetAttributesOperation_AttributesAreNotReturned_SetsContinueProcessingToFalse()
        {
            //Given
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<GetAttributesParameters>()))
                .Returns(new List<AttributeModel>());
            var parameters = new AttributePublisherServiceParameters();

            //When
            operation.Execute(parameters);

            //Then
            mockGetAttributesQueryHandler.Verify(m => m.Search(It.IsAny<GetAttributesParameters>()), Times.Once);
            Assert.AreEqual(0, parameters.Attributes.Count);
            Assert.IsFalse(parameters.ContinueProcessing);
        }
    }
}
