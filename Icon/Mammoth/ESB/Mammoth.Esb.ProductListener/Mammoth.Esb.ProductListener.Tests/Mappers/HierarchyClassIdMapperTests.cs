using Mammoth.Esb.ProductListener.Cache;
using Mammoth.Esb.ProductListener.Mappers;
using Mammoth.Esb.ProductListener.Models;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using System.Collections.Generic;
using System.Linq;

namespace Mammoth.Esb.ProductListener.Tests.Mappers
{
    [TestClass]
    public class HierarchyClassIdMapperTests
    {
        private HierarchyClassIdMapper mapper;
        private Mock<IHierarchyClassCache> mockCache;

        [TestInitialize]
        public void Initialize()
        {
            mockCache = new Mock<IHierarchyClassCache>();
            mapper = new HierarchyClassIdMapper(mockCache.Object);
        }

        [TestMethod]
        public void PopulateHierarchyClassDatabaseIds_ListOfHierarchyClassExist_ShouldPopulateHierarchyClassIdsOnProducts()
        {
            //Given
            var taxDictionary = new Dictionary<string, int>
            {
                { "1111111", 1 },
                { "1111112", 2 },
                { "1111113", 3 }
            };
            var testProducts = new List<GlobalAttributesModel>()
            {
                new GlobalAttributesModel { MessageTaxClassHCID = taxDictionary.Keys.ElementAt(0) },
                new GlobalAttributesModel { MessageTaxClassHCID = taxDictionary.Keys.ElementAt(1) },
                new GlobalAttributesModel { MessageTaxClassHCID = taxDictionary.Keys.ElementAt(2) }
            };

            mockCache.Setup(m => m.GetTaxDictionary())
                .Returns(taxDictionary);

            //When
            mapper.PopulateHierarchyClassDatabaseIds(testProducts);

            //Then
            foreach (var product in testProducts)
            {
                Assert.IsNotNull(product.TaxClassHCID);
                Assert.AreNotEqual(0, product.TaxClassHCID);
            }
            for (int i = 0; i < testProducts.Count; i++)
            {
                Assert.AreEqual(taxDictionary[testProducts[i].MessageTaxClassHCID], testProducts[i].TaxClassHCID);
            }
        }
    }
}
