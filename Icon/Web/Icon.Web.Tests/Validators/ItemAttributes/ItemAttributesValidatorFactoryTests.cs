using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;
using Icon.Common;
using Icon.Common.Models;
using Icon.Common.Validators.ItemAttributes;

namespace Icon.Web.Tests.Unit.Validators.ItemAttributes
{
    [TestClass]
    public class ItemAttributesValidatorFactoryTests
    {
        private const string AttributeName = "Test";

        private ItemAttributesValidatorFactory factory;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> getAttributesQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            getAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();

            factory = new ItemAttributesValidatorFactory(getAttributesQueryHandler.Object);
        }

        [TestMethod]
        public void CreateItemAttributesJsonValidator_AttributeIsPickList_ReturnsPickListValidator()
        {
            //Given
            getAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = AttributeName, IsPickList = true } });

            //When
            var validator = factory.CreateItemAttributesJsonValidator(AttributeName);

            //Then
            Assert.IsInstanceOfType(validator, typeof(ItemAttributesPickListValidator));
        }

        [TestMethod]
        public void CreateItemAttributesJsonValidator_AttributeIsBoolean_ReturnsBooleanValidator()
        {
            //Given
            getAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = AttributeName, DataTypeName = Constants.DataTypeNames.Boolean } });

            //When
            var validator = factory.CreateItemAttributesJsonValidator(AttributeName);

            //Then
            Assert.IsInstanceOfType(validator, typeof(ItemAttributesBooleanValidator));
        }

        [TestMethod]
        public void CreateItemAttributesJsonValidator_AttributeIsText_ReturnsTextValidator()
        {
            //Given
            getAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = AttributeName, DataTypeName = Constants.DataTypeNames.Text } });

            //When
            var validator = factory.CreateItemAttributesJsonValidator(AttributeName);

            //Then
            Assert.IsInstanceOfType(validator, typeof(ItemAttributesTextValidator));
        }

        [TestMethod]
        public void CreateItemAttributesJsonValidator_AttributeIsNumeric_ReturnsNumericValidator()
        {
            //Given
            getAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { MinimumNumber = "1",MaximumNumber = "19", NumberOfDecimals = "0",  AttributeName = AttributeName, DataTypeName = Constants.DataTypeNames.Number } });

            //When
            var validator = factory.CreateItemAttributesJsonValidator(AttributeName);

            //Then
            Assert.IsInstanceOfType(validator, typeof(ItemAttributesNumericItemValidator));
        }

        [TestMethod]
        public void CreateItemAttributesJsonValidator_AttributeIsDate_ReturnsDateValidator()
        {
            //Given
            getAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = AttributeName, DataTypeName = Constants.DataTypeNames.Date } });

            //When
            var validator = factory.CreateItemAttributesJsonValidator(AttributeName);

            //Then
            Assert.IsInstanceOfType(validator, typeof(ItemAttributesDateValidator));
        }

        [TestMethod]
        [ExpectedException(typeof(InvalidOperationException))]
        public void CreateItemAttributesJsonValidator_ValidatorDoesNotExistForDataType_ThrowsException()
        {
            //Given
            getAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = AttributeName, DataTypeName = "NotSupported" } });

            //When
            var validator = factory.CreateItemAttributesJsonValidator(AttributeName);

            //Then
            Assert.IsInstanceOfType(validator, typeof(ItemAttributesDateValidator));
        }

        [TestMethod]
        [ExpectedException(typeof(ArgumentException))]
        public void CreateItemAttributesJsonValidator_AttributeDoesNotExist_ThrowsException()
        {
            //Given
            getAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel> { new AttributeModel { AttributeName = "Not Existing" } });

            //When
            var validator = factory.CreateItemAttributesJsonValidator(AttributeName);
        }
    }
}
