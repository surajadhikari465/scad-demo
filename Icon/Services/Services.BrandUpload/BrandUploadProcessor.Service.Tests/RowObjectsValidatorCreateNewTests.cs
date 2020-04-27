using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.DataAccess.Queries;
using BrandUploadProcessor.Service.Interfaces;
using BrandUploadProcessor.Service.Validation;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BrandUploadProcessor.Service.Tests
{
    [TestClass]
    public class RowObjectsValidatorCreateNewTests
    {
        private RowObjectsValidator validator;
        private IRegexTextValidator regexTextValidator;
        private IBrandsCache brandsCache;
        private Mock<IQueryHandler<EmptyQueryParameters<List<BrandModel>>, List<BrandModel>>> GetBrandsQueryHandler;

        private Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>,
            IEnumerable<BrandAttributeModel>>> getBrandAttributesQueryHandler;


        [TestInitialize]
        public void Init()
        {

            GetBrandsQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<List<BrandModel>>, List<BrandModel>>>();
            GetBrandsQueryHandler.Setup(s => s.Search(It.IsAny<EmptyQueryParameters<List<BrandModel>>>()))
                .Returns(TestHelpers.GetBrands());

            getBrandAttributesQueryHandler = new Mock<IQueryHandler<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>, IEnumerable<BrandAttributeModel>>>();
            getBrandAttributesQueryHandler.Setup(s => s.Search(It.IsAny<EmptyQueryParameters<IEnumerable<BrandAttributeModel>>>()))
                .Returns(TestHelpers.GetBrandAttributeModels());

            regexTextValidator = new RegexTextValidator();

            brandsCache = new BrandsCache(GetBrandsQueryHandler.Object);
            brandsCache.Refresh();

             validator= new RowObjectsValidator(regexTextValidator,brandsCache);
        }

        [TestMethod]
        public void Validate_CreateNew_1BrandNoTraits_1ValidRow()
        {

            var expectedName = "testname";
            var expectedAbbr = "tst1";

            var expectedValidRows = 1;
            var expectdInvalidRows = 0;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows,rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectdInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
        }


        [TestMethod]
        public void Validate_CreateNew_1BrandNoTraitsDupeName_1RowError()
        {

            var expectedName = brandsCache.Brands[0].BrandName;
            var expectedAbbr = "tst1";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
        }



        [TestMethod]
        public void Validate_CreateNew_1BrandNoTraitsDupeAbbr_1RowError()
        {

            var expectedName = "asdflasdjkf ramdom";
            var expectedAbbr = brandsCache.Brands[0].BrandAbbreviation;

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
        }

        [TestMethod]
        public void Validate_CreateNew_1BrandNameIsRemove_2RowErrors()
        {

            var expectedName = Constants.RemoveExcelValue;
            var expectedAbbr = "asdf";
            var expectedErrorMsg = $"'{Constants.BrandNameColumnHeader}' cannot be {Constants.RemoveExcelValue}";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 2;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg,rowObjectValidatorResponse.InvalidRows[0].Error);
        }

        [TestMethod]
        public void Validate_CreateNew_1BrandAbbrIsRemove_2RowErrors()
        {

            var expectedName = "asldkjfals Random";
            var expectedAbbr = Constants.RemoveExcelValue;
            var expectedErrorMsg = $"'{Constants.BrandAbbreviationColumnHeader}' cannot be {Constants.RemoveExcelValue}";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 2;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
        }

        [TestMethod]
        public void Validate_CreateNew_1BrandBrandIdNotNull_1RowError_BrandIdMustBeNull()
        {

            var expectedName = "asldkjfals Random";
            var expectedAbbr = "t944";
            var expectedErrorMsg = $"'{Constants.BrandIdColumnHeader}' must be empty when creating new brands.";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "1"),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
        }

        [TestMethod]
        public void Validate_CreateNew_MultipleBrandsDuplicateNamesInWorksheet_2RowErrors()
        {

            var expectedName = "random brand name";
            
            var expectedErrorMsg =
                $"'{Constants.BrandNameColumnHeader}' has invalid value. '{expectedName}' exists more than once in the worksheet and must be unique.";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 2;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "999"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                }),
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "111"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[1].Error);
        }

        [TestMethod]
        public void Validate_CreateNew_MultipleBrandsDuplicateAbbreviationsInWorksheet_2RowErrors()
        {

            var expectedAbbr = "999";

            var expectedErrorMsg =
                $"'{Constants.BrandAbbreviationColumnHeader}' has invalid value. '{expectedAbbr}' exists more than once in the worksheet and must be unique.";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 2;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random name 1"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                }),
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random name 2"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[1].Error);
        }



        [TestMethod]
        public void Validate_CreateNew_MultipleBrands1DuplicateNameFromDatabase_1ValidRow1RowError()
        {

            var expectedName = brandsCache.Brands[1].BrandName;

            var expectedErrorMsg =
                $"'{Constants.BrandNameColumnHeader}' has invalid value. '{expectedName}' already exists in the database and must be unique.";

            var expectedValidRows = 1;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "999"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                }),
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random Name"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "111"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
        }

        [TestMethod]
        public void Validate_CreateNew_MultipleBrands1DuplicateAbbreviationsFromDatabase_1ValidRow1RowError()
        {

            var expectedAbbr = brandsCache.Brands[1].BrandAbbreviation;

            var expectedErrorMsg =
                $"'{Constants.BrandAbbreviationColumnHeader}' has invalid value. '{expectedAbbr}' already exists in the database and must be unique.";

            var expectedValidRows = 1;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random name 1"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                }),
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random name 2"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "111"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
  
        }


        [TestMethod]
        public void Validate_CreateNew_1BrandInvalidParentCompany_1RowError()
        {

            var invalidParentCompany = "invalid brand name";

            var expectedErrorMsg =
                $"'{Constants.ParentCompanyColumnHeader}' has invalid value. '{invalidParentCompany}' does not exist in Icon.";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random name 1"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "t99"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, invalidParentCompany)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);

        }

        [TestMethod]
        public void Validate_CreateNew_1BrandValidParentCompany_1ValidRow()
        {

            var validParentCompany = brandsCache.Brands[1].BrandName;

            var expectedValidRows = 1;
            var expectedInvalidRowErrors = 0;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random name 1"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "t99"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, validParentCompany)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);

        }

        [TestMethod]
        public void Validate_CreateNew_1BrandValidTraitsAreRemove_4RowErrors_RemoveIsntAllowedForCreateNew()
        {

            var removeValue = Constants.RemoveExcelValue;

            var expectedErrorMsgPartial =
                $"has invalid value. '{Constants.RemoveExcelValue}' cannot be used when creating new brands";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 4;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "random name 1"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "t99"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, removeValue),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, removeValue),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, removeValue),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, removeValue)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);

            foreach (var invalidRowError in rowObjectValidatorResponse.InvalidRows)
            {
                Assert.IsTrue(invalidRowError.Error.EndsWith(expectedErrorMsgPartial));
            }

        }

        [TestMethod]
        public void Validate_CreateNew_1BrandTraitValuesDontMatchRegexPatternsExcludingParentCompany_5RowErrors()
        {
            //Parent Company uses different validation so its excluded from this test.

            var badZipCode = "123456798-123456";
            var badLocality = "1".PadRight(40, '1');
            var badDesignation = "BadDesignation";
            var badName = "bad.brand|name";
            var badAbbr = "bad.abbreviation!";
            var expectedErrorMsgPartial = " does not meet traitPattern";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 5;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, null),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, badName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  badAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, badZipCode),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, badLocality),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, badDesignation),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, null)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.CreateNew, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);

            foreach (var invalidRowError in rowObjectValidatorResponse.InvalidRows)
            {
                Assert.IsTrue(invalidRowError.Error.Contains(expectedErrorMsgPartial));
            }
        }

        [TestCleanup]
        public void Cleanup()
        {

        }
    }
}