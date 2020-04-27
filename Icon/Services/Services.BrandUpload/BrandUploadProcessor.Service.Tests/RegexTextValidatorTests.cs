using BrandUploadProcessor.Service.Validation;
using BrandUploadProcessor.Service.Validation.Interfaces;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BrandUploadProcessor.Service.Tests
{
    [TestClass]
    public class RegexTextValidatorTests
    {
        private IRegexTextValidator validator;

        [TestInitialize]
        public void Init()
        {
            validator = new RegexTextValidator();
        }

        [TestMethod]
        public void RegexTextValidator_PatternMatches_ReturnsNoErrors()
        {
            var regexPattern = "(Global|Regional)$";
            var value = "Global";

            var result = validator.Validate(regexPattern, value);

            Assert.IsTrue(result.IsValid);
            Assert.IsNull(result.Error);

        }
        [TestMethod]
        public void RegexTextValidator_PatternDoesntMatch_Returns1Error()
        {
            var regexPattern = "(Global|Regional)$";
            var value = "BadValue";
            var expectedErrorMsg = $"'{value}' does not meet traitPattern [ {regexPattern} ] requirements.";

            var result = validator.Validate(regexPattern, value);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(expectedErrorMsg, result.Error);
        }

        [TestCleanup]
        public void Cleanup() { }

    }
}