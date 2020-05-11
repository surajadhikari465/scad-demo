using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using BrandUploadProcessor.Common.Models;
using BrandUploadProcessor.Service.Mappers;
using BrandUploadProcessor.Service.Validation;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Dapper;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrandUploadProcessor.Service.Tests
{
    [TestClass]
    public class RegexTextValidatorTests
    {
        private IRegexTextValidator validator;
        private IDbConnection dbconnection;

        [TestInitialize]
        public void Init()
        {
            dbconnection = new SqlConnection(ConfigurationManager.ConnectionStrings["IconConnectionString"].ConnectionString);
            validator = new RegexTextValidator(new AttributeErrorMessageMapper());
        }

        [TestMethod]
        public void RegexTextValidator_PatternMatches_ReturnsNoErrors()
        {
            var regexPattern = "(A|B)$";
            var value = "A";
            var attributeColumn = new AttributeColumn {RegexPattern = regexPattern};

            var result = validator.Validate(attributeColumn, value);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public void RegexTextValidator_BrandAbbreviationAllowsLettersNumbersSpacesAmpersandsOnly_Matches_ReturnsNoErrors()
        {
            var regexPattern = GetTraitPatternByTraitCode("BA");
            var value = "abc123& ";
            var attributeColumn = CreateAttributeColumn("Brand Abbreviation", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public void RegexTextValidator_BrandAbbreviationAllowsLettersNumbersSpacesAmpersandsOnly_DoesntMatch_ReturnsError()
        {
            var regexPattern = GetTraitPatternByTraitCode("BA");
            var value = "abc123&!";
            var expectedErrorMessage = "Brand Abbreviation allows 10 or fewer valid characters (Letters, Numbers, and Ampersands only)";
            var attributeColumn = CreateAttributeColumn("Brand Abbreviation", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(expectedErrorMessage,result.Error);
        }



        [TestMethod]
        public void RegexTextValidator_LocalityAllows35chars_DoesntMatch_ReturnsError()
        {
            var regexPattern = GetTraitPatternByTraitCode("LCL");
            var value = "abc123&!123456789123456789123456789132456789";
            var expectedErrorMessage = "Locality allows up to 35 characters";
            var attributeColumn = CreateAttributeColumn("Locality", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(expectedErrorMessage, result.Error);
        }

        [TestMethod]
        public void RegexTextValidator_LocalityAllows35chars_Matches_NoError()
        {
            var regexPattern = GetTraitPatternByTraitCode("LCL");
            var value = "abc123&!12";
            var attributeColumn = CreateAttributeColumn("Locality", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Error);
        }


        [TestMethod]
        public void RegexTextValidator_DesignationTrait_PatternDoesntMatch_Returns1Error()
        {
            var regexPattern = GetTraitPatternByTraitCode("GRD");
            var value = "BadValue";
            var expectedErrorMsg = "Designation allowed values are 'Global' or 'Regional'";
            var attributeColumn = CreateAttributeColumn("Designation", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [TestMethod]
        public void RegexTextValidator_DesignationTrait_PatternMatches_NoError()
        {
            var regexPattern = GetTraitPatternByTraitCode("GRD");
            var value = "Global";
            var attributeColumn = CreateAttributeColumn("Designation", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Error);
        }


        [TestMethod]
        public void RegexTextValidator_ZipCodeTrait_PatternMatches_NoError()
        {
            var regexPattern = GetTraitPatternByTraitCode("ZIP");
            var value = "78A6-13";
            var attributeColumn = CreateAttributeColumn("Zip Code", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Error);
        }

        [TestMethod]
        public void RegexTextValidator_ZipCodeTrait_PatternDoesntMatch_ReturnsError()
        {
            var regexPattern = GetTraitPatternByTraitCode("ZIP");
            var value = "78A6130123456789123456798";
            var expectedErrorMessage = "Zip Code must be 10 characters or less and only contain letters, numbers, space and hyphens";
            var attributeColumn = CreateAttributeColumn("Zip Code", regexPattern);

            var result = validator.Validate(attributeColumn, value);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(expectedErrorMessage, result.Error);
        }


        [TestCleanup]
        public void Cleanup() { dbconnection.Dispose();}


        private AttributeColumn CreateAttributeColumn(string header, string regexPattern)
        {
            return new AttributeColumn { ColumnHeader = new ColumnHeader { Name = header }, RegexPattern = regexPattern };
        }
        private string GetTraitPatternByTraitCode(string traitCode)
        {
            return dbconnection.QueryFirst<string>("select traitpattern from dbo.trait where traitcode = @traitCode",
                new {traitCode});
        }
    }
}