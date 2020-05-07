using Icon.Common;
using Icon.Common.DataAccess;
using Icon.Common.Models;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Infrastructure.ItemSearch;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Models;
using Icon.Web.Mvc.Utility;
using Icon.Web.Mvc.Validators;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class AttributeViewModelValidatorTests
    {
        private AttributeViewModelValidator validator;
        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>> mockGetAttributesQueryHandler;
        private AttributeViewModel viewModel;
        private Mock<IQueryHandler<GetItemsParameters, GetItemsResult>> mockGetItemsQueryHandler;
        private Mock<IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel>> mockGetAttributeByAttributeIdQuery;
        private Mock<IAttributesHelper> mockAttributesHelper;
        private Mock<IQueryHandler<DoesAttributeExistOnItemsParameters, bool>> mockDoesAttributeExistOnItemsQueryHandler;
        private AttributeModel attributeModel;

        [TestInitialize]
        public void Initialize()
        {
            mockGetAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<AttributeModel>>, IEnumerable<AttributeModel>>>();
            mockGetItemsQueryHandler = new Mock<IQueryHandler<GetItemsParameters, GetItemsResult>>();
            mockGetAttributeByAttributeIdQuery = new Mock<IQueryHandler<GetAttributeByAttributeIdParameters, AttributeModel>>();
            mockDoesAttributeExistOnItemsQueryHandler = new Mock<IQueryHandler<DoesAttributeExistOnItemsParameters, bool>>();
            mockAttributesHelper = new Mock<IAttributesHelper>();

            validator = new AttributeViewModelValidator(
                mockGetAttributesQueryHandler.Object,
                mockGetItemsQueryHandler.Object,
                mockGetAttributeByAttributeIdQuery.Object,
                mockDoesAttributeExistOnItemsQueryHandler.Object,
                mockAttributesHelper.Object);
            viewModel = new AttributeViewModel();
            attributeModel = new AttributeModel();
        }

        [TestMethod]
        public void Validate_ViewModelIsValidBoolean_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Boolean;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }        

        [TestMethod]
        public void Validate_ViewModelIsValidDate_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Date;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ViewModelIsValidText_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ViewModelIsValidNumber_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "10";
            viewModel.MaximumNumber = "20";
            viewModel.NumberOfDecimals = "4";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_ViewModelIsValidPickList_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "test"},
                new PickListModel { PickListValue = ""},
                new PickListModel { PickListValue = "   "},
                new PickListModel { PickListValue = null},
            };
            viewModel.IsPickList = true;
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(".*");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DisplayNameIsEmpty_InvalidResult()
        {
            //Given
            viewModel.DisplayName = string.Empty;
            viewModel.TraitCode = "Tst";

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "'Display Name' must not be empty."));
        }

        [TestMethod]
        public void Validate_DisplayNameIsNull_InvalidResult()
        {
            //Given
            viewModel.DisplayName = null;
            viewModel.TraitCode = "Tst";

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "'Display Name' must not be empty."));
        }

        [TestMethod]
        public void Validate_TraitCodeIsEmpty_InvalidResult()
        {
            //Given
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = string.Empty;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "'Trait Code' must not be empty."));
        }

        [TestMethod]
        public void Validate_TraitCodeIsNull_InvalidResult()
        {
            //Given
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = null;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "'Trait Code' must not be empty."));
        }

        [TestMethod]
        public void Validate_TraitCodeMoreThanThreecharcaters_InvalidResult()
        {
            //Given
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Test";
            var length = viewModel.TraitCode.Length;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "The length of 'Trait Code' must be 3 characters or fewer. You entered " + length + " characters."));
        }

        [TestMethod]
        public void Validate_DisplayNameAlreadyExists_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 2;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>
                {
                    new AttributeModel { AttributeId = 1, DisplayName = "Test" }
                });

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "An attribute already exists with the same display name. Display names must be unique."));
        }

        [TestMethod]
        public void Validate_TraitCodeAlreadyExists_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 2;
            viewModel.DisplayName = "TestNamr";
            viewModel.TraitCode = "Tst";
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>
                {
                    new AttributeModel { AttributeId = 1, TraitCode = "Tst" }
                });

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "An attribute already exists with the same trait code. Trait codes must be unique."));
        }

        [TestMethod]
        public void Validate_XmlTraitDescriptionAlreadyExists_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 2;
            viewModel.DisplayName = "Tst";
            viewModel.TraitCode = "Tst";

            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>
                {
                    new AttributeModel { AttributeId = 1, XmlTraitDescription = "Tst" }
                });

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Entered display name cannot be used. It is utilized already by downstream systems."));
        }

        [TestMethod]
        public void Validate_SameAttributeDisplayName_ValidResult()
        {
            //Given
            viewModel.AttributeId = 1;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>
                {
                    new AttributeModel { AttributeId = 1, DisplayName = "Test" }
                });
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel { IsSelected = true }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_SameAttributeTraitCode_ValidResult()
        {
            //Given
            viewModel.AttributeId = 1;
            viewModel.DisplayName = "TestName";
            viewModel.TraitCode = "Tst";
            mockGetAttributesQueryHandler.Setup(m => m.Search(It.IsAny<EmptyQueryParameters<IEnumerable<AttributeModel>>>()))
                .Returns(new List<AttributeModel>
                {
                    new AttributeModel { AttributeId = 1, TraitCode = "Tst" }
                });
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel { IsSelected = true }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_MinimumNumberIsNull_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = null;
            viewModel.MaximumNumber = "20";
            viewModel.NumberOfDecimals = "4";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Minimum Number' must not be empty.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MinimumNumberIsLessThanMinimumNumberMin_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = (Constants.MinimumNumberMin - 1).ToString();
            viewModel.MaximumNumber = "20";
            viewModel.NumberOfDecimals = "4";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Minimum Number must be between -9999999999.9999 and 9999999999.9999.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MinimumNumberIsGreaterThanMinimumNumberMax_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = (Constants.MinimumNumberMax + 1).ToString();
            viewModel.MaximumNumber = "20";
            viewModel.NumberOfDecimals = "4";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.IsNotNull(result.Errors.Single(e => e.ErrorMessage == "Maximum Number must be greater than Minimum Number."));
            Assert.IsNotNull(result.Errors.Single(e => e.ErrorMessage == "Minimum Number must be between -9999999999.9999 and 9999999999.9999."));
        }

        [TestMethod]
        public void Validate_MaximumNumberIsNull_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaximumNumber = null;
            viewModel.MinimumNumber = "20";
            viewModel.NumberOfDecimals = "4";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Maximum Number' must not be empty.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaximumNumberIsLessThanMaximumNumberMin_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaximumNumber = (Constants.MaximumNumberMin - 1).ToString();
            viewModel.MinimumNumber = (Constants.MaximumNumberMin - 2).ToString();
            viewModel.NumberOfDecimals = "4";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(2, result.Errors.Count);
            Assert.IsNotNull(result.Errors.SingleOrDefault(e => e.ErrorMessage == "Minimum Number must be between -9999999999.9999 and 9999999999.9999."));
            Assert.IsNotNull(result.Errors.SingleOrDefault(e => e.ErrorMessage == "Maximum Number must be between -9999999999.9999 and 9999999999.9999."));
        }

        [TestMethod]
        public void Validate_MaximumNumberIsGreaterThanMaximumNumberMax_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaximumNumber = (Constants.MaximumNumberMax + 1).ToString();
            viewModel.MinimumNumber = "20";
            viewModel.NumberOfDecimals = "4";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Maximum Number must be between -9999999999.9999 and 9999999999.9999.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NumberOfDecimalsIsNull_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "10";
            viewModel.MaximumNumber = "20";
            viewModel.NumberOfDecimals = null;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Number of Decimals' must not be empty.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NumberOfDecimalsIsLessThanNumberOfDecimalsMin_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "10";
            viewModel.MaximumNumber = "20";
            viewModel.NumberOfDecimals = (Constants.NumberOfDecimalsMin - 1).ToString();

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Number of Decimals must be between 0 and 4.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NumberOfDecimalsIsGreaterThanNumberOfDecimalsMax_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "10";
            viewModel.MaximumNumber = "20";
            viewModel.NumberOfDecimals = (Constants.NumberOfDecimalsMax + 1).ToString();

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.NumberOfDecimalsInclusiveBetween, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaxLengthAllowedIsNull_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = null;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Max Length of Attribute Value' must not be empty.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaxLengthAllowedIsLessThanMaxLengthAllowedMin_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = Constants.MaxLengthAllowedMin - 1;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Max Length of Attribute Value' must be between 1 and 999999999. You entered 0.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaxLengthAllowedIsGreaterThanMaxLengthAllowedMax_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = Constants.MaxLengthAllowedMax + 1;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("'Max Length of Attribute Value' must be between 1 and 999999999. You entered 1000000000.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NoCharacterSetsSelected_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = false }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("At least one option for allowable characters must be selected.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NullPickListData_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>();
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("At least one pick list value must be entered.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_EmptyPickListData_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>();
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("At least one pick list value must be entered.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NullPickListValue_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = null }
            };
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("At least one pick list value must be entered.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_EmptyPickListValue_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = string.Empty }
            };
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("At least one pick list value must be entered.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_PickListValueIsGreaterThanMaxLengthAllowed_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = new string('s', viewModel.MaxLengthAllowed.Value + 1) }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(".*");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.PickListValueMaxLengthAllowed, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_PickListValueIsGreaterThan50_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 100;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = new string('s', Constants.MaxPickListValueLength + 1) }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(".*");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_PickListValueMatchesRegex_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 50;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "a" }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(".*");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_PickListValueDoesNotMatcheRegex_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 50;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "A" }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns("^[^A]*$");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.PickListInvalidValue, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MinimumNumberIsEmpty_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaximumNumber = "10";
            viewModel.NumberOfDecimals = "1";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            viewModel.MinimumNumber = null;
            var result1 = validator.Validate(viewModel);
            viewModel.MinimumNumber = "";
            var result2 = validator.Validate(viewModel);
            viewModel.MinimumNumber = "  ";
            var result3 = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result1.IsValid);
            Assert.IsFalse(result2.IsValid);
            Assert.IsFalse(result3.IsValid);

            Assert.AreEqual("'Minimum Number' must not be empty.", result1.Errors.Single().ErrorMessage);
            Assert.AreEqual("'Minimum Number' must not be empty.", result2.Errors.Single().ErrorMessage);
            Assert.AreEqual("'Minimum Number' must not be empty.", result3.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaximumNumberIsEmpty_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "10";
            viewModel.NumberOfDecimals = "1";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = "20";
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            viewModel.MaximumNumber = null;
            var result1 = validator.Validate(viewModel);
            viewModel.MaximumNumber = "";
            var result2 = validator.Validate(viewModel);
            viewModel.MaximumNumber = "  ";
            var result3 = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result1.IsValid);
            Assert.IsFalse(result2.IsValid);
            Assert.IsFalse(result3.IsValid);

            Assert.AreEqual("'Maximum Number' must not be empty.", result1.Errors.Single().ErrorMessage);
            Assert.AreEqual("'Maximum Number' must not be empty.", result2.Errors.Single().ErrorMessage);
            Assert.AreEqual("'Maximum Number' must not be empty.", result3.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NumberOfDecimalsIsEmpty_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaximumNumber = "10";
            viewModel.MinimumNumber = "1";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            viewModel.NumberOfDecimals = null;
            var result1 = validator.Validate(viewModel);
            viewModel.NumberOfDecimals = "";
            var result2 = validator.Validate(viewModel);
            viewModel.NumberOfDecimals = "  ";
            var result3 = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result1.IsValid);
            Assert.IsFalse(result2.IsValid);
            Assert.IsFalse(result3.IsValid);

            Assert.AreEqual("'Number of Decimals' must not be empty.", result1.Errors.Single().ErrorMessage);
            Assert.AreEqual("'Number of Decimals' must not be empty.", result2.Errors.Single().ErrorMessage);
            Assert.AreEqual("'Number of Decimals' must not be empty.", result3.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MinimumNumberNotDecimal_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaximumNumber = "10";
            viewModel.NumberOfDecimals = "1";
            viewModel.MinimumNumber = "abc";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.MinimumNumberValidDecimal, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaximumNumberNotDecimal_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaximumNumber = "abc";
            viewModel.NumberOfDecimals = "1";
            viewModel.MinimumNumber = "1";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.MaximumNumberValidDecimal, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NumberOfDigitsNotInt_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "1";
            viewModel.MaximumNumber = "11";
            viewModel.NumberOfDecimals = "abc";

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.NumberOfDecimalsValidInteger, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_PickListValueWithItemsAssociation_ValidResult()
        {
            //Given
            GetItemsParameters parameters = new GetItemsParameters();
            viewModel.AttributeName = "Test";
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "Test" }
            };
            parameters.ItemAttributeJsonParameters = new List<ItemSearchCriteria>()
            {
                new ItemSearchCriteria("TestAttribute",AttributeSearchOperator.ContainsAll,"Test")
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_MinimumNumberLessthanCurrentValue_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 1;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.MinimumNumber = "5";
            viewModel.NumberOfDecimals = "2";
            viewModel.MaximumNumber = "10";
            viewModel.Action = ActionEnum.Update;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = "4";
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.CurrentMinimumNumberMustBeLessThanNewMinimumNumber, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaximumNumberGreaterthanCurrentValue_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 1;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.MinimumNumber = "5";
            viewModel.NumberOfDecimals = "2";
            viewModel.MaximumNumber = "10";
            viewModel.Action = ActionEnum.Update;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = "11";
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.CurrentMaximumNumberIsGreaterThanNewMaximumNumber, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_NumberOfDecimalsGreaterthanCurrentValue_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 1;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.MinimumNumber = "5";
            viewModel.NumberOfDecimals = "1";
            viewModel.MaximumNumber = "10";
            viewModel.Action = ActionEnum.Update;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = "2";
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.CurrentNumberOfDecimalsGreaterThanNewNumberOfDecimals, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_MaxLengthAllowedGreaterthanCurrentValue_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 1;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel { IsSelected = true }
            };
            viewModel.Action = ActionEnum.Update;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MaxLengthAllowed = 100;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.CurrentMaxLengthLessThanNewMaxLength, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_PickListValueDeleteData_ValidResult()
        {
            //Given
            viewModel.AttributeName = "Test";
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.IsPickList = true;
            List<PickListModel> pickListModelList = new List<PickListModel>
            {
                new PickListModel{ PickListValue="Test", IsPickListSelectedForDelete = true},

            };
            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_AttributeIsPickListWithOnlyWhiteSpaceSelectedAsCharacterSet_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = " " }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^[\s]*$");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_AttributeIsPickListWithOnlyWhiteSpaceSelectedAsCharacterSetAndPickListValueIsEmpty_InValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = null },
                new PickListModel { PickListValue = " " },
                new PickListModel { PickListValue = string.Empty }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^[\s]*$");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(ValidationMessages.PickListCollectionIsEmpty, result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_AttributeIsPickListWithMultipleValuesAndOneIsEmpty_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "test1" },
                new PickListModel { PickListValue = string.Empty },
                new PickListModel { PickListValue = "test2" },
                new PickListModel { PickListValue = "test3" }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^.*$");
            attributeModel.IsPickList = viewModel.IsPickList;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_MinimumNumberIs0OnAdd_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "0";
            viewModel.MaximumNumber = "1";
            viewModel.NumberOfDecimals = "1";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_MinimumNumberIs0OnUpdate_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "0";
            viewModel.MaximumNumber = "1";
            viewModel.NumberOfDecimals = "1";
            viewModel.Action = ActionEnum.Update;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_MaximumNumberIs0OnAdd_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "-1";
            viewModel.MaximumNumber = "0";
            viewModel.NumberOfDecimals = "1";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_MaximumNumberIs0OnUpdate_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "-1";
            viewModel.MaximumNumber = "0";
            viewModel.NumberOfDecimals = "1";
            viewModel.Action = ActionEnum.Update;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_NumberOfDecimaIs0OnAdd_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "0";
            viewModel.MaximumNumber = "0";
            viewModel.NumberOfDecimals = "0";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_NumberOfDecimalsIs0OnUpdate_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "0";
            viewModel.MaximumNumber = "0";
            viewModel.NumberOfDecimals = "0";
            viewModel.Action = ActionEnum.Update;

            attributeModel.AttributeId = viewModel.AttributeId;
            attributeModel.MinimumNumber = viewModel.MinimumNumber;
            attributeModel.MaximumNumber = viewModel.MaximumNumber;
            attributeModel.NumberOfDecimals = viewModel.NumberOfDecimals;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_PickListMarkedForDeletionAreNotValidatedForLengthOrRegex_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 5;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "test1" },
                new PickListModel { PickListValue = new string('c', viewModel.MaxLengthAllowed.Value + 1), IsPickListSelectedForDelete = true },
                new PickListModel { PickListValue = "test3" },
                new PickListModel { PickListValue = "!@$#@$", IsPickListSelectedForDelete = true },
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^[a-z0-9]*$");
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockGetItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new GetItemsResult { TotalRecordsCount = 0 });

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_AllPickListValuesAreMarkedForDeletion_InValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 5;
            viewModel.Action = ActionEnum.Update;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "test1", IsPickListSelectedForDelete = true },
                new PickListModel { PickListValue = "test3", IsPickListSelectedForDelete = true },
                new PickListModel { PickListValue = "!@$#@$", IsPickListSelectedForDelete = true },
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^.*$");
            attributeModel.MaxLengthAllowed = viewModel.MaxLengthAllowed;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockGetItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new GetItemsResult { TotalRecordsCount = 0 });

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(ValidationMessages.PickListMustHaveOneNonDeletedPickListValue.IndexOf(result.Errors.Single().ErrorMessage) > 0);
        }

        [TestMethod]
        public void Validate_AtLeastOnePickListValueIsNotMarkedForDeletion_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 5;
            viewModel.Action = ActionEnum.Update;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "test1" },
                new PickListModel { PickListValue = "test3", IsPickListSelectedForDelete = true },
                new PickListModel { PickListValue = "!@$#@$", IsPickListSelectedForDelete = true },
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^.*$");
            attributeModel.MaxLengthAllowed = viewModel.MaxLengthAllowed;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockGetItemsQueryHandler.Setup(m => m.Search(It.IsAny<GetItemsParameters>()))
                .Returns(new GetItemsResult { TotalRecordsCount = 0 });

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_AttributeIsChangingFromTextAttributeToPickListAndAttributeExistsOnItem_InvalidResult()
        {
            //Given
            viewModel.Action = ActionEnum.Update;
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "test1" },
                new PickListModel { PickListValue = "test2" },
                new PickListModel { PickListValue = "test3" }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^.*$");
            attributeModel.IsPickList = false;
            attributeModel.MaxLengthAllowed = viewModel.MaxLengthAllowed;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockDoesAttributeExistOnItemsQueryHandler.Setup(m => m.Search(It.IsAny<DoesAttributeExistOnItemsParameters>()))
                .Returns(true);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.AreEqual("Unable to switch to a PickList. Items exist that have values for this attribute.", result.Errors.Single().ErrorMessage);
        }

        [TestMethod]
        public void Validate_AttributeIsChangingFromTextAttributeToPickListAndAttributeDoesntExistOnItem_ValidResult()
        {
            //Given
            viewModel.Action = ActionEnum.Update;
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MaxLengthAllowed = 10;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };
            viewModel.IsPickList = true;
            viewModel.PickListData = new List<PickListModel>
            {
                new PickListModel { PickListValue = "test1" },
                new PickListModel { PickListValue = "test2" },
                new PickListModel { PickListValue = "test3" }
            };
            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>()))
                .Returns(@"^.*$");
            attributeModel.IsPickList = false;
            attributeModel.MaxLengthAllowed = viewModel.MaxLengthAllowed;
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockDoesAttributeExistOnItemsQueryHandler.Setup(m => m.Search(It.IsAny<DoesAttributeExistOnItemsParameters>()))
                .Returns(false);

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueWithNumberOfDecimaIs0_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "0";
            viewModel.MaximumNumber = "0";
            viewModel.NumberOfDecimals = "0";
            viewModel.DefaultValue = "0";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueWithNumberOfDecimalMoreThenNumberOfDecimals_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "0";
            viewModel.MaximumNumber = "1";
            viewModel.NumberOfDecimals = "0";
            viewModel.DefaultValue = "0.2";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueExceedsMaximumNumber_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "0";
            viewModel.MaximumNumber = "1";
            viewModel.NumberOfDecimals = "0";
            viewModel.DefaultValue = "2";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueLessThenMinimumNumber_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Number;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.MinimumNumber = "1";
            viewModel.MaximumNumber = "100";
            viewModel.NumberOfDecimals = "0";
            viewModel.DefaultValue = "0";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueDate_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Date;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = System.DateTime.Now.ToString("yyyy-MM-dd");
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueDate_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Date;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "2020-13-01";
            viewModel.Action = ActionEnum.Add;

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }


        [TestMethod]
        public void Validate_DefaultValueText_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt 123";
            viewModel.MaxLengthAllowed = 200;
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[0-9]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextWithAllSpecialCharacters_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt 123 &%#`~";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = true;
            viewModel.SpecialCharacterSetSelected = "All";
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[0-9]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextWithSpecificSpecialCharacters_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt 123 &";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = true;
            viewModel.SpecialCharactersAllowed = "&";
            viewModel.SpecialCharacterSetSelected = "Specific";
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[0-9]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextWithSpecificSpecialCharacters_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt 123 &@";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = true;
            viewModel.SpecialCharactersAllowed = "&*";
            viewModel.SpecialCharacterSetSelected = "Specific";
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[0-9]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };


            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(),
                                                                       It.IsAny<List<CharacterSetModel>>(),
                                                                       It.IsAny<string>())).Returns("^[A-Za-z0-9\\s&*]*$");
            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextWithSpecialCharacters_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt 123 &";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = false;
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[0-9]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" }
            };


            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(),
                                                                       It.IsAny<List<CharacterSetModel>>(),
                                                                       It.IsAny<string>())).Returns("^[A-Za-z0-9\\s]*$");

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextWithNoSpace_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt 123";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = false;
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[0-9]*" },
            };

            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(),
                                                                       It.IsAny<List<CharacterSetModel>>(),
                                                                       It.IsAny<string>())).Returns("^[A-Za-z0-9]*$");

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextNoNumbers_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt 123";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = false;
            viewModel.Action = ActionEnum.Add;

            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
                new CharacterSetModel{ IsSelected = true, RegEx = @"\s" },
            };

            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(),
                                                                          It.IsAny<List<CharacterSetModel>>(),
                                                                          It.IsAny<string>())).Returns("^[A-Za-z\\s]*$");

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextNoLowerCase_InvalidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TeSt";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = false;
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
            };

            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>())).Returns("^([A-Z]|[^a-z0-9\\s])*$");

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextUpperCaseOnly_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "TEST";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = false;
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[A-Z]*" },
            };

            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>())).Returns("^([A-Z]|[^a-z0-9\\s])*$");

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextLowCaseOnly_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "test";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = false;
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[a-z]*" },
            };

            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>())).Returns("^([a-z]|[^A-Z0-9\\s])*$");

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_DefaultValueTextNumericOnly_ValidResult()
        {
            //Given
            viewModel.DataTypeId = (int)DataType.Text;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.DefaultValue = "123";
            viewModel.MaxLengthAllowed = 200;
            viewModel.IsSpecialCharactersSelected = false;
            viewModel.Action = ActionEnum.Add;
            viewModel.AvailableCharacterSets = new List<CharacterSetModel>
            {
                new CharacterSetModel{ IsSelected = true, RegEx = "[0-9]*" },
            };

            mockAttributesHelper.Setup(m => m.CreateCharacterSetRegexPattern(It.IsAny<int>(), It.IsAny<List<CharacterSetModel>>(), It.IsAny<string>())).Returns("^([0-9]|[^A-Za-z\\s])*$");

            //When
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
        }

        [TestMethod]
        public void Validate_IsActiveIsFalseAndIsRequiredIsFalseAndItemCountIsZero_ValidResult()
        {
            //Given
            viewModel.AttributeId = 123;
            viewModel.DataTypeId = (int)DataType.Date;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.IsActive = false;
            viewModel.IsRequired = false;

            //When
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockDoesAttributeExistOnItemsQueryHandler.Setup(m => m.Search(It.IsAny<DoesAttributeExistOnItemsParameters>()))
                .Returns(false);
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsTrue(result.IsValid);
            Assert.IsTrue(result.Errors.Count == 0);
        }

        [TestMethod]
        public void Validate_IsActiveIsFalseAndIsRequiredIsFalseAndItemCountIsNotZero_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 123;
            viewModel.DataTypeId = (int)DataType.Date;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.IsActive = false;
            viewModel.IsRequired = false;

            //When
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockDoesAttributeExistOnItemsQueryHandler.Setup(m => m.Search(It.IsAny<DoesAttributeExistOnItemsParameters>()))
                .Returns(true);
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Attribute is associated with one or more items. Cannot hide."));
        }

        [TestMethod]
        public void Validate_IsActiveIsFalseAndIsRequiredIsTruAndItemCountIsZero_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 123;
            viewModel.DataTypeId = (int)DataType.Date;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.IsActive = false;
            viewModel.IsRequired = true;

            //When
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockDoesAttributeExistOnItemsQueryHandler.Setup(m => m.Search(It.IsAny<DoesAttributeExistOnItemsParameters>()))
                .Returns(false);
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
            Assert.IsTrue(result.Errors.Any(e => e.ErrorMessage == "Attribute is required. Cannot hide."));
        }

        [TestMethod]
        public void Validate_IsActiveIsFalseAndIsRequiredIsTrueAndItemCountIsNotZero_InvalidResult()
        {
            //Given
            viewModel.AttributeId = 123;
            viewModel.DataTypeId = (int)DataType.Date;
            viewModel.DisplayName = "Test";
            viewModel.TraitCode = "Tst";
            viewModel.IsActive = false;
            viewModel.IsRequired = true;

            //When
            mockGetAttributeByAttributeIdQuery.Setup(m => m.Search(It.IsAny<GetAttributeByAttributeIdParameters>()))
                .Returns(attributeModel);
            mockDoesAttributeExistOnItemsQueryHandler.Setup(m => m.Search(It.IsAny<DoesAttributeExistOnItemsParameters>()))
                .Returns(true);
            var result = validator.Validate(viewModel);

            //Then
            Assert.IsFalse(result.IsValid);
        }
    }
}