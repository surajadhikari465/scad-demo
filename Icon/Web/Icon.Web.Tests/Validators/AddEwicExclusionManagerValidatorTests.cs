using Icon.Common.DataAccess;
using Icon.Web.DataAccess.Infrastructure;
using Icon.Web.DataAccess.Managers;
using Icon.Web.DataAccess.Models;
using Icon.Web.DataAccess.Queries;
using Icon.Web.Mvc.Validators.Managers;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System;
using System.Collections.Generic;

namespace Icon.Web.Tests.Unit.Validators
{
    [TestClass]
    public class AddEwicExclusionManagerValidatorTests
    {
        private AddEwicExclusionManagerValidator validator;
        private Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>> mockScanCodeExistsQuery;
        private string testScanCode;

        [TestInitialize]
        public void Initialize()
        {
            mockScanCodeExistsQuery = new Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>>();

            validator = new AddEwicExclusionManagerValidator(
                mockScanCodeExistsQuery.Object);

            testScanCode = "22222222";
        }

        [TestMethod]
        public void AddEwicExclusionManagerValidator_ScanCodeDoesNotExistInIcon_InvalidResultShouldBeReturned()
        {
            // Given.
            mockScanCodeExistsQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());

            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            var result = validator.Validate(manager);

            // Then.
            string errorMessage = String.Format("Scan Code {0} does not exist in Icon.", testScanCode);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(errorMessage, result.Error);
        }

        [TestMethod]
        public void AddEwicExclusionManagerValidator_ScanCodeExistsInIcon_ValidResultShouldBeReturned()
        {
            // Given.
            mockScanCodeExistsQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel> { new ItemSearchModel() });

            var manager = new AddEwicExclusionManager
            {
                ScanCode = testScanCode
            };

            // When.
            var result = validator.Validate(manager);

            // Then.
            Assert.IsTrue(result.IsValid);
        }
    }
}
