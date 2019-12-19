using Icon.Common.DataAccess;
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
    [TestClass] [Ignore]
    public class AddEwicMappingManagerValidatorTests
    {
        private AddEwicMappingManagerValidator validator;
        private Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>> mockWfmScanCodeExistsQuery;
        private Mock<IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>>> mockAplScanCodeExistsQuery;
        private Mock<IQueryHandler<GetCurrentEwicMappingParameters, string>> mockGetCurrentEwicMappingQuery;
        private string testAplScanCode;
        private string testWfmScanCode;

        [TestInitialize]
        public void Initialize()
        {
            mockWfmScanCodeExistsQuery = new Mock<IQueryHandler<GetItemsByBulkScanCodeSearchParameters, List<ItemSearchModel>>>();
            mockAplScanCodeExistsQuery = new Mock<IQueryHandler<GetAplScanCodesParameters, List<EwicAplScanCodeModel>>>();
            mockGetCurrentEwicMappingQuery = new Mock<IQueryHandler<GetCurrentEwicMappingParameters, string>>();

            validator = new AddEwicMappingManagerValidator(
                mockWfmScanCodeExistsQuery.Object,
                mockAplScanCodeExistsQuery.Object,
                mockGetCurrentEwicMappingQuery.Object);

            testAplScanCode = "22222222";
            testWfmScanCode = "22222229";

            mockWfmScanCodeExistsQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel> { new ItemSearchModel() });
            mockAplScanCodeExistsQuery.Setup(q => q.Search(It.IsAny<GetAplScanCodesParameters>())).Returns(new List<EwicAplScanCodeModel> { new EwicAplScanCodeModel { ScanCode = testAplScanCode } });
        }

        [TestMethod]
        public void AddEwicMappingManagerValidator_WfmScanCodeDoesNotExistInIcon_InvalidResultShouldBeReturned()
        {
            // Given.
            mockWfmScanCodeExistsQuery.Setup(q => q.Search(It.IsAny<GetItemsByBulkScanCodeSearchParameters>())).Returns(new List<ItemSearchModel>());

            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            var result = validator.Validate(manager);

            // Then.
            string errorMessage = String.Format("Scan Code {0} does not exist in Icon.", testWfmScanCode);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(errorMessage, result.Error);
        }

        [TestMethod]
        public void AddEwicMappingManagerValidator_AplScanCodeDoesNotExistInTheApl_InvalidResultShouldBeReturned()
        {
            // Given.
            mockAplScanCodeExistsQuery.Setup(q => q.Search(It.IsAny<GetAplScanCodesParameters>())).Returns(new List<EwicAplScanCodeModel>());

            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            var result = validator.Validate(manager);

            // Then.
            string errorMessage = String.Format("Scan Code {0} does not exist in the APL.", testAplScanCode);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(errorMessage, result.Error);
        }

        [TestMethod]
        public void AddEwicMappingManagerValidator_WfmScanCodeIsAlreadyMapped_InvalidResultShouldBeReturned()
        {
            // Given.
            string currentMapping = "222222222";

            mockGetCurrentEwicMappingQuery.Setup(q => q.Search(It.IsAny<GetCurrentEwicMappingParameters>())).Returns(currentMapping);

            var manager = new AddEwicMappingManager
            {
                AplScanCode = testAplScanCode,
                WfmScanCode = testWfmScanCode
            };

            // When.
            var result = validator.Validate(manager);

            // Then.
            string errorMessage = String.Format("WFM Scan Code {0} is already mapped to APL scan code {1}.", testWfmScanCode, currentMapping);

            Assert.IsFalse(result.IsValid);
            Assert.AreEqual(errorMessage, result.Error);
        }
    }
}
