using System;
using System.Data.SqlClient;
using BulkItemUploadProcessor.Common.Builder;
using System.Runtime.Serialization;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace BulkItemUploadProcessor.Common.Tests
{
    [TestClass]
    public class ErrorMessageBuilderTests
    {
        private const string UpdateError = "Error in updating data.";
        private const string InvalidHierarchyError = "The hierarchy value selected does not exist in Icon.";
        private const string DuplicateScanCodeError = "Item with this scan code already exists in Icon.";

        ErrorMessageBuilder errorMessageBuilder;
        SqlExceptionBuilder sqlExceptionBuilder;

       [TestInitialize]
        public void Init()
        {
            errorMessageBuilder = new ErrorMessageBuilder();
            sqlExceptionBuilder = new SqlExceptionBuilder();
        }

        [TestMethod]
        public void Validate_BuildErrorMessageSqlExceptionErrorNumber547IsPassed_ShouldReturnInvalidHierarchyError()
        {
            sqlExceptionBuilder.WithErrorMessage("HierarchyClass Error");
            sqlExceptionBuilder.WithErrorNumber(547);
            var exception = sqlExceptionBuilder.Build();

            //when
            var message = errorMessageBuilder.BuildErrorMessage(exception);

            //then

            Assert.AreEqual(InvalidHierarchyError,message);
        }

        [TestMethod]
        public void Validate_BuildErrorMessageSqlExceptionErrorNumber2627IsPassed_ShouldReturnDuplicateScanCodeError()
        {
            sqlExceptionBuilder.WithErrorMessage("ScanCode Error");
            sqlExceptionBuilder.WithErrorNumber(2627);
            var exception = sqlExceptionBuilder.Build();

            //when
            var message = errorMessageBuilder.BuildErrorMessage(exception);

            //then

            Assert.AreEqual(DuplicateScanCodeError, message);
        }

        [TestMethod]
        public void Validate_BuildErrorMessageSqlExceptionErrorNumberOtherThan547And2627IsPassed_ShouldReturnUpdateError()
        {
            sqlExceptionBuilder.WithErrorMessage("Error");
            sqlExceptionBuilder.WithErrorNumber(1009);
            var exception = sqlExceptionBuilder.Build();

            //when
            var message = errorMessageBuilder.BuildErrorMessage(exception);

            //then

            Assert.AreEqual(UpdateError, message);
        }
    }
}