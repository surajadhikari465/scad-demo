using System.Collections.Generic;
using BrandUploadProcessor.Common;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Interfaces;
using BrandUploadProcessor.Service.Mappers;
using BrandUploadProcessor.Service.Validation;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Icon.Common.DataAccess;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;

namespace BrandUploadProcessor.Service.Tests
{
    [TestClass]
    public class RowObjectsValidatorUpdateExistingTests
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

            regexTextValidator = new RegexTextValidator(new AttributeErrorMessageMapper());

            brandsCache = new BrandsCache(GetBrandsQueryHandler.Object);
            brandsCache.Refresh();

            validator = new RowObjectsValidator(regexTextValidator, brandsCache);
        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandNoTraits_1ValidRow()
        {

            var expectedName = "testname";
            var expectedAbbr = "tst1";
            var expectedId = brandsCache.Brands[1].BrandId;

            var expectedValidRows = 1;
            var expectdInvalidRows = 0;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedId.ToString()),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectdInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandNoTraitsDupeName_1RowError()
        {

            var expectedName = brandsCache.Brands[0].BrandName;
            var expectedAbbr = "tst1";
            var expectedBrandId = "2";

            var expectedValidRows = 0;
            var expectedInvalidRows = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedBrandId),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandDupeAbbrFromDatabseThatsNotCurrentBrand_1RowError()
        {

            var expectedName = "asdflasdjkf random";
            var expectedAbbr = brandsCache.Brands[0].BrandAbbreviation;
            var expectedBrandId = "2";

            var expectedValidRows = 0;
            var expectedInvalidRows = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedBrandId),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
        }


        [TestMethod]
        public void Validate_UpdateExisting_1BrandNoBrandId_2RowErrors_BrandIdRequiredForUpdate()
        {

            var expectedName = "asdflasdjkf random";
            var expectedAbbr = "Asdf";
            

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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
        }


        [TestMethod]
        public void Validate_UpdateExisting_1BrandDupeAbbrFromDatabseThatIsCurrentBrand_1ValidRow()
        {

            var expectedName = "asdflasdjkf random";
            var expectedAbbr = brandsCache.Brands[0].BrandAbbreviation;
            var expectedBrandId = "1";

            var expectedValidRows = 1;
            var expectedInvalidRows = 0;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedBrandId),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
        }

        [TestMethod]
        public void Validate_UpdateExisting_BrandInWorksheetMatchesBrandInDBByIrmaLengthRules_NotSameBrandId_1ErrorRow()
        {
            var irmaLengthName = "1234567890123456789012345";
            var uniqueAfter25Name = irmaLengthName + "1";
            var nonDupeBrandId = "1";

            var dupebrand = new BrandModel {BrandAbbreviation = "11", BrandId = 9999999, BrandName = irmaLengthName};

            brandsCache.Brands.Add(dupebrand);

            var expectedValidRows = 0;
            var expectedInvalidRows = 1;
            var expectedErrorMessage =
                $"Brand name '{uniqueAfter25Name}' trimmed to 25 characters already exists in the spreadsheet. Change the brand name so that the first 25 characters are unique.";

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, nonDupeBrandId),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, uniqueAfter25Name),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "999"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMessage, rowObjectValidatorResponse.InvalidRows[0].Error);
        }


        [TestMethod]
        public void Validate_UpdateExisting_BrandNameInWorksheetMatchesBrandInDBByIrmaLengthRules_SameBrandId_NoErrors()
        {
            var irmaLengthName = "1234567890123456789012345";
            var uniqueAfter25Name = irmaLengthName + "1";
            

            var dupebrand = new BrandModel { BrandAbbreviation = "11", BrandId = 9999999, BrandName = irmaLengthName };

            brandsCache.Brands.Add(dupebrand);

            var expectedValidRows = 1;
            var expectedInvalidRows = 0;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, dupebrand.BrandId.ToString()),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, uniqueAfter25Name),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "999"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
        }




        [TestMethod]
        public void Validate_UpdateExisting_2BrandDupeAbbrFromWorksheet_2RowErrors()
        {

            
            var expectedAbbr = brandsCache.Brands[0].BrandAbbreviation;
            var expectedBrandId = "1";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 2;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedBrandId),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "name 1"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                }),
                TestHelpers.CreateRowObject(2, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedBrandId),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "name 2"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  expectedAbbr),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
        }

        [TestMethod]
        public void Validate_UpdateExisting_2BrandDupeNameFromWorksheet_2RowErrors()
        {


            var expectedName = brandsCache.Brands[0].BrandName;
            var expectedBrandId = "1";

            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 2;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedBrandId),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "ab1"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                }),
                TestHelpers.CreateRowObject(2, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, expectedBrandId),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, expectedName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader,  "Ab2"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };
            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandNameIsRemove_2RowErrors()
        {

            var expectedName = Constants.RemoveExcelValue;
            var expectedAbbr = "asdf";
            var expectedErrorMsg = Constants.ErrorMessages.InvalidRemoveBrandName;

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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandAbbrIsRemove_2RowErrors()
        {

            var expectedName = "asldkjfals Random";
            var expectedAbbr = Constants.RemoveExcelValue;
            var expectedErrorMsg = Constants.ErrorMessages.InvalidRemoveBrandAbbreviation;

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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);
        }


        [TestMethod]
        public void Validate_UpdateExisting_1BrandInvalidParentCompany_1RowError()
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
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "1"),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expectedErrorMsg, rowObjectValidatorResponse.InvalidRows[0].Error);

        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandValidParentCompany_1ValidRow()
        {

            var validParentCompany = brandsCache.Brands[1].BrandName;

            var expectedValidRows = 1;
            var expectedInvalidRows = 0;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "1"),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);

        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandValidTraitsAreRemove_1ValidRow_RemoveAllowedForUpdateExisting()
        {

            var removeValue = Constants.RemoveExcelValue;
            var expectedValidRows = 1;
            var expectedInvalidRows = 0;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "1"),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);


        }

        [TestMethod]
        public void Validate_UpdateExisting_1BrandTraitValuesDontMatchRegexPatternsExcludingParentCompany_5InvalidRows()
        {
            //Parent Company uses different validation so its excluded from this test.

            var badZipCode = "123456798-123456";
            var badLocality = "1".PadRight(40, '1');
            var badDesignation = "BadDesignation";
            var badName = "bad.brand|name";
            var badAbbr = "bad.abbreviation!";

            var expectedValidRows = 0;
            var expectedInvalidRows = 5;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "1"),
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

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects, columnHeaders, brandAttributeModels);

            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRows, rowObjectValidatorResponse.InvalidRows.Count);
        }

        [TestMethod]
        public void Validate_UpdateExisting_MissingBrandName_1RowError()
        {

            var missingBrandName = string.Empty;
            var expextedErrorMessage = Constants.ErrorMessages.RequiredBrandName;
            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "1"),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, missingBrandName),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "AA"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };

            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects,
                columnHeaders, brandAttributeModels);
            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expextedErrorMessage, rowObjectValidatorResponse.InvalidRows[0].Error);

        }

        [TestMethod]
        public void Validate_UpdateExisting_NonIntegerBrandId_1RowError()
        {
            var invalidBrandId = "a";
            var expextedErrorMessage = Constants.ErrorMessages.InvalidBrandIdDataType;
            var expectedValidRows = 0;
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, invalidBrandId),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "Test brand"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, "Test abbrev"),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };

            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects,
                columnHeaders, brandAttributeModels);
            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expextedErrorMessage, rowObjectValidatorResponse.InvalidRows[0].Error);
        }

        [TestMethod]
        public void Validate_UpdateExisting_MissingBrandAbbreviation_1RowError()
        {

            var missingBrandAbbreviation = string.Empty;
            var expextedErrorMessage = Constants.ErrorMessages.RequiredBrandAbbreviation;
            var expectedValidRows = 0;  
            var expectedInvalidRowErrors = 1;

            var rowObjects = new List<RowObject>
            {
                TestHelpers.CreateRowObject(1, new List<ParsedCell>
                {
                    TestHelpers.CreateParsedCell(Constants.BrandIdColumnHeader, "1"),
                    TestHelpers.CreateParsedCell(Constants.BrandNameColumnHeader, "Test brand"),
                    TestHelpers.CreateParsedCell(Constants.BrandAbbreviationColumnHeader, missingBrandAbbreviation),
                    TestHelpers.CreateParsedCell(Constants.ZipCodeColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.LocalityColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.DesignationColumnHeader, string.Empty),
                    TestHelpers.CreateParsedCell(Constants.ParentCompanyColumnHeader, string.Empty)
                })
            };

            var columnHeaders = TestHelpers.GetHeaders();
            var brandAttributeModels = TestHelpers.GetBrandAttributeModels();

            var rowObjectValidatorResponse = validator.Validate(Enums.FileModeTypeEnum.UpdateExisting, rowObjects,
                columnHeaders, brandAttributeModels);
            Assert.AreEqual(expectedValidRows, rowObjectValidatorResponse.ValidRows.Count);
            Assert.AreEqual(expectedInvalidRowErrors, rowObjectValidatorResponse.InvalidRows.Count);
            Assert.AreEqual(expextedErrorMessage, rowObjectValidatorResponse.InvalidRows[0].Error);

        }

        [TestCleanup]
        public void Cleanup() { }

    }
}