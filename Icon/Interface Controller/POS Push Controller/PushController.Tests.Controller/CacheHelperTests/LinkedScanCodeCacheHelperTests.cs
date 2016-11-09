using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Exceptions;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System;
using System.Collections.Generic;

namespace PushController.Tests.Controller.CacheHelperTests
{
    [TestClass]
    public class LinkedScanCodeCacheHelperTests
    {
        private LinkedScanCodeCacheHelper cacheHelper;
        private Mock<IQueryHandler<GetCurrentLinkedScanCodesQuery, List<CurrentLinkedScanCodeModel>>> mockGetCurrentLinkedScanCodesQueryHandler;
        private List<string> testScanCodes;
        private List<int> testBusinessUnits;
        private string testLinkedScanCode;

        [TestInitialize]
        public void Initialize()
        {
            mockGetCurrentLinkedScanCodesQueryHandler = new Mock<IQueryHandler<GetCurrentLinkedScanCodesQuery, List<CurrentLinkedScanCodeModel>>>();

            Cache.scanCodeByBusinessUnitToLinkedScanCode.Clear();
        }

        private void BuildCacheHelper()
        {
            cacheHelper = new LinkedScanCodeCacheHelper(mockGetCurrentLinkedScanCodesQueryHandler.Object);
        }

        [TestMethod]
        public void PopulateLinkedScanCodeCache_KeyIsNotCached_ValueShouldBeAdded()
        {
            // Given.
            BuildCacheHelper();

            testScanCodes = new List<string> { "1111" };
            testBusinessUnits = new List<int> { 88888 };
            testLinkedScanCode = "2222";

            var mockQueryResult = new CurrentLinkedScanCodeModel { ScanCode = testScanCodes[0], LinkedScanCode = testLinkedScanCode, BusinessUnitId = testBusinessUnits[0] };
            mockGetCurrentLinkedScanCodesQueryHandler.Setup(q => q.Execute(It.IsAny<GetCurrentLinkedScanCodesQuery>())).Returns(new List<CurrentLinkedScanCodeModel> { mockQueryResult });

            var testKey = new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]);

            // When.
            cacheHelper.Populate(new List<Tuple<string, int>> { testKey });

            // Then.
            Assert.AreEqual(testScanCodes.Count, Cache.scanCodeByBusinessUnitToLinkedScanCode.Count);
            Assert.AreEqual(testLinkedScanCode, Cache.scanCodeByBusinessUnitToLinkedScanCode[testKey]);
        }

        [TestMethod]
        public void PopulateLinkedScanCodeCache_KeyIsAlreadyCached_ValueShouldBeUpdated()
        {
            // Given.
            BuildCacheHelper();

            testScanCodes = new List<string> { "1111" };
            testBusinessUnits = new List<int> { 88888 };
            testLinkedScanCode = "2222";

            var mockQueryResult = new CurrentLinkedScanCodeModel { ScanCode = testScanCodes[0], LinkedScanCode = null, BusinessUnitId = testBusinessUnits[0] };
            mockGetCurrentLinkedScanCodesQueryHandler.Setup(q => q.Execute(It.IsAny<GetCurrentLinkedScanCodesQuery>())).Returns(new List<CurrentLinkedScanCodeModel> { mockQueryResult });

            var testKey = new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]);
            Cache.scanCodeByBusinessUnitToLinkedScanCode.Add(testKey, testLinkedScanCode);

            // When.
            cacheHelper.Populate(new List<Tuple<string, int>> { testKey });

            // Then.
            Assert.AreEqual(testScanCodes.Count, Cache.scanCodeByBusinessUnitToLinkedScanCode.Count);
            Assert.IsNull(Cache.scanCodeByBusinessUnitToLinkedScanCode[testKey]);
        }

        [TestMethod]
        public void RetrieveCachedLinkedScanCode_ScanCodeIsFound_ScanCodeShouldBeReturned()
        {
            // Given.
            BuildCacheHelper();

            testScanCodes = new List<string> { "1111" };
            testBusinessUnits = new List<int> { 88888 };
            testLinkedScanCode = "2222";

            var testKey = new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]);
            Cache.scanCodeByBusinessUnitToLinkedScanCode.Add(testKey, testLinkedScanCode);

            // When.
            var retrievedLinkedScanCode = cacheHelper.Retrieve(new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]));

            // Then.
            Assert.AreEqual(testLinkedScanCode, retrievedLinkedScanCode);
        }

        [TestMethod]
        public void RetrieveCachedLinkedScanCode_ScanCodeIsNotFound_ShouldReturnNull()
        {
            // Given.
            BuildCacheHelper();

            testScanCodes = new List<string> { "1111", "3333" };
            testBusinessUnits = new List<int> { 88888 };
            testLinkedScanCode = "2222";

            var testKey = new Tuple<string, int>(testScanCodes[0], testBusinessUnits[0]);
            var testNotFoundKey = new Tuple<string, int>(testScanCodes[1], testBusinessUnits[0]);

            Cache.scanCodeByBusinessUnitToLinkedScanCode.Add(testKey, testLinkedScanCode);

            // When.
            var retrievedScanCode = cacheHelper.Retrieve(testNotFoundKey);

            // Then.
            Assert.IsNull(retrievedScanCode);
        }
    }
}
