using Mammoth.Common.DataAccess.CommandQuery;
using Mammoth.Esb.ProductListener.Cache;
using Mammoth.Esb.ProductListener.Models;
using Mammoth.Esb.ProductListener.Queries;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;

namespace Mammoth.Esb.ProductListener.Tests.Cache
{
    [TestClass]
    public class HierarchyClassCacheTests
    {
        private HierarchyClassCache cache;
        private Mock<IQueryHandler<GetHierarchyClassesParameters, List<HierarchyClassModel>>> mockGetHierarchyClassesQueryHandler;

        [TestInitialize]
        public void Initialize()
        {
            mockGetHierarchyClassesQueryHandler = new Mock<IQueryHandler<GetHierarchyClassesParameters, List<HierarchyClassModel>>>();
            cache = new HierarchyClassCache(mockGetHierarchyClassesQueryHandler.Object);
        }

        [TestMethod]
        public void GetFinancialDictionary_TaxClassesExistInDatabase_ShouldCacheTaxClasses()
        {
            //Given
            List<HierarchyClassModel> testHierarchyClassModel = new List<HierarchyClassModel>
            {
                new HierarchyClassModel { HierarchyClassName = "1111111 Test Tax 1", HierarchyClassId = 1 },
                new HierarchyClassModel { HierarchyClassName = "1111112 Test Tax 1", HierarchyClassId = 2 },
                new HierarchyClassModel { HierarchyClassName = "1111113 Test Tax 1", HierarchyClassId = 3 },
            };
            mockGetHierarchyClassesQueryHandler.Setup(m => m.Search(It.IsAny<GetHierarchyClassesParameters>()))
                .Returns(testHierarchyClassModel);

            //When
            var dictionary = cache.GetTaxDictionary();

            //Then
            Assert.AreEqual(dictionary.Count, 3);
            Assert.AreEqual(dictionary["1111111"], 1);
            Assert.AreEqual(dictionary["1111112"], 2);
            Assert.AreEqual(dictionary["1111113"], 3);
        }
    }
}
