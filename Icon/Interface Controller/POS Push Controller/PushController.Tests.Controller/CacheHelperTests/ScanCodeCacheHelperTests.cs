using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using PushController.Common;
using PushController.Common.Exceptions;
using PushController.Common.Models;
using PushController.Controller.CacheHelpers;
using PushController.DataAccess.Interfaces;
using PushController.DataAccess.Queries;
using System.Collections.Generic;

namespace PushController.Tests.Controller.CacheHelperTests
{
    [TestClass]
    public class ScanCodeCacheHelperTests
    {
        private ScanCodeCacheHelper cacheHelper;
        private Mock<IQueryHandler<GetScanCodesByIdentifierBulkQuery, List<ScanCodeModel>>> mockGetScanCodesQueryHandler;
        private List<string> testIdentifiers;

        [TestInitialize]
        public void Initialize()
        {
            mockGetScanCodesQueryHandler = new Mock<IQueryHandler<GetScanCodesByIdentifierBulkQuery, List<ScanCodeModel>>>();

            Cache.identifierToScanCode.Clear();
        }

        private void BuildCacheHelper()
        {
            cacheHelper = new ScanCodeCacheHelper(mockGetScanCodesQueryHandler.Object);
        }

        [TestMethod]
        public void PopulateScanCodeCache_ScanCodeIsAlreadyCached_ScanCodeShouldNotBeCachedAgain()
        {
            // Given.
            BuildCacheHelper();

            testIdentifiers = new List<string> { "1111", "2222", "3333" };

            foreach (var identifier in testIdentifiers)
            {
                Cache.identifierToScanCode.Add(identifier, new ScanCodeModel());
            }

            // When.
            cacheHelper.Populate(new List<string> { "2222" });

            // Then.
            Assert.AreEqual(testIdentifiers.Count, Cache.identifierToScanCode.Count);
        }

        [TestMethod]
        public void PopulateScanCodeCache_ScanCodeIsNotYetCached_ScanCodeShouldBeCached()
        {
            // Given.
            mockGetScanCodesQueryHandler.Setup(q => q.Execute(It.IsAny<GetScanCodesByIdentifierBulkQuery>())).Returns(new List<ScanCodeModel> { new ScanCodeModel { ScanCode = "4444" } });

            BuildCacheHelper();

            testIdentifiers = new List<string> { "1111", "2222", "3333" };

            foreach (var identifier in testIdentifiers)
            {
                Cache.identifierToScanCode.Add(identifier, new ScanCodeModel());
            }

            // When.
            cacheHelper.Populate(new List<string> { "4444" });

            // Then.
            var cachedScanCodeModel = Cache.identifierToScanCode["4444"];

            Assert.AreEqual(testIdentifiers.Count + 1, Cache.identifierToScanCode.Count);
            Assert.IsNotNull(cachedScanCodeModel);
            Assert.AreEqual("4444", cachedScanCodeModel.ScanCode);
        }

        [TestMethod]
        public void RetrieveCachedScanCode_ScanCodeIsFound_ScanCodeShouldBeReturned()
        {
            // Given.
            BuildCacheHelper();

            testIdentifiers = new List<string> { "1111", "2222", "3333" };

            foreach (var identifier in testIdentifiers)
            {
                Cache.identifierToScanCode.Add(identifier, new ScanCodeModel { ScanCode = identifier });
            }

            // When.
            var retrievedScanCode = cacheHelper.Retrieve("2222");

            // Then.
            Assert.AreEqual("2222", retrievedScanCode.ScanCode);
        }

        [TestMethod]
        [ExpectedException(typeof(ScanCodeNotFoundException))]
        public void RetrieveCachedScanCode_ScanCodeIsNotFound_ExceptionShouldBeThrown()
        {
            // Given.
            BuildCacheHelper();

            testIdentifiers = new List<string> { "1111", "2222", "3333" };

            foreach (var identifier in testIdentifiers)
            {
                Cache.identifierToScanCode.Add(identifier, new ScanCodeModel { ScanCode = identifier });
            }

            // When.
            var retrievedScanCode = cacheHelper.Retrieve("4444");

            // Then.
            // Expected exception.
        }
    }
}
