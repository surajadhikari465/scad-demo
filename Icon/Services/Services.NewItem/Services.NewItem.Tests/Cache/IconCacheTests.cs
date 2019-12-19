using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Services.NewItem.Cache;
using Icon.Common.DataAccess;
using Services.NewItem.Queries;
using System.Collections.Generic;
using Moq;
using Services.NewItem.Models;

namespace Services.NewItem.Tests.Cache
{
    [TestClass]
    public class IconCacheTests
    {
        private IconCache iconCache;
        private Mock<IQueryHandler<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery, Dictionary<string, int>>> mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler;
        private Mock<IQueryHandler<GetHierarchyClassTraitQuery, Dictionary<int, string>>> mockGetHierarchyClassTraitQueryHandler;
        private Mock<IQueryHandler<GetBrandDictionaryQuery, Dictionary<int, BrandModel>>> mockGetBrandDictionaryQueryHandler;
        private Mock<IQueryHandler<GetTaxClassDictionaryQuery, Dictionary<string, TaxClassModel>>> mockGetTaxDictionaryQueryHandler;
        private Mock<IQueryHandler<GetNationalClassDictionaryQuery, Dictionary<string, NationalClassModel>>> mockGetNationalClassDictionaryQueryHandler;
        private Mock<IQueryHandler<GetSubTeamsDictionaryQuery, Dictionary<string, SubTeamModel>>> mockGetSubTeamModelDictionaryQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler = new Mock<IQueryHandler<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery, Dictionary<string, int>>>();
            mockGetHierarchyClassTraitQueryHandler = new Mock<IQueryHandler<GetHierarchyClassTraitQuery, Dictionary<int, string>>>();
            mockGetBrandDictionaryQueryHandler = new Mock<IQueryHandler<GetBrandDictionaryQuery, Dictionary<int, BrandModel>>>();
            mockGetTaxDictionaryQueryHandler = new Mock<IQueryHandler<GetTaxClassDictionaryQuery, Dictionary<string, TaxClassModel>>>();
            mockGetNationalClassDictionaryQueryHandler = new Mock<IQueryHandler<GetNationalClassDictionaryQuery, Dictionary<string, NationalClassModel>>>();
            mockGetSubTeamModelDictionaryQueryHandler = new Mock<IQueryHandler<GetSubTeamsDictionaryQuery, Dictionary<string, SubTeamModel>>>();

            iconCache = new IconCache(mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Object, 
                mockGetHierarchyClassTraitQueryHandler.Object,
                mockGetBrandDictionaryQueryHandler.Object,
                mockGetTaxDictionaryQueryHandler.Object,
                mockGetNationalClassDictionaryQueryHandler.Object,
                mockGetSubTeamModelDictionaryQueryHandler.Object);
        }

        [TestMethod]
        public void TaxClassCodesToIdDictionary_CacheIsExpired_ShouldReturnTaxClassDictionary()
        {
            //Given
            Dictionary<string, int> expectedDictionary = new Dictionary<string, int>();
            mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery>()))
                .Returns(expectedDictionary);

            //When
            var actualDictionary = iconCache.TaxClassCodesToIdDictionary;

            //Then
            Assert.AreEqual(expectedDictionary, actualDictionary);
        }

        [TestMethod]
        public void TaxClassCodesToIdDictionary_CacheIsNotExpired_ShouldReturnTaxClassDictionaryAndNotCallQueryHandler()
        {
            //Given
            Dictionary<string, int> expectedDictionary = new Dictionary<string, int>();
            mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery>()))
                .Returns(expectedDictionary);

            //When
            var actualDictionary = iconCache.TaxClassCodesToIdDictionary;
            actualDictionary = iconCache.TaxClassCodesToIdDictionary;

            //Then
            mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Verify(m => m.Search(It.IsAny<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery>()), Times.Once);
            Assert.AreEqual(expectedDictionary, actualDictionary);
        }

        [TestMethod]
        public void NationalClassCodesToIdDictionary_CacheIsExpired_ShouldReturnNationalClassDictionary()
        {
            //Given
            Dictionary<string, int> expectedDictionary = new Dictionary<string, int>();
            mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery>()))
                .Returns(expectedDictionary);

            //When
            var actualDictionary = iconCache.NationalClassCodesToIdDictionary;

            //Then
            Assert.AreEqual(expectedDictionary, actualDictionary);
        }

        [TestMethod]
        public void NationalClassCodesToIdDictionary_CacheIsNotExpired_ShouldReturnNationalClassDictionary()
        {
            //Given
            Dictionary<string, int> expectedDictionary = new Dictionary<string, int>();
            mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery>()))
                .Returns(expectedDictionary);

            //When
            var actualDictionary = iconCache.NationalClassCodesToIdDictionary;
            actualDictionary = iconCache.NationalClassCodesToIdDictionary;

            //Then
            mockGetHierarchyClassCodesToHierarchyClassIdsDictionaryQueryHandler.Verify(m => m.Search(It.IsAny<GetHierarchyClassCodesToHierarchyClassIdsDictionaryQuery>()), Times.Once);
            Assert.AreEqual(expectedDictionary, actualDictionary);
        }

        [TestMethod]
        public void BrandIdToAbbreviationDictionary_CacheIsExpired_ShouldReturnBrandAbbreviationDictionary()
        {
            //Given
            Dictionary<int, string> expectedDictionary = new Dictionary<int, string>();
            mockGetHierarchyClassTraitQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyClassTraitQuery>()))
                .Returns(expectedDictionary);

            //When
            var actualDictionary = iconCache.BrandIdToAbbreviationDictionary;

            //Then
            Assert.AreEqual(expectedDictionary, actualDictionary);
        }

        [TestMethod]
        public void BrandIdToAbbreviationDictionary_CacheIsNotExpired_ShouldReturnBrandAbbreviationDictionaryNotCallQueryHandler()
        {
            //Given
            Dictionary<int, string> expectedDictionary = new Dictionary<int, string>();
            mockGetHierarchyClassTraitQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyClassTraitQuery>()))
                .Returns(expectedDictionary);

            //When
            var actualDictionary = iconCache.BrandIdToAbbreviationDictionary;
            actualDictionary = iconCache.BrandIdToAbbreviationDictionary;

            //Then
            mockGetHierarchyClassTraitQueryHandler.Verify(m => m.Search(It.IsAny<GetHierarchyClassTraitQuery>()), Times.Once);
            Assert.AreEqual(expectedDictionary, actualDictionary);
        }
    }
}
